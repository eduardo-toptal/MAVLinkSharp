using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace MAVLinkSharp.Runtime {

    /// <summary>
    /// BaseDataStream implements a high-throughput byte-stream transport skeleton.
    ///
    /// Thread architecture:
    ///
    ///     RCV thread  -> receives bytes from the transport
    ///     SND thread  -> sends outgoing buffers
    ///     PRS thread  -> parses received buffers
    ///
    /// Receive side:
    ///     transport receive -> copy into m_rcv_pool -> parser thread
    ///
    /// Send side:
    ///     Send(...) -> copy into m_snd_pool -> sender thread
    ///
    /// Caller-owned buffers and streams are never used directly after Send(...)
    /// returns. All send/receive work is done on pooled Packet instances.
    /// </summary>
    public abstract class BaseDataStream : IDisposable {

        /// <summary>
        /// Handler for when data is ready to be sent.
        /// Provides the buffer and the valid byte length.
        /// </summary>
        public Action<byte[],int> OnDataSendEvent;

        /// <summary>
        /// Handler for when data is ready to be received and used.
        /// Provides the buffer and the valid byte length.
        /// </summary>
        public Action<byte[],int> OnDataReceiveEvent;

        /// <summary>Receive worker thread.</summary>
        private readonly Thread m_rcvThread;

        /// <summary>Send worker thread.</summary>
        private readonly Thread m_sndThread;

        /// <summary>Parser worker thread.</summary>
        private readonly Thread m_prsThread;

        /// <summary>
        /// Cancellation source used to stop worker threads.
        /// </summary>
        private readonly CancellationTokenSource m_cts = new();

        /// <summary>
        /// Signal used to wake the send thread when new data arrives.
        /// </summary>
        private readonly AutoResetEvent m_sendSignal = new(false);

        /// <summary>
        /// Signal used to wake the parser thread when received data arrives.
        /// </summary>
        private readonly AutoResetEvent m_parseSignal = new(false);

        /// <summary>
        /// Send packet pool.
        /// </summary>
        private readonly DataPacketPool m_snd_pool;

        /// <summary>
        /// Receive packet pool.
        /// </summary>
        private readonly DataPacketPool m_rcv_pool;

        private volatile bool m_started;
        private volatile bool m_disposed;

        /// <summary>
        /// Indicates worker threads should exit.
        /// </summary>
        private bool WillCancelThread { get { return m_disposed || m_cts.IsCancellationRequested; } }

        /// <summary>
        /// Initializes the data stream and worker threads.
        /// </summary>
        protected BaseDataStream(string p_name = null, int p_pool_count = 512, int p_buffer_len = 4096) {

            string threadName = string.IsNullOrWhiteSpace(p_name) ? GetType().Name : p_name;

            m_rcvThread = new Thread(ReceiveLoop) { Name = threadName + ".RCV",IsBackground = true };
            m_sndThread = new Thread(SendLoop)    { Name = threadName + ".SND",IsBackground = true };
            m_prsThread = new Thread(ParseLoop)   { Name = threadName + ".PRS",IsBackground = true };

            m_snd_pool = new DataPacketPool(p_pool_count,p_buffer_len);
            m_rcv_pool = new DataPacketPool(p_pool_count,p_buffer_len);
        }

        /// <summary>
        /// Starts the receive, send, and parser threads.
        /// </summary>
        public void Start() {
            if(m_started || m_disposed) return;
            m_started = true;
            m_rcvThread.Start();
            m_sndThread.Start();
            m_prsThread.Start();
        }

        /// <summary>
        /// Queues a buffer for transmission.
        /// Entire buffer will be sent.
        /// </summary>
        public void Send(byte[] p_data) {
            if(p_data == null) return;
            Send(p_data,p_data.Length);
        }

        /// <summary>
        /// Queues a buffer region for transmission.
        /// Only the first p_length bytes are sent.
        /// </summary>
        public void Send(byte[] p_data,int p_length) {
            if(!m_started || m_disposed) return;
            if(p_data == null || p_length <= 0) return;
            if(p_length > p_data.Length) p_length = p_data.Length;
            if(!m_snd_pool.Push(p_data,p_length)) return;
            m_sendSignal.Set();
        }

        /// <summary>
        /// Queues stream contents for transmission.
        /// </summary>
        public void Send(Stream p_stream) {
            if(!m_started || m_disposed)    return;
            if(!m_snd_pool.Push(p_stream))  return;
            m_sendSignal.Set();
        }

        /// <summary>
        /// Main receive loop.
        ///
        /// Derived transports implement TryReceiveOnce().
        /// </summary>
        private void ReceiveLoop() {

            byte[] buffer;
            int length;

            while(!WillCancelThread) {
                if(!TryReceiveOnce(out buffer,out length)) { Thread.Yield(); continue; }
                if(m_rcv_pool.Push(buffer,length)) m_parseSignal.Set();
            }

            #if UNITY_2017_1_OR_NEWER
            UnityEngine.Debug.Log($"DataStream> Thread {Thread.CurrentThread.Name} Complete!");
            #endif

        }

        /// <summary>
        /// Main send loop.
        ///
        /// Sends queued packets from the send pool.
        /// </summary>
        private void SendLoop() {

            while(!WillCancelThread) {                
                while(m_snd_pool.Pop(out DataPacketPool.Packet packet)) {
                    try {
                        OnDataSend(packet.buffer,packet.length);
                        if(OnDataSendEvent != null) OnDataSendEvent(packet.buffer,packet.length);
                    }
                    finally {
                        m_snd_pool.Return(packet);
                    }
                    continue;
                }
                m_sendSignal.WaitOne();
                //Thread.Yield();
            }

            #if UNITY_2017_1_OR_NEWER
            UnityEngine.Debug.Log($"DataStream> Thread {Thread.CurrentThread.Name} Complete!");
            #endif

        }

        /// <summary>
        /// Parser loop.
        ///
        /// Parses queued receive packets.
        /// </summary>
        private void ParseLoop() {

            while(!WillCancelThread) {
                while(m_rcv_pool.Pop(out DataPacketPool.Packet packet)) {
                    try {
                        OnDataReceive(packet.buffer,packet.length);
                        if(OnDataReceiveEvent != null) OnDataReceiveEvent(packet.buffer,packet.length);
                    }
                    finally {
                        m_rcv_pool.Return(packet);
                    }
                    continue;
                }
                m_parseSignal.WaitOne();
                //Thread.Yield();
            }

            #if UNITY_2017_1_OR_NEWER
            UnityEngine.Debug.Log($"DataStream> Thread {Thread.CurrentThread.Name} Complete!");
            #endif

        }

        /// <summary>
        /// Stops worker threads and releases resources.
        /// </summary>
        public void Dispose() {

            if(m_disposed) return;

            m_disposed = true;

            m_cts.Cancel();

            m_sendSignal.Set();
            m_parseSignal.Set();

            OnDispose();

            m_rcvThread.Join();
            m_sndThread.Join();
            m_prsThread.Join();

            m_snd_pool.Clear();
            m_rcv_pool.Clear();

            m_sendSignal.Dispose();
            m_parseSignal.Dispose();
            m_cts.Dispose();
        }

        /// <summary>
        /// Transport receive implementation.
        ///
        /// Should return a buffer containing received bytes.
        /// </summary>
        protected abstract bool TryReceiveOnce(
            out byte[] p_buffer,
            out int p_length
        );

        /// <summary>
        /// Transport send implementation.
        /// </summary>
        protected abstract void OnDataSend(
            byte[] p_data,
            int p_length
        );

        /// <summary>
        /// Application parser implementation.
        /// </summary>
        protected abstract void OnDataReceive(
            byte[] p_data,
            int p_length
        );

        /// <summary>
        /// Allows derived transports to release resources
        /// such as sockets or serial ports.
        /// </summary>
        protected virtual void OnDispose() { }
    }
}
