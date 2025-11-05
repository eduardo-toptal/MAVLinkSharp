using System;
using System.Collections.Generic;

#pragma warning disable CS8618
#pragma warning disable CS1522

namespace MAVLinkSharp.Runtime {
    
    /// <summary>
    /// Utility for Samplig CRC
    /// </summary>
    public class MAVLinkCRC {

        #region MessageID to CRC
        /// <summary>
        /// Returns the message CRC generated from the definition XML
        /// </summary>
        static public byte GetMessageCRC(int p_msg_id) {
            switch(p_msg_id) {
                case 0     : return 50;    //HEARTBEAT
                case 1     : return 124;   //SYS_STATUS
                case 2     : return 137;   //SYSTEM_TIME
                case 4     : return 237;   //PING
                case 5     : return 217;   //CHANGE_OPERATOR_CONTROL
                case 6     : return 104;   //CHANGE_OPERATOR_CONTROL_ACK
                case 7     : return 119;   //AUTH_KEY
                case 8     : return 117;   //LINK_NODE_STATUS
                case 11    : return 89;    //SET_MODE
                case 20    : return 214;   //PARAM_REQUEST_READ
                case 21    : return 159;   //PARAM_REQUEST_LIST
                case 22    : return 220;   //PARAM_VALUE
                case 23    : return 168;   //PARAM_SET
                case 24    : return 24;    //GPS_RAW_INT
                case 25    : return 23;    //GPS_STATUS
                case 26    : return 170;   //SCALED_IMU
                case 27    : return 144;   //RAW_IMU
                case 28    : return 67;    //RAW_PRESSURE
                case 29    : return 115;   //SCALED_PRESSURE
                case 30    : return 39;    //ATTITUDE
                case 31    : return 246;   //ATTITUDE_QUATERNION
                case 32    : return 185;   //LOCAL_POSITION_NED
                case 33    : return 104;   //GLOBAL_POSITION_INT
                case 34    : return 237;   //RC_CHANNELS_SCALED
                case 35    : return 244;   //RC_CHANNELS_RAW
                case 36    : return 222;   //SERVO_OUTPUT_RAW
                case 37    : return 212;   //MISSION_REQUEST_PARTIAL_LIST
                case 38    : return 9;     //MISSION_WRITE_PARTIAL_LIST
                case 39    : return 254;   //MISSION_ITEM
                case 40    : return 230;   //MISSION_REQUEST
                case 41    : return 28;    //MISSION_SET_CURRENT
                case 42    : return 28;    //MISSION_CURRENT
                case 43    : return 132;   //MISSION_REQUEST_LIST
                case 44    : return 221;   //MISSION_COUNT
                case 45    : return 232;   //MISSION_CLEAR_ALL
                case 46    : return 11;    //MISSION_ITEM_REACHED
                case 47    : return 153;   //MISSION_ACK
                case 48    : return 41;    //SET_GPS_GLOBAL_ORIGIN
                case 49    : return 39;    //GPS_GLOBAL_ORIGIN
                case 50    : return 78;    //PARAM_MAP_RC
                case 51    : return 196;   //MISSION_REQUEST_INT
                case 54    : return 15;    //SAFETY_SET_ALLOWED_AREA
                case 55    : return 3;     //SAFETY_ALLOWED_AREA
                case 61    : return 167;   //ATTITUDE_QUATERNION_COV
                case 62    : return 183;   //NAV_CONTROLLER_OUTPUT
                case 63    : return 119;   //GLOBAL_POSITION_INT_COV
                case 64    : return 191;   //LOCAL_POSITION_NED_COV
                case 65    : return 118;   //RC_CHANNELS
                case 66    : return 148;   //REQUEST_DATA_STREAM
                case 67    : return 21;    //DATA_STREAM
                case 69    : return 243;   //MANUAL_CONTROL
                case 70    : return 124;   //RC_CHANNELS_OVERRIDE
                case 73    : return 38;    //MISSION_ITEM_INT
                case 74    : return 20;    //VFR_HUD
                case 75    : return 158;   //COMMAND_INT
                case 76    : return 152;   //COMMAND_LONG
                case 77    : return 143;   //COMMAND_ACK
                case 80    : return 14;    //COMMAND_CANCEL
                case 81    : return 106;   //MANUAL_SETPOINT
                case 82    : return 49;    //SET_ATTITUDE_TARGET
                case 83    : return 22;    //ATTITUDE_TARGET
                case 84    : return 143;   //SET_POSITION_TARGET_LOCAL_NED
                case 85    : return 140;   //POSITION_TARGET_LOCAL_NED
                case 86    : return 5;     //SET_POSITION_TARGET_GLOBAL_INT
                case 87    : return 150;   //POSITION_TARGET_GLOBAL_INT
                case 89    : return 231;   //LOCAL_POSITION_NED_SYSTEM_GLOBAL_OFFSET
                case 90    : return 183;   //HIL_STATE
                case 91    : return 63;    //HIL_CONTROLS
                case 92    : return 54;    //HIL_RC_INPUTS_RAW
                case 93    : return 47;    //HIL_ACTUATOR_CONTROLS
                case 100   : return 175;   //OPTICAL_FLOW
                case 101   : return 102;   //GLOBAL_VISION_POSITION_ESTIMATE
                case 102   : return 158;   //VISION_POSITION_ESTIMATE
                case 103   : return 208;   //VISION_SPEED_ESTIMATE
                case 104   : return 56;    //VICON_POSITION_ESTIMATE
                case 105   : return 93;    //HIGHRES_IMU
                case 106   : return 138;   //OPTICAL_FLOW_RAD
                case 107   : return 108;   //HIL_SENSOR
                case 108   : return 32;    //SIM_STATE
                case 109   : return 185;   //RADIO_STATUS
                case 110   : return 84;    //FILE_TRANSFER_PROTOCOL
                case 111   : return 34;    //TIMESYNC
                case 112   : return 174;   //CAMERA_TRIGGER
                case 113   : return 124;   //HIL_GPS
                case 114   : return 237;   //HIL_OPTICAL_FLOW
                case 115   : return 4;     //HIL_STATE_QUATERNION
                case 116   : return 76;    //SCALED_IMU2
                case 117   : return 128;   //LOG_REQUEST_LIST
                case 118   : return 56;    //LOG_ENTRY
                case 119   : return 116;   //LOG_REQUEST_DATA
                case 120   : return 134;   //LOG_DATA
                case 121   : return 237;   //LOG_ERASE
                case 122   : return 203;   //LOG_REQUEST_END
                case 123   : return 250;   //GPS_INJECT_DATA
                case 124   : return 87;    //GPS2_RAW
                case 125   : return 203;   //POWER_STATUS
                case 126   : return 220;   //SERIAL_CONTROL
                case 127   : return 25;    //GPS_RTK
                case 128   : return 226;   //GPS2_RTK
                case 129   : return 46;    //SCALED_IMU3
                case 130   : return 29;    //DATA_TRANSMISSION_HANDSHAKE
                case 131   : return 223;   //ENCAPSULATED_DATA
                case 132   : return 85;    //DISTANCE_SENSOR
                case 133   : return 6;     //TERRAIN_REQUEST
                case 134   : return 229;   //TERRAIN_DATA
                case 135   : return 203;   //TERRAIN_CHECK
                case 136   : return 1;     //TERRAIN_REPORT
                case 137   : return 195;   //SCALED_PRESSURE2
                case 138   : return 109;   //ATT_POS_MOCAP
                case 139   : return 168;   //SET_ACTUATOR_CONTROL_TARGET
                case 140   : return 181;   //ACTUATOR_CONTROL_TARGET
                case 141   : return 47;    //ALTITUDE
                case 142   : return 72;    //RESOURCE_REQUEST
                case 143   : return 131;   //SCALED_PRESSURE3
                case 144   : return 127;   //FOLLOW_TARGET
                case 146   : return 103;   //CONTROL_SYSTEM_STATE
                case 147   : return 154;   //BATTERY_STATUS
                case 148   : return 178;   //AUTOPILOT_VERSION
                case 149   : return 200;   //LANDING_TARGET
                case 162   : return 189;   //FENCE_STATUS
                case 192   : return 36;    //MAG_CAL_REPORT
                case 225   : return 208;   //EFI_STATUS
                case 230   : return 163;   //ESTIMATOR_STATUS
                case 231   : return 105;   //WIND_COV
                case 232   : return 151;   //GPS_INPUT
                case 233   : return 35;    //GPS_RTCM_DATA
                case 234   : return 150;   //HIGH_LATENCY
                case 235   : return 179;   //HIGH_LATENCY2
                case 241   : return 90;    //VIBRATION
                case 242   : return 104;   //HOME_POSITION
                case 243   : return 85;    //SET_HOME_POSITION
                case 244   : return 95;    //MESSAGE_INTERVAL
                case 245   : return 130;   //EXTENDED_SYS_STATE
                case 246   : return 184;   //ADSB_VEHICLE
                case 247   : return 81;    //COLLISION
                case 248   : return 8;     //V2_EXTENSION
                case 249   : return 204;   //MEMORY_VECT
                case 250   : return 49;    //DEBUG_VECT
                case 251   : return 170;   //NAMED_VALUE_FLOAT
                case 252   : return 44;    //NAMED_VALUE_INT
                case 253   : return 83;    //STATUSTEXT
                case 254   : return 46;    //DEBUG
                case 256   : return 71;    //SETUP_SIGNING
                case 257   : return 131;   //BUTTON_CHANGE
                case 258   : return 187;   //PLAY_TUNE
                case 259   : return 92;    //CAMERA_INFORMATION
                case 260   : return 146;   //CAMERA_SETTINGS
                case 261   : return 179;   //STORAGE_INFORMATION
                case 262   : return 12;    //CAMERA_CAPTURE_STATUS
                case 263   : return 133;   //CAMERA_IMAGE_CAPTURED
                case 264   : return 49;    //FLIGHT_INFORMATION
                case 265   : return 26;    //MOUNT_ORIENTATION
                case 266   : return 193;   //LOGGING_DATA
                case 267   : return 35;    //LOGGING_DATA_ACKED
                case 268   : return 14;    //LOGGING_ACK
                case 269   : return 109;   //VIDEO_STREAM_INFORMATION
                case 270   : return 59;    //VIDEO_STREAM_STATUS
                case 271   : return 22;    //CAMERA_FOV_STATUS
                case 275   : return 126;   //CAMERA_TRACKING_IMAGE_STATUS
                case 276   : return 18;    //CAMERA_TRACKING_GEO_STATUS
                case 280   : return 70;    //GIMBAL_MANAGER_INFORMATION
                case 281   : return 48;    //GIMBAL_MANAGER_STATUS
                case 282   : return 123;   //GIMBAL_MANAGER_SET_ATTITUDE
                case 283   : return 74;    //GIMBAL_DEVICE_INFORMATION
                case 284   : return 99;    //GIMBAL_DEVICE_SET_ATTITUDE
                case 285   : return 137;   //GIMBAL_DEVICE_ATTITUDE_STATUS
                case 286   : return 210;   //AUTOPILOT_STATE_FOR_GIMBAL_DEVICE
                case 287   : return 1;     //GIMBAL_MANAGER_SET_PITCHYAW
                case 288   : return 20;    //GIMBAL_MANAGER_SET_MANUAL_CONTROL
                case 290   : return 251;   //ESC_INFO
                case 291   : return 10;    //ESC_STATUS
                case 299   : return 19;    //WIFI_CONFIG_AP
                case 300   : return 217;   //PROTOCOL_VERSION
                case 301   : return 243;   //AIS_VESSEL
                case 310   : return 28;    //UAVCAN_NODE_STATUS
                case 311   : return 95;    //UAVCAN_NODE_INFO
                case 320   : return 243;   //PARAM_EXT_REQUEST_READ
                case 321   : return 88;    //PARAM_EXT_REQUEST_LIST
                case 322   : return 243;   //PARAM_EXT_VALUE
                case 323   : return 78;    //PARAM_EXT_SET
                case 324   : return 132;   //PARAM_EXT_ACK
                case 330   : return 23;    //OBSTACLE_DISTANCE
                case 331   : return 91;    //ODOMETRY
                case 332   : return 236;   //TRAJECTORY_REPRESENTATION_WAYPOINTS
                case 333   : return 231;   //TRAJECTORY_REPRESENTATION_BEZIER
                case 334   : return 72;    //CELLULAR_STATUS
                case 335   : return 225;   //ISBD_LINK_STATUS
                case 336   : return 245;   //CELLULAR_CONFIG
                case 339   : return 199;   //RAW_RPM
                case 340   : return 99;    //UTM_GLOBAL_POSITION
                case 350   : return 232;   //DEBUG_FLOAT_ARRAY
                case 360   : return 11;    //ORBIT_EXECUTION_STATUS
                case 370   : return 75;    //SMART_BATTERY_INFO
                case 373   : return 117;   //GENERATOR_STATUS
                case 375   : return 251;   //ACTUATOR_OUTPUT_STATUS
                case 380   : return 232;   //TIME_ESTIMATE_TO_TARGET
                case 385   : return 147;   //TUNNEL
                case 386   : return 132;   //CAN_FRAME
                case 387   : return 4;     //CANFD_FRAME
                case 388   : return 8;     //CAN_FILTER_MODIFY
                case 390   : return 156;   //ONBOARD_COMPUTER_STATUS
                case 395   : return 0;     //COMPONENT_INFORMATION
                case 397   : return 182;   //COMPONENT_METADATA
                case 400   : return 110;   //PLAY_TUNE_V2
                case 401   : return 183;   //SUPPORTED_TUNES
                case 410   : return 160;   //EVENT
                case 411   : return 106;   //CURRENT_EVENT_SEQUENCE
                case 412   : return 33;    //REQUEST_EVENT
                case 413   : return 77;    //RESPONSE_EVENT_ERROR
                case 9000  : return 113;   //WHEEL_DISTANCE
                case 9005  : return 117;   //WINCH_STATUS
                case 12900 : return 114;   //OPEN_DRONE_ID_BASIC_ID
                case 12901 : return 254;   //OPEN_DRONE_ID_LOCATION
                case 12902 : return 140;   //OPEN_DRONE_ID_AUTHENTICATION
                case 12903 : return 249;   //OPEN_DRONE_ID_SELF_ID
                case 12904 : return 77;    //OPEN_DRONE_ID_SYSTEM
                case 12905 : return 49;    //OPEN_DRONE_ID_OPERATOR_ID
                case 12915 : return 94;    //OPEN_DRONE_ID_MESSAGE_PACK
                case 12918 : return 139;   //OPEN_DRONE_ID_ARM_STATUS
                case 12919 : return 7;     //OPEN_DRONE_ID_SYSTEM_UPDATE
                case 12920 : return 20;    //HYGROMETER_SENSOR
            }            
            return 0;
        }
        #endregion
        
