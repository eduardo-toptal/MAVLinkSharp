
using MAVLinkBindings;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using static MAVLink;

namespace MAVLinkExamples {
    internal class MAVLinkExamples {

        #region class MAVLinkMsgQueue<T>
        /// <summary>
        /// Base Class for mavlink message queue receiving from different sources.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public class MAVLinkMsgQueue<T> {

            /// <summary>
            /// Name ofthis queue
            /// </summary>
            public string name;

            /// <summary>
            /// Flag that tells logging will be done per message
            /// </summary>
            public bool logEnabled;

            /// <summary>
            /// Raw Packets
            /// </summary>
            public List<byte[]> packets;

            /// <summary>
            /// Message Queues
            /// </summary>            
            public List<T> queue;

            /// <summary>
            /// Reference to the stream
            /// </summary>
            protected MemoryStream rcv_ss;
            protected MemoryStream snd_ss;

            /// <summary>
            /// Internals
            /// </summary>
            private TcpClient m_tcp;
            private UdpClient m_udp;
            private bool m_is_polling;
            private byte[] m_buffer;
            private IPEndPoint m_any_ep;

            /// <summary>
            /// CTOR
            /// </summary>
            public MAVLinkMsgQueue(string p_name="") { 
                name = p_name;
                packets = new List<byte[]>(); 
                queue   = new List<T>();
                m_is_polling = false; 
                m_buffer = new byte[256];
                rcv_ss = new MemoryStream(280);
                snd_ss = new MemoryStream(1024*64);     
                m_any_ep = new IPEndPoint(IPAddress.Parse("0.0.0.0"),0);
            }
            
            /// <summary>
            /// Set this queue for a tcp client
            /// </summary>
            /// <param name="p_client"></param>
            public void SetClient(TcpClient p_client) { m_tcp = p_client; }
            
            /// <summary>
            /// Sets this queue for an udp client
            /// </summary>
            /// <param name="p_client"></param>
            public void SetClient(UdpClient p_client) { m_udp = p_client; }

            /// <summary>
            /// Adda Message to the Queue
            /// </summary>
            /// <param name="p_message"></param>
            public void AddMessage(T p_message) {
                lock(queue) queue.Add(p_message);
            }

            /// <summary>
            /// Clears the queue 
            /// </summary>
            public void Clear() {
                lock(queue) {
                    for(int i=0;i<queue.Count;i++) {
                        OnMessageClear(queue[i]);
                    }
                    queue.Clear();
                }
            }

            /// <summary>
            /// Handler for per message cleaning
            /// </summary>
            /// <param name="p_message"></param>
            virtual protected void OnMessageClear(T p_message) { }

            /// <summary>
            /// Poll for new packets
            /// </summary>
            public void Poll() {
                if(m_tcp==null) if(m_udp==null) return;
                PollTCP();
                PollUDP();
            }

            /// <summary>
            /// Adds a packet filling with the current buffer information
            /// </summary>
            /// <param name="p_length"></param>
            private void AddPacket(byte[] p_buffer,int p_length) {
                byte[] b   = p_buffer==null ? m_buffer : p_buffer;
                byte[] pkt = new byte[p_length];
                for(int i=0;i< pkt.Length;i++) pkt[i] = b[i];
                lock(packets)packets.Add(pkt);
            }

            /// <summary>
            /// Poll TCP Client
            /// </summary>
            private void PollTCP() {
                if(m_tcp==null)  return;
                if(m_is_polling) return;
                m_is_polling=true;
                ReceiveTCP();
            }

            /// <summary>
            /// Poll for UDP Packets
            /// </summary>
            private void PollUDP() {
                if(m_udp==null)  return;
                if(m_is_polling) return;
                m_is_polling=true;
                ReceiveUDP();
            }

            /// <summary>
            /// Async Receive Data
            /// </summary>
            private async void ReceiveTCP() {                
                int c = 0;
                try { c = await m_tcp.GetStream().ReadAsync(m_buffer); } catch(Exception) { }                
                if(c<=0) { m_is_polling=false; return; }
                AddPacket(null,c);
                m_is_polling=false;                
            }

            /// <summary>
            /// Async Read UDP Packets
            /// </summary>
            private async void ReceiveUDP() {                
                byte[] b = null;
                int c=0;                                
                UdpReceiveResult res;
                try { res = await m_udp.ReceiveAsync(); } catch(Exception) { m_is_polling = false; return; }
                b = res.Buffer;
                c = b==null ? 0 : b.Length;                
                if(c<=0) { m_is_polling = false; return; }                
                AddPacket(b,c);                
                m_is_polling=false;
            }

