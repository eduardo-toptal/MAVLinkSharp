using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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

            WriteFloat(SensorChannel.AccelX,Noise(0f,0.1f));
            WriteFloat(SensorChannel.AccelY,Noise(0f,0.1f));
            WriteFloat(SensorChannel.AccelZ,Noise(-9.8f,0.1f));

            WriteFloat(SensorChannel.GyroscopeX,Noise(0f,0.1f));
            WriteFloat(SensorChannel.GyroscopeY,Noise(0f,0.1f));
            WriteFloat(SensorChannel.GyroscopeZ,Noise(0f,0.1f));

            /*
            WriteFloat(SensorChannel.MagnetometerX,Noise(0.2f,0.01f));
            WriteFloat(SensorChannel.MagnetometerY,Noise(0.05f,0.01f));
            WriteFloat(SensorChannel.MagnetometerZ,Noise(0.05f,0.01f));
            //*/

            WriteFloat(SensorChannel.Temperature,Noise(30f,2f));
            WriteFloat(SensorChannel.PressureAbsolute,Noise(1013f,0.1f));
            WriteFloat(SensorChannel.PressureDifferential,Noise(0.1f,0.1f));
            WriteFloat(SensorChannel.PressureAltitude,Noise(0.1f,0.1f));

            WriteFloat(SensorChannel.Yaw,Noise(0f,0.1f));
            WriteFloat(SensorChannel.Pitch,Noise(0f,0.1f));
            WriteFloat(SensorChannel.Roll,Noise(0f,0.1f));
            WriteFloat(SensorChannel.YawSpeed,Noise(0f,0.1f));
            WriteFloat(SensorChannel.PitchSpeed,Noise(0f,0.1f));
            WriteFloat(SensorChannel.RollSpeed,Noise(0f,0.1f));

            WriteFloat(SensorChannel.AirspeedIndicated,Noise(0f,0.01f));
            WriteFloat(SensorChannel.AirSpeedTrue,Noise(0f,0.01f));

            WriteFloat(SensorChannel.QuatW,Noise(1f,0.1f));
            WriteFloat(SensorChannel.QuatX,Noise(0f,0.1f));
            WriteFloat(SensorChannel.QuatY,Noise(0f,0.1f));
            WriteFloat(SensorChannel.QuatZ,Noise(0f,0.1f));

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
        /// Address to access the ground control system
        /// </summary>
        public string QGCAddress     = $"udp://127.0.0.1:14550";
        /// <summary>
        /// Address to access PX4 in SITL mode and sync HIL data
        /// </summary>
        public string PX4HILAddress  = $"tcp://127.0.0.1:4560";
        /// <summary>
        /// [optional] Address to access PX4 and intercep CTRL messages between PX4 and QCG
        /// </summary>
        public string PX4CtrlAddress = $"udp://127.0.0.1:14580";

        #region Protocols
        /// <summary>
        /// Returns the network protocol for the 
        /// </summary>
        /// <returns></returns>
        public ProtocolType GetQGCProtocol    () { return ParseProtocol(QGCAddress   ); }
        public ProtocolType GetPX4HILProtocol () { return ParseProtocol(PX4HILAddress); }
        public ProtocolType GetPX4CtrlProtocol() { return ParseProtocol(PX4CtrlAddress); }

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
        public IPEndPoint GetQGCEndPoint() { return ParseEndPoint(QGCAddress);  }

        /// <summary>
        /// Returns the PX4 HIL EndPoint
        /// </summary>
        /// <returns></returns>
        public IPEndPoint GetPX4HILEndPoint() { return ParseEndPoint(PX4HILAddress); }

        /// <summary>
        /// Returns the PX4 Ctrl EndPoint
        /// </summary>
        /// <returns></returns>
        public IPEndPoint GetPX4CtrlEndPoint() { return ParseEndPoint(PX4CtrlAddress); }

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
        public MAVLinkInterface ctrl { get; private set; }

        /// <summary>
        /// Reference to the QGC interface (QGC -> PX4)
        /// </summary>
        public MAVLinkInterface qgc { get; private set; }

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
        /// Internals
        /// </summary>
        private Thread m_thread;
        private bool   m_thread_active;
        private bool   m_hil_heartbeat;
        private bool   m_qgc_heartbeat;
        private bool   m_hil_controls;

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
            m_thread.Priority = ThreadPriority.Normal;
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
            if (hil  != null) { hil.Close();  }
            if (qgc  != null) { qgc.Close();  }
            if (ctrl != null) { ctrl.Close(); }
        }

        /// <summary>
        /// Updates the state machine
        /// </summary>
        new public void Update() {

            switch (state) {
                case MAVLinkAppState.Idle: { } break;

                case MAVLinkAppState.PX4Error: {
                    state = MAVLinkAppState.Idle;
                    if (OnStateChangeEvent != null) OnStateChangeEvent(state);
                }
                break;

                case MAVLinkAppState.PX4Disconnect: {
                    state = MAVLinkAppState.PX4Error;
                    if (OnStateChangeEvent != null) OnStateChangeEvent(state);
                }
                break;

                case MAVLinkAppState.Initialize: {                                        
                    //Vehicle System
                    vehicle = new MAVLinkSystem(1,MAV_TYPE.QUADROTOR,"vehicle");
                    vehicle.network = this;
                    //PX4 GCS Networking
                    IPEndPoint hil_ep           = settings.GetPX4HILEndPoint();
                    IPEndPoint ctrl_local_ep    = new IPEndPoint(hil_ep.Address,0);
                    IPEndPoint ctrl_remote_ep   = settings.GetPX4CtrlEndPoint();
                    IPEndPoint qgc_ep           = settings.GetQGCEndPoint();

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

                    //UDP Links such as GCS/PX4 CTRL

                    UdpClient ctrl_udp = new UdpClient(ctrl_local_ep);
                    ctrl_udp.Connect(ctrl_remote_ep);
                    ctrl = new MAVLinkUDP(ctrl_udp,"ctrl");
                    ctrl.network = this;

                    UdpClient gcs_udp = new UdpClient();
                    gcs_udp.Connect(qgc_ep);
                    qgc = new MAVLinkUDP(gcs_udp,"qgc");
                    qgc.network = this;


                    MAVLinkEntity px4_qgc_router = new MAVLinkEntity($"px4-qgc-router");
                    px4_qgc_router.network = this;
                    // GCS <- vehicle -> PX4 CTRL
                    
                    qgc.Link(px4_qgc_router);
                    px4_qgc_router.Link(ctrl);
                    ctrl.Link(px4_qgc_router);
                    px4_qgc_router.Link(qgc);
                    //*/
                    //qgc.Link(ctrl);
                    //ctrl.Link(qgc);
                    

                    /*
                    //Link HIL to messages debug on QGC
                    hil.Link(qgc);
                    //Ignored messages
                    qgc.ignored = new List<MAVLinkMessageFilter>() {
                        MAVLinkMessageFilter.FilterAll($"vehicle|HEARTBEAT")
                    };
                    //*/

                    //Link PX4 HIL to get actuators                    
                    vehicle.Link(hil);
                    hil.Link(vehicle);

                    //Link vehicle[N] to router for systems to intercept QGC <> PX4 messageflow                    
                    px4_qgc_router.Link(vehicle);
                    //Make PX4 HIL ignore the QGC and CTRL
                    hil.ignored = new List<MAVLinkMessageFilter>() { MAVLinkMessageFilter.FilterSender($"ctrl|qgc") };

                    /*
                    mv_logger.ignored = new List<MAVLinkMessageFilter>() {
                        MAVLinkMessageFilter.FilterMessage("GPS_RAW_INT|VIBRATION|MANUAL_CONTROL|HIGHRES_IMU|SYS_STATUS|TIMESYNC|ACTUATOR_CONTROL_TARGET|SERVO_OUTPUT_RAW"),
                        MAVLinkMessageFilter.FilterMessage("EXTENDED_SYS_STATE|BATTERY_STATUS|PING|LINK_NODE_STATUS|HOME_POSITION|SYSTEM_TIME|UTM_GLOBAL_POSITION|GLOBAL_POSITION_INT"),
                        MAVLinkMessageFilter.FilterMessage("ODOMETRY|VFR_HUD|ATTITUDE_QUATERNION|ATTITUDE|ATTITUDE_TARGET|ALTITUDE|POSITION_TARGET_LOCAL_NED|GPS_GLOBAL_ORIGIN|ESTIMATOR_STATUS|LOCAL_POSITION_NED"),
                        MAVLinkMessageFilter.FilterMessage("HEARTBEAT"),
                        MAVLinkMessageFilter.FilterMessage("HIL_*")
                    };
                    //*/


                    //Set system as PX4
                    vehicle.autopilot = MAV_AUTOPILOT.PX4;
                    //Disable the system and block heartbeats
                    vehicle.enabled = false;
                    vehicle.alive = false;

                    if (OnStateChangeEvent != null) OnStateChangeEvent(state);

                    //Starts Listening to PX4                        
                    switch (settings.GetPX4HILProtocol()) {
                        case ProtocolType.Tcp: {
                            ((MAVLinkTCP)hil).Listen(hil_ep.Address,hil_ep.Port);
                            state = MAVLinkAppState.PX4Wait;
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
                            if (cl.client == null)    break;
                            if (!cl.client.Connected) break;
                            if (!m_hil_heartbeat)     break;
                            //Activate the vehicle
                            vehicle.alive   = true;
                            vehicle.enabled = true;

                            UberSensor s = new UberSensor();
                            s.syncRate = 5;
                            s.system   = vehicle;

                            //Prepare sensor warmup to send first batch of data
                            state = MAVLinkAppState.PX4SensorWarmup;
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
                    HEARTBEAT_MSG qgc_ping = new HEARTBEAT_MSG() {
                        autopilot = (byte)MAV_AUTOPILOT.PX4,
                        type = (byte)MAV_TYPE.GCS,
                        mavlink_version = 3
                    };
                    MAVLinkMessage qgc_ping_msg = qgc.CreateMessage(MSG_ID.HEARTBEAT,qgc_ping,false,0,0);
                    qgc.Send(qgc_ping_msg,true,true);
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
                    if ( hil == null  ) is_connected = false; else
                    if (!hil.connected) is_connected = false;
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
