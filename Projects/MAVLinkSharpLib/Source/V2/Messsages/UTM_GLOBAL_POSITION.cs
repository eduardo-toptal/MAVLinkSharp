        
using System.IO;
using System.Runtime.InteropServices;
using MAVLinkSharp.Runtime;

#pragma warning disable CS0675

namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// The global position resulting from GPS and sensor fusion.
    /// </summary>    
    public struct UtmGlobalPositionData : IMAVLinkMessageData {

        /// <summary>
        /// Message Id Associated w/ this Struct
        /// </summary>    
        public int GetId() { return 340; }

        public ulong                Time;            //Time of applicability of position (microseconds since UNIX epoch).
        public int                  Lat;             //Latitude (WGS84)
        public int                  Lon;             //Longitude (WGS84)
        public int                  Alt;             //Altitude (WGS84)
        public int                  RelativeAlt;     //Altitude above ground
        public int                  NextLat;         //Next waypoint, latitude (WGS84)
        public int                  NextLon;         //Next waypoint, longitude (WGS84)
        public int                  NextAlt;         //Next waypoint, altitude (WGS84)
        public short                Vx;              //Ground X speed (latitude, positive north)
        public short                Vy;              //Ground Y speed (longitude, positive east)
        public short                Vz;              //Ground Z speed (altitude, positive down)
        public ushort               HAcc;            //Horizontal position uncertainty (standard deviation)
        public ushort               VAcc;            //Altitude uncertainty (standard deviation)
        public ushort               VelAcc;          //Speed uncertainty (standard deviation)
        public ushort               UpdateRate;      //Time until next update. Set to 0 if unknown or in data driven mode.
        public byte[]               UasId;           //Unique UAS ID.
        public UtmFlightStateFlags  FlightState;     //Flight state
        public UtmDataAvailFlags    Flags;           //Bitwise OR combination of the data available flags.    

        #region CTOR
        /// <summary>
        /// Instantiates a new UtmGlobalPositionData
        /// </summary>    
        /*
        public UtmGlobalPositionData() {
            Init();
        }
        */
        public void Init() {
            Time              = default(ulong              );
            Lat               = default(int                );
            Lon               = default(int                );
            Alt               = default(int                );
            RelativeAlt       = default(int                );
            NextLat           = default(int                );
            NextLon           = default(int                );
            NextAlt           = default(int                );
            Vx                = default(short              );
            Vy                = default(short              );
            Vz                = default(short              );
            HAcc              = default(ushort             );
            VAcc              = default(ushort             );
            VelAcc            = default(ushort             );
            UpdateRate        = default(ushort             );
            UasId             = new byte[ 18];
            FlightState       = default(UtmFlightStateFlags);
            Flags             = default(UtmDataAvailFlags  );
        }
        #endregion

        #region Read Buffer
        /// <summary>
        /// Reads the data from Buffer into this struct
        /// </summary>    
        public int Read(byte[] p_buffer,int p_offset=0) {
            int    l = 70;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals
            int[]  LS8  = MAVLinkCRC.U8_LSH8, LS16 = MAVLinkCRC.U8_LSH16, LS24 = MAVLinkCRC.U8_LSH24, LS32 = MAVLinkCRC.U8_LSH32, LS40 = MAVLinkCRC.U8_LSH40, LS48 = MAVLinkCRC.U8_LSH48, LS56 = MAVLinkCRC.U8_LSH56;
            Span<byte> b = p_buffer.AsSpan(p_offset);            
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            Time              = (ulong              ) ((ulong)b[p++] | (ulong)LS8[b[p++]] | (ulong)LS16[b[p++]] | (ulong)LS24[b[p++]] | (ulong)LS32[b[p++]] | (ulong)LS40[b[p++]] | (ulong)LS48[b[p++]] | (ulong)LS56[b[p++]]);
            Lat               = (int                ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            Lon               = (int                ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            Alt               = (int                ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            RelativeAlt       = (int                ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            NextLat           = (int                ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            NextLon           = (int                ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            NextAlt           = (int                ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            Vx                = (short              ) (b[p++] | LS8[b[p++]]);
            Vy                = (short              ) (b[p++] | LS8[b[p++]]);
            Vz                = (short              ) (b[p++] | LS8[b[p++]]);
            HAcc              = (ushort             ) (b[p++] | LS8[b[p++]]);
            VAcc              = (ushort             ) (b[p++] | LS8[b[p++]]);
            VelAcc            = (ushort             ) (b[p++] | LS8[b[p++]]);
            UpdateRate        = (ushort             ) (b[p++] | LS8[b[p++]]);
            for(int i=0;i<18 ;i++) { UasId[i]          = (byte               ) (b[p++]); }
            FlightState       = (UtmFlightStateFlags) (b[p++]);
            Flags             = (UtmDataAvailFlags  ) (b[p++]);            
            return p;
        }
        #endregion

        #region Write Buffer
        /// <summary>
        /// Writes the message data into a Buffer
        /// </summary>    
        public int Write(byte[] p_buffer,int p_offset=0) {
            int    l = 70;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals            
            Span<byte> b = p_buffer.AsSpan(p_offset);
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            b[p++] = (byte)(      Time);
            b[p++] = (byte)((long)Time>>8 );
            b[p++] = (byte)((long)Time>>16);
            b[p++] = (byte)((long)Time>>24);
            b[p++] = (byte)((long)Time>>32);
            b[p++] = (byte)((long)Time>>40);
            b[p++] = (byte)((long)Time>>48);
            b[p++] = (byte)((long)Time>>56);
            b[p++] = (byte)(      Lat);
            b[p++] = (byte)((int)Lat>>8 );
            b[p++] = (byte)((int)Lat>>16);
            b[p++] = (byte)((int)Lat>>24);
            b[p++] = (byte)(      Lon);
            b[p++] = (byte)((int)Lon>>8 );
            b[p++] = (byte)((int)Lon>>16);
            b[p++] = (byte)((int)Lon>>24);
            b[p++] = (byte)(      Alt);
            b[p++] = (byte)((int)Alt>>8 );
            b[p++] = (byte)((int)Alt>>16);
            b[p++] = (byte)((int)Alt>>24);
            b[p++] = (byte)(      RelativeAlt);
            b[p++] = (byte)((int)RelativeAlt>>8 );
            b[p++] = (byte)((int)RelativeAlt>>16);
            b[p++] = (byte)((int)RelativeAlt>>24);
            b[p++] = (byte)(      NextLat);
            b[p++] = (byte)((int)NextLat>>8 );
            b[p++] = (byte)((int)NextLat>>16);
            b[p++] = (byte)((int)NextLat>>24);
            b[p++] = (byte)(      NextLon);
            b[p++] = (byte)((int)NextLon>>8 );
            b[p++] = (byte)((int)NextLon>>16);
            b[p++] = (byte)((int)NextLon>>24);
            b[p++] = (byte)(      NextAlt);
            b[p++] = (byte)((int)NextAlt>>8 );
            b[p++] = (byte)((int)NextAlt>>16);
            b[p++] = (byte)((int)NextAlt>>24);
            b[p++] = (byte)(      Vx);
            b[p++] = (byte)((int)Vx>>8 );
            b[p++] = (byte)(      Vy);
            b[p++] = (byte)((int)Vy>>8 );
            b[p++] = (byte)(      Vz);
            b[p++] = (byte)((int)Vz>>8 );
            b[p++] = (byte)(      HAcc);
            b[p++] = (byte)((int)HAcc>>8 );
            b[p++] = (byte)(      VAcc);
            b[p++] = (byte)((int)VAcc>>8 );
            b[p++] = (byte)(      VelAcc);
            b[p++] = (byte)((int)VelAcc>>8 );
            b[p++] = (byte)(      UpdateRate);
            b[p++] = (byte)((int)UpdateRate>>8 );
            for(int i=0;i< 18;i++) {
                b[p++] = (byte)(UasId[i]);
            }
            b[p++] = (byte)(FlightState);
            b[p++] = (byte)(Flags);
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
            int l = 70;
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
