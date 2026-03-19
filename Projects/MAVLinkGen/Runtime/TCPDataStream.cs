using System;
using System.Net.Http;
using System.Net.Sockets;

namespace MAVLinkSharp.Runtime {

    /// <summary>
    /// TCPDataStream implements a TCP transport for BaseDataStream.
    ///
    /// Data flow:
    ///
    ///     TCP socket -> NetworkStream.Read -> m_buffer
    ///        ↓
    ///     BaseDataStream m_rcv_pool
    ///        ↓
    ///     Parser thread -> OnDataReceive()
    ///
    /// The receive thread blocks on NetworkStream.Read().
    /// Closing the socket during Dispose() will unblock the read and
    /// allow the thread to exit cleanly.
    ///
    /// This class performs no protocol parsing. It only moves raw bytes
    /// between the TCP stream and the BaseDataStream infrastructure.
    /// </summary>
    public class TCPDataStream : BaseDataStream {

        /// <summary>
        /// TCP client associated with the stream.
        /// </summary>
        protected readonly TcpClient m_client;

        /// <summary>
        /// Network stream used for TCP communication.
        /// </summary>
        protected readonly NetworkStream m_stream;

        /// <summary>
        /// Reusable receive buffer used by the receive thread.
        /// Data is copied into pooled packets before parsing.
        /// </summary>
        protected readonly byte[] m_buffer;

        /// <summary>
        /// Initializes the TCP data stream.
        /// </summary>
        /// <param name="p_client">Connected TCP client.</param>
        /// <param name="p_buffer_size">Size of the receive buffer.</param>
        /// <param name="p_name">Optional thread name prefix.</param>
        public TCPDataStream(TcpClient p_client,string p_name = null,int p_buffer_size = 4096) : base(p_name ?? "TCP",p_buffer_len: p_buffer_size <= 0 ? 4096 : p_buffer_size) {
            if(p_client == null) return;
            if(p_buffer_size <= 0) p_buffer_size = 1024;
            m_client = p_client;
            m_stream = m_client.GetStream();
            m_buffer = new byte[p_buffer_size];
        }

        /// <summary>
        /// Performs one receive operation from the TCP stream.
        ///
        /// Called repeatedly by the BaseDataStream receive thread.
        /// </summary>
        protected override bool TryReceiveOnce(out byte[] p_buffer,out int p_length) {
            p_buffer = m_buffer;
            p_length = 0;
            if(m_stream == null) return false;
            try { p_length = m_stream.Read(m_buffer,0,m_buffer.Length); }
            catch { return false; }
            if(p_length <= 0) return false;
            return true;
        }

        /// <summary>
        /// Sends a buffer region over the TCP stream.
        ///
        /// Called by the BaseDataStream send thread.
        /// </summary>
        protected override void OnDataSend(byte[] p_data,int p_length) {
            if(m_stream == null) return;
            if(p_data == null || p_length <= 0) return;
            if(p_length > p_data.Length) p_length = p_data.Length;
            try { m_stream.Write(p_data,0,p_length); } catch { }
        }

        /// <summary>
        /// Called during disposal to release TCP resources.
        ///
        /// Closing the stream will unblock any blocking Read() call.
        /// </summary>
        protected override void OnDispose() {            
            try { 
                if(m_client!=null) {
                    m_client.LingerState = new LingerOption(true,0);
                    m_client?.Client.Shutdown(SocketShutdown.Both);
                    m_client?.Close();
                }                
            } 
            catch(System.Exception p_err) {
                #if UNITY_2017_1_OR_NEWER
                UnityEngine.Debug.LogWarning($"TCPDataStream> Dispose Error\n{p_err.Message}");
                #endif                
            }
        }

        /// <summary>
        /// Called by the parser thread when new data arrives.
        ///
        /// Derived classes should override this method to process
        /// the incoming byte stream.
        /// </summary>
        protected override void OnDataReceive(byte[] p_data,int p_length) { }
    }
}
