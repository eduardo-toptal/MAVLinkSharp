        
using System.IO;
using System.Runtime.InteropServices;
using MAVLinkSharp.Runtime;

#pragma warning disable CS0675

namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// The general system state. If the system is following the MAVLink standard, the system state is mainly defined by three orthogonal states/modes: The system mode, which is either LOCKED (motors shut down and locked), MANUAL (system under RC control), GUIDED (system with autonomous position control, position setpoint controlled manually) or AUTO (system guided by path/waypoint planner). The NAV_MODE defined the current flight state: LIFTOFF (often an open-loop maneuver), LANDING, WAYPOINTS or VECTOR. This represents the internal navigation state machine. The system status shows whether the system is currently active or not and if an emergency occurred. During the CRITICAL and EMERGENCY states the MAV is still considered to be active, but should start emergency procedures autonomously. After a failure occurred it should first move from active to critical to allow manual intervention and then move to emergency after a certain timeout.
    /// </summary>    
    public struct SysStatusData : IMAVLinkMessageData {

        /// <summary>
        /// Message Id Associated w/ this Struct
        /// </summary>    
        public int GetId() { return 1; }

        public MAVSysStatusSensorFlags          OnboardControlSensorsPresent;                //Bitmap showing which onboard controllers and sensors are present. Value of 0: not present. Value of 1: present.
        public MAVSysStatusSensorFlags          OnboardControlSensorsEnabled;                //Bitmap showing which onboard controllers and sensors are enabled:  Value of 0: not enabled. Value of 1: enabled.
        public MAVSysStatusSensorFlags          OnboardControlSensorsHealth;                 //Bitmap showing which onboard controllers and sensors have an error (or are operational). Value of 0: error. Value of 1: healthy.
        public ushort                           Load;                                        //Maximum usage in percent of the mainloop time. Values: [0-1000] - should always be below 1000
        public ushort                           VoltageBattery;                              //Battery voltage, UINT16_MAX: Voltage not sent by autopilot
        public short                            CurrentBattery;                              //Battery current, -1: Current not sent by autopilot
        public ushort                           DropRateComm;                                //Communication drop rate, (UART, I2C, SPI, CAN), dropped packets on all links (packets that were corrupted on reception on the MAV)
        public ushort                           ErrorsComm;                                  //Communication errors (UART, I2C, SPI, CAN), dropped packets on all links (packets that were corrupted on reception on the MAV)
        public ushort                           ErrorsCount1;                                //Autopilot-specific errors
        public ushort                           ErrorsCount2;                                //Autopilot-specific errors
        public ushort                           ErrorsCount3;                                //Autopilot-specific errors
        public ushort                           ErrorsCount4;                                //Autopilot-specific errors
        public sbyte                            BatteryRemaining;                            //Battery energy remaining, -1: Battery remaining energy not sent by autopilot
        public MAVSysStatusSensorExtendedFlags  OnboardControlSensorsPresentExtended;        //Bitmap showing which onboard controllers and sensors are present. Value of 0: not present. Value of 1: present.
        public MAVSysStatusSensorExtendedFlags  OnboardControlSensorsEnabledExtended;        //Bitmap showing which onboard controllers and sensors are enabled:  Value of 0: not enabled. Value of 1: enabled.
        public MAVSysStatusSensorExtendedFlags  OnboardControlSensorsHealthExtended;         //Bitmap showing which onboard controllers and sensors have an error (or are operational). Value of 0: error. Value of 1: healthy.    

        #region CTOR
        /// <summary>
        /// Instantiates a new SysStatusData
        /// </summary>    
        /*
        public SysStatusData() {
            Init();
        }
        */
        public void Init() {
            OnboardControlSensorsPresent                  = default(MAVSysStatusSensorFlags        );
            OnboardControlSensorsEnabled                  = default(MAVSysStatusSensorFlags        );
            OnboardControlSensorsHealth                   = default(MAVSysStatusSensorFlags        );
            Load                                          = default(ushort                         );
            VoltageBattery                                = default(ushort                         );
            CurrentBattery                                = default(short                          );
            DropRateComm                                  = default(ushort                         );
            ErrorsComm                                    = default(ushort                         );
            ErrorsCount1                                  = default(ushort                         );
            ErrorsCount2                                  = default(ushort                         );
            ErrorsCount3                                  = default(ushort                         );
            ErrorsCount4                                  = default(ushort                         );
            BatteryRemaining                              = default(sbyte                          );
            OnboardControlSensorsPresentExtended          = default(MAVSysStatusSensorExtendedFlags);
            OnboardControlSensorsEnabledExtended          = default(MAVSysStatusSensorExtendedFlags);
            OnboardControlSensorsHealthExtended           = default(MAVSysStatusSensorExtendedFlags);
        }
        #endregion

        #region Read Buffer
        /// <summary>
        /// Reads the data from Buffer into this struct
        /// </summary>    
        public int Read(byte[] p_buffer,int p_offset=0) {
            int    l = 43;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals
            int[]  LS8  = MAVLinkCRC.U8_LSH8, LS16 = MAVLinkCRC.U8_LSH16, LS24 = MAVLinkCRC.U8_LSH24, LS32 = MAVLinkCRC.U8_LSH32, LS40 = MAVLinkCRC.U8_LSH40, LS48 = MAVLinkCRC.U8_LSH48, LS56 = MAVLinkCRC.U8_LSH56;
            Span<byte> b = p_buffer.AsSpan(p_offset);            
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            OnboardControlSensorsPresent                  = (MAVSysStatusSensorFlags        ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            OnboardControlSensorsEnabled                  = (MAVSysStatusSensorFlags        ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            OnboardControlSensorsHealth                   = (MAVSysStatusSensorFlags        ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            Load                                          = (ushort                         ) (b[p++] | LS8[b[p++]]);
            VoltageBattery                                = (ushort                         ) (b[p++] | LS8[b[p++]]);
            CurrentBattery                                = (short                          ) (b[p++] | LS8[b[p++]]);
            DropRateComm                                  = (ushort                         ) (b[p++] | LS8[b[p++]]);
            ErrorsComm                                    = (ushort                         ) (b[p++] | LS8[b[p++]]);
            ErrorsCount1                                  = (ushort                         ) (b[p++] | LS8[b[p++]]);
            ErrorsCount2                                  = (ushort                         ) (b[p++] | LS8[b[p++]]);
            ErrorsCount3                                  = (ushort                         ) (b[p++] | LS8[b[p++]]);
            ErrorsCount4                                  = (ushort                         ) (b[p++] | LS8[b[p++]]);
            BatteryRemaining                              = (sbyte                          ) (b[p++]);
            OnboardControlSensorsPresentExtended          = (MAVSysStatusSensorExtendedFlags) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            OnboardControlSensorsEnabledExtended          = (MAVSysStatusSensorExtendedFlags) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            OnboardControlSensorsHealthExtended           = (MAVSysStatusSensorExtendedFlags) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);            
            return p;
        }
        #endregion

        #region Write Buffer
        /// <summary>
        /// Writes the message data into a Buffer
        /// </summary>    
        public int Write(byte[] p_buffer,int p_offset=0) {
            int    l = 43;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals            
            Span<byte> b = p_buffer.AsSpan(p_offset);
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            b[p++] = (byte)(      OnboardControlSensorsPresent);
            b[p++] = (byte)((int)OnboardControlSensorsPresent>>8 );
            b[p++] = (byte)((int)OnboardControlSensorsPresent>>16);
            b[p++] = (byte)((int)OnboardControlSensorsPresent>>24);
            b[p++] = (byte)(      OnboardControlSensorsEnabled);
            b[p++] = (byte)((int)OnboardControlSensorsEnabled>>8 );
            b[p++] = (byte)((int)OnboardControlSensorsEnabled>>16);
            b[p++] = (byte)((int)OnboardControlSensorsEnabled>>24);
            b[p++] = (byte)(      OnboardControlSensorsHealth);
            b[p++] = (byte)((int)OnboardControlSensorsHealth>>8 );
            b[p++] = (byte)((int)OnboardControlSensorsHealth>>16);
            b[p++] = (byte)((int)OnboardControlSensorsHealth>>24);
            b[p++] = (byte)(      Load);
            b[p++] = (byte)((int)Load>>8 );
            b[p++] = (byte)(      VoltageBattery);
            b[p++] = (byte)((int)VoltageBattery>>8 );
            b[p++] = (byte)(      CurrentBattery);
            b[p++] = (byte)((int)CurrentBattery>>8 );
            b[p++] = (byte)(      DropRateComm);
            b[p++] = (byte)((int)DropRateComm>>8 );
            b[p++] = (byte)(      ErrorsComm);
            b[p++] = (byte)((int)ErrorsComm>>8 );
            b[p++] = (byte)(      ErrorsCount1);
            b[p++] = (byte)((int)ErrorsCount1>>8 );
            b[p++] = (byte)(      ErrorsCount2);
            b[p++] = (byte)((int)ErrorsCount2>>8 );
            b[p++] = (byte)(      ErrorsCount3);
            b[p++] = (byte)((int)ErrorsCount3>>8 );
            b[p++] = (byte)(      ErrorsCount4);
            b[p++] = (byte)((int)ErrorsCount4>>8 );
            b[p++] = (byte)(BatteryRemaining);
            b[p++] = (byte)(      OnboardControlSensorsPresentExtended);
            b[p++] = (byte)((int)OnboardControlSensorsPresentExtended>>8 );
            b[p++] = (byte)((int)OnboardControlSensorsPresentExtended>>16);
            b[p++] = (byte)((int)OnboardControlSensorsPresentExtended>>24);
            b[p++] = (byte)(      OnboardControlSensorsEnabledExtended);
            b[p++] = (byte)((int)OnboardControlSensorsEnabledExtended>>8 );
            b[p++] = (byte)((int)OnboardControlSensorsEnabledExtended>>16);
            b[p++] = (byte)((int)OnboardControlSensorsEnabledExtended>>24);
            b[p++] = (byte)(      OnboardControlSensorsHealthExtended);
            b[p++] = (byte)((int)OnboardControlSensorsHealthExtended>>8 );
            b[p++] = (byte)((int)OnboardControlSensorsHealthExtended>>16);
            b[p++] = (byte)((int)OnboardControlSensorsHealthExtended>>24);
            return p;
        }
        #endregion

        #region Read Stream
        /// <summary>
        /// Reads the struct data from a stream
        /// </summary>
        /// <param name="p_stream"></param>
        /// <returns></returns>
        public int Read(Stream p_stream) {
            Stream ss = p_stream;
            if(ss==null) return 0;
            int l = 43;
            if(ss.Length - ss.Position < l) return 0;
            byte[] b;            
            long p = ss.Position;
            if(ss is MemoryStream) {
                MemoryStream ms = ( MemoryStream ) ss;
                b = ms.GetBuffer();
            }
            else {
                b = new byte[l];
                p = 0;
                ss.Read(b,0,l);                
            }
            return Read(b,(int)p);
        }
        #endregion

        #region Write Stream
        /// <summary>
        /// Writes the struct data into a Stream
        /// </summary>
        /// <param name="p_stream"></param>
        /// <returns></returns>
        public int Write(Stream p_stream) {
            Stream ss = p_stream;
            if(ss==null) return 0;
            MemoryStream ms = new MemoryStream();
            int c = Read(ms);
            ms.Position=0;
            ms.CopyTo(ss);            
            ms.Close();
            return c;
        }
        #endregion

    }

}