            /// <summary>
            /// Parses the existing packets and populate the queue.
            /// </summary>
            public void Parse() {
                lock(packets) {
                    if (packets.Count<=0) return;                    
                    lock(rcv_ss) {
                        rcv_ss.SetLength(0);
                        for (int i=0;i<packets.Count; i++) {
                            byte[] b = packets[i];                                                    
                            rcv_ss.Write(b);                            
                        }                   
                        rcv_ss.Position=0;
                        ParseMessages();
                    }
                    packets.Clear();
                }
                if(logEnabled)
                lock(queue) {
                    for(int i=0;i<queue.Count;i++) {
                        string log = PrintMessage(queue[i]);
                        Console.WriteLine($"[{name}] {log}");
                    }
                }
            }

            /// <summary>
            /// Creates the message for different mavlink libs
            /// </summary>
            virtual protected void ParseMessages() { }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="p_msg"></param>
            /// <returns></returns>
            virtual protected string PrintMessage(T p_msg)  { return ""; }

            /// <summary>
            /// Serializes the queue for sending.
            /// </summary>
            public void Serialize() {
                lock(queue) {
                    if(queue.Count<=0) return;
                    lock(snd_ss) {
                        snd_ss.SetLength(0);
                        for(int i=0;i<queue.Count;i++) {
                            T msg = queue[i];
                            SerializeMessage(msg);                        
                        }                    
                    }                    
                }
            }

            /// <summary>
            /// Sends this message queue to a given target
            /// </summary>
            /// <param name="p_target"></param>
            public void Send(TcpClient p_target) {
                TcpClient t = p_target;
                if(t==null) return; 
                lock(snd_ss) {
                    try { t.GetStream().Write(snd_ss.GetBuffer(),0,(int)snd_ss.Length); } catch(Exception) { }                                
                }                
            }

            /// <summary>
            /// Sends this message queue to a given target
            /// </summary>
            /// <param name="p_target"></param>
            public void Send(UdpClient p_target) {
                UdpClient t = p_target;
                if(t==null) return; 
                lock(snd_ss) {
                    snd_ss.Position=0;                
                    try { t.Send(snd_ss.GetBuffer(),(int)snd_ss.Length); } catch(Exception) { }                
                }                
            }

            public void SendPackets(UdpClient p_target) {
                UdpClient t = p_target;
                if(t==null) return; 
                lock(packets) {
                    for(int i=0;i<packets.Count;i++) {
                        byte[] b = packets[i];
                        try { t.Send(b,b.Length); } catch(Exception) { }
                    }
                    packets.Clear();
                }
            }

            public void SendPackets(TcpClient p_target) {
                TcpClient t = p_target;
                if(t==null) return; 
                lock(packets) {
                    for(int i=0;i<packets.Count;i++) {
                        byte[] b = packets[i];
                        try { t.GetStream().Write(b,0,b.Length); } catch(Exception) { }                                        
                    }
                    packets.Clear();
                }
            }

            /// <summary>
            /// Per message serialization
            /// </summary>
            /// <param name="p_msg"></param>
            virtual protected void SerializeMessage(T p_msg) { }

        }

        #endregion

        #region class MAVLinkMsgQueueNew
        /// <summary>
        /// Extension to queue messages based on the new system
        /// </summary>
        public class MAVLinkMsgQueueNew : MAVLinkMsgQueue<MAVLinkMsg> {

            /// <summary>
            /// Reference to the reader.
            /// </summary>
            protected MAVLinkReader reader;
            protected MAVLinkWriter writer;

            public MAVLinkMsgQueueNew(string p_name="") : base(p_name) {
                reader = new MAVLinkReader(rcv_ss);
                writer = new MAVLinkWriter(snd_ss);
            }

            protected override void ParseMessages() {
                bool will_read = true;
                while(will_read) {                    
                    MAVLinkMsg msg = MAVLinkMsg.GetPool();
                    MAVLinkParseResult res = MAVLinkParseResult.Unknown;
                    res = reader.Read(ref msg);                       
                    switch(res) {
                        case MAVLinkParseResult.Success:    lock(queue) queue.Add(msg); break;
                        case MAVLinkParseResult.NotFound: 
                        case MAVLinkParseResult.Incomplete: will_read=false; MAVLinkMsg.SetPool(msg); break;
                        case MAVLinkParseResult.BadCRC:     MAVLinkMsg.SetPool(msg); break;
                    }                                                    
                }
            }

            protected override void OnMessageClear(MAVLinkMsg p_message) {
                MAVLinkMsg.SetPool(p_message);
            }

            protected override void SerializeMessage(MAVLinkMsg p_msg) {                
                writer.WriteV2(p_msg);                                
            }

            protected override string PrintMessage(MAVLinkMsg p_msg) {
                return $"{p_msg.messageId}";
            }

        }
        #endregion