        /// <summary>
        /// Consts
        /// </summary>
        internal const ushort    X25_INIT_CRC     = 0xffff;
        internal const ushort    X25_VALIDATE_CRC = 0xf0b8;

        /// <summary>
        /// LUT
        /// </summary>        
        static public int[] U8_LSH8;
        static public int[] U8_LSH16;
        static public int[] U8_LSH24;
        static public int[] U8_LSH32;
        static public int[] U8_LSH40;
        static public int[] U8_LSH48;
        static public int[] U8_LSH56;

        static public int[] U8_RSH8;
        static public int[] U8_RSH16;
        static public int[] U8_RSH24;
        static public int[] U8_RSH32;
        static public int[] U8_RSH40;
        static public int[] U8_RSH48;
        static public int[] U8_RSH56;

        static public int[] U16_RSH8;

        /*
        static public int[] U16_RSH16;
        static public int[] U16_RSH24;
        static public int[] U16_RSH32;
        static public int[] U16_RSH40;
        static public int[] U16_RSH48;
        static public int[] U16_RSH56;
        //*/

        static public int[] U8_LSH3;
        static public int[] U8_RSH4;        
        static public int[] U8_LSH4;
        static public int[][] U8_XOR;
        static public ushort[][] U16_CRC;

        /// <summary>
        /// CTOR
        /// </summary>
        static MAVLinkCRC() {
            Init();
        }

