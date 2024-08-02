using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine.Experimental.AI;
using static MAVLink;

#pragma warning disable CS8600
#pragma warning disable CS8602
#pragma warning disable CS8603
#pragma warning disable CS8618
#pragma warning disable CS8625

namespace MAVLinkSharp {


    public class UberSensor : MAVLinkSensor {

        static private MAV_SYS_STATUS_SENSOR ubs_mask =
            MAV_SYS_STATUS_SENSOR.ABSOLUTE_PRESSURE | MAV_SYS_STATUS_SENSOR.DIFFERENTIAL_PRESSURE |
            MAV_SYS_STATUS_SENSOR._3D_ACCEL | MAV_SYS_STATUS_SENSOR._3D_GYRO | MAV_SYS_STATUS_SENSOR._3D_MAG |
            MAV_SYS_STATUS_SENSOR.GPS | MAV_SYS_STATUS_SENSOR.Z_ALTITUDE_CONTROL | MAV_SYS_STATUS_SENSOR.BATTERY |
            MAV_SYS_STATUS_SENSOR.ANGULAR_RATE_CONTROL | MAV_SYS_STATUS_SENSOR.ATTITUDE_STABILIZATION |
            MAV_SYS_STATUS_SENSOR.RC_RECEIVER;

        private double gps_clock;
        //private double batt_clock;

        public UberSensor(string p_name = "") : base(ubs_mask,(MAV_COMPONENT)1,MAV_TYPE.GENERIC,p_name) { }

        protected override void OnUpdate() {

            /*
            WriteFloat(SensorChannel.AccelX,Noise(0f,0.1f));
            WriteFloat(SensorChannel.AccelY,Noise(0f,0.1f));
            WriteFloat(SensorChannel.AccelZ,Noise(-9.8f,0.1f));
            //*/
            /*
            WriteFloat(SensorChannel.GyroX,Noise(0f,0.1f));
            WriteFloat(SensorChannel.GyroY,Noise(0f,0.1f));
            WriteFloat(SensorChannel.GyroZ,Noise(0f,0.1f));
            //*/
            /*
            WriteFloat(SensorChannel.MagnetometerX,Noise(0.2f,0.01f));
            WriteFloat(SensorChannel.MagnetometerY,Noise(0.05f,0.01f));
            WriteFloat(SensorChannel.MagnetometerZ,Noise(0.05f,0.01f));
            //*/

            /*
            WriteFloat(SensorChannel.Temperature,Noise(30f,2f));
            WriteFloat(SensorChannel.PressureAbsolute,Noise(1013f,0.1f));
            WriteFloat(SensorChannel.PressureDifferential,Noise(0.1f,0.1f));
            WriteFloat(SensorChannel.PressureAltitude,Noise(0.1f,0.1f));
            //*/

            /*
            WriteFloat(SensorChannel.YawSpeed,Noise(0f,0.1f));
            WriteFloat(SensorChannel.PitchSpeed,Noise(0f,0.1f));
            WriteFloat(SensorChannel.RollSpeed,Noise(0f,0.1f));
            //*/

            WriteFloat(SensorChannel.AirspeedIndicated,Noise(0f,0.01f));
            WriteFloat(SensorChannel.AirSpeedTrue,Noise(0f,0.01f));

            /*
            WriteFloat(SensorChannel.QuatW,Noise(1f,0.1f));
            WriteFloat(SensorChannel.QuatX,Noise(0f,0.1f));
            WriteFloat(SensorChannel.QuatY,Noise(0f,0.1f));
            WriteFloat(SensorChannel.QuatZ,Noise(0f,0.1f));
            //*/

            gps_clock += clock.deltaTime * 1000.0;
            if (gps_clock > 100) {
                gps_clock = 0;

                //37.351282965432226, -121.92465272540913
                //37.35260967945035, -121.92419287567044
                /*
                WriteInt(SensorChannel.LatitudeWGS,(int)(Noise(37.351282965432226f * 1E7f,0.01f)));
                WriteInt(SensorChannel.LongitudeWGS,(int)(Noise(-121.92465272540913f * 1E7f,0.01f)));
                WriteInt(SensorChannel.AltitudeGPS,(int)(Noise(122.0f,1) * 1E3));
                WriteUShort(SensorChannel.HDOP,(ushort)(Noise(1,0.1f) * 100));
                WriteUShort(SensorChannel.VDOP,(ushort)(Noise(1,0.1f) * 100));
                WriteByte(SensorChannel.FixType,3);
                WriteByte(SensorChannel.SatelliteVisible,10);
                WriteShort(SensorChannel.VelocityNorth,(short)(Noise(0.1f,0.1f)));
                WriteShort(SensorChannel.VelocityEast,(short)(Noise(0.1f,0.1f)));
                WriteShort(SensorChannel.VelocityDown,(short)(Noise(0.1f,0.1f)));
                WriteUShort(SensorChannel.GroundSpeedGPS,(ushort)(Noise(0.1f,0.1f)));
                //*/
            }
        }

    }




