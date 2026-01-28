using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

#pragma warning disable CS8603
#pragma warning disable CS8632
#pragma warning disable CS8600

namespace MAVLinkSharp.Runtime {

    /// <summary>
    /// Class that describes an TCP MAVLinkConnection
    /// </summary>
    public class MAVLinkTCP : MAVLinkConnection {

        /// <summary>
        /// Flag that tells a client has connected in this connection.
        /// </summary>
        public bool connected { get { return m_client==null ? false : m_client.Connected; } }

        /// <summary>
        /// Reference to the connected client.
        /// </summary>
        public TcpClient client { get { return m_client; } }

        /// <summary>
        /// Internals
        /// </summary>
        private TcpListener? m_conn;
        private TcpClient?   m_client;
        private Task?        m_listen_tsk;
        private byte[]       m_buffer;
        private object m_lock_ns;

        /// <summary>
        /// CTOR
        /// </summary>
        public MAVLinkTCP(string p_name="") : base(p_name) {                        
            m_buffer  = new byte[1024 * 80];
            m_lock_ns = new object();
        }

        /// <summary>
        /// Starts listening into the local port.
        /// </summary>
        /// <param name="p_port"></param>
        public void Start(int p_port=0) {
            if(m_conn!=null) { 
                try { m_conn.Stop(); } catch(System.Exception){ }
                m_conn   = null;
                m_client = null;
            }
            //Console.WriteLine($"[{name}] Waiting Client...");
            m_conn = new TcpListener(IPAddress.Parse("0.0.0.0"),p_port);            
            m_conn.Start();                        
            m_listen_tsk =
            Task.Run(async delegate() { 
                m_client = await m_conn.AcceptTcpClientAsync();
                m_client.NoDelay = true;
                m_client.Client.ReceiveBufferSize = 4 * 1024 * 1024; //   4 MB receive buffer
                m_client.Client.SendBufferSize    = 1 * 1024 * 1024; // 512 KB send buffer
                m_listen_tsk = null;
                //Console.WriteLine($"[{name}] Client Connected!");
            });
        }

        /// <summary>
        /// Handler for sending data packets thru the link.
        /// </summary>
        /// <param name="p_packet"></param>
        /// <param name="p_length"></param>
        override protected void OnPacketSend(byte[] p_packet,int p_length) {
            if(m_conn  ==null) return;
            if(m_client==null) return;            
            try { 
                NetworkStream ns = m_client.GetStream();                
                ns.Write(p_packet,0,p_length);                        
            } catch(System.Exception) { }
        }

        protected override void OnPacketReceive(out byte[]? p_buffer,out int p_length) {
            byte[] d = null;
            p_buffer = d;
            p_length = 0;
            if(m_conn   == null) return;
            if(m_client == null) return;            
            int c = 0;
            try { 
                NetworkStream ns = m_client.GetStream();                
                c = ns.Read(m_buffer);                 
            } catch(System.Exception){ }
            if(c<=0) return;
            p_buffer = m_buffer;
            p_length = c;
        }

        /// <summary>
        /// DTOR
        /// </summary>
        protected override void OnDispose() {
            base.OnDispose();
            if (m_conn != null) try { m_conn.Stop(); } catch (System.Exception) { }
            if (m_listen_tsk != null) try { m_listen_tsk.Dispose(); } catch(System.Exception) { }            
            m_client = null;
        }

    }
}
