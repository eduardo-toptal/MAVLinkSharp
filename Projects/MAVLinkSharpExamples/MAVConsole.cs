using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using static MAVLink;

#pragma warning disable CS8600
#pragma warning disable CS8602
#pragma warning disable CS8604
#pragma warning disable CS0168

namespace MAVConsole // Note: actual namespace depends on the project name.
{
    
    public class MAVConsole {

        //Console FSM
        public enum State {
            Idle=0,
            WaitingTCP,
            Initialize,
            QGCWait,
            QGCBoot,
            Running
        }

        //IPs
        static string local_ip      = "127.0.0.1";
        static string host_ip       = "192.168.86.50";
        static string remote_ip     = "192.168.86.40";        
        //PX4 Station        
        static int    hil_port      = 4560;        
        static int    gcs_port      = 14550;        
        static int    ctrl_port_remote   = 14580;
        static int    ctrl_port_local    = 0;

        public class MAVLinkLogger : MAVLinkEntity {

            public MAVLinkLogger(string p_name="") : base(p_name) { }

            private string current_id  = "";
            private double log_counter = 0;

            override protected void OnMessage(MAVLinkEntity p_caller,MAVLinkMessage p_msg) {

                MSG_ID msg_id   = (MSG_ID)p_msg.msgid;
                string msg_id_s = $"{p_caller.name}.{p_msg.msgid.ToString()}";

                bool group = true;

                if(msg_id == MSG_ID.PARAM_VALUE) group = false;

                if(group)
                if(msg_id_s == current_id) {                         
                    if ((clock.elapsed - log_counter)  > 1.0) { Console.Write($"."); log_counter = clock.elapsed; } return; 
                }

                if ((msg_id_s != current_id) || !group) { Console.WriteLine(); current_id = msg_id_s; log_counter = 0; }

                Console.Write($"{p_caller.name.PadRight(7)}> [{p_msg.sysid,3}:{p_msg.compid,3}] {msg_id}");

                switch (msg_id) {
                    case MSG_ID.COMMAND_LONG: {
                        COMMAND_LONG_MSG d = p_msg.ToStructure<COMMAND_LONG_MSG>();
                        MAV_CMD cmd = (MAV_CMD)d.command;
                        Console.Write($" -> {cmd}({d.param1},{d.param2},{d.param3},{d.param4},{d.param5}) : ACK[{d.confirmation}] Target[{d.target_system}:{d.target_component}]");                        
                        switch(cmd) {
                            case MAV_CMD.REQUEST_MESSAGE: {
                                Console.Write($" -> {(MSG_ID)d.param1}");
                            }
                            break;
                        }
                    }
                    break;

                    case MSG_ID.COMMAND_ACK: {
                        COMMAND_ACK_MSG d = p_msg.ToStructure<COMMAND_ACK_MSG>();
                        MAV_CMD cmd = (MAV_CMD)d.command;
                        Console.Write($" -> {cmd} Target[{d.target_system}:{d.target_component}]");                        
                    }
                    break;

                    case MSG_ID.PARAM_REQUEST_READ: {
                        PARAM_REQUEST_READ_MSG d = p_msg.ToStructure<PARAM_REQUEST_READ_MSG>();
                        string id = ASCIIEncoding.UTF8.GetString(d.param_id);
                        short idx = d.param_index;
                        Console.Write($" -> id[{id} @ {idx}] ");
                    }
                    break;

                    case MSG_ID.PARAM_VALUE: {
                        PARAM_VALUE_MSG d = p_msg.ToStructure<PARAM_VALUE_MSG>();
                        string id = ASCIIEncoding.UTF8.GetString(d.param_id);
                        ushort idx = d.param_index;
                        ushort c = d.param_count;
                        Console.Write($" -> id[{id} @ {idx}/{c}] ");
                    }
                    break;

                    case MSG_ID.STATUSTEXT: {
                        STATUSTEXT_MSG d = p_msg.ToStructure<STATUSTEXT_MSG>();
                        string vs = ASCIIEncoding.UTF8.GetString(d.text);                                                
                        Console.Write($" -> {vs}");
                    }
                    break;

                }

                
                
            }

        }