        #region void Init
        /// <summary>
        /// Initializes LUT and cached info.
        /// </summary>
        static public void Init() {
            if(m_has_init) return;
            m_has_init = true;

            U16_RSH8  = new int[65536]; for(int i=0;i<65536;i++) U16_RSH8 [i] = i>> 8;

            /*
            U16_RSH16 = new int[65536]; for(int i=0;i<65536;i++) U16_RSH16[i] = i>>16;
            U16_RSH24 = new int[65536]; for(int i=0;i<65536;i++) U16_RSH24[i] = i>>24;
            U16_RSH32 = new int[65536]; for(int i=0;i<65536;i++) U16_RSH32[i] = i>>32;
            U16_RSH40 = new int[65536]; for(int i=0;i<65536;i++) U16_RSH40[i] = i>>40;
            U16_RSH48 = new int[65536]; for(int i=0;i<65536;i++) U16_RSH48[i] = i>>48;
            U16_RSH56 = new int[65536]; for(int i=0;i<65536;i++) U16_RSH56[i] = i>>56;
            //*/

            U8_LSH3  = new int[256]; for(int i=0;i<256;i++) U8_LSH3 [i] = i<< 3;
            U8_LSH4  = new int[256]; for(int i=0;i<256;i++) U8_LSH4 [i] = i<< 4;
            U8_LSH8  = new int[256]; for(int i=0;i<256;i++) U8_LSH8 [i] = i<< 8;            
            U8_LSH16 = new int[256]; for(int i=0;i<256;i++) U8_LSH16[i] = i<<16;            
            U8_LSH24 = new int[256]; for(int i=0;i<256;i++) U8_LSH24[i] = i<<24;
            U8_LSH32 = new int[256]; for(int i=0;i<256;i++) U8_LSH32[i] = i<<32;
            U8_LSH40 = new int[256]; for(int i=0;i<256;i++) U8_LSH40[i] = i<<40;
            U8_LSH48 = new int[256]; for(int i=0;i<256;i++) U8_LSH48[i] = i<<48;
            U8_LSH56 = new int[256]; for(int i=0;i<256;i++) U8_LSH56[i] = i<<56;

            U8_RSH4  = new int[256]; for(int i=0;i<256;i++) U8_RSH4 [i] = i>>4;
            U8_RSH8  = new int[256]; for(int i=0;i<256;i++) U8_RSH8 [i] = i>>8;            
            U8_RSH16 = new int[256]; for(int i=0;i<256;i++) U8_RSH16[i] = i>>16;            
            U8_RSH24 = new int[256]; for(int i=0;i<256;i++) U8_RSH24[i] = i>>24;
            U8_RSH32 = new int[256]; for(int i=0;i<256;i++) U8_RSH32[i] = i>>32;
            U8_RSH40 = new int[256]; for(int i=0;i<256;i++) U8_RSH40[i] = i>>40;
            U8_RSH48 = new int[256]; for(int i=0;i<256;i++) U8_RSH48[i] = i>>48;
            U8_RSH56 = new int[256]; for(int i=0;i<256;i++) U8_RSH56[i] = i>>56;

            U8_XOR = new int[256][];
            for(int i=0;i<U8_XOR.Length;i++) U8_XOR[i] = new int[256];
            for(int i=0;i<256;i++) for(int j=0;j<256;j++) U8_XOR[i][j] = i^j;

            U16_CRC = new ushort[65536][];
            for(int i=0;i<U16_CRC.Length;i++) U16_CRC[i] = new ushort[256];
            for(int i=0;i<65536;i++) for(int j=0;j<256  ;j++) { U16_CRC[i][j] = (ushort)i; AccumulateBase(ref U16_CRC[i][j],(byte)j); }

        }
        static bool m_has_init;
        #endregion

