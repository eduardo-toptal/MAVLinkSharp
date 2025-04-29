        
using System.IO;
using System.Runtime.InteropServices;
using MAVLinkSharp.Runtime;

#pragma warning disable CS0675

namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// Second GPS data.
    /// </summary>    
    public struct Gps2RawData : IMAVLinkMessageData {

        /// <summary>
        /// Message Id Associated w/ this Struct
        /// </summary>    
        public int GetId() { return 124; }

        public ulong            TimeUsec;              //Timestamp (UNIX Epoch time or time since system boot). The receiving end can infer timestamp format (since 1.1.1970 or since system boot) by checking for the magnitude of the number.
        public int              Lat;                   //Latitude (WGS84)
        public int              Lon;                   //Longitude (WGS84)
        public int              Alt;                   //Altitude (MSL). Positive for up.
        public uint             DgpsAge;               //Age of DGPS info
        public ushort           Eph;                   //GPS HDOP horizontal dilution of position (unitless * 100). If unknown, set to: UINT16_MAX
        public ushort           Epv;                   //GPS VDOP vertical dilution of position (unitless * 100). If unknown, set to: UINT16_MAX
        public ushort           Vel;                   //GPS ground speed. If unknown, set to: UINT16_MAX
        public ushort           Cog;                   //Course over ground (NOT heading, but direction of movement): 0.0..359.99 degrees. If unknown, set to: UINT16_MAX
        public GpsFixTypeFlags  FixType;               //GPS fix type.
        public byte             SatellitesVisible;     //Number of satellites visible. If unknown, set to UINT8_MAX
        public byte             DgpsNumch;             //Number of DGPS satellites
        public ushort           Yaw;                   //Yaw in earth frame from north. Use 0 if this GPS does not provide yaw. Use UINT16_MAX if this GPS is configured to provide yaw and is currently unable to provide it. Use 36000 for north.
        public int              AltEllipsoid;          //Altitude (above WGS84, EGM96 ellipsoid). Positive for up.
        public uint             HAcc;                  //Position uncertainty.
        public uint             VAcc;                  //Altitude uncertainty.
        public uint             VelAcc;                //Speed uncertainty.
        public uint             HdgAcc;                //Heading / track uncertainty    

        #region CTOR
        /// <summary>
        /// Instantiates a new Gps2RawData
        /// </summary>    
        public Gps2RawData() {
            TimeUsec                = default(ulong          );
            Lat                     = default(int            );
            Lon                     = default(int            );
            Alt                     = default(int            );
            DgpsAge                 = default(uint           );
            Eph                     = default(ushort         );
            Epv                     = default(ushort         );
            Vel                     = default(ushort         );
            Cog                     = default(ushort         );
            FixType                 = default(GpsFixTypeFlags);
            SatellitesVisible       = default(byte           );
            DgpsNumch               = default(byte           );
            Yaw                     = default(ushort         );
            AltEllipsoid            = default(int            );
            HAcc                    = default(uint           );
            VAcc                    = default(uint           );
            VelAcc                  = default(uint           );
            HdgAcc                  = default(uint           );
        }
        #endregion

        #region Read Buffer
        /// <summary>
        /// Reads the data from Buffer into this struct
        /// </summary>    
        public int Read(byte[] p_buffer,int p_offset=0) {
            int    l = 57;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals
            int[]  LS8  = MAVLinkCRC.U8_LSH8, LS16 = MAVLinkCRC.U8_LSH16, LS24 = MAVLinkCRC.U8_LSH24, LS32 = MAVLinkCRC.U8_LSH32, LS40 = MAVLinkCRC.U8_LSH40, LS48 = MAVLinkCRC.U8_LSH48, LS56 = MAVLinkCRC.U8_LSH56;
            Span<byte> b = p_buffer.AsSpan(p_offset);            
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            TimeUsec                = (ulong          ) ((ulong)b[p++] | (ulong)LS8[b[p++]] | (ulong)LS16[b[p++]] | (ulong)LS24[b[p++]] | (ulong)LS32[b[p++]] | (ulong)LS40[b[p++]] | (ulong)LS48[b[p++]] | (ulong)LS56[b[p++]]);
            Lat                     = (int            ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            Lon                     = (int            ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            Alt                     = (int            ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            DgpsAge                 = (uint           ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            Eph                     = (ushort         ) (b[p++] | LS8[b[p++]]);
            Epv                     = (ushort         ) (b[p++] | LS8[b[p++]]);
            Vel                     = (ushort         ) (b[p++] | LS8[b[p++]]);
            Cog                     = (ushort         ) (b[p++] | LS8[b[p++]]);
            FixType                 = (GpsFixTypeFlags) (b[p++]);
            SatellitesVisible       = (byte           ) (b[p++]);
            DgpsNumch               = (byte           ) (b[p++]);
            Yaw                     = (ushort         ) (b[p++] | LS8[b[p++]]);
            AltEllipsoid            = (int            ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            HAcc                    = (uint           ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            VAcc                    = (uint           ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            VelAcc                  = (uint           ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            HdgAcc                  = (uint           ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);            
            return p;
        }
        #endregion

        #region Write Buffer
        /// <summary>
        /// Writes the message data into a Buffer
        /// </summary>    
        public int Write(byte[] p_buffer,int p_offset=0) {
            int    l = 57;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals            
            Span<byte> b = p_buffer.AsSpan(p_offset);
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            b[p++] = (byte)(      TimeUsec);
            b[p++] = (byte)((long)TimeUsec>>8 );
            b[p++] = (byte)((long)TimeUsec>>16);
            b[p++] = (byte)((long)TimeUsec>>24);
            b[p++] = (byte)((long)TimeUsec>>32);
            b[p++] = (byte)((long)TimeUsec>>40);
            b[p++] = (byte)((long)TimeUsec>>48);
            b[p++] = (byte)((long)TimeUsec>>56);
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
            b[p++] = (byte)(      DgpsAge);
            b[p++] = (byte)((int)DgpsAge>>8 );
            b[p++] = (byte)((int)DgpsAge>>16);
            b[p++] = (byte)((int)DgpsAge>>24);
            b[p++] = (byte)(      Eph);
            b[p++] = (byte)((int)Eph>>8 );
            b[p++] = (byte)(      Epv);
            b[p++] = (byte)((int)Epv>>8 );
            b[p++] = (byte)(      Vel);
            b[p++] = (byte)((int)Vel>>8 );
            b[p++] = (byte)(      Cog);
            b[p++] = (byte)((int)Cog>>8 );
            b[p++] = (byte)(FixType);
            b[p++] = (byte)(SatellitesVisible);
            b[p++] = (byte)(DgpsNumch);
            b[p++] = (byte)(      Yaw);
            b[p++] = (byte)((int)Yaw>>8 );
            b[p++] = (byte)(      AltEllipsoid);
            b[p++] = (byte)((int)AltEllipsoid>>8 );
            b[p++] = (byte)((int)AltEllipsoid>>16);
            b[p++] = (byte)((int)AltEllipsoid>>24);
            b[p++] = (byte)(      HAcc);
            b[p++] = (byte)((int)HAcc>>8 );
            b[p++] = (byte)((int)HAcc>>16);
            b[p++] = (byte)((int)HAcc>>24);
            b[p++] = (byte)(      VAcc);
            b[p++] = (byte)((int)VAcc>>8 );
            b[p++] = (byte)((int)VAcc>>16);
            b[p++] = (byte)((int)VAcc>>24);
            b[p++] = (byte)(      VelAcc);
            b[p++] = (byte)((int)VelAcc>>8 );
            b[p++] = (byte)((int)VelAcc>>16);
            b[p++] = (byte)((int)VelAcc>>24);
            b[p++] = (byte)(      HdgAcc);
            b[p++] = (byte)((int)HdgAcc>>8 );
            b[p++] = (byte)((int)HdgAcc>>16);
            b[p++] = (byte)((int)HdgAcc>>24);
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
            int l = 57;
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
