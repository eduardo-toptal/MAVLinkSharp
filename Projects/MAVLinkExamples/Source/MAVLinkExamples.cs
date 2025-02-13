
using MAVLinkBindings;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using static MAVLink;

namespace MAVLinkExamples {
    internal class MAVLinkExamples {

        public enum ExampleState {
            Initialize,
            UpdatePX4TCP,
            UpdatePX4UDP,
            UpdateGCSUDP
        }

        static void Main(string[] p_args) {

            MAVLinkCRC.Init();
            
            TcpClient   px4_hil = null;
            TcpListener px4_tcp_server = new TcpListener(IPAddress.Parse("0.0.0.0"),4560);            
            px4_tcp_server.Start();                        
            Action cb_px4_client = 
            async delegate() {
                Console.WriteLine($"MAVLink> Listening 0.0.0.0:4560");
                px4_hil = await px4_tcp_server.AcceptTcpClientAsync();
                Console.WriteLine($"MAVLink> Client Conected / Local: {px4_hil.Client.LocalEndPoint} | Remote: {px4_hil.Client.RemoteEndPoint}");
            };
            cb_px4_client();
            
            //UdpClient gcs_udp = new UdpClient(20580);
            //gcs_udp.Connect(IPAddress.Parse($"192.168.0.101"),14561);
            UdpClient gcs_udp = new UdpClient(21570);
            gcs_udp.Connect(IPAddress.Parse($"192.168.0.101"),19570);
            UdpClient px4_udp = new UdpClient(14551);
            px4_udp.Connect(IPAddress.Parse($"172.28.62.125"),18570);

            MemoryStream  mvw_s = new MemoryStream();
            MAVLinkWriter mvw   = new MAVLinkWriter(mvw_s);

            List<MAVLinkMsg> px4_hil_q     = new List<MAVLinkMsg>();
            List<MAVLinkMsg> px4_hil_snd_q = new List<MAVLinkMsg>();
            List<MAVLinkMsg> px4_udp_q     = new List<MAVLinkMsg>();
            List<MAVLinkMsg> gcs_udp_q     = new List<MAVLinkMsg>();

            Stopwatch app_clock = new Stopwatch();
            app_clock.Start();

            Stopwatch app_clock_dt = new Stopwatch();
            app_clock_dt.Start();

            float heartbeat_t=0f;
            float hil_t=0f;


            //Message Queues
            ThreadPool.QueueUserWorkItem(delegate(object? p_state) {
                
                MemoryStream  mvr_s = new MemoryStream();
                MAVLinkReader mvr   = new MAVLinkReader(mvr_s);
                
                byte[] px4_hil_buffer = new byte[ushort.MaxValue];
                
                bool px4_hil_tsk_active = false;                
                ValueTask<int> px4_hil_tsk = default;

                Task<UdpReceiveResult> tsk_udp      = null;
                Task<UdpReceiveResult> px4_udp_tsk  = null;
                Task<UdpReceiveResult> gcs_udp_tsk  = null;

                IPEndPoint any_ep = new IPEndPoint(IPAddress.Any,0);

                while(true) {
                    

                    int c;                    
                    MAVLinkMsg msg;

                    #region PX4 HIL Queue
                    //PX4 HIL Data                    
                    if(!px4_hil_tsk_active) { 
                        px4_hil_tsk_active = true; 
                        Action cb_px4_hil = 
                        async delegate() {
                            if(px4_hil==null) { px4_hil_tsk_active = false; return; }
                            int c = await px4_hil.GetStream().ReadAsync(px4_hil_buffer,0,px4_hil_buffer.Length); 
                            bool will_read = true;
                            if(c>0)                            
                            lock(mvr_s) {
                                mvr_s.SetLength(0);
                                mvr_s.Write(px4_hil_buffer,0, c);
                                mvr_s.Position=0;
                                will_read = true;
                                while(will_read) {
                                    msg = MAVLinkMsg.GetPool();
                                    switch(mvr.Read(ref msg)) {
                                        case MAVLinkParseResult.Success:    lock(px4_hil_q) px4_hil_q.Add(msg); break;
                                        case MAVLinkParseResult.NotFound: 
                                        case MAVLinkParseResult.Incomplete: will_read=false; MAVLinkMsg.SetPool(msg); break;
                                        case MAVLinkParseResult.BadCRC:     MAVLinkMsg.SetPool(msg); break;
                                    }                                
                                }   
                            }                            
                            px4_hil_tsk_active = false;
                        };                        
                        cb_px4_hil();                        
                    }
                    #endregion

                    #region PX4 UDP Queue
                    //PX4 UDP Data                  
                    if(px4_udp_tsk==null) { 
                        Action cb_px4_udp = 
                        async delegate() {
                            px4_udp_tsk = px4_udp.ReceiveAsync();
                            try {  await px4_udp_tsk; } catch(Exception) { px4_udp_tsk = null; }
                            if(px4_udp_tsk==null) return;
                            bool is_error     = px4_udp_tsk.IsFaulted   || px4_udp_tsk.IsCanceled;
                            bool is_completed = px4_udp_tsk.IsCompleted || px4_udp_tsk.IsCompletedSuccessfully || is_error;
                            bool will_read = true;
                            int c = is_error ? 0 : px4_udp_tsk.Result.Buffer.Length;
                            if(c>0)
                            lock(mvr_s) {
                                mvr_s.SetLength(0);
                                mvr_s.Write(px4_udp_tsk.Result.Buffer,0, c);
                                mvr_s.Position=0;
                                will_read = true;
                                while(will_read) {
                                    msg = MAVLinkMsg.GetPool();
                                    switch(mvr.Read(ref msg)) {
                                        case MAVLinkParseResult.Success:    lock(px4_udp_q) px4_udp_q.Add(msg); break;
                                        case MAVLinkParseResult.NotFound: 
                                        case MAVLinkParseResult.Incomplete: will_read=false; MAVLinkMsg.SetPool(msg); break;
                                        case MAVLinkParseResult.BadCRC:     MAVLinkMsg.SetPool(msg); break;
                                    }                                
                                }   
                            }                            
                            px4_udp_tsk = null;
                        };
                        cb_px4_udp();                        
                    }
                    #endregion

                    #region GCS UDP Queue
                    //GCS UDP Data                  
                    if(gcs_udp_tsk==null) { 
                        Action cb_gcs_udp = 
                        async delegate() {
                            gcs_udp_tsk = gcs_udp.ReceiveAsync();
                            try {  await gcs_udp_tsk; } catch(Exception) { gcs_udp_tsk=null; }
                            if(gcs_udp_tsk==null) return;
                            bool is_error     = gcs_udp_tsk.IsFaulted   || gcs_udp_tsk.IsCanceled;
                            bool is_completed = gcs_udp_tsk.IsCompleted || gcs_udp_tsk.IsCompletedSuccessfully || is_error;
                            bool will_read = true;
                            int c = is_error ? 0 : gcs_udp_tsk.Result.Buffer.Length;
                            if(c>0)
                            lock(mvr_s) {
                                mvr_s.SetLength(0);
                                mvr_s.Write(gcs_udp_tsk.Result.Buffer,0, c);
                                mvr_s.Position=0;
                                will_read = true;
                                while(will_read) {
                                    msg = MAVLinkMsg.GetPool();
                                    switch(mvr.Read(ref msg)) {
                                        case MAVLinkParseResult.Success:    lock(gcs_udp_q) gcs_udp_q.Add(msg); break;
                                        case MAVLinkParseResult.NotFound: 
                                        case MAVLinkParseResult.Incomplete: will_read=false; MAVLinkMsg.SetPool(msg); break;
                                        case MAVLinkParseResult.BadCRC:     MAVLinkMsg.SetPool(msg); break;
                                    }                                
                                }   
                            }
                            gcs_udp_tsk = null;
                        };                        
                        cb_gcs_udp();                        
                    }
                    #endregion

                    Thread.Sleep(1);
                }
            });

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
                    px4_hil_snd_q.Add(msg);
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
                    px4_hil_snd_q.Add(msg);

                    msg = MAVLinkMsg.GetPool();
                    msg.systemId    = 1;
                    msg.componentId = 0;
                    msg.data = new HilSensorData() {
                       TimeUsec = time_usec,                                    
                    };
                    msg.messageId = (MAVLinkMsgId)msg.data.GetId();
                    px4_hil_snd_q.Add(msg);

                    msg = MAVLinkMsg.GetPool();
                    msg.systemId    = 1;
                    msg.componentId = 0;
                    msg.data = new HilGpsData() {
                       TimeUsec = time_usec,                       
                       FixType = 3,
                       SatellitesVisible = 10,
                    };
                    msg.messageId = (MAVLinkMsgId)msg.data.GetId();
                    px4_hil_snd_q.Add(msg);
                }

