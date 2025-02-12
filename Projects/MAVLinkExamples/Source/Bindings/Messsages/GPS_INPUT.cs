        
using System.IO;
using System.Runtime.InteropServices;

#pragma warning disable CS0675

namespace MAVLinkBindings {

    /// <summary>
    /// GPS sensor input message.  This is a raw sensor value sent by the GPS. This is NOT the global position estimate of the system.
    /// </summary>    
    public struct GpsInputData : IMAVLinkMessageData {

        /// <summary>
        /// Message Id Associated w/ this Struct
        /// </summary>    
        public int GetId() { return 232; }

        public ulong                TimeUsec;              //Timestamp (UNIX Epoch time or time since system boot). The receiving end can infer timestamp format (since 1.1.1970 or since system boot) by checking for the magnitude of the number.
        public uint                 TimeWeekMs;            //GPS time (from start of GPS week)
        public int                  Lat;                   //Latitude (WGS84)
        public int                  Lon;                   //Longitude (WGS84)
        public float                Alt;                   //Altitude (MSL). Positive for up.
        public float                Hdop;                  //GPS HDOP horizontal dilution of position (unitless). If unknown, set to: UINT16_MAX
        public float                Vdop;                  //GPS VDOP vertical dilution of position (unitless). If unknown, set to: UINT16_MAX
        public float                Vn;                    //GPS velocity in north direction in earth-fixed NED frame
        public float                Ve;                    //GPS velocity in east direction in earth-fixed NED frame
        public float                Vd;                    //GPS velocity in down direction in earth-fixed NED frame
        public float                SpeedAccuracy;         //GPS speed accuracy
        public float                HorizAccuracy;         //GPS horizontal accuracy
        public float                VertAccuracy;          //GPS vertical accuracy
        public GpsInputIgnoreFlags  IgnoreFlags;           //Bitmap indicating which GPS input flags fields to ignore.  All other fields must be provided.
        public ushort               TimeWeek;              //GPS week number
        public byte                 GpsId;                 //ID of the GPS for multiple GPS inputs
        public byte                 FixType;               //0-1: no fix, 2: 2D fix, 3: 3D fix. 4: 3D with DGPS. 5: 3D with RTK
        public byte                 SatellitesVisible;     //Number of satellites visible.
        public ushort               Yaw;                   //Yaw of vehicle relative to Earth's North, zero means not available, use 36000 for north    

        #region CTOR
        /// <summary>
        /// Instantiates a new GpsInputData
        /// </summary>    
        public GpsInputData() {
            TimeUsec                = default(ulong              );
            TimeWeekMs              = default(uint               );
            Lat                     = default(int                );
            Lon                     = default(int                );
            Alt                     = default(float              );
            Hdop                    = default(float              );
            Vdop                    = default(float              );
            Vn                      = default(float              );
            Ve                      = default(float              );
            Vd                      = default(float              );
            SpeedAccuracy           = default(float              );
            HorizAccuracy           = default(float              );
            VertAccuracy            = default(float              );
            IgnoreFlags             = default(GpsInputIgnoreFlags);
            TimeWeek                = default(ushort             );
            GpsId                   = default(byte               );
            FixType                 = default(byte               );
            SatellitesVisible       = default(byte               );
            Yaw                     = default(ushort             );
        }
        #endregion

        #region Read Buffer
        /// <summary>
        /// Reads the data from Buffer into this struct
        /// </summary>    
        public int Read(byte[] p_buffer,int p_offset=0) {
            int    l = 65;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals
            int[]  LS8  = MAVLinkCRC.U8_LSH8, LS16 = MAVLinkCRC.U8_LSH16, LS24 = MAVLinkCRC.U8_LSH24, LS32 = MAVLinkCRC.U8_LSH32, LS40 = MAVLinkCRC.U8_LSH40, LS48 = MAVLinkCRC.U8_LSH48, LS56 = MAVLinkCRC.U8_LSH56;
            Span<byte> b = p_buffer.AsSpan(p_offset);            
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            TimeUsec                = (ulong              ) ((ulong)b[p++] | (ulong)LS8[b[p++]] | (ulong)LS16[b[p++]] | (ulong)LS24[b[p++]] | (ulong)LS32[b[p++]] | (ulong)LS40[b[p++]] | (ulong)LS48[b[p++]] | (ulong)LS56[b[p++]]);
            TimeWeekMs              = (uint               ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            Lat                     = (int                ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            Lon                     = (int                ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            Alt                     = (float              ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Hdop                    = (float              ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Vdop                    = (float              ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Vn                      = (float              ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Ve                      = (float              ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Vd                      = (float              ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            SpeedAccuracy           = (float              ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            HorizAccuracy           = (float              ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            VertAccuracy            = (float              ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            IgnoreFlags             = (GpsInputIgnoreFlags) (b[p++] | LS8[b[p++]]);
            TimeWeek                = (ushort             ) (b[p++] | LS8[b[p++]]);
            GpsId                   = (byte               ) (b[p++]);
            FixType                 = (byte               ) (b[p++]);
            SatellitesVisible       = (byte               ) (b[p++]);
            Yaw                     = (ushort             ) (b[p++] | LS8[b[p++]]);            
            return p;
        }
        #endregion

        #region Write Buffer
        /// <summary>
        /// Writes the message data into a Buffer
        /// </summary>    
        public int Write(byte[] p_buffer,int p_offset=0) {
            int    l = 65;
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
            b[p++] = (byte)(      TimeWeekMs);
            b[p++] = (byte)((int)TimeWeekMs>>8 );
            b[p++] = (byte)((int)TimeWeekMs>>16);
            b[p++] = (byte)((int)TimeWeekMs>>24);
            b[p++] = (byte)(      Lat);
            b[p++] = (byte)((int)Lat>>8 );
            b[p++] = (byte)((int)Lat>>16);
            b[p++] = (byte)((int)Lat>>24);
            b[p++] = (byte)(      Lon);
            b[p++] = (byte)((int)Lon>>8 );
            b[p++] = (byte)((int)Lon>>16);
            b[p++] = (byte)((int)Lon>>24);
            MemoryMarshal.Write(b.Slice(p, 4), in Alt                    ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), in Hdop                   ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), in Vdop                   ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), in Vn                     ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), in Ve                     ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), in Vd                     ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), in SpeedAccuracy          ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), in HorizAccuracy          ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), in VertAccuracy           ); p+=4;
            b[p++] = (byte)(      IgnoreFlags);
            b[p++] = (byte)((int)IgnoreFlags>>8 );
            b[p++] = (byte)(      TimeWeek);
            b[p++] = (byte)((int)TimeWeek>>8 );
            b[p++] = (byte)(GpsId);
            b[p++] = (byte)(FixType);
            b[p++] = (byte)(SatellitesVisible);
            b[p++] = (byte)(      Yaw);
            b[p++] = (byte)((int)Yaw>>8 );
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
            int l = 65;
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