    #region enum MAVLinkAppState
    /// <summary>
    /// Application State for the internal FSM.
    /// </summary>
    public enum MAVLinkAppState {
        Idle=0,
        Initialize,
        PX4Wait,
        PX4Success,        
        PX4SensorWarmup,
        PX4Error,
        PX4Disconnect,
        QGCInitialize,
        QGCWait,
        QGCSuccess,
        Running,
        Message
    }
    #endregion

    #region class MAVLinkAppSettings
    /// <summary>
    /// Class that encapsulates the needed settings for connecting to PX4 and QGC
    /// </summary>
    [System.Serializable]
    public class MAVLinkAppSettings {

        /// <summary>
        /// Address to access PX4 in SITL mode and sync HIL data
        /// </summary>
        public string PX4HILAddress  = $"tcp://0.0.0.0:4560";

        /// <summary>
        /// Address to access the ground control system
        /// </summary>
        public string GCSAddress = $"udp://127.0.0.1";        
        /// <summary>
        /// [optional] Address to access PX4 and intercep CTRL messages between PX4 and QCG
        /// </summary>
        public string PX4Address = $"udp://127.0.0.1";

        /// <summary>
        /// Port to listen incoming MAVLink messages
        /// </summary>
        public int PX4LocalPort = 18570;

        /// <summary>
        /// Port to send MAVLink messages
        /// </summary>
        public int PX4RemotePort = 18570;

        /// <summary>
        /// Port to listen incoming MAVLink messages
        /// </summary>
        public int GCSLocalPort = 14550;

        /// <summary>
        /// Port to send MAVLink messages
        /// </summary>
        public int GCSRemotePort = 14550;

        #region Protocols
        /// <summary>
        /// Returns the network protocol for the 
        /// </summary>
        /// <returns></returns>
        public ProtocolType GetGCSProtocol    () { return ParseProtocol(GCSAddress   ); }
        public ProtocolType GetPX4HILProtocol () { return ParseProtocol(PX4HILAddress); }
        public ProtocolType GetPX4Protocol    () { return ParseProtocol(PX4Address); }

        /// <summary>
        /// Utility
        /// </summary>        
        internal ProtocolType ParseProtocol(string p_address) {
            string tk = p_address.ToLower();
            if (tk.Contains("udp")) return ProtocolType.Udp;
            if (tk.Contains("tcp")) return ProtocolType.Tcp;
            return ProtocolType.Unknown;
        }
        #endregion

        #region EndPoints
        /// <summary>
        /// Returns the QGC EndPoint
        /// </summary>
        /// <returns></returns>
        public IPEndPoint GetGCSEndpoint() { return ParseEndPoint(GCSAddress);  }

        /// <summary>
        /// Returns the PX4 HIL EndPoint
        /// </summary>
        /// <returns></returns>
        public IPEndPoint GetPX4HILEndPoint() { return ParseEndPoint(PX4HILAddress); }

        /// <summary>
        /// Returns the PX4 Ctrl EndPoint
        /// </summary>
        /// <returns></returns>
        public IPEndPoint GetPX4EndPoint() { return ParseEndPoint(PX4Address); }