                List<MAVLinkMsg> msg_q;
                //string msg_log = "";

                msg_q = px4_hil_snd_q;
                lock(msg_q) {                    
                    if(msg_q.Count>0) {
                        mvw_s.SetLength(0);
                        //msg_log = "";                        
                        for(int i=0;i<msg_q.Count;i++) {
                            MAVLinkMsg msg = msg_q[i];
                            mvw.WriteV2(msg);
                            //msg_log += $" | {msg.messageId}";
                            MAVLinkMsg.SetPool(msg);
                        }
                        mvw_s.Position=0;          
                        if(px4_hil!=null) {
                        //lock(px4_hil) {
                            try { 
                                px4_hil.GetStream().Write(mvw_s.GetBuffer(),0,(int)mvw_s.Length);
                            }
                            catch(Exception err) {
                                Console.WriteLine($"[hil] ERROR / {err.Message}");
                            }
                        //}
                        }
                        gcs_udp.Send(mvw_s.GetBuffer(),(int)mvw_s.Length);
                        //Console.WriteLine($"[sim] -> [hil] {msg_log}");                        
                    }
                    msg_q.Clear();
                }
                
                msg_q = px4_hil_q;
                lock(msg_q) {                    
                    if(msg_q.Count>0) {
                        mvw_s.SetLength(0);
                        //msg_log = "";
                        for(int i=0;i<msg_q.Count;i++) {
                            MAVLinkMsg msg = msg_q[i];
                            mvw.WriteV2(msg);
                            //msg_log += $" | {msg.messageId}";
                            MAVLinkMsg.SetPool(msg);
                        }
                        mvw_s.Position=0;
                        gcs_udp.Send(mvw_s.GetBuffer(),(int)mvw_s.Length);
                        //Console.WriteLine($"[hil] -> [gcs] messages{msg_log}");                        
                    }
                    msg_q.Clear();
                }