        public class UberSensor : MAVLinkSensor {

            static private MAV_SYS_STATUS_SENSOR ubs_mask =
                MAV_SYS_STATUS_SENSOR.ABSOLUTE_PRESSURE    | MAV_SYS_STATUS_SENSOR.DIFFERENTIAL_PRESSURE  |
                MAV_SYS_STATUS_SENSOR._3D_ACCEL            | MAV_SYS_STATUS_SENSOR._3D_GYRO               | MAV_SYS_STATUS_SENSOR._3D_MAG |                
                MAV_SYS_STATUS_SENSOR.GPS                  | MAV_SYS_STATUS_SENSOR.Z_ALTITUDE_CONTROL     | MAV_SYS_STATUS_SENSOR.BATTERY | 
                MAV_SYS_STATUS_SENSOR.ANGULAR_RATE_CONTROL | MAV_SYS_STATUS_SENSOR.ATTITUDE_STABILIZATION |
                MAV_SYS_STATUS_SENSOR.RC_RECEIVER;

            private double gps_clock;
            //private double batt_clock;

            public UberSensor(string p_name="") : base(ubs_mask,(MAV_COMPONENT)1,MAV_TYPE.GENERIC,p_name) { }

            protected override void OnUpdate() {

                WriteFloat(SensorChannel.AccelerometerX,Noise(   0f,0.1f));
                WriteFloat(SensorChannel.AccelerometerY,Noise(   0f,0.1f));
                WriteFloat(SensorChannel.AccelerometerZ,Noise(-9.8f,0.1f));

                WriteFloat(SensorChannel.AirspeedIndicated,Noise(0f,0.01f));
                WriteFloat(SensorChannel.AirSpeedTrue     ,Noise(0f,0.01f));

                WriteFloat(SensorChannel.GyroscopeX,Noise(0f,0.1f));
                WriteFloat(SensorChannel.GyroscopeY,Noise(0f,0.1f));
                WriteFloat(SensorChannel.GyroscopeZ,Noise(0f,0.1f));

                WriteFloat(SensorChannel.MagnetometerX,Noise(0.2f ,0.01f));
                WriteFloat(SensorChannel.MagnetometerY,Noise(0.05f,0.01f));
                WriteFloat(SensorChannel.MagnetometerZ,Noise(0.05f,0.01f));

                WriteFloat(SensorChannel.Temperature         ,Noise(30f,2f));
                WriteFloat(SensorChannel.PressureAbsolute    ,Noise(1013f,5f));
                WriteFloat(SensorChannel.PressureDifferential,Noise(0.1f,0.1f));
                WriteFloat(SensorChannel.PressureAltitude    ,Noise(0.1f,0.1f));
                                
                WriteFloat(SensorChannel.Yaw       ,Noise(0f,0.1f));
                WriteFloat(SensorChannel.Pitch     ,Noise(0f,0.1f));
                WriteFloat(SensorChannel.Roll      ,Noise(0f,0.1f));
                WriteFloat(SensorChannel.YawSpeed  ,Noise(0f,0.1f));
                WriteFloat(SensorChannel.PitchSpeed,Noise(0f,0.1f));
                WriteFloat(SensorChannel.RollSpeed ,Noise(0f,0.1f));

                WriteFloat(SensorChannel.QuatW      ,Noise(1f,0.1f));
                WriteFloat(SensorChannel.QuatX      ,Noise(0f,0.1f));
                WriteFloat(SensorChannel.QuatY      ,Noise(0f,0.1f));
                WriteFloat(SensorChannel.QuatZ      ,Noise(0f,0.1f));

                gps_clock += clock.deltaTime*1000.0;
                if(gps_clock > 50) {
                    gps_clock = 0;
                    
                    WriteInt    (SensorChannel.LatitudeWGS  ,(int)(Noise(-30.0346471f * 1E7f,0.01f)));
                    WriteInt    (SensorChannel.LongitudeWGS ,(int)(Noise(-51.2176584f * 1E7f,0.01f)));
                    WriteInt    (SensorChannel.AltitudeGPS  ,(int)(Noise(122.0f,1) * 1E3));
                    WriteUShort (SensorChannel.HDOP         ,(ushort)(Noise(1,0.1f) * 100));
                    WriteUShort (SensorChannel.VDOP         ,(ushort)(Noise(1,0.1f) * 100));
                    WriteByte   (SensorChannel.FixType         , 3);
                    WriteByte   (SensorChannel.SatelliteVisible,10);
                    WriteShort  (SensorChannel.VelocityNorth  ,(short)(Noise(0.1f,0.1f)));
                    WriteShort  (SensorChannel.VelocityEast   ,(short)(Noise(0.1f,0.1f)));
                    WriteShort  (SensorChannel.VelocityDown   ,(short)(Noise(0.1f,0.1f)));
                    WriteUShort (SensorChannel.GroundSpeedGPS ,(ushort)(Noise(0.1f,0.1f)));                                
                    
                }
            }

        }

