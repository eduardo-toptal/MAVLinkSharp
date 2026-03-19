using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace MAVLinkSharp.Runtime {

    /// <summary>
    /// UDPDataStream implements a UDP transport for BaseDataStream.
    ///
    /// Data flow:
    ///
    ///     UDP socket -> UdpClient.Receive -> m_buffer
    ///        ↓
    ///     BaseDataStream m_rcv_pool
    ///        ↓
    ///     Parser thread -> OnDataReceive()
    ///
    /// This class supports multiple send targets. Each outgoing packet
    /// is transmitted to every registered target.
    ///
    /// Datagram boundaries are preserved by the transport receive call,
    /// but the parser still receives raw byte chunks.
    ///
    /// The receive thread blocks on UdpClient.Receive().
    /// Closing the client during Dispose() will unblock the receive
    /// operation and allow the thread to exit cleanly.
    /// </summary>
    public class UDPDataStream : BaseDataStream {

        /// <summary>
        /// UDP client used for communication.
        /// </summary>
        protected UdpClient m_client;

        /// <summary>
        /// Reusable receive buffer reference.
        /// Present for symmetry with the TCP transport.
        /// </summary>
        protected readonly byte[] m_buffer;

        /// <summary>
        /// List of target endpoints to send packets to.
        /// </summary>
        protected readonly List<IPEndPoint> m_targets = new();

        /// <summary>
        /// Synchronizes access to the target list.
        /// </summary>
        protected readonly object m_targetLock = new();

        private IPEndPoint m_rcv_ep;

        /// <summary>
        /// Initializes the UDP data stream.
        /// </summary>
        /// <param name="p_client">UDP client used by the transport.</param>
        /// <param name="p_buffer_size">Receive buffer size hint.</param>
        /// <param name="p_name">Optional thread name prefix.</param>
        public UDPDataStream(UdpClient p_client,string p_name = null,int p_buffer_size = 4096) : base(p_name ?? "UDP",p_buffer_len: p_buffer_size <= 0 ? 4096 : p_buffer_size) {
            if(p_client == null) return;
            if(p_buffer_size <= 0) p_buffer_size = 1024;
            m_client = p_client;
            m_buffer = new byte[p_buffer_size];
            m_rcv_ep = new IPEndPoint(IPAddress.Any,0);
        }

        /// <summary>
        /// Adds a target endpoint that outgoing packets will be sent to.
        /// </summary>
        public void AddTarget(IPEndPoint p_target) {
            if(p_target == null) return;
            lock(m_targetLock) m_targets.Add(p_target);
        }

        /// <summary>
        /// Performs one UDP receive operation.
        ///
        /// Called repeatedly by the BaseDataStream receive thread.
        /// </summary>
        protected override bool TryReceiveOnce(out byte[] p_buffer,out int p_length) {

            p_buffer = m_buffer;
            p_length = 0;

            if(m_client == null) return false;

            byte[] data;

            try { data = m_client.Receive(ref m_rcv_ep); } catch { return false; }

            if(data == null || data.Length == 0) return false;

            p_buffer = data;
            p_length = data.Length;

            return true;
        }

        /// <summary>
        /// Sends a buffer region to all registered target endpoints.
        ///
        /// Called by the BaseDataStream send thread.
        /// </summary>
        protected override void OnDataSend(byte[] p_data,int p_length) {
            if(m_client == null) return;
            if(p_data == null || p_length <= 0) return;
            if(p_length > p_data.Length) p_length = p_data.Length;
            lock(m_targetLock) {
                for(int i = 0; i < m_targets.Count; i++) {
                    try { m_client.Send(p_data,p_length,m_targets[i]); }
                    catch { }
                }
            }
        }

        /// <summary>
        /// Releases UDP resources during disposal.
        /// </summary>
        protected override void OnDispose() {            
            try { m_client?.Close(); } 
            catch(System.Exception p_err) {
                #if UNITY_2017_1_OR_NEWER
                UnityEngine.Debug.LogWarning($"UDPDataStream> Dispose Error\n{p_err.Message}");
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