                msg_q = px4_udp_q;
                lock(msg_q) {
                    if(msg_q.Count>0) {
                        mvw_s.SetLength(0);
                        //msg_log = "";
                        for(int i=0;i<msg_q.Count;i++) {
                            MAVLinkMsg msg = msg_q[i];
                            mvw.WriteV2(msg);
                            //msg_log += $" | {msg.messageId}";
                            MAVLinkMsg.SetPool(msg);
                        }
                        mvw_s.Position=0;
                        gcs_udp.Send(mvw_s.GetBuffer(),(int)mvw_s.Length);
                        //Console.WriteLine($"[px4] -> [gcs] {msg_log}");                        
                    }
                    msg_q.Clear();
                }

                msg_q = gcs_udp_q;
                lock(msg_q) {
                    if(msg_q.Count>0) {
                        mvw_s.SetLength(0);
                        //msg_log = "";
                        for(int i=0;i<msg_q.Count;i++) {
                            MAVLinkMsg msg = msg_q[i];
                            mvw.WriteV2(msg);
                            //msg_log += $" | {msg.messageId}";
                            MAVLinkMsg.SetPool(msg);
                        }
                        mvw_s.Position=0;
                        px4_udp.Send(mvw_s.GetBuffer(),(int)mvw_s.Length);
                        //Console.WriteLine($"[gcs] -> [px4] {msg_log}");                        
                    }
                    msg_q.Clear();
                }
                
                Thread.Sleep(1);
            }

            

        }
    }
}