        /// <summary>
        /// Utility
        /// </summary>        
        internal IPEndPoint ParseEndPoint(string p_address) {
            string tk = p_address.ToLower();
            tk = tk.Replace("udp://","");
            tk = tk.Replace("tcp://","");
            string[] tkl = tk.Split(":");
            if (tkl.Length <= 0) return null;
            string ip_s   = tkl[0].Trim();
            string port_s = tkl.Length <= 1 ? "0" : tkl[1].Trim();
            IPAddress ip = IPAddress.Parse(ip_s);
            int port = 0;
            int.TryParse(port_s,out port);
            return new IPEndPoint(ip, port);
        }
        #endregion

    }
    #endregion

    /// <summary>
    /// Class that implements an application containing the building blocks to connecting to PX4 and exchange MAVLink messages w/ QGC as well offboard controls.
    /// </summary>
    public class MAVLinkApplication : MAVLinkNetwork {

        /// <summary>
        /// Current App State
        /// </summary>
        public MAVLinkAppState state { get; private set; }

        /// <summary>
        /// Reference to the settings.
        /// </summary>
        public MAVLinkAppSettings settings { get; private set; }

        /// <summary>
        /// Reference to the vehicle
        /// </summary>
        public MAVLinkSystem vehicle {  get; private set; }

        /// <summary>
        /// Reference to the HIL interface
        /// </summary>
        public MAVLinkInterface hil {  get; private set; }

        /// <summary>
        /// Reference to the CTRL interface (PX4 -> QGC)
        /// </summary>
        public MAVLinkInterface px4 { get; private set; }
        
        /// <summary>
        /// Reference to the QGC interface (QGC -> PX4)
        /// </summary>
        public MAVLinkInterface gcs { get; private set; }

        /// <summary>
        /// Speed of execution
        /// </summary>
        public int syncRate;

        /// <summary>
        /// Handler for when this app state changes
        /// </summary>
        public Action<MAVLinkAppState> OnStateChangeEvent;
        /// <summary>
        /// Handler for when this app is looping in a state
        /// </summary>
        public Action<MAVLinkAppState> OnStateUpdateEvent;

        /// <summary>
        /// Flag that tells to skip HIL clients and start running
        /// </summary>
        public bool skipHILConnection;

        /// <summary>
        /// Internals
        /// </summary>
        private Thread m_thread;
        private bool   m_thread_active;
        private bool   m_hil_heartbeat;
        private bool   m_qgc_heartbeat;
        private bool   m_hil_controls;
        private bool   m_debug_thread_alive;

        /// <summary>
        /// CTOR.
        /// </summary>
        /// <param name="p_id"></param>
        /// <param name="p_name"></param>
        public MAVLinkApplication(MAVLinkAppSettings p_settings = null, string p_name ="") : base(p_name) {
            settings        = p_settings == null ? new MAVLinkAppSettings() : p_settings;
            m_thread_active = false;
            syncRate        = 0;
        }

        /// <summary>
        /// Runs this app
        /// </summary>
        public void Run() {
            Dispose();
            state           = MAVLinkAppState.Initialize;
            m_thread_active = true;
            m_thread = new Thread(OnThreadUpdate);
            //m_thread.Priority = ThreadPriority.Normal;
            m_thread.Start();
        }

        /// <summary>
        /// Stops this app
        /// </summary>
        public void Stop() {
            Dispose();
        }

        /// <summary>
        /// Disposes this app.
        /// </summary>
        protected void Dispose() {
            m_thread_active = false;
            if (m_thread != null) if(!m_thread.Join(1000)) m_thread.Abort();
            m_thread = null;
            clock = new Clock();
            m_hil_heartbeat = false;
            m_hil_controls  = false;
            m_qgc_heartbeat = false;
            OnDispose();
            if (hil  != null) { hil.Close(); }
            if (gcs  != null) { gcs.Close(); }
            if (px4  != null) { px4.Close(); }            
            m_debug_thread_alive = false;
        }

