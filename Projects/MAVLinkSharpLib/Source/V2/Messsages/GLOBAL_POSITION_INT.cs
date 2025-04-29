        
using System;
using System.IO;
using System.Runtime.InteropServices;
using MAVLinkSharp.Runtime;

#pragma warning disable CS0675

namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// The filtered global position (e.g. fused GPS and accelerometers). The position is in GPS-frame (right-handed, Z-up). It
    /// is designed as scaled integer message since the resolution of float is not sufficient.
    /// </summary>    
    public struct GlobalPositionIntData : IMAVLinkMessageData {

        /// <summary>
        /// Message Id Associated w/ this Struct
        /// </summary>    
        public int GetId() { return 33; }

        public uint    TimeBootMs;      //Timestamp (time since system boot).
        public int     Lat;             //Latitude, expressed
        public int     Lon;             //Longitude, expressed
        public int     Alt;             //Altitude (MSL). Note that virtually all GPS modules provide both WGS84 and MSL.
        public int     RelativeAlt;     //Altitude above ground
        public short   Vx;              //Ground X Speed (Latitude, positive north)
        public short   Vy;              //Ground Y Speed (Longitude, positive east)
        public short   Vz;              //Ground Z Speed (Altitude, positive down)
        public ushort  Hdg;             //Vehicle heading (yaw angle), 0.0..359.99 degrees. If unknown, set to: UINT16_MAX    

        #region CTOR
        /// <summary>
        /// Instantiates a new GlobalPositionIntData
        /// </summary>    
        /*
        public GlobalPositionIntData() {
            Init();
        }
        */
        public void Init() {
            TimeBootMs        = default(uint  );
            Lat               = default(int   );
            Lon               = default(int   );
            Alt               = default(int   );
            RelativeAlt       = default(int   );
            Vx                = default(short );
            Vy                = default(short );
            Vz                = default(short );
            Hdg               = default(ushort);
        }
        #endregion

        #region Read Buffer
        /// <summary>
        /// Reads the data from Buffer into this struct
        /// </summary>    
        public int Read(byte[] p_buffer,int p_offset=0) {
            int    l = 28;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals
            int[]  LS8  = MAVLinkCRC.U8_LSH8, LS16 = MAVLinkCRC.U8_LSH16, LS24 = MAVLinkCRC.U8_LSH24, LS32 = MAVLinkCRC.U8_LSH32, LS40 = MAVLinkCRC.U8_LSH40, LS48 = MAVLinkCRC.U8_LSH48, LS56 = MAVLinkCRC.U8_LSH56;
            Span<byte> b = p_buffer.AsSpan(p_offset);            
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            TimeBootMs        = (uint  ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            Lat               = (int   ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            Lon               = (int   ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            Alt               = (int   ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            RelativeAlt       = (int   ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            Vx                = (short ) (b[p++] | LS8[b[p++]]);
            Vy                = (short ) (b[p++] | LS8[b[p++]]);
            Vz                = (short ) (b[p++] | LS8[b[p++]]);
            Hdg               = (ushort) (b[p++] | LS8[b[p++]]);            
            return p;
        }
        #endregion

        #region Write Buffer
        /// <summary>
        /// Writes the message data into a Buffer
        /// </summary>    
        public int Write(byte[] p_buffer,int p_offset=0) {
            int    l = 28;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals            
            Span<byte> b = p_buffer.AsSpan(p_offset);
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            b[p++] = (byte)(      TimeBootMs);
            b[p++] = (byte)((int)TimeBootMs>>8 );
            b[p++] = (byte)((int)TimeBootMs>>16);
            b[p++] = (byte)((int)TimeBootMs>>24);
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
            b[p++] = (byte)(      Vx);
            b[p++] = (byte)((int)Vx>>8 );
            b[p++] = (byte)(      Vy);
            b[p++] = (byte)((int)Vy>>8 );
            b[p++] = (byte)(      Vz);
            b[p++] = (byte)((int)Vz>>8 );
            b[p++] = (byte)(      Hdg);
            b[p++] = (byte)((int)Hdg>>8 );
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
            int l = 28;
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
