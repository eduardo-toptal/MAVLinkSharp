using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

#pragma warning disable CS8603
#pragma warning disable CS0168
#pragma warning disable CS8632

namespace MAVLinkSharp.Runtime {

    /// <summary>
    /// Class that describes an UDP MAVLinkConnection
    /// </summary>
    public class MAVLinkUDP : MAVLinkConnection {

        /// <summary>
        /// Internals
        /// </summary>
        private UdpClient? m_client;
        private List<IPEndPoint> m_targets;
        private IPEndPoint m_rcv_ep;

        /// <summary>
        /// CTOR
        /// </summary>
        public MAVLinkUDP(string p_name="") : base(p_name) {            
            m_targets = new List<IPEndPoint>();
            m_rcv_ep  = new IPEndPoint(IPAddress.Parse("0.0.0.0"),0);
        }

        /// <summary>
        /// Starts listening into the local port.
        /// </summary>
        /// <param name="p_port"></param>
        public void Start(int p_port=0) {
            if(m_client!=null) { 
                try { m_client.Close(); } catch(System.Exception){ }
                m_client = null;
            }
            try {
                //m_client = p_port<=0 ? new UdpClient() : new UdpClient(p_port);
                m_client = new UdpClient();                
                m_client.Client.ExclusiveAddressUse = false;
                m_client.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                if (p_port > 0) {
                    m_client.Client.Bind(new IPEndPoint(IPAddress.Any, p_port));
                }
                m_client.Client.ReceiveBufferSize = 4 * 1024 * 1024;
                m_client.Client.SendBufferSize    = 1 * 1024 * 1024;
            }
            catch(System.Exception p_err) {
                #if UNITY_2017_1_OR_NEWER
                UnityEngine.Debug.LogWarning($"MAVLinkUDP> Start / Error\n{p_err.Message}");
                #endif
            }
        }

        /// <summary>
        /// Sets the target endpoints to send
        /// </summary>
        /// <param name="p_targets"></param>
        public void SetTargets(params IPEndPoint[] p_targets) {
            m_targets = new List<IPEndPoint>(p_targets);
        }

        /// <summary>
        /// Handler for sending data packets thru the link.
        /// </summary>
        /// <param name="p_packet"></param>
        /// <param name="p_length"></param>
        override protected void OnPacketSend(byte[] p_packet,int p_length) {
            if(m_client==null) return;
            for(int i=0;i<m_targets.Count;i++) {
                IPEndPoint ep = m_targets[i];
                try { m_client.Send(p_packet,p_length, ep); } catch(System.Exception) { }
            }            
        }

        public void SendPacket(byte[] p_packet,int p_length=-1) {
            int len = p_length < 0 ? (p_packet == null ? -1 : p_packet.Length) : p_length;
            if (len < 0) return;
            for (int i = 0; i < m_targets.Count; i++) {
                IPEndPoint ep = m_targets[i];
                try { m_client.Send(p_packet, p_length, ep); } catch (System.Exception) { }
            }
        }

        /// <summary>
        /// Handler for byte data received.
        /// </summary>
        /// <param name="p_buffer"></param>
        /// <param name="p_length"></param>
        protected override void OnPacketReceive(out byte[]? p_buffer,out int p_length) {
            byte[]? d = null;
            p_buffer = d;
            p_length = 0;
            if(m_client==null) return;
            try { d = m_client.Receive(ref m_rcv_ep); } catch(System.Exception p_err) { }   
            p_buffer = d;
            p_length = d==null ? 0 : d.Length;
        }

        /// <summary>
        /// DTOR
        /// </summary>
        protected override void OnDispose() {
            base.OnDispose();
            if (m_client!=null) m_client.Dispose();
        }

    }
}