        public enum MAVLinkCommModeFlag {            
            Receive = (1<<0),
            Send    = (1<<1)
        }

        public class MAVLinkComm {

            public MAVLinkCommModeFlag mode;

            public string name;

            public Socket conn;

            private MAVLinkStream m_snd;
            private MAVLinkStream m_rcv;
            private Task<SocketReceiveFromResult> m_rcv_task;
            private IPEndPoint m_rcv_ep;
            private byte[] m_rcv_d;
            private ArraySegment<byte> m_rcv_buffer;   
            
            public MAVLinkComm(IPEndPoint p_endpoint,ProtocolType p_protocol,MAVLinkCommModeFlag p_mode) {
                mode = p_mode;
                conn = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, p_protocol);
                
                IPEndPoint any_ep = new IPEndPoint(IPAddress.Any,p_endpoint.Port);

                if((mode & MAVLinkCommModeFlag.Receive)!=0) {                    
                    conn.Bind(any_ep);
                }

                if((mode & MAVLinkCommModeFlag.Send)!=0) {                    
                    conn.Connect(p_endpoint);
                }

                if((mode & MAVLinkCommModeFlag.Send   )!=0) m_snd = new MAVLinkStream();
                if((mode & MAVLinkCommModeFlag.Receive)!=0) m_rcv = new MAVLinkStream();

                m_rcv_ep = new IPEndPoint(IPAddress.Any,0);
                m_rcv_d  = new byte[16*1024];
                m_rcv_buffer = new ArraySegment<byte>(m_rcv_d);

            }

            public MAVLinkComm(string p_address,int p_port,ProtocolType p_protocol,MAVLinkCommModeFlag p_mode) : this(new IPEndPoint(IPAddress.Parse(p_address),p_port),p_protocol,p_mode) { }

            public MAVLinkComm(IPAddress p_address,int p_port,ProtocolType p_protocol,MAVLinkCommModeFlag p_mode) : this(new IPEndPoint(p_address,p_port),p_protocol,p_mode) { }

            public MAVLinkComm(int p_port,ProtocolType p_protocol) : this("0.0.0.0",p_port,p_protocol, MAVLinkCommModeFlag.Receive) { }

            public void Write(MAVLinkMessage p_message) {
                if(m_snd==null) return;                
                m_snd.Write(p_message);
            }

            public MAVLinkMessage Read() {
                if(m_rcv==null) return null;                
                return m_rcv.ReadMessage();                
            }

            public void Close() {
                if(conn!=null) {
                    conn.Close();                    
                }
            }

            public void Update() {

                if((mode & MAVLinkCommModeFlag.Receive)!=0) {
                    //If not receiving start task
                    if(m_rcv_task==null) { 
                        m_rcv_task = conn.ReceiveFromAsync(m_rcv_buffer, SocketFlags.None,m_rcv_ep);                         
                    }
                    //If receiving poll result
                    else {
                        bool is_completed = m_rcv_task.IsCompleted;
                        if(is_completed) {
                            bool is_error = m_rcv_task.IsCanceled || m_rcv_task.IsFaulted;
                            if(!is_error) {
                                SocketReceiveFromResult res = m_rcv_task.Result;
                                int len = res.ReceivedBytes;
                                if(len>0) m_rcv.Write(m_rcv_d,0,len);
                                //IPEndPoint res_ep = res.RemoteEndPoint is IPEndPoint ? (IPEndPoint)res.RemoteEndPoint : null;
                                //if(res_ep!=null) UnityEngine.Debug.Log($"[{name}] RECEIVE >> {res_ep.Address}:{res_ep.Port}");
                            }                        
                            m_rcv_task=null;
                        }                        
                    }                    
                }

                if((mode & MAVLinkCommModeFlag.Send)!=0) {
                    byte[] d = m_snd.Read();                    
                    if(d!=null) if(d.Length>0) conn.Send(d);
                }

            }

        }

