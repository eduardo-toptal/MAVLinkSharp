        
using System.IO;
using System.Runtime.InteropServices;
using MAVLinkSharp.Runtime;

#pragma warning disable CS0675

namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// Message appropriate for high latency connections like Iridium (version 2)
    /// </summary>    
    public struct HighLatency2Data : IMAVLinkMessageData {

        /// <summary>
        /// Message Id Associated w/ this Struct
        /// </summary>    
        public int GetId() { return 235; }

        public uint               Timestamp;          //Timestamp (milliseconds since boot or Unix epoch)
        public int                Latitude;           //Latitude
        public int                Longitude;          //Longitude
        public ushort             CustomMode;         //A bitfield for use for autopilot-specific flags (2 byte version).
        public short              Altitude;           //Altitude above mean sea level
        public short              TargetAltitude;     //Altitude setpoint
        public ushort             TargetDistance;     //Distance to target waypoint or position
        public ushort             WpNum;              //Current waypoint number
        public HlFailureFlag      FailureFlags;       //Bitmap of failure flags.
        public MAVTypeFlags       Type;               //Type of the MAV (quadrotor, helicopter, etc.)
        public MAVAutopilotFlags  Autopilot;          //Autopilot type / class. Use MAV_AUTOPILOT_INVALID for components that are not flight controllers.
        public byte               Heading;            //Heading
        public byte               TargetHeading;      //Heading setpoint
        public byte               Throttle;           //Throttle
        public byte               Airspeed;           //Airspeed
        public byte               AirspeedSp;         //Airspeed setpoint
        public byte               Groundspeed;        //Groundspeed
        public byte               Windspeed;          //Windspeed
        public byte               WindHeading;        //Wind heading
        public byte               Eph;                //Maximum error horizontal position since last message
        public byte               Epv;                //Maximum error vertical position since last message
        public sbyte              TemperatureAir;     //Air temperature from airspeed sensor
        public sbyte              ClimbRate;          //Maximum climb rate magnitude since last message
        public sbyte              Battery;            //Battery level (-1 if field not provided).
        public sbyte              Custom0;            //Field for custom payload.
        public sbyte              Custom1;            //Field for custom payload.
        public sbyte              Custom2;            //Field for custom payload.    

        #region CTOR
        /// <summary>
        /// Instantiates a new HighLatency2Data
        /// </summary>    
        /*
        public HighLatency2Data() {
            Init();
        }
        */
        public void Init() {
            Timestamp            = default(uint             );
            Latitude             = default(int              );
            Longitude            = default(int              );
            CustomMode           = default(ushort           );
            Altitude             = default(short            );
            TargetAltitude       = default(short            );
            TargetDistance       = default(ushort           );
            WpNum                = default(ushort           );
            FailureFlags         = default(HlFailureFlag    );
            Type                 = default(MAVTypeFlags     );
            Autopilot            = default(MAVAutopilotFlags);
            Heading              = default(byte             );
            TargetHeading        = default(byte             );
            Throttle             = default(byte             );
            Airspeed             = default(byte             );
            AirspeedSp           = default(byte             );
            Groundspeed          = default(byte             );
            Windspeed            = default(byte             );
            WindHeading          = default(byte             );
            Eph                  = default(byte             );
            Epv                  = default(byte             );
            TemperatureAir       = default(sbyte            );
            ClimbRate            = default(sbyte            );
            Battery              = default(sbyte            );
            Custom0              = default(sbyte            );
            Custom1              = default(sbyte            );
            Custom2              = default(sbyte            );
        }
        #endregion

        #region Read Buffer
        /// <summary>
        /// Reads the data from Buffer into this struct
        /// </summary>    
        public int Read(byte[] p_buffer,int p_offset=0) {
            int    l = 42;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals
            int[]  LS8  = MAVLinkCRC.U8_LSH8, LS16 = MAVLinkCRC.U8_LSH16, LS24 = MAVLinkCRC.U8_LSH24, LS32 = MAVLinkCRC.U8_LSH32, LS40 = MAVLinkCRC.U8_LSH40, LS48 = MAVLinkCRC.U8_LSH48, LS56 = MAVLinkCRC.U8_LSH56;
            Span<byte> b = p_buffer.AsSpan(p_offset);            
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            Timestamp            = (uint             ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            Latitude             = (int              ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            Longitude            = (int              ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            CustomMode           = (ushort           ) (b[p++] | LS8[b[p++]]);
            Altitude             = (short            ) (b[p++] | LS8[b[p++]]);
            TargetAltitude       = (short            ) (b[p++] | LS8[b[p++]]);
            TargetDistance       = (ushort           ) (b[p++] | LS8[b[p++]]);
            WpNum                = (ushort           ) (b[p++] | LS8[b[p++]]);
            FailureFlags         = (HlFailureFlag    ) (b[p++] | LS8[b[p++]]);
            Type                 = (MAVTypeFlags     ) (b[p++]);
            Autopilot            = (MAVAutopilotFlags) (b[p++]);
            Heading              = (byte             ) (b[p++]);
            TargetHeading        = (byte             ) (b[p++]);
            Throttle             = (byte             ) (b[p++]);
            Airspeed             = (byte             ) (b[p++]);
            AirspeedSp           = (byte             ) (b[p++]);
            Groundspeed          = (byte             ) (b[p++]);
            Windspeed            = (byte             ) (b[p++]);
            WindHeading          = (byte             ) (b[p++]);
            Eph                  = (byte             ) (b[p++]);
            Epv                  = (byte             ) (b[p++]);
            TemperatureAir       = (sbyte            ) (b[p++]);
            ClimbRate            = (sbyte            ) (b[p++]);
            Battery              = (sbyte            ) (b[p++]);
            Custom0              = (sbyte            ) (b[p++]);
            Custom1              = (sbyte            ) (b[p++]);
            Custom2              = (sbyte            ) (b[p++]);            
            return p;
        }
        #endregion

        #region Write Buffer
        /// <summary>
        /// Writes the message data into a Buffer
        /// </summary>    
        public int Write(byte[] p_buffer,int p_offset=0) {
            int    l = 42;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals            
            Span<byte> b = p_buffer.AsSpan(p_offset);
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            b[p++] = (byte)(      Timestamp);
            b[p++] = (byte)((int)Timestamp>>8 );
            b[p++] = (byte)((int)Timestamp>>16);
            b[p++] = (byte)((int)Timestamp>>24);
            b[p++] = (byte)(      Latitude);
            b[p++] = (byte)((int)Latitude>>8 );
            b[p++] = (byte)((int)Latitude>>16);
            b[p++] = (byte)((int)Latitude>>24);
            b[p++] = (byte)(      Longitude);
            b[p++] = (byte)((int)Longitude>>8 );
            b[p++] = (byte)((int)Longitude>>16);
            b[p++] = (byte)((int)Longitude>>24);
            b[p++] = (byte)(      CustomMode);
            b[p++] = (byte)((int)CustomMode>>8 );
            b[p++] = (byte)(      Altitude);
            b[p++] = (byte)((int)Altitude>>8 );
            b[p++] = (byte)(      TargetAltitude);
            b[p++] = (byte)((int)TargetAltitude>>8 );
            b[p++] = (byte)(      TargetDistance);
            b[p++] = (byte)((int)TargetDistance>>8 );
            b[p++] = (byte)(      WpNum);
            b[p++] = (byte)((int)WpNum>>8 );
            b[p++] = (byte)(      FailureFlags);
            b[p++] = (byte)((int)FailureFlags>>8 );
            b[p++] = (byte)(Type);
            b[p++] = (byte)(Autopilot);
            b[p++] = (byte)(Heading);
            b[p++] = (byte)(TargetHeading);
            b[p++] = (byte)(Throttle);
            b[p++] = (byte)(Airspeed);
            b[p++] = (byte)(AirspeedSp);
            b[p++] = (byte)(Groundspeed);
            b[p++] = (byte)(Windspeed);
            b[p++] = (byte)(WindHeading);
            b[p++] = (byte)(Eph);
            b[p++] = (byte)(Epv);
            b[p++] = (byte)(TemperatureAir);
            b[p++] = (byte)(ClimbRate);
            b[p++] = (byte)(Battery);
            b[p++] = (byte)(Custom0);
            b[p++] = (byte)(Custom1);
            b[p++] = (byte)(Custom2);
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
            int l = 42;
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