        #region class MAVLinkMsgQueueOld
        /// <summary>
        /// Extension to queue messages based on the new system
        /// </summary>
        public class MAVLinkMsgQueueOld : MAVLinkMsgQueue<MAVLinkMessage> {

            /// <summary>
            /// Reference to the reader.
            /// </summary>
            protected MavlinkParse parser;

            public MAVLinkMsgQueueOld(string p_name) : base(p_name) {
                parser = new MavlinkParse();
            }

            protected override void ParseMessages() {
                bool will_read = true;
                while(will_read) {
                    if(rcv_ss.Position>=rcv_ss.Length) break;
                    MAVLinkMessage msg = parser.ReadPacket(rcv_ss);
                    if(msg==null) break;
                    AddMessage(msg);
                }
            }

            protected override void SerializeMessage(MAVLinkMessage p_msg) {
                byte[] d = parser.GenerateMAVLinkPacket20((MSG_ID)p_msg.msgid,p_msg.data,false,p_msg.sysid,p_msg.compid,p_msg.seq);
                snd_ss.Write(d);
            }

            protected override string PrintMessage(MAVLinkMessage p_msg) {
                return $"{(MSG_ID)p_msg.msgid}";
            }

        }
        #endregion


        static void Main(string[] p_args) {

            MAVLinkCRC.Init();

            /*
            MAVLinkMsgQueueNew px4_mav_q = new MAVLinkMsgQueueNew("mav");
            MAVLinkMsgQueueNew px4_hil_q = new MAVLinkMsgQueueNew("hil");
            MAVLinkMsgQueueNew px4_udp_q = new MAVLinkMsgQueueNew("px4");
            MAVLinkMsgQueueNew gcs_udp_q = new MAVLinkMsgQueueNew("gcs");
            //*/

            
            MAVLinkMsgQueueOld px4_mav_q = new MAVLinkMsgQueueOld("mav");
            MAVLinkMsgQueueOld px4_hil_q = new MAVLinkMsgQueueOld("hil");
            MAVLinkMsgQueueOld px4_udp_q = new MAVLinkMsgQueueOld("px4");
            MAVLinkMsgQueueOld gcs_udp_q = new MAVLinkMsgQueueOld("gcs");
            //*/

            IPEndPoint any_ep = new IPEndPoint(IPAddress.Parse("0.0.0.0"),0);

            //px4_hil_q.logEnabled = true;
            //px4_udp_q.logEnabled = true;
            //gcs_udp_q.logEnabled = true;
            
            TcpClient   px4_hil = null;
            TcpListener px4_tcp_server = new TcpListener(IPAddress.Parse("0.0.0.0"),4560);            
            px4_tcp_server.Start();                        
            Action cb_px4_client = 
            async delegate() {
                Console.WriteLine($"MAVLink> Listening 0.0.0.0:4560");
                px4_hil = await px4_tcp_server.AcceptTcpClientAsync();
                px4_hil_q.SetClient(px4_hil);
                Console.WriteLine($"MAVLink> Client Connected / Local: {px4_hil.Client.LocalEndPoint} | Remote: {px4_hil.Client.RemoteEndPoint}");
            };
            cb_px4_client();
            
            //UdpClient gcs_udp = new UdpClient(20580);
            //gcs_udp.Connect(IPAddress.Parse($"192.168.0.101"),14561);
            
            UdpClient gcs_udp = new UdpClient(21570);
            gcs_udp.Connect(IPAddress.Parse($"192.168.0.101"),19570);
            
            UdpClient px4_udp = new UdpClient(14551);
            px4_udp.Connect(IPAddress.Parse($"172.28.62.125"),18570);

            px4_udp_q.SetClient(px4_udp);
            gcs_udp_q.SetClient(gcs_udp);

            Stopwatch app_clock = new Stopwatch();
            app_clock.Start();

            Stopwatch app_clock_dt = new Stopwatch();
            app_clock_dt.Start();

            float heartbeat_t=0f;
            float hil_t=0f;

            //Message Queues

            MavlinkParse mvlp = new MavlinkParse();
            byte[] msg_b;

            Thread thd_queues = new Thread(delegate() { 
                while(true) {
                    //Fetch Packets
                    px4_hil_q.Poll();
                    px4_udp_q.Poll();
                    gcs_udp_q.Poll();
                    //Parse Packets into Messages                    
                    px4_hil_q.Parse();
                    px4_udp_q.Parse();
                    gcs_udp_q.Parse();
                }
            });
            
            Thread thd_main = new Thread(delegate() { 
                
                while(true) {     

                    

                    ulong time_usec = (ulong)app_clock.Elapsed.TotalMicroseconds;
                    float dt = (float)app_clock_dt.Elapsed.TotalSeconds;
                    app_clock_dt.Restart();

                    heartbeat_t+=dt;
                    if(heartbeat_t>=1f) {
                        heartbeat_t=0f;                
                        
                        MAVLinkMsg msg = MAVLinkMsg.GetPool();
                        msg.systemId    = 1;
                        msg.componentId = 0;
                        msg.data = new HeartbeatData() { Autopilot = MAVAutopilotFlags.Px4, MavlinkVersion = 3 };
                        msg.messageId = (MAVLinkMsgId)msg.data.GetId();
                        px4_mav_q.AddMessage(msg);
                        //*/

                        /*
                        msg_b =
                        mvlp.GenerateMAVLinkPacket20(MSG_ID.HEARTBEAT,
                        new HEARTBEAT_MSG() { 
                            autopilot = (byte)MAVAutopilotFlags.Px4,
                            mavlink_version = 3
                        },false,1,0);
                        MAVLinkMessage msg = new MAVLinkMessage(msg_b);                                                
                        px4_mav_q.AddMessage(msg);
                        //*/
                    }

                    hil_t+=dt;
                    if(hil_t>=0.1f) {
                        hil_t=0f;

                        
                        MAVLinkMsg msg;
                    
                        msg = MAVLinkMsg.GetPool();                    
                        msg.systemId    = 1;
                        msg.componentId = 0;
                        msg.data = new HilStateQuaternionData() {
                           TimeUsec = time_usec,
                           Alt = 98400,
                           AttitudeQuaternion = new float[] { 1,0,0,0 },
                           Lat = 325026457,
                           Lon = -837504386,
                           Vx  = 0,
                           Vy  = 0,
                           Vz  = 0,
                        };
                        msg.messageId = (MAVLinkMsgId)msg.data.GetId();
                        px4_mav_q.AddMessage(msg);

                        msg = MAVLinkMsg.GetPool();
                        msg.systemId    = 1;
                        msg.componentId = 0;
                        msg.data = new HilSensorData() {
                           TimeUsec = time_usec,                                    
                        };
                        msg.messageId = (MAVLinkMsgId)msg.data.GetId();
                        px4_mav_q.AddMessage(msg);

                        msg = MAVLinkMsg.GetPool();
                        msg.systemId    = 1;
                        msg.componentId = 0;
                        msg.data = new HilGpsData() {
                           TimeUsec = time_usec,                       
                           FixType = 3,
                           SatellitesVisible = 10,
                        };
                        msg.messageId = (MAVLinkMsgId)msg.data.GetId();
                        px4_mav_q.AddMessage(msg);
                        //*/

                        /*
                        MAVLinkMessage msg;

                        msg_b =
                        mvlp.GenerateMAVLinkPacket20(MSG_ID.HIL_STATE_QUATERNION,
                        new HIL_STATE_QUATERNION_MSG() { 
                           time_usec = time_usec,
                           alt = 98400,
                           attitude_quaternion = new float[] { 1,0,0,0 },
                           lat = 325026457,
                           lon = -837504386,
                           vx  = 0,
                           vy  = 0,
                           vz  = 0,
                        },false,1,0);                        
                        msg = new MAVLinkMessage(msg_b);
                        px4_mav_q.AddMessage(msg);

                        msg_b =
                        mvlp.GenerateMAVLinkPacket20(MSG_ID.HIL_SENSOR,
                        new HIL_SENSOR_MSG() { 
                           time_usec = time_usec,                           
                        },false,1,0);                        
                        msg = new MAVLinkMessage(msg_b);
                        px4_mav_q.AddMessage(msg);

                        msg_b =
                        mvlp.GenerateMAVLinkPacket20(MSG_ID.HIL_GPS,
                        new HIL_GPS_MSG() { 
                           time_usec = time_usec,       
                           fix_type = 3,
                           satellites_visible = 10,
                        },false,1,0);                        
                        msg = new MAVLinkMessage(msg_b);
                        px4_mav_q.AddMessage(msg);
                        //*/
                    }
                    

                    if(px4_hil!=null) {
                        px4_mav_q.Serialize();
                        px4_mav_q.Send(px4_hil);
                        //px4_mav_q.Send(gcs_udp);
                    }
                
                
                    px4_udp_q.Serialize();
                    px4_udp_q.Send(gcs_udp);
                    //px4_udp_q.SendPackets(gcs_udp);
                
                    gcs_udp_q.Serialize();
                    gcs_udp_q.Send(px4_udp);
                    //gcs_udp_q.SendPackets(px4_udp);

                    if(px4_hil!=null) px4_mav_q.Clear();
                    px4_udp_q.Clear();
                    gcs_udp_q.Clear();
                    //*/
                    //await Task.Delay(5); 
                    Thread.Sleep(5);
                }
                
            });
            thd_main.Start();

            thd_main.Join();



        }
    }
}