        /// <summary>
        /// Given a list of data returns the accumulated CRC.
        /// </summary>
        /// <param name="p_data"></param>
        /// <returns></returns>
        static public ushort GetCRC(List<object> p_data) {
            ushort res = X25_INIT_CRC;
            for(int i=0;i<p_data.Count;i++) {
                object it = p_data[i];
                if(it is byte  ) Accumulate(ref res, (byte  )it); else
                if(it is byte[]) Accumulate(ref res, (byte[])it); else
                if(it is string) Accumulate(ref res, (string)it);
            }
            return res;
        }

        /// <summary>
        /// Returns the CRC16 for the specified data
        /// </summary>
        /// <param name="p_data"></param>
        /// <returns></returns>
        static public ushort GetCRC(byte[] p_data,int p_offset,int p_length) {
            ushort res = X25_INIT_CRC;
            Accumulate(ref res,p_data,p_offset,p_length);
            return res;            
        }

        /// <summary>
        /// Returns the CRC16 for the specified data
        /// </summary>
        /// <param name="p_data"></param>
        /// <returns></returns>
        static public ushort GetCRC(Span<byte> p_data) {
            ushort res = X25_INIT_CRC;
            Accumulate(ref res,p_data);
            return res;            
        }

        /// <summary>
        /// Retruns the byte section for MAVLink
        /// </summary>
        /// <param name="p_crc"></param>
        /// <returns></returns>
        static public byte GetCRCExtra(ushort p_crc) {
            //return (byte)((p_crc & 0xFF) ^ (p_crc >> 8));
            return (byte)((p_crc & 0xFF) ^ (U16_RSH8[p_crc]));
        }
 