        /// <summary>
        /// Updates the state machine
        /// </summary>
        new public void Update() {

            if (!enabled) return;

            switch (state) {
                case MAVLinkAppState.Idle: { 

                }
                break;

                case MAVLinkAppState.PX4Error: {
                    state = MAVLinkAppState.Idle;
                    if (OnStateChangeEvent != null) OnStateChangeEvent(state);
                }
                break;

                case MAVLinkAppState.PX4Disconnect: {
                    Stop();
                    Run();
                    //state = MAVLinkAppState.PX4Error;
                    //if (OnStateChangeEvent != null) OnStateChangeEvent(state);
                }
                break;

                case MAVLinkAppState.Initialize: {                                        
                    //Vehicle System
                    vehicle = new MAVLinkSystem(1,MAV_TYPE.QUADROTOR,"vehicle");                    
                    vehicle.network = this;
                    //PX4 GCS Networking
                    IPEndPoint hil_ep           = settings.GetPX4HILEndPoint();                    
                    IPEndPoint px4_ep           = settings.GetPX4EndPoint();
                    IPEndPoint gcs_ep           = settings.GetGCSEndpoint();

                    //HIL/PX4 Links
                    switch (settings.GetPX4HILProtocol()) {
                        case ProtocolType.Tcp: hil = new MAVLinkTCP("hil"); break;
                    }
                    if (hil == null) {
                        state = MAVLinkAppState.PX4Error;
                        if (OnStateChangeEvent != null) OnStateChangeEvent(state);
                        break;
                    }
                    hil.network = this;

                    MAVLinkEntity px4_qgc_router = new MAVLinkEntity($"px4-qgc-router");
                    px4_qgc_router.syncRate = 0;
                    px4_qgc_router.network = this;
                    // GCS <- vehicle -> PX4 CTRL

                    /*
                    UnityEngine.Debug.Log($"MAVLinkApplication> Creating CTRL UDP [{ctrl_remote_ep.Address}:{ctrl_remote_ep.Port}]");

                    MAVLinkComm gcs_snd = new MAVLinkComm(qgc_ep.Address, 19570, ProtocolType.Udp, MAVLinkCommModeFlag.Send | MAVLinkCommModeFlag.Receive);
                    gcs_snd.name = "GCS";
                    MAVLinkComm px4_snd = new MAVLinkComm(ctrl_remote_ep.Address, 18570, ProtocolType.Udp, MAVLinkCommModeFlag.Send | MAVLinkCommModeFlag.Receive);
                    px4_snd.name = "PX4";

                    Thread thd = 
                    new Thread(delegate() {    

                        if(gcs_snd==null) return;                        
                        if(px4_snd==null) return;

                        MAVLinkMessage msg;
                        MSG_ID msg_id;


                        while(true) {
                            Thread.Sleep(1);
                            if(!m_debug_thread_alive) break;

                            //Receive GCS
                            msg = gcs_snd.Read();
                            if(msg!=null) {
                                msg_id = (MSG_ID)msg.msgid;
                                UnityEngine.Debug.Log($"MAVLinkApplication> [GCS] [{msg_id}]");
                                px4_snd.Write(msg);
                            }

                            //Receive PX4
                            
                            msg = px4_snd.Read();
                            if(msg!=null) {
                                msg_id = (MSG_ID)msg.msgid;
                                UnityEngine.Debug.Log($"MAVLinkApplication> [PX4] [{msg_id}]");
                                gcs_snd.Write(msg);
                            }
                                                        
                            gcs_snd.Update();                            
                            px4_snd.Update();
                            
                        }

                        
                        gcs_snd.Close();                        
                        px4_snd.Close();                        

                    });

                    m_debug_thread_alive = true;
                    thd.Start();
                    //*/

                    //UDP Links such as GCS/PX4 CTRL

                    int px4_local_port  = settings.PX4LocalPort;
                    int px4_remote_port = settings.PX4RemotePort;
                    int gcs_local_port  = settings.GCSLocalPort;
                    int gcs_remote_port = settings.GCSRemotePort;
                    
                    UnityEngine.Debug.Log($"MAVLinkApplication> Creating PX4 UDP / Listen: {px4_local_port} Connect: {px4_ep.Address}:{px4_remote_port}");
                    UdpClient conn_px4 = new UdpClient(px4_local_port);
                    conn_px4.Connect(new IPEndPoint(px4_ep.Address,px4_remote_port));
                    px4 = new MAVLinkUDP(conn_px4,"px4");                    
                    px4.network  = this;

                    UnityEngine.Debug.Log($"MAVLinkApplication> Creating GCS UDP / Listen: {gcs_local_port} Connect: {gcs_ep.Address}:{gcs_remote_port}");
                    UdpClient conn_gcs = new UdpClient(gcs_local_port);
                    conn_gcs.Connect(new IPEndPoint(gcs_ep.Address,gcs_remote_port));                    
                    gcs = new MAVLinkUDP(conn_gcs,"gcs");                    
                    gcs.network  = this;

                    gcs.Link(px4);
                    px4.Link(gcs);    
                    
                    //For Manual Control
                    px4.Link(vehicle);
                    gcs.Link(vehicle);
                    
                    //Link PX4 HIL to get actuators                    
                    vehicle.Link(hil);
                    hil.Link(vehicle);

                    //Make PX4 HIL ignore the QGC and CTRL
                    hil.ignored = new List<MAVLinkMessageFilter>() { MAVLinkMessageFilter.FilterSender($"px4|gcs") };

                    //Set system as PX4
                    vehicle.autopilot = MAV_AUTOPILOT.PX4;
                    //Disable the system and block heartbeats
                    vehicle.enabled = false;
                    vehicle.alive   = false;

                    if (OnStateChangeEvent != null) OnStateChangeEvent(state);

                    //Starts Listening to PX4                        
                    switch (settings.GetPX4HILProtocol()) {
                        case ProtocolType.Tcp: {
                            state = MAVLinkAppState.PX4Wait;
                            Task.Run(delegate() { 
                                try {
                                    ((MAVLinkTCP)hil).Listen(hil_ep.Address, hil_ep.Port);
                                }
                                catch(Exception p_err) {
                                    state = MAVLinkAppState.PX4Error;
                                    Console.WriteLine($"MAVLinkApplication> Initialize / {p_err.Message}");
                                }                            
                            });                            
                            if (OnStateChangeEvent != null) OnStateChangeEvent(state);
                        }
                        break;

                        default: {
                            state = MAVLinkAppState.PX4Error;
                            if (OnStateChangeEvent != null) OnStateChangeEvent(state);
                        }
                        break;
                    }

                }
                break;

                case MAVLinkAppState.PX4Wait: {
                    switch (settings.GetPX4HILProtocol()) {
                        case ProtocolType.Tcp: {
                            MAVLinkTCP cl = hil as MAVLinkTCP;

                            bool can_run = true;

                            if (cl.client == null)    can_run = false; else
                            if (!cl.client.Connected) can_run = false; else
                            if (!m_hil_heartbeat)     can_run = false;

                            if(skipHILConnection)     can_run = true;

                            if(!can_run) break;

                            /*
                            //Activate the vehicle
                            vehicle.alive   = true;
                            vehicle.enabled = true;

                            //UberSensor s = new UberSensor();
                            //s.syncRate = 5;
                            //s.system   = vehicle;

                            //Prepare sensor warmup to send first batch of data
                            state = MAVLinkAppState.PX4SensorWarmup;
                            //*/

                            state = MAVLinkAppState.Running;
                            if (OnStateChangeEvent != null) OnStateChangeEvent(state);
                        }
                        break;
                    }
                }
                break;

                case MAVLinkAppState.PX4SensorWarmup: {
                    //Wait for first actuators
                    if (!m_hil_controls) break;
                    state = MAVLinkAppState.PX4Success;
                    if (OnStateChangeEvent != null) OnStateChangeEvent(state);
                }
                break;

                case MAVLinkAppState.PX4Success: {
                    //Sends QGC a ping to trigger all mavlink handshakes
                    if (gcs == null) {
                        state = MAVLinkAppState.QGCSuccess;
                        if (OnStateChangeEvent != null) OnStateChangeEvent(state);
                        break;
                    }
                    HEARTBEAT_MSG qgc_ping = new HEARTBEAT_MSG() {
                        autopilot = (byte)MAV_AUTOPILOT.PX4,
                        type = (byte)MAV_TYPE.GCS,
                        mavlink_version = 3
                    };
                    MAVLinkMessage qgc_ping_msg = gcs.CreateMessage(MSG_ID.HEARTBEAT,qgc_ping,false,0,0);
                    gcs.Send(qgc_ping_msg,true,true);
                    state = MAVLinkAppState.QGCWait;
                    if (OnStateChangeEvent != null) OnStateChangeEvent(state);
                }
                break;

                case MAVLinkAppState.QGCWait: {
                    //Wait for QGC first heartbeat
                    if (!m_qgc_heartbeat) break;
                    state = MAVLinkAppState.QGCSuccess;
                    if (OnStateChangeEvent != null) OnStateChangeEvent(state);
                }
                break;

                case MAVLinkAppState.QGCSuccess: {
                    state = MAVLinkAppState.Running;
                    if (OnStateChangeEvent != null) OnStateChangeEvent(state);
                }
                break;

                case MAVLinkAppState.Running: {
                    bool is_connected = true;
                    if ( hil == null  ) is_connected   = false; else
                    if (!hil.connected) is_connected   = false;
                    if(skipHILConnection) is_connected = true;
                    if(!is_connected) {
                        state = MAVLinkAppState.PX4Disconnect;
                        if (OnStateChangeEvent != null) OnStateChangeEvent(state);
                        break;
                    }
                }
                break;

            }

            //Runs the update related event
            switch (state) {
                case MAVLinkAppState.Idle: break;
                default:
                if (OnStateUpdateEvent != null) OnStateUpdateEvent(state);
                break;
            }

        }

