        
using System;
using System.IO;
using System.Runtime.InteropServices;
using MAVLinkSharp.Runtime;

#pragma warning disable CS0675

namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// Message appropriate for high latency connections like Iridium
    /// </summary>    
    public struct HighLatencyData : IMAVLinkMessageData {

        /// <summary>
        /// Message Id Associated w/ this Struct
        /// </summary>    
        public int GetId() { return 234; }

        public uint                 CustomMode;           //A bitfield for use for autopilot-specific flags.
        public int                  Latitude;             //Latitude
        public int                  Longitude;            //Longitude
        public short                Roll;                 //roll
        public short                Pitch;                //pitch
        public ushort               Heading;              //heading
        public short                HeadingSp;            //heading setpoint
        public short                AltitudeAmsl;         //Altitude above mean sea level
        public short                AltitudeSp;           //Altitude setpoint relative to the home position
        public ushort               WpDistance;           //distance to target
        public MAVModeFlag          BaseMode;             //Bitmap of enabled system modes.
        public MAVLandedStateFlags  LandedState;          //The landed state. Is set to MAV_LANDED_STATE_UNDEFINED if landed state is unknown.
        public sbyte                Throttle;             //throttle (percentage)
        public byte                 Airspeed;             //airspeed
        public byte                 AirspeedSp;           //airspeed setpoint
        public byte                 Groundspeed;          //groundspeed
        public sbyte                ClimbRate;            //climb rate
        public byte                 GpsNsat;              //Number of satellites visible. If unknown, set to UINT8_MAX
        public GpsFixTypeFlags      GpsFixType;           //GPS Fix type.
        public byte                 BatteryRemaining;     //Remaining battery (percentage)
        public sbyte                Temperature;          //Autopilot temperature (degrees C)
        public sbyte                TemperatureAir;       //Air temperature (degrees C) from airspeed sensor
        public byte                 Failsafe;             //failsafe (each bit represents a failsafe where 0=ok, 1=failsafe active (bit0:RC, bit1:batt, bit2:GPS, bit3:GCS, bit4:fence)
        public byte                 WpNum;                //current waypoint number    

        #region CTOR
        /// <summary>
        /// Instantiates a new HighLatencyData
        /// </summary>    
        /*
        public HighLatencyData() {
            Init();
        }
        */
        public void Init() {
            CustomMode             = default(uint               );
            Latitude               = default(int                );
            Longitude              = default(int                );
            Roll                   = default(short              );
            Pitch                  = default(short              );
            Heading                = default(ushort             );
            HeadingSp              = default(short              );
            AltitudeAmsl           = default(short              );
            AltitudeSp             = default(short              );
            WpDistance             = default(ushort             );
            BaseMode               = default(MAVModeFlag        );
            LandedState            = default(MAVLandedStateFlags);
            Throttle               = default(sbyte              );
            Airspeed               = default(byte               );
            AirspeedSp             = default(byte               );
            Groundspeed            = default(byte               );
            ClimbRate              = default(sbyte              );
            GpsNsat                = default(byte               );
            GpsFixType             = default(GpsFixTypeFlags    );
            BatteryRemaining       = default(byte               );
            Temperature            = default(sbyte              );
            TemperatureAir         = default(sbyte              );
            Failsafe               = default(byte               );
            WpNum                  = default(byte               );
        }
        #endregion

        #region Read Buffer
        /// <summary>
        /// Reads the data from Buffer into this struct
        /// </summary>    
        public int Read(byte[] p_buffer,int p_offset=0) {
            int    l = 40;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals
            int[]  LS8  = MAVLinkCRC.U8_LSH8, LS16 = MAVLinkCRC.U8_LSH16, LS24 = MAVLinkCRC.U8_LSH24, LS32 = MAVLinkCRC.U8_LSH32, LS40 = MAVLinkCRC.U8_LSH40, LS48 = MAVLinkCRC.U8_LSH48, LS56 = MAVLinkCRC.U8_LSH56;
            Span<byte> b = p_buffer.AsSpan(p_offset);            
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            CustomMode             = (uint               ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            Latitude               = (int                ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            Longitude              = (int                ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            Roll                   = (short              ) (b[p++] | LS8[b[p++]]);
            Pitch                  = (short              ) (b[p++] | LS8[b[p++]]);
            Heading                = (ushort             ) (b[p++] | LS8[b[p++]]);
            HeadingSp              = (short              ) (b[p++] | LS8[b[p++]]);
            AltitudeAmsl           = (short              ) (b[p++] | LS8[b[p++]]);
            AltitudeSp             = (short              ) (b[p++] | LS8[b[p++]]);
            WpDistance             = (ushort             ) (b[p++] | LS8[b[p++]]);
            BaseMode               = (MAVModeFlag        ) (b[p++]);
            LandedState            = (MAVLandedStateFlags) (b[p++]);
            Throttle               = (sbyte              ) (b[p++]);
            Airspeed               = (byte               ) (b[p++]);
            AirspeedSp             = (byte               ) (b[p++]);
            Groundspeed            = (byte               ) (b[p++]);
            ClimbRate              = (sbyte              ) (b[p++]);
            GpsNsat                = (byte               ) (b[p++]);
            GpsFixType             = (GpsFixTypeFlags    ) (b[p++]);
            BatteryRemaining       = (byte               ) (b[p++]);
            Temperature            = (sbyte              ) (b[p++]);
            TemperatureAir         = (sbyte              ) (b[p++]);
            Failsafe               = (byte               ) (b[p++]);
            WpNum                  = (byte               ) (b[p++]);            
            return p;
        }
        #endregion

        #region Write Buffer
        /// <summary>
        /// Writes the message data into a Buffer
        /// </summary>    
        public int Write(byte[] p_buffer,int p_offset=0) {
            int    l = 40;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals            
            Span<byte> b = p_buffer.AsSpan(p_offset);
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            b[p++] = (byte)(      CustomMode);
            b[p++] = (byte)((int)CustomMode>>8 );
            b[p++] = (byte)((int)CustomMode>>16);
            b[p++] = (byte)((int)CustomMode>>24);
            b[p++] = (byte)(      Latitude);
            b[p++] = (byte)((int)Latitude>>8 );
            b[p++] = (byte)((int)Latitude>>16);
            b[p++] = (byte)((int)Latitude>>24);
            b[p++] = (byte)(      Longitude);
            b[p++] = (byte)((int)Longitude>>8 );
            b[p++] = (byte)((int)Longitude>>16);
            b[p++] = (byte)((int)Longitude>>24);
            b[p++] = (byte)(      Roll);
            b[p++] = (byte)((int)Roll>>8 );
            b[p++] = (byte)(      Pitch);
            b[p++] = (byte)((int)Pitch>>8 );
            b[p++] = (byte)(      Heading);
            b[p++] = (byte)((int)Heading>>8 );
            b[p++] = (byte)(      HeadingSp);
            b[p++] = (byte)((int)HeadingSp>>8 );
            b[p++] = (byte)(      AltitudeAmsl);
            b[p++] = (byte)((int)AltitudeAmsl>>8 );
            b[p++] = (byte)(      AltitudeSp);
            b[p++] = (byte)((int)AltitudeSp>>8 );
            b[p++] = (byte)(      WpDistance);
            b[p++] = (byte)((int)WpDistance>>8 );
            b[p++] = (byte)(BaseMode);
            b[p++] = (byte)(LandedState);
            b[p++] = (byte)(Throttle);
            b[p++] = (byte)(Airspeed);
            b[p++] = (byte)(AirspeedSp);
            b[p++] = (byte)(Groundspeed);
            b[p++] = (byte)(ClimbRate);
            b[p++] = (byte)(GpsNsat);
            b[p++] = (byte)(GpsFixType);
            b[p++] = (byte)(BatteryRemaining);
            b[p++] = (byte)(Temperature);
            b[p++] = (byte)(TemperatureAir);
            b[p++] = (byte)(Failsafe);
            b[p++] = (byte)(WpNum);
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
            int l = 40;
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
