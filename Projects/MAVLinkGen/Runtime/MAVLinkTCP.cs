using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading;
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
        public MAVLinkTCP(string p_name="") : base(null,p_name) {                        
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
            try {
                m_conn = new TcpListener(IPAddress.Parse("0.0.0.0"), p_port);
                m_conn.Start();                
            }
            catch(System.Exception p_err) {
                #if UNITY_2017_1_OR_NEWER
                UnityEngine.Debug.LogWarning($"MAVLinkTCP> Start / Error\n{p_err.Message}");
                #endif
            }
            m_listen_tsk =
            Task.Run(async delegate() {
                try {                    
                    m_client = await m_conn.AcceptTcpClientAsync();
                    m_client.NoDelay = true;
                    m_client.Client.ReceiveBufferSize = 4 * 1024 * 1024;
                    m_client.Client.SendBufferSize    = 4 * 1024 * 1024;
                    m_listen_tsk = null;
                    #if UNITY_2017_1_OR_NEWER
                    UnityEngine.Debug.Log($"MAVLinkTCP> [{name}] Client Connected!");
                    #endif                    
                    TCPDataStream dds = new TCPDataStream(m_client,name,320);
                    SetStream(dds);
                }
                catch(System.Exception p_err) {
                    #if UNITY_2017_1_OR_NEWER
                    UnityEngine.Debug.LogWarning($"MAVLinkTCP> Start / AcceptTcpClientAsync - Error\n{p_err.Message}");
                    #endif
                }
            });
        }

        /// <summary>
        /// DTOR
        /// </summary>
        protected override void OnDispose() {

            //Finish Accept Task
            if(m_listen_tsk != null) try { m_listen_tsk.Dispose(); } catch(System.Exception) { }
            m_listen_tsk = null;

            if(m_conn != null) { 
                try {                    
                    m_conn.Stop();
                    try { m_conn.Server.Close();   } catch { }
                    try { m_conn.Server.Dispose(); } catch { }                    
                }
                catch(System.Exception p_err) { 
                    #if UNITY_2017_1_OR_NEWER
                    UnityEngine.Debug.LogWarning($"MAVLinkTCP> Dispose Error \n {p_err.Message}");
                    #endif
                }
            }
            m_conn = null;

            base.OnDispose();

        }

    }
}