        /// <summary>
        /// Handles some messages within MAVLink App FSM
        /// </summary>
        /// <param name="p_sender"></param>
        /// <param name="p_msg"></param>
        protected override void OnMessage(MAVLinkEntity p_sender,MAVLinkMessage p_msg) {

            base.OnMessage(p_sender,p_msg);

            MSG_ID msg_id = (MSG_ID)p_msg.msgid;

            switch (state) {

                #region PX4Wait
                //While in PX4Wait, waits for first HIL heartbeat
                case MAVLinkAppState.PX4Wait: {                    
                    switch (msg_id) {
                        case MSG_ID.HEARTBEAT: {
                            HEARTBEAT_MSG d = p_msg.ToStructure<HEARTBEAT_MSG>();
                            if (d.autopilot == (byte)MAV_AUTOPILOT.PX4) m_hil_heartbeat = true;
                        }
                        break;
                    }
                }
                break;
                #endregion

                #region PX4SensorWarmup
                //During sensor warmup wait for the first HIL_ACTUATORS
                case MAVLinkAppState.PX4SensorWarmup: {
                    if (msg_id == MSG_ID.HIL_ACTUATOR_CONTROLS) {
                        m_hil_controls = true;
                    }
                }
                break;
                #endregion

                #region QGCWait
                //Wait for QGC heartbeat response
                case MAVLinkAppState.QGCWait: {
                    switch (msg_id) {
                        case MSG_ID.HEARTBEAT: {
                            HEARTBEAT_MSG d = p_msg.ToStructure<HEARTBEAT_MSG>();
                            if (d.type == (byte)MAV_TYPE.GCS) m_qgc_heartbeat = true;
                        }
                        break;
                    }
                }
                break;
                #endregion

            }
        }

        /// <summary>
        /// Thread loop
        /// </summary>
        private void OnThreadUpdate() {
            while(true) {
                if (!m_thread_active) break;
                if (!enabled) continue;
                switch (state) {
                    case MAVLinkAppState.Idle: break;
                    default:                    
                    base.Update();
                    break;
                }
                Thread.Sleep(syncRate);
            }
        }

        /// <summary>
        /// Called when this app is disposed
        /// </summary>
        virtual protected void OnDispose() { }

    }
}
