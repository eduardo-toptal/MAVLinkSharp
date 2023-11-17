using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

#pragma warning disable CS8600
#pragma warning disable CS8602
#pragma warning disable CS8603
#pragma warning disable CS8618
#pragma warning disable CS8625

namespace MAVConsole {

    /// <summary>
    /// Class that implements a TCP based interface to sync messages over network.
    /// </summary>
    public class MAVLinkTCP : MAVLinkInterface {

        /// <summary>
        /// Reference to the client
        /// </summary>
        public TcpClient client { get; set; }

        /// <summary>
        /// Internals
        /// </summary>
        private TcpListener m_server;
        private Task<TcpClient> m_server_task;
        private Task<int>       m_rcv_tsk;
        private byte[]          m_rcv_buff;
        
        /// <summary>
        /// CTOR.
        /// </summary>
        /// <param name="p_name"></param>
        public MAVLinkTCP(TcpClient p_client,string p_name = "") : this(p_name) {
            if (p_client == null) throw new ArgumentNullException();
            client = p_client;
        }

        /// <summary>
        /// CTOR.
        /// </summary>
        /// <param name="p_name"></param>
        public MAVLinkTCP(string p_name = "") : base(p_name) {
            m_rcv_buff = new byte[65 * 1024];
        }

        /// <summary>
        /// Starts this TCP interface as server and wait for the TcpClient
        /// </summary>
        /// <param name="p_addr"></param>
        /// <param name="p_port"></param>
        public void Listen(IPAddress p_addr,int p_port) {
            m_server = new TcpListener(new IPEndPoint(p_addr,p_port));
            m_server.Start();
            m_server_task = m_server.AcceptTcpClientAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p_port"></param>
        public void Listen(int p_port) { Listen(IPAddress.Any,p_port); }

        /// <summary>
        /// Flushes the data into the clien't buffer
        /// </summary>
        /// <param name="p_data"></param>
        protected override void OnDataSend(byte[] p_data) {
            if(client == null)    return;
            if(!client.Connected) return;
            NetworkStream ns = client.GetStream();
            try {
                if(ns!=null)ns.Write(p_data,0,p_data.Length);
            }
            catch (Exception) {                
            }            
        }

        /// <summary>
        /// Handles the UDP client network logixc
        /// </summary>
        override protected void OnUpdate() {
            //Updates main logic
            base.OnUpdate();
            //Check if ther is a valid client
            bool has_client = client != null;
            //Client is available then we can start syncing data
            if(has_client) {
                //Skip if not connected yet
                if (!client.Connected) return;
                //Check if there is any receiving task ongoing
                Task<int> tsk = m_rcv_tsk;
                bool is_read = tsk != null;
                if(is_read) {
                    switch(tsk.Status) {
                        case TaskStatus.Canceled:
                        case TaskStatus.Faulted: {
                            //Invalidate if error and try again
                            m_rcv_tsk = null;
                        }
                        break;

                        case TaskStatus.RanToCompletion: {
                            //Fetch the data and pipe it thru the stream
                            int    c = tsk.Result;
                            byte[] b = m_rcv_buff;
                            OnDataReceive(b,0,c);                            
                            m_rcv_tsk=null;
                        }
                        break;
                    }
                }
                else {
                    //Fetch the active stream
                    NetworkStream ns = client.GetStream();                    
                    try {
                        //Start the receive task
                        m_rcv_tsk = ns.ReadAsync(m_rcv_buff,0,m_rcv_buff.Length);
                    } catch (Exception) { }
                }                
            }
            //If no client check if running in server mode
            else {
                Task<TcpClient> tsk = m_server_task;
                bool is_listening = tsk != null;
                //There is an active listening task
                if(is_listening) {
                    //Check status until completion
                    switch(tsk.Status) {
                        case TaskStatus.Canceled:
                        case TaskStatus.Faulted: {
                            //Report any error
                            IPEndPoint ep = (m_server.LocalEndpoint is IPEndPoint) ? (IPEndPoint)m_server.LocalEndpoint : null;
                            string ip_s = ep == null ? $"<null>" : ep.Address.ToString();
                            string p_s  = ep == null ? $"<null>" : ep.Port.ToString();
                            Console.WriteLine($"\nMAVLinkTCP> [{name}] Listening Failed [tcp://{ip_s}:{p_s}]");
                        }
                        break;

                        case TaskStatus.RanToCompletion: {
                            //Fetch the client and log the results
                            client = tsk.Result;
                            IPEndPoint ep = (client.Client.RemoteEndPoint is IPEndPoint) ? (IPEndPoint)client.Client.RemoteEndPoint : null;
                            string ip_s = ep == null ? $"<null>" : ep.Address.ToString();
                            string p_s  = ep == null ? $"<null>" : ep.Port.ToString();
                            Console.WriteLine($"\nMAVLinkTCP> [{name}] Connected to [tcp://{ip_s}:{p_s}]");                            
                        }
                        break;
                    }

                }
            }
            
        }

    }
}