        /// <summary>
        /// Accumulate a single byte
        /// </summary>
        /// <param name="p_crc"></param>
        /// <param name="p_data"></param>
        static internal void AccumulateBase(ref ushort p_crc,byte p_data) {
            byte tmp;
            byte crc_low = (byte)(p_crc & 0xff);
            //tmp = (byte)U8_XOR[p_data , crc_low];
            tmp = (byte)U8_XOR[p_data][crc_low];
            //tmp   = (byte)(p_data ^ crc_low);            
            tmp   = (byte)(tmp    ^ tmp<<4 );
            //p_crc = (ushort)((p_crc>>8) ^ (tmp<<8) ^ (tmp <<3) ^ (tmp>>4));
            int xor_a = U16_RSH8[p_crc];
            int xor_b = U8_LSH8[tmp];
            int xor_c = U8_LSH3[tmp];
            int xor_d = U8_RSH4[tmp];
            p_crc = (ushort)(xor_a ^ xor_b ^ xor_c ^ xor_d);
            //p_crc = (ushort)(xor_ab ^ xor_cd);
        }

        /// <summary>
        /// Accumulates a bytem cached version
        /// </summary>
        /// <param name="p_crc"></param>
        /// <param name="p_data"></param>
        static public void Accumulate(ref ushort p_crc,byte p_data) {
           //p_crc = U16_CRC[p_crc,p_data];
           p_crc = U16_CRC[p_crc][p_data];
        }

        /// <summary>
        /// Accumulate RawBytes
        /// </summary>
        /// <param name="data"></param>
        static public void Accumulate(ref ushort p_crc,byte[] p_data,int p_offset,int p_length) {
            for(int i=0;i<p_length; i++) Accumulate(ref p_crc,p_data[p_offset+i]);            
        }

        static public void Accumulate(ref ushort p_crc,byte[] p_data) {
            for(int i=0;i<p_data.Length; i++) Accumulate(ref p_crc,p_data[i]);            
        }

        /// <summary>
        /// Accumulate RawBytes
        /// </summary>
        /// <param name="data"></param>
        static public void Accumulate(ref ushort p_crc,Span<byte> p_data) {
            for(int i=0;i<p_data.Length; i++) Accumulate(ref p_crc,p_data[i]);            
        }

        /// <summary>
        /// Acumulate the string bytes
        /// </summary>
        /// <param name="p_data"></param>
        static public void Accumulate(ref ushort p_crc,string p_data) {            
            for(int i=0;i<p_data.Length; i++) Accumulate(ref p_crc,(byte)p_data[i]);
        }


    }
}
