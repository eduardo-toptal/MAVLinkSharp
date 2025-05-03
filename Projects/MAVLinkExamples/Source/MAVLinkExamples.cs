
using MAVLinkSharp.Bindings;
using MAVLinkSharp.Runtime;
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
            /// Handler for incoming messages
            /// </summary>
            public Action<T> OnMessageEvent;

            /// <summary>
            /// Message Queues
            /// </summary>            
            public List<T> m_msg_in_q;
            public List<T> m_msg_out_q;

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
                m_msg_in_q    = new List<T>();
                m_msg_out_q   = new List<T>();
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
            public void SendMessage(T p_message) {
                lock(snd_ss) {
                    SerializeMessage(p_message);                 
                } 
            }

            /// <summary>
            /// Flushes outgoing messages
            /// </summary>
            public void Flush() {
                UdpClient t_udp = m_udp;
                TcpClient t_tcp = m_tcp;

                int    b_len = 0;
                byte[] b     = snd_ss.GetBuffer();

                lock(snd_ss) {
                    b_len = (int)snd_ss.Position;
                    if(b_len<=0) return;
                }
                
                if(t_udp!=null) 
                lock(snd_ss) {                                                           
                    try { t_udp.Send(b,b_len); } catch(Exception) { }                
                }

                if(t_tcp!=null) 
                lock(snd_ss) {                    
                    try { t_tcp.GetStream().Write(b,0,b_len); } catch(Exception) { }                                
                } 
                
                lock(snd_ss) {
                    snd_ss.Position=0;
                }

            }

            /// <summary>
            /// Handler for per message cleaning
            /// </summary>
            /// <param name="p_message"></param>
            virtual protected void OnMessageClear(T p_message) { }

            virtual protected void OnMessage(T p_message) { }

            /// <summary>
            /// Returns a flag telling this queue is active 
            /// </summary>
            /// <returns></returns>
            public bool IsActive() {
                if(packets.Count    > 0) return true;
                if(snd_ss.Position  > 0) return true;
                //if(m_msg_in_q.Count > 0) return true;
                return false;
            }

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
                //lock(packets)
                    packets.Add(pkt);
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
                try {                     
                    c = await m_tcp.GetStream().ReadAsync(m_buffer);                     
                } catch(Exception) { }                
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
                List<byte[]> pkt_l = null;
                //lock(packets) {
                    if (packets.Count<=0) return;                    
                    pkt_l = new List<byte[]>(packets);
                    packets.Clear();
                //}

                //lock(rcv_ss) {
                    rcv_ss.SetLength(0);
                    for (int i=0;i<pkt_l.Count; i++) {
                        byte[] b = pkt_l[i];                                                    
                        rcv_ss.Write(b);                            
                    }                   
                    rcv_ss.Position=0;                    
                    ParseMessages();
                //}               

            }

            /// <summary>
            /// Fetch messages in the queue and them over the callback handlers.
            /// </summary>
            public void Dispatch() {
                List<T> q = m_msg_in_q;
                //lock(q) {
                    if(q.Count<=0) return;
                    for(int  i=0;i<q.Count;i++) {
                        T m = q[i];
                        if(logEnabled) {
                            string log = PrintMessage(m);
                            if(!string.IsNullOrEmpty(log))Console.WriteLine($"[{name}] {log}");
                        }
                        if(OnMessageEvent!=null) OnMessageEvent(m);
                    }
                    q.Clear();
                //}
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
            /// List of ids of messages to not log
            /// </summary>
            public List<MAVLinkMsgId> logIgnore;

            /// <summary>
            /// Reference to the reader.
            /// </summary>
            protected MAVLinkReader reader;
            protected MAVLinkWriter writer;

            public MAVLinkMsgQueueNew(string p_name="") : base(p_name) {
                reader    = new MAVLinkReader(rcv_ss);
                writer    = new MAVLinkWriter(snd_ss);
                logIgnore = new List<MAVLinkMsgId>();
            }

            protected override void ParseMessages() {
                bool will_read = true;
                while(will_read) {                    
                    MAVLinkMsg msg = MAVLinkMsg.GetPool();
                    MAVLinkParseResult res = MAVLinkParseResult.Unknown;
                    res = reader.Read(ref msg);  
                    List<MAVLinkMsg> q = m_msg_in_q;
                    switch(res) {
                        case MAVLinkParseResult.Success:    lock(q) q.Add(msg); break;
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
                if(logIgnore.Contains(p_msg.messageId)) return "";
                switch(p_msg.messageId) {
                    case MAVLinkMsgId.Heartbeat: {
                        HeartbeatData d = (HeartbeatData)p_msg.data;                        
                        return $"{p_msg.messageId} | type: {d.Type} sys:{p_msg.systemId} | comp: {p_msg.componentId} | autopilot: {d.Autopilot} v{d.MavlinkVersion}";
                    }
                    break;

                    case MAVLinkMsgId.CommandLong: {
                        CommandLongData d = (CommandLongData)p_msg.data;
                        return $"{p_msg.messageId} | type: {d.Command} sys:{p_msg.systemId} | comp: {p_msg.componentId}";
                    }
                    break;

                    case MAVLinkMsgId.ParamValue: {
                        ParamValueData d = (ParamValueData)p_msg.data;
                        return $"{p_msg.messageId} | id: {new string(d.ParamId).TrimEnd('\0').PadRight(24)} type: {d.ParamType.ToString().PadRight(12)} v: {d.ParamValue.ToString().PadRight(8)} sys:{p_msg.systemId} | comp: {p_msg.componentId}";
                    }
                    break;

                }
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
            /// List of ids of messages to not log
            /// </summary>
            public List<MSG_ID> logIgnore;

            /// <summary>
            /// Reference to the reader.
            /// </summary>
            protected MavlinkParse parser;

            public MAVLinkMsgQueueOld(string p_name) : base(p_name) {
                parser = new MavlinkParse();
                logIgnore = new List<MSG_ID>();
            }

            protected override void ParseMessages() {
                bool will_read = true;
                while(will_read) {
                    if(rcv_ss.Position>=rcv_ss.Length) break;
                    MAVLinkMessage msg = parser.ReadPacket(rcv_ss);
                    if(msg==null) break;
                    List<MAVLinkMessage> q = m_msg_in_q;
                    lock(q)q.Add(msg);
                }
            }

            protected override void SerializeMessage(MAVLinkMessage p_msg) {
                byte[] d = parser.GenerateMAVLinkPacket20((MSG_ID)p_msg.msgid,p_msg.data,false,p_msg.sysid,p_msg.compid,p_msg.seq);
                snd_ss.Write(d);
            }

            protected override string PrintMessage(MAVLinkMessage p_msg) {
                if(logIgnore.Contains((MSG_ID)p_msg.msgid)) return "";
                switch((MSG_ID)p_msg.msgid) {
                    case MSG_ID.HEARTBEAT: {
                        HEARTBEAT_MSG d = (HEARTBEAT_MSG)p_msg.data;                        
                        return $"{p_msg.msgid} | type: {d.type} sys:{p_msg.sysid} | comp: {p_msg.compid} | autopilot: {d.autopilot} v{d.mavlink_version}";
                    }
                    break;
                }
                return $"{(MSG_ID)p_msg.msgid}";
            }

        }
        #endregion


        static void Main(string[] p_args) {

            IPEndPoint any_ep = new IPEndPoint(IPAddress.Parse("0.0.0.0"),0);

            

            //MAVLinkCRC.Init();

            /*
            MAVLinkMsgQueueNew px4_mav_q = new MAVLinkMsgQueueNew("mav");
            MAVLinkMsgQueueNew px4_hil_q = new MAVLinkMsgQueueNew("hil");
            MAVLinkMsgQueueNew px4_udp_q = new MAVLinkMsgQueueNew("px4");
            MAVLinkMsgQueueNew gcs_udp_q = new MAVLinkMsgQueueNew("gcs");

            px4_hil_q.logIgnore = new List<MAVLinkMsgId>() { MAVLinkMsgId.HilActuatorControls };

            px4_udp_q.logIgnore = new List<MAVLinkMsgId>() { 
                MAVLinkMsgId.GpsRawInt,
                MAVLinkMsgId.AttitudeQuaternion,
                MAVLinkMsgId.LocalPositionNed,
                MAVLinkMsgId.GlobalPositionInt,
                MAVLinkMsgId.VfrHud,
                MAVLinkMsgId.ActuatorControlTarget,
                MAVLinkMsgId.Altitude,
                MAVLinkMsgId.Attitude,
                MAVLinkMsgId.AttitudeTarget,
                MAVLinkMsgId.BatteryStatus,
                MAVLinkMsgId.DistanceSensor,
                MAVLinkMsgId.ExtendedSysState,
                MAVLinkMsgId.Gps2Raw,
                MAVLinkMsgId.PositionTargetLocalNed,
                MAVLinkMsgId.ScaledImu,
                MAVLinkMsgId.SmartBatteryInfo,
                MAVLinkMsgId.SysStatus,
                MAVLinkMsgId.Timesync,
                MAVLinkMsgId.UtmGlobalPosition,
                MAVLinkMsgId.EscInfo,
                MAVLinkMsgId.HomePosition,
                MAVLinkMsgId.LinkNodeStatus,
                MAVLinkMsgId.Ping,
                MAVLinkMsgId.SystemTime,
            };

            gcs_udp_q.logIgnore = new List<MAVLinkMsgId>() { 
                MAVLinkMsgId.ManualControl,                
            };
            //*/

            /*
            MAVLinkMsgQueueOld px4_mav_q = new MAVLinkMsgQueueOld("mav");
            MAVLinkMsgQueueOld px4_hil_q = new MAVLinkMsgQueueOld("hil");
            MAVLinkMsgQueueOld px4_udp_q = new MAVLinkMsgQueueOld("px4");
            MAVLinkMsgQueueOld gcs_udp_q = new MAVLinkMsgQueueOld("gcs");
            px4_hil_q.logIgnore = new List<MSG_ID>() { MSG_ID.HIL_ACTUATOR_CONTROLS };
            //*/

            /*
            

            px4_hil_q.logEnabled = false;
            px4_udp_q.logEnabled = false;
            gcs_udp_q.logEnabled = false;

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
            
            
            
            UdpClient gcs_udp = new UdpClient(21570);
            gcs_udp.Connect(IPAddress.Parse($"192.168.0.100"),19570);
            
            UdpClient px4_udp = new UdpClient(14551);
            px4_udp.Connect(IPAddress.Parse($"172.28.62.125"),20570);
            //*/

            /*
            ThreadPool.QueueUserWorkItem(delegate(object? so) { 
               IPEndPoint any_ep = new IPEndPoint(IPAddress.Parse("0.0.0.0"),0);
               while(true) {
                    byte[] b = null;
                    try { b = gcs_udp.Receive(ref any_ep); } catch(Exception){ }
                    if(b==null) continue;
                    px4_udp.Send(b,b.Length);                    
                }               
            });

            ThreadPool.QueueUserWorkItem(delegate(object? so) { 
               IPEndPoint any_ep = new IPEndPoint(IPAddress.Parse("0.0.0.0"),0);
               while(true) {
                    byte[] b = null;
                    try { b = px4_udp.Receive(ref any_ep); } catch(Exception){ }
                    if(b==null) continue;
                    gcs_udp.Send(b,b.Length);                    
                }               
            });
            //*/

            Stopwatch app_clock = new Stopwatch();
            app_clock.Start();

            Stopwatch app_clock_dt = new Stopwatch();
            app_clock_dt.Start();

            float heartbeat_t=0f;
            float hil_t=0f;

            /*
            px4_udp_q.SetClient(px4_udp);
            gcs_udp_q.SetClient(gcs_udp);

            //Message Queues

            MavlinkParse mvlp = new MavlinkParse();
            byte[] msg_b;

            Thread thd_queues = new Thread(delegate() { 
                while(true) {

                    //Fetch Packets
                    px4_hil_q.Poll();
                    px4_udp_q.Poll();
                    gcs_udp_q.Poll();

                    bool is_active = px4_hil_q.IsActive() || px4_udp_q.IsActive() || gcs_udp_q.IsActive();

                    //Parse Packets into Messages                    
                    px4_hil_q.Parse();
                    px4_udp_q.Parse();
                    gcs_udp_q.Parse();

                    if(!is_active) {                         
                        Thread.Sleep(20); 
                    }

                    //Dispatch Messages to Handlers
                    px4_hil_q.Dispatch();
                    px4_udp_q.Dispatch();
                    gcs_udp_q.Dispatch();

                    //Flush Messages Buffered to be Sent
                    px4_hil_q.Flush();
                    px4_udp_q.Flush();
                    gcs_udp_q.Flush();

                    Thread.Yield();
                }
            });
            thd_queues.Name = "POLLING";
            thd_queues.Start();
            //*/
            
            Thread thd_main = new Thread(delegate() { 

                MAVLinkNetwork mvl_network = new MAVLinkNetwork("mavlink");

                MAVLinkTCP px4_hil = new MAVLinkTCP("hil");
                MAVLinkUDP px4_udp = new MAVLinkUDP("px4");
                MAVLinkUDP gcs_udp = new MAVLinkUDP("gcs");

                MAVLinkNode node_router = new MAVLinkNode("router");

                
                px4_udp.Link(node_router);
                node_router.Link(gcs_udp);
                gcs_udp.Link(node_router);
                node_router.Link(px4_udp);

                //px4_udp.Link(gcs_udp);                
                //gcs_udp.Link(px4_udp);

                //px4_udp.next = gcs_udp;
                //gcs_udp.next = px4_udp;
                

                //*/

                px4_hil.Start(4560);

                px4_udp.Start(14551);
                px4_udp.SetTargets(new IPEndPoint(IPAddress.Parse($"172.28.62.125"),20570));
                
                gcs_udp.Start(21570);
                gcs_udp.SetTargets(new IPEndPoint(IPAddress.Parse($"192.168.0.100"),19570));

                /*
                px4_udp.OnMessageReceived = 
                delegate(MAVLinkMsg p_msg) {
                    gcs_udp.Send(p_msg);
                };

                gcs_udp.OnMessageReceived = 
                delegate(MAVLinkMsg p_msg) {
                    px4_udp.Send(p_msg);
                };
                //*/

                mvl_network.Start();

                /*
                UdpClient gcs_udp_cl = new UdpClient(21570);
                gcs_udp_cl.Connect(IPAddress.Parse($"192.168.0.100"),19570);

                UdpClient px4_udp_cl = new UdpClient(14551);
                px4_udp_cl.Connect(IPAddress.Parse($"172.28.62.125"),20570);

                ThreadPool.QueueUserWorkItem(delegate(object? so){ 
                    while(true) {
                        byte[] d = null;
                        try { d = gcs_udp_cl.Receive(ref any_ep); } catch(Exception) {}                        
                        if(d==null) continue;
                        px4_udp_cl.Send(d);
                        Thread.Yield();
                    }                        
                });

                ThreadPool.QueueUserWorkItem(delegate(object? so){ 
                    while(true) {
                        byte[] d = null;
                        try { d = px4_udp_cl.Receive(ref any_ep); } catch(Exception) {}                        
                        if(d==null) continue;
                        gcs_udp_cl.Send(d);
                        Thread.Yield();
                    }                        
                });
                //*/

                /*
                px4_hil_q.OnMessageEvent = 
                delegate(MAVLinkMessage msg) {
                    
                };
                //*/

                /*
                px4_udp_q.OnMessageEvent = 
                delegate(MAVLinkMessage msg) {                    
                    gcs_udp_q.SendMessage(msg);
                };
                //*/

                /*
                gcs_udp_q.OnMessageEvent = 
                delegate(MAVLinkMessage msg) {     

                    px4_udp_q.SendMessage(msg);
                    
                    switch((MSG_ID)msg.msgid) {
                        case MSG_ID.COMMAND_LONG: {

                            COMMAND_LONG_MSG msg_d   = (COMMAND_LONG_MSG)msg.data;
                            MAV_CMD          msg_cmd = (MAV_CMD)msg_d.command;

                            switch(msg_cmd) {

                                case MAV_CMD.REQUEST_AUTOPILOT_CAPABILITIES: {                             

                                    byte          sid = msg_d.target_system;
                                    MAV_COMPONENT cid = (MAV_COMPONENT)msg_d.target_component;
                                    float[]         p = new float[] { msg_d.param1, msg_d.param2, msg_d.param3, msg_d.param4, msg_d.param5, msg_d.param6, msg_d.param7 };
                            
                                    switch(cid) {
                                        case MAV_COMPONENT.MAV_COMP_ID_VISUAL_INERTIAL_ODOMETRY: {

                                            byte  ack_cid = msg_d.target_component;
                                            byte  ack_sid = msg_d.target_system;      
                                            msg_b = 
                                            mvlp.GenerateMAVLinkPacket20(MSG_ID.COMMAND_ACK,
                                            new COMMAND_ACK_MSG() { 
                                                command          = (ushort)msg_cmd,
                                                result           = (byte)MAV_RESULT.ACCEPTED,
                                                target_component = ack_cid,
                                                target_system    = ack_sid
                                            },false,ack_sid,ack_cid);
                                            MAVLinkMessage cmd_ack = new MAVLinkMessage(msg_b);
                                            gcs_udp_q.SendMessage(cmd_ack);
                                            
                                            AUTOPILOT_VERSION_MSG d = new AUTOPILOT_VERSION_MSG();

                                            byte[] sb;
                                            byte[] nb;

                                            sb = UTF8Encoding.UTF8.GetBytes("1.10"); nb = new byte[8]; for(int i=0;i<sb.Length;i++) { if(i>=nb.Length) break; nb[i]=sb[i]; }
                                            d.flight_custom_version = nb;

                                            d.middleware_custom_version = new byte[] { 0xff,0,0,3,0,0,0,0,0 };

                                            sb = UTF8Encoding.UTF8.GetBytes("1.10"); nb = new byte[8]; for(int i=0;i<sb.Length;i++) { if(i>=nb.Length) break; nb[i]=sb[i]; }
                                            d.os_custom_version = nb;

                                            nb = new byte[18]; for(int i=0;i<nb.Length;i++) nb[i]=0;
                                            d.uid2 = nb;

                                            msg_b =
                                            mvlp.GenerateMAVLinkPacket20(MSG_ID.AUTOPILOT_VERSION,d,false,ack_sid,ack_cid);
                                            MAVLinkMessage auto_pilot_msg = new MAVLinkMessage(msg_b);
                                            gcs_udp_q.SendMessage(auto_pilot_msg);
                                        }
                                        break;
                                    }
                                }
                                break;
                                
                            }
                    
                        }
                        break;
                    }

                    
                };
                //*/

                
                /*
                px4_hil_q.OnMessageEvent = 
                delegate(MAVLinkMsg msg) {
                    
                };
                //*/

                /*
                px4_udp_q.OnMessageEvent = 
                delegate(MAVLinkMsg msg) {                    
                    gcs_udp_q.SendMessage(msg);
                };
                //*/

                Stopwatch hil_ac_clk = new Stopwatch();
                hil_ac_clk.Start();
                int hil_act_c=0;

                //gcs_udp_q.OnMessageEvent = 
                //delegate(MAVLinkMsg msg) {     
                mvl_network.OnMessageEvent = 
                delegate(MAVLinkNode p_sender,MAVLinkMsg msg)  {

                    //Console.WriteLine($"[{p_sender.name}] {msg.messageId}");

                    //px4_udp_q.SendMessage(msg);


                    
                    switch(msg.messageId) {

                        case MAVLinkMsgId.HilActuatorControls: {
                            hil_act_c++;                            
                            if(hil_ac_clk.Elapsed.Seconds<1.0) break;
                            Console.WriteLine($"[{p_sender.name}] {msg.messageId} @ {hil_act_c}HZ");
                            hil_ac_clk.Restart();
                            hil_act_c=0;
                        }
                        break;

                        case MAVLinkMsgId.CommandLong: {
                            CommandLongData cmd_d = (CommandLongData)msg.data;
                            switch(cmd_d.Command) {
                                 case MAVCmdFlags.RequestAutopilotCapabilities: {
                                        
                                        byte              sid = cmd_d.TargetSystem;
                                        MAVComponentFlags cid = (MAVComponentFlags)cmd_d.TargetComponent;
                                        float[]           p = new float[] { cmd_d.Param1, cmd_d.Param2, cmd_d.Param3, cmd_d.Param4, cmd_d.Param5, cmd_d.Param6, cmd_d.Param7 };
                            
                                        //Console.WriteLine($"RequestAutopilotCapabilities / Sys[{sid}] Component[{cid}] Param[{string.Join(",",p)}]");

                                        switch(cid) {

                                            case MAVComponentFlags.MavCompIdVisualInertialOdometry: {

                                                    
                                                byte  ack_cid = cmd_d.TargetComponent;
                                                byte  ack_sid = cmd_d.TargetSystem;

                                                MAVLinkMsg cmd_ack = MAVLinkMsg.GetPool();

                                                cmd_ack.messageId   = MAVLinkMsgId.CommandAck;
                                                cmd_ack.systemId    = ack_sid;
                                                cmd_ack.componentId = ack_cid;
                                                cmd_ack.data = new CommandAckData() {
                                                    Command          = cmd_d.Command,
                                                    Result           = MAVResultFlags.Accepted,
                                                    TargetComponent  = ack_cid,
                                                    TargetSystem     = ack_sid
                                                };                                                
                                                
                                                //gcs_udp_q.SendMessage(cmd_ack);
                                                gcs_udp.Send(cmd_ack);

                                                MAVLinkMsg.SetPool(cmd_ack);
                                    
                                                AutopilotVersionData d = new AutopilotVersionData();

                                                d.Init();

                                                byte[] sb;
                                                byte[] nb;

                                                sb = UTF8Encoding.UTF8.GetBytes("1.10"); nb = new byte[8]; for(int j=0;j<sb.Length;j++) { if(j>=nb.Length) break; nb[j]=sb[j]; }
                                                d.FlightCustomVersion = nb;

                                                d.MiddlewareCustomVersion = new byte[] { 0xff,0,0,3,0,0,0,0,0 };

                                                sb = UTF8Encoding.UTF8.GetBytes("1.10"); nb = new byte[8]; for(int j = 0 ; j < sb.Length;j++) { if(j>=nb.Length) break; nb[j]=sb[j]; }
                                                d.OsCustomVersion = nb;

                                                nb = new byte[18]; for(int j=0;j<nb.Length;j++) nb[j]=0;
                                                d.Uid2 = nb;

                                                MAVLinkMsg auto_pilot_msg  = MAVLinkMsg.GetPool();
                                                auto_pilot_msg.messageId   = MAVLinkMsgId.AutopilotVersion;
                                                auto_pilot_msg.systemId    = ack_sid;
                                                auto_pilot_msg.componentId = ack_cid;
                                                auto_pilot_msg.data = d;

                                                //gcs_udp_q.SendMessage(auto_pilot_msg);      
                                                gcs_udp.Send(auto_pilot_msg.data,ack_sid,ack_cid);
                                            

                                                MAVLinkMsg.SetPool(auto_pilot_msg);

                                            }
                                            break;
                                        }

                                    }
                                    break;
                            }
                        }
                        break;
                    }

                    
                };
                //*/

                while(true) {     

                    ulong time_usec = (ulong)app_clock.Elapsed.Ticks/10;
                    float dt = (float)app_clock_dt.Elapsed.TotalSeconds;
                    app_clock_dt.Restart();

                    heartbeat_t+=dt;
                    if(heartbeat_t>=1f) {
                        heartbeat_t=0f;                
                        
                        px4_hil.Send(MAVLinkMsgId.Heartbeat,new HeartbeatData() { Autopilot = MAVAutopilotFlags.Px4 },1,0);

                        /*
                        MAVLinkMsg msg  = MAVLinkMsg.GetPool();
                        msg.systemId    = 1;
                        msg.componentId = 0;
                        msg.data        = new HeartbeatData() { Autopilot = MAVAutopilotFlags.Px4 };
                        msg.messageId   = (MAVLinkMsgId)msg.data.GetId();
                        px4_hil_q.SendMessage(msg);
                        //*/

                        /*
                        msg_b =
                        mvlp.GenerateMAVLinkPacket20(MSG_ID.HEARTBEAT,
                        new HEARTBEAT_MSG() { 
                            autopilot = (byte)MAVAutopilotFlags.Px4,
                            mavlink_version = 4
                        },false,1,0);
                        MAVLinkMessage msg = new MAVLinkMessage(msg_b);                                                
                        px4_hil_q.SendMessage(msg);
                        //*/
                    }

                    hil_t+=dt;
                    if(hil_t>=0.001f) {
                        hil_t=0f;

                        /*
                        px4_hil.Send(new HilStateQuaternionData() {
                           TimeUsec = time_usec,
                           Alt = 98400,
                           AttitudeQuaternion = new float[] { 1,0,0,0 },
                           Lat = 325026457,
                           Lon = -837504386,
                           Vx  = 0,
                           Vy  = 0,
                           Vz  = 0,
                        },1,0);

                        px4_hil.Send(new HilSensorData() {
                           TimeUsec = time_usec,                                    
                        },1,0);

                        px4_hil.Send(new HilGpsData() {
                           TimeUsec = time_usec,                       
                           FixType = 3,
                           SatellitesVisible = 10,
                        },1,0);
                        //*/

                        /*
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
                        px4_hil_q.SendMessage(msg);

                        MAVLinkMsg.SetPool(msg);  

                        msg = MAVLinkMsg.GetPool();
                        msg.systemId    = 1;
                        msg.componentId = 0;
                        msg.data = new HilSensorData() {
                           TimeUsec = time_usec,                                    
                        };
                        msg.messageId = (MAVLinkMsgId)msg.data.GetId();
                        px4_hil_q.SendMessage(msg);

                        MAVLinkMsg.SetPool(msg);

                        msg = MAVLinkMsg.GetPool();
                        msg.systemId    = 1;
                        msg.componentId = 0;
                        msg.data = new HilGpsData() {
                           TimeUsec = time_usec,                       
                           FixType = 3,
                           SatellitesVisible = 10,
                        };
                        msg.messageId = (MAVLinkMsgId)msg.data.GetId();
                        px4_hil_q.SendMessage(msg);

                        MAVLinkMsg.SetPool(msg);

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
                        px4_hil_q.SendMessage(msg);

                        msg_b =
                        mvlp.GenerateMAVLinkPacket20(MSG_ID.HIL_SENSOR,
                        new HIL_SENSOR_MSG() { 
                           time_usec = time_usec,                           
                        },false,1,0);                        
                        msg = new MAVLinkMessage(msg_b);
                        px4_hil_q.SendMessage(msg);

                        msg_b =
                        mvlp.GenerateMAVLinkPacket20(MSG_ID.HIL_GPS,
                        new HIL_GPS_MSG() { 
                           time_usec = time_usec,       
                           fix_type = 3,
                           satellites_visible = 10,
                        },false,1,0);                        
                        msg = new MAVLinkMessage(msg_b);
                        px4_hil_q.SendMessage(msg);
                        //*/
                    }

                    Thread.Yield();
                }
                
            });
            thd_main.Start();

            thd_main.Name = "MAIN";

            thd_main.Join();



        }
    }
}
