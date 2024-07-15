using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable CS8600
#pragma warning disable CS8602
#pragma warning disable CS8603
#pragma warning disable CS8618
#pragma warning disable CS8625

namespace MAVLinkSharp {

    /// <summary>
    /// Class that implements an UDP based interface to sync messages over network.
    /// </summary>
    public class MAVLinkUDP : MAVLinkInterface {

        /// <summary>
        /// Reference to the client
        /// </summary>
        public UdpClient client { get; set; }

        /// <summary>
        /// Latest endpoint from the last message
        /// </summary>
        public IPEndPoint messageEndPoint {get; private set; }

        /// <summary>
        /// Flag that tells there is a UDPClient and its connected
        /// </summary>
        public override bool connected { get { return client == null ? false : (client.Client==null ? false : client.Client.Connected); } }

        /// <summary>
        /// Internals
        /// </summary>
        private Task<UdpReceiveResult> m_rcv_tsk;
        
        /// <summary>
        /// CTOR.
        /// </summary>
        /// <param name="p_name"></param>
        public MAVLinkUDP(UdpClient p_client,string p_name = "") : base(p_name) {
            if (p_client == null) throw new ArgumentNullException();
            client = p_client;
            client.Client.ReceiveBufferSize = 1024 * 512;
            client.Client.SendBufferSize    = 1024 * 512;
        }

        /// <summary>
        /// Flushes the data into the clien't buffer
        /// </summary>
        /// <param name="p_data"></param>
        protected override void OnDataSend(byte[] p_data) {
            if(client==null) return;
            if(client.Client==null) return;                        
            try {
                client.Send(p_data,p_data.Length);                            
            }
            catch(System.Exception) {                
            }            
        }

        /// <summary>
        /// Handles the UDP client network logixc
        /// </summary>
        override protected void OnUpdate() {
            //Updates the main logic
            base.OnUpdate();

            Task<UdpReceiveResult> tsk = m_rcv_tsk;
            bool is_receiving = tsk != null;
            bool is_valid = client == null ? false : (client.Client == null ? false : true);

            if (!is_valid) return;            
            if (is_receiving) {                
                switch(tsk.Status) {
                    case TaskStatus.RanToCompletion: { 
                        UdpReceiveResult res = tsk.Result;                       
                        byte[] b = res.Buffer;
                        messageEndPoint = res.RemoteEndPoint;
                        OnDataReceive(b,0,b.Length);
                        m_rcv_tsk = null;
                    }
                    break;
                    case TaskStatus.Faulted:
                    case TaskStatus.Canceled: m_rcv_tsk = null; break;
                }
            }
            else {
                m_rcv_tsk = client.ReceiveAsync();
            }
        }

        /// <summary>
        /// Handler for when closing this interface
        /// </summary>
        protected override void OnClose() {
            if (client != null) client.Close();
        }

    }
}
