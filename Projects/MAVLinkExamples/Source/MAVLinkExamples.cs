
using MAVLinkBindings;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using static MAVLink;

namespace MAVLinkExamples {
    internal class MAVLinkExamples {

        static void Main(string[] p_args) {

            MAVLinkCRC.Init();
            
            TcpClient   px4_hil = null;
            TcpListener px4_tcp_server = new TcpListener(IPAddress.Parse("0.0.0.0"),4560);            
            px4_tcp_server.Start();            
            Console.WriteLine($"MAVLink> Listening 0.0.0.0:4560");
            px4_hil = px4_tcp_server.AcceptTcpClient();
            Console.WriteLine($"MAVLink> Client Conected / Local: {px4_hil.Client.LocalEndPoint} | Remote: {px4_hil.Client.RemoteEndPoint}");
            
            UdpClient gcs_udp = new UdpClient(20580);
            gcs_udp.Connect(IPAddress.Parse($"192.168.0.101"),14561);
            //UdpClient gcs_udp = new UdpClient(21570);
            //gcs_udp.Connect(IPAddress.Parse($"192.168.0.101"),19570);
            UdpClient px4_udp = new UdpClient(14551);
            px4_udp.Connect(IPAddress.Parse($"172.28.62.125"),18570);

            MAVLinkWriter px4_hil_mvw = new MAVLinkWriter(new MemoryStream());            

            MAVLinkReader gcs_mvr = new MAVLinkReader(new MemoryStream());
            MAVLinkReader px4_mvr = new MAVLinkReader(new MemoryStream());

            MAVLinkWriter gcs_mvw = new MAVLinkWriter(new MemoryStream());
            MAVLinkWriter px4_mvw = new MAVLinkWriter(new MemoryStream());

            List<MAVLinkMsg> gcs_queue = new List<MAVLinkMsg>();
            List<MAVLinkMsg> px4_queue = new List<MAVLinkMsg>();

            Stopwatch app_clock = new Stopwatch();
            app_clock.Start();

            Stopwatch app_clock_dt = new Stopwatch();
            app_clock_dt.Start();

            float heartbeat_t=0f;
            float hil_t=0f;


            //GCS UDP
            ThreadPool.QueueUserWorkItem(delegate(object? p_state) {
                while(true) {
                    IPEndPoint ep = new IPEndPoint(IPAddress.Any,0);
                    byte[] d = null;
                    try { d = gcs_udp.Receive(ref ep); } catch(System.Exception) { }
                    if(d==null) continue;
                    MemoryStream ms = (MemoryStream)gcs_mvr.Stream;
                    ms.SetLength(0);                    
                    gcs_mvr.Stream.Write(d);
                    gcs_mvr.Stream.Position=0;
                    MAVLinkMsg msg = new MAVLinkMsg();                        
                    while(true) {
                        MAVLinkParseResult res = gcs_mvr.Read(ref msg);
                        if(res == MAVLinkParseResult.Incomplete) break;
                        if(res == MAVLinkParseResult.NotFound  ) break;
                        if(res != MAVLinkParseResult.Success) continue;
                        lock(gcs_queue) gcs_queue.Add(msg);    
                        string cmd_long = msg.messageId == MAVLinkMsgId.CommandLong ? $" | {((CommandLongData)msg.data).Command}" : $"";
                        switch(msg.messageId) {                                
                            default: Console.WriteLine($"MAVLink> [gcs.udp] {msg.messageId}{cmd_long}"); break;
                        }
                    }
                    Thread.Sleep(1);
                }
            });

            //PX4 UDP
            ThreadPool.QueueUserWorkItem(delegate(object? p_state) {
                while(true) {
                    IPEndPoint ep = new IPEndPoint(IPAddress.Any,0);
                    byte[] d = null; 
                    try { d = px4_udp.Receive(ref ep); } catch(System.Exception) { }
                    if(d==null) continue;
                    lock(px4_mvr) {
                        MemoryStream ms = (MemoryStream)px4_mvr.Stream;
                        ms.SetLength(0);                    
                        px4_mvr.Stream.Write(d);
                        px4_mvr.Stream.Position=0;
                        MAVLinkMsg msg = new MAVLinkMsg();                        
                        while(true) {
                            MAVLinkParseResult res = px4_mvr.Read(ref msg);
                            if(res == MAVLinkParseResult.Incomplete) break;
                            if(res == MAVLinkParseResult.NotFound  ) break;
                            if(res != MAVLinkParseResult.Success) continue;
                            lock(px4_queue) px4_queue.Add(msg);    
                            string cmd_long = msg.messageId == MAVLinkMsgId.CommandLong ? $" | {((CommandLongData)msg.data).Command}" : $"";
                            switch(msg.messageId) {                                
                                default: Console.WriteLine($"MAVLink> [px4.udp] {msg.messageId}{cmd_long}"); break;
                            }
                        }                        
                    }                    
                    Thread.Sleep(1);
                }
            });

            //PX4 TCP
            ThreadPool.QueueUserWorkItem(delegate(object? p_state) {
                byte[] msg_buff = new byte[280];
                while(true) {
                    if(px4_hil==null) continue;
                    IPEndPoint ep = new IPEndPoint(IPAddress.Any,0);
                    int c = px4_hil.GetStream().Read(msg_buff);
                    if(c<=0) continue;
                    lock(px4_mvr) {
                        MemoryStream ms = (MemoryStream)px4_mvr.Stream;
                        ms.SetLength(0);                    
                        px4_mvr.Stream.Write(msg_buff,0,c);
                        px4_mvr.Stream.Position=0;
                        MAVLinkMsg msg = new MAVLinkMsg();                        
                        while(true) {
                            MAVLinkParseResult res = px4_mvr.Read(ref msg);
                            if(res == MAVLinkParseResult.Incomplete) break;
                            if(res == MAVLinkParseResult.NotFound  ) break;
                            if(res != MAVLinkParseResult.Success) continue;
                            lock(px4_queue) px4_queue.Add(msg);    
                            string cmd_long = msg.messageId == MAVLinkMsgId.CommandLong ? $" | {((CommandLongData)msg.data).Command}" : $"";
                            switch(msg.messageId) {
                                case MAVLinkMsgId.HilActuatorControls: break;
                                case MAVLinkMsgId.Heartbeat: { HeartbeatData msg_d = (HeartbeatData)msg.data;  Console.WriteLine($"MAVLink> [px4.tcp] {msg.messageId} Sys:{msg.systemId} Cmp: {msg.componentId} | Autopilot: {msg_d.Autopilot} Type: {msg_d.Type} Status: {msg_d.SystemStatus}"); break; }
                                default: Console.WriteLine($"MAVLink> [px4.tcp] {msg.messageId}{cmd_long}"); break;
                            }                            
                        }                                                
                    }
                    Thread.Sleep(1);
                }
            });
            

            while(true) {     
                ulong time_usec = (ulong)app_clock.Elapsed.TotalMicroseconds;
                float dt = (float)app_clock_dt.Elapsed.TotalSeconds;
                app_clock_dt.Restart();

                //Resets PX4 TCP Vehicle
                {
                    MemoryStream ms = (MemoryStream)px4_hil_mvw.Stream;
                    ms.SetLength(0);                    
                }

                heartbeat_t+=dt;
                if(heartbeat_t>=1f) {
                    heartbeat_t=0f;                
                    Console.WriteLine($"MAVLink> [sim] {MAVLinkMsgId.Heartbeat}");
                    px4_hil_mvw.WriteV2(1,0,new HeartbeatData() { Autopilot = MAVAutopilotFlags.Px4, MavlinkVersion = 3 });
                }

                hil_t+=dt;
                if(hil_t>=0.1f) {
                    hil_t=0f;
                    //Console.WriteLine($"MAVLink> [sim] {MAVLinkMsgId.HilStateQuaternion}");
                    px4_hil_mvw.WriteV2(1,0,new HilStateQuaternionData() {
                       TimeUsec = time_usec,
                       Alt = 98400,
                       AttitudeQuaternion = new float[] { 1,0,0,0 },
                       Lat = 325026457,
                       Lon = -837504386,
                       Vx  = 0,
                       Vy  = 0,
                       Vz  = 0,
                    });
                    px4_hil_mvw.WriteV2(1,0,new HilSensorData() {
                       TimeUsec = time_usec,                                    
                    });
                    px4_hil_mvw.WriteV2(1,0,new HilGpsData() {
                       TimeUsec = time_usec,                       
                       FixType = 3,
                       SatellitesVisible = 10,
                    });
                }

                lock(gcs_queue) while(gcs_queue.Count>0) { MAVLinkMsg msg = gcs_queue[0]; gcs_queue.RemoveAt(0); Console.WriteLine($"    [gcs -> px4] {msg.messageId}"); px4_mvw.WriteV2(msg); }
                lock(px4_queue) while(px4_queue.Count>0) { MAVLinkMsg msg = px4_queue[0]; px4_queue.RemoveAt(0); Console.WriteLine($"    [px4 -> gcs] {msg.messageId}"); gcs_mvw.WriteV2(msg); }

                try {
                    MemoryStream ms = (MemoryStream)px4_hil_mvw.Stream;
                    ms.Position = 0;
                    if(px4_hil!=null)ms.CopyTo(px4_hil.GetStream());                      
                }
                catch(System.Exception) { }

                if(px4_mvw.Stream.Length>0) {
                    MemoryStream ms = (MemoryStream)px4_mvw.Stream;
                    ms.Position=0;
                    byte[] b = ms.ToArray();
                    px4_udp.Send(b);
                    ms.SetLength(0);
                }

                if(gcs_mvw.Stream.Length>0) {
                    MemoryStream ms = (MemoryStream)gcs_mvw.Stream;
                    ms.Position=0;
                    byte[] b = ms.ToArray();
                    gcs_udp.Send(b);
                    ms.SetLength(0);
                }
                
                
                Thread.Sleep(1);
            }

        }
    }
}
