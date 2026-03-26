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
        public MAVLinkUDP(string p_name="") : base(null,p_name) {            
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
                m_client = new UdpClient();
                m_client.EnableBroadcast = true;
                //m_client.Client.ExclusiveAddressUse = false;
                //m_client.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                if (p_port > 0) {
                    m_client.Client.Bind(new IPEndPoint(IPAddress.Any, p_port));
                }
                m_client.Client.ReceiveBufferSize = 4 * 1024 * 1024;
                m_client.Client.SendBufferSize    = 4 * 1024 * 1024;
            }
            catch(System.Exception p_err) {
                #if UNITY_2017_1_OR_NEWER
                UnityEngine.Debug.LogWarning($"MAVLinkUDP> Start / Error\n{p_err.Message}");
                #endif
            }

            UDPDataStream dss = new UDPDataStream(m_client,name,320);
            SetStream(dss);

        }

        /// <summary>
        /// Sets the target endpoints to send
        /// </summary>
        /// <param name="p_targets"></param>
        public void SetTargets(params IPEndPoint[] p_targets) {
            UDPDataStream dss = Stream == null ? null : (UDPDataStream)Stream;
            if(dss==null) return;
            foreach(IPEndPoint it in p_targets) dss.AddTarget(it);
        }

        /// <summary>
        /// DTOR
        /// </summary>
        protected override void OnDispose() {            
            base.OnDispose();            
        }

    }
}