        static void Main(string[] args) {

            
            

            Console.WriteLine($"MAVConsole> Starting Console Simulation");

            //Main Network
            MAVLinkNetwork mv_network      = new MAVLinkNetwork("pdwnet");
            //Vehicle System
            MAVLinkSystem  mv_system       = new MAVLinkSystem(1, MAV_TYPE.QUADROTOR,"vehicle");
            mv_system.network  = mv_network;
            mv_system.syncRate = 1;
            //Debugger
            Console.WriteLine($"MAVConsole> Creating MAVLink Message Logger");
            MAVLinkLogger mv_logger        = new MAVLinkLogger("logger");
            mv_logger.network = mv_network;

            //PX4 GCS Networking
            IPEndPoint hil_ep         = new IPEndPoint(IPAddress.Parse($"{remote_ip}"),hil_port        );            
            IPEndPoint ctrl_local_ep  = new IPEndPoint(IPAddress.Parse($"{host_ip  }"),ctrl_port_local );
            IPEndPoint ctrl_remote_ep = new IPEndPoint(IPAddress.Parse($"{remote_ip}"),ctrl_port_remote);
            IPEndPoint gcs_ep         = new IPEndPoint(IPAddress.Parse($"{local_ip }"),gcs_port        );

            //HIL/PX4 Links
            MAVLinkTCP mv_hil_conn = new MAVLinkTCP("hil");
            mv_hil_conn.network = mv_network;
            mv_hil_conn.syncRate = 1;
            
            Console.WriteLine($"MAVConsole> Creating MAVLinkTCP [{mv_hil_conn.name}] at tcp://{hil_ep.Address}:{hil_ep.Port}");

            //UDP Links such as GCS/PX4 CTRL
            UdpClient  gcs_udp = new UdpClient();
            gcs_udp.Connect(gcs_ep);
            UdpClient  ctrl_udp = new UdpClient(ctrl_local_ep);
            ctrl_udp.Connect(ctrl_remote_ep);

            MAVLinkUDP     mv_ctrl_conn = new MAVLinkUDP(ctrl_udp,"ctrl");
            MAVLinkUDP     mv_gcs_conn  = new MAVLinkUDP(gcs_udp ,"gcs");            
            Console.WriteLine($"MAVConsole> Creating MAVLinkUDP [{mv_ctrl_conn.name}]  Local[{ctrl_udp.Client.LocalEndPoint}] Remote[{ctrl_udp.Client.RemoteEndPoint}]");
            Console.WriteLine($"MAVConsole> Creating MAVLinkUDP [{mv_gcs_conn.name }] at udp://{gcs_ep.Address }:{gcs_ep.Port }");

            //Link UDP interfaces to network
            mv_gcs_conn.network   = mv_network;
            mv_ctrl_conn.network  = mv_network;
            mv_gcs_conn.syncRate  = 1;
            mv_ctrl_conn.syncRate = 1;
            // GCS <-> PX4 CTRL
            mv_gcs_conn.Link(mv_ctrl_conn);
            mv_ctrl_conn.Link(mv_gcs_conn);
            //Link the spam HIL to debug on QGC
            mv_hil_conn.Link(mv_gcs_conn);
            mv_gcs_conn.Link(mv_logger);

            //Ignored messages
            mv_gcs_conn.ignored = new List<MAVLinkMessageFilter>() {
                MAVLinkMessageFilter.FilterAll($"vehicle|HEARTBEAT")
            };

            //Link PX4 HIL to get actuators
            mv_system.Link(mv_hil_conn);
            mv_hil_conn.Link(mv_system);
            //Link to logger for debugging
            mv_hil_conn.Link(mv_logger);

            mv_logger.ignored = new List<MAVLinkMessageFilter>() {
                MAVLinkMessageFilter.FilterMessage("GPS_RAW_INT|VIBRATION|MANUAL_CONTROL|HIGHRES_IMU|SYS_STATUS|TIMESYNC|ACTUATOR_CONTROL_TARGET|SERVO_OUTPUT_RAW"),
                MAVLinkMessageFilter.FilterMessage("EXTENDED_SYS_STATE|BATTERY_STATUS|PING|LINK_NODE_STATUS|HOME_POSITION|SYSTEM_TIME|UTM_GLOBAL_POSITION|GLOBAL_POSITION_INT"),
                MAVLinkMessageFilter.FilterMessage("ODOMETRY|VFR_HUD|ATTITUDE_QUATERNION|ATTITUDE|ATTITUDE_TARGET|ALTITUDE|POSITION_TARGET_LOCAL_NED|GPS_GLOBAL_ORIGIN|ESTIMATOR_STATUS|LOCAL_POSITION_NED"),
                MAVLinkMessageFilter.FilterMessage("HEARTBEAT"),
                MAVLinkMessageFilter.FilterMessage("HIL_*")
            };


            //Set system as PX4
            mv_system.autopilot = MAV_AUTOPILOT.PX4;
            //Disable the system and block heartbeats
            mv_system.enabled   = false;
            mv_system.alive     = false;
            
            //Starts Listening to PX4
            Console.Write($"MAVConsole> Waiting Connection");
            mv_hil_conn.Listen(IPAddress.Parse(host_ip),hil_port);

            //Keyboard Input Loop
            #region Input
            Thread thd_input =
            new Thread(delegate () {
                while (true) {
                    ConsoleKeyInfo k = Console.ReadKey(true);
                    //MAVLinkMessage msg = null;
                    switch (k.Key) {
                        case ConsoleKey.D1: {                            
                        }
                        break;
                        case ConsoleKey.D2: {
                        }
                        break;
                    }
                    Thread.Sleep(10);
                }
            });
            thd_input.Start();
            #endregion

            int wait_count = 0;
            State state = State.WaitingTCP;

            double qgc_wait_t = 0;

            while(true) {

                switch(state) {
                    case State.WaitingTCP: {
                        mv_network.Update();
                        if(mv_hil_conn.client != null) { state = State.Initialize; break; }
                        if (wait_count++ > 100) { wait_count = 0; Console.Write($"."); }
                        Thread.Sleep(1);                        
                    }
                    continue;

                    case State.Initialize: {
                        Console.WriteLine($"");
                        Console.WriteLine($"MAVConsole> Initialize");                        
                        mv_system.alive     = true;
                        mv_system.enabled   = true;
                        
                        //Sends QGC a ping to trigger all mavlink handshakes
                        HEARTBEAT_MSG gcs_ping = new HEARTBEAT_MSG() {
                            autopilot       = (byte)MAV_AUTOPILOT.PX4,
                            type            = (byte)MAV_TYPE.GCS,
                            mavlink_version = 3
                        };
                        MAVLinkMessage gcs_ping_msg = mv_gcs_conn.CreateMessage(MSG_ID.HEARTBEAT,gcs_ping,false,1,1);
                        mv_gcs_conn.Send(gcs_ping_msg,true,true);
                        state = State.QGCWait;
                    }
                    break;

                    case State.QGCWait: {
                        mv_network.Update();
                        qgc_wait_t += mv_network.clock.deltaTime;
                        if (qgc_wait_t > 3.0) {
                            state = State.QGCBoot;
                        }
                    }
                    break;

                    case State.QGCBoot: {
                        UberSensor mv_sys_sensor = new UberSensor();
                        mv_sys_sensor.syncRate = 5;
                        mv_sys_sensor.system = mv_system;
                        state = State.Running;                            
                    }
                    break;

                    case State.Running: {
                        //Update all entities/system/components/interfaces
                        mv_network.Update();
                        Thread.Sleep(0);
                    }
                    break;

                }

            }

        }
    }
//*/
}
/*
INFO  [commander] Preflight check: FAILED
WARN  [PreFlightCheck] Arming denied! angular velocity invalid
WARN  [PreFlightCheck] Arming denied! attitude invalid
WARN  [PreFlightCheck] Arming denied! manual control lost
INFO  [commander] Prearm check: FAILED
INFO  [HealthFlags] DEVICE              STATUS
INFO  [HealthFlags] ----------------------------------
INFO  [HealthFlags] GYRO:
INFO  [HealthFlags] ACC:
INFO  [HealthFlags] MAG:                EN      OFF
INFO  [HealthFlags] PRESS:              EN      OFF
INFO  [HealthFlags] AIRSP:
INFO  [HealthFlags] GPS:
INFO  [HealthFlags] OPT:
INFO  [HealthFlags] VIO:
INFO  [HealthFlags] LASER:
INFO  [HealthFlags] GTRUTH:
INFO  [HealthFlags] RATES:
INFO  [HealthFlags] ATT:
INFO  [HealthFlags] YAW:
INFO  [HealthFlags] ALTCTL:
INFO  [HealthFlags] POS:
INFO  [HealthFlags] MOT:                EN      OK
INFO  [HealthFlags] RC  :
INFO  [HealthFlags] GYRO2:
INFO  [HealthFlags] ACC2:
INFO  [HealthFlags] MAG2:
INFO  [HealthFlags] GEOFENCE:
INFO  [HealthFlags] AHRS:                       ERR
INFO  [HealthFlags] TERRAIN:
INFO  [HealthFlags] REVMOT:
INFO  [HealthFlags] LOGGIN:
INFO  [HealthFlags] BATT:
INFO  [HealthFlags] PROX:
INFO  [HealthFlags] SATCOM:
INFO  [HealthFlags] PREARM:
INFO  [HealthFlags] OBSAVD:                     ERR

WARN  [PreFlightCheck] Preflight: GPS Vertical Pos Drift too high
INFO  [commander] Preflight check: OK
INFO  [commander] Prearm check: OK
INFO  [HealthFlags] DEVICE              STATUS
INFO  [HealthFlags] ----------------------------------
INFO  [HealthFlags] GYRO:
INFO  [HealthFlags] ACC:
INFO  [HealthFlags] MAG:                EN      OK
INFO  [HealthFlags] PRESS:              EN      OK
INFO  [HealthFlags] AIRSP:
INFO  [HealthFlags] GPS:                        ERR
INFO  [HealthFlags] OPT:
INFO  [HealthFlags] VIO:
INFO  [HealthFlags] LASER:
INFO  [HealthFlags] GTRUTH:
INFO  [HealthFlags] RATES:
INFO  [HealthFlags] ATT:
INFO  [HealthFlags] YAW:
INFO  [HealthFlags] ALTCTL:
INFO  [HealthFlags] POS:
INFO  [HealthFlags] MOT:                EN      OK
INFO  [HealthFlags] RC  :               EN      OK
INFO  [HealthFlags] GYRO2:
INFO  [HealthFlags] ACC2:
INFO  [HealthFlags] MAG2:               EN      OK
INFO  [HealthFlags] GEOFENCE:
INFO  [HealthFlags] AHRS:               EN      OK
INFO  [HealthFlags] TERRAIN:
INFO  [HealthFlags] REVMOT:
INFO  [HealthFlags] LOGGIN:
INFO  [HealthFlags] BATT:
INFO  [HealthFlags] PROX:
INFO  [HealthFlags] SATCOM:
INFO  [HealthFlags] PREARM:             EN      OK
INFO  [HealthFlags] OBSAVD:

//*/