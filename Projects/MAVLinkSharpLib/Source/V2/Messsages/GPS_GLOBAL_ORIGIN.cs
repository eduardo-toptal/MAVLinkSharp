        
using System;
using System.IO;
using System.Runtime.InteropServices;
using MAVLinkSharp.Runtime;

#pragma warning disable CS0675

namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// Publishes the GPS coordinates of the vehicle local origin (0,0,0) position. Emitted whenever a new GPS-Local position mapping is requested or set - e.g. following SET_GPS_GLOBAL_ORIGIN message.
    /// </summary>    
    public struct GpsGlobalOriginData : IMAVLinkMessageData {

        /// <summary>
        /// Message Id Associated w/ this Struct
        /// </summary>    
        public int GetId() { return 49; }

        public int    Latitude;     //Latitude (WGS84)
        public int    Longitude;    //Longitude (WGS84)
        public int    Altitude;     //Altitude (MSL). Positive for up.
        public ulong  TimeUsec;     //Timestamp (UNIX Epoch time or time since system boot). The receiving end can infer timestamp format (since 1.1.1970 or since system boot) by checking for the magnitude of the number.    

        #region CTOR
        /// <summary>
        /// Instantiates a new GpsGlobalOriginData
        /// </summary>    
        /*
        public GpsGlobalOriginData() {
            Init();
        }
        */
        public void Init() {
            Latitude       = default(int  );
            Longitude      = default(int  );
            Altitude       = default(int  );
            TimeUsec       = default(ulong);
        }
        #endregion

        #region Read Buffer
        /// <summary>
        /// Reads the data from Buffer into this struct
        /// </summary>    
        public int Read(byte[] p_buffer,int p_offset=0) {
            int    l = 20;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals
            int[]  LS8  = MAVLinkCRC.U8_LSH8, LS16 = MAVLinkCRC.U8_LSH16, LS24 = MAVLinkCRC.U8_LSH24, LS32 = MAVLinkCRC.U8_LSH32, LS40 = MAVLinkCRC.U8_LSH40, LS48 = MAVLinkCRC.U8_LSH48, LS56 = MAVLinkCRC.U8_LSH56;
            Span<byte> b = p_buffer.AsSpan(p_offset);            
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            Latitude       = (int  ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            Longitude      = (int  ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            Altitude       = (int  ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            TimeUsec       = (ulong) ((ulong)b[p++] | (ulong)LS8[b[p++]] | (ulong)LS16[b[p++]] | (ulong)LS24[b[p++]] | (ulong)LS32[b[p++]] | (ulong)LS40[b[p++]] | (ulong)LS48[b[p++]] | (ulong)LS56[b[p++]]);            
            return p;
        }
        #endregion

        #region Write Buffer
        /// <summary>
        /// Writes the message data into a Buffer
        /// </summary>    
        public int Write(byte[] p_buffer,int p_offset=0) {
            int    l = 20;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals            
            Span<byte> b = p_buffer.AsSpan(p_offset);
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            b[p++] = (byte)(      Latitude);
            b[p++] = (byte)((int)Latitude>>8 );
            b[p++] = (byte)((int)Latitude>>16);
            b[p++] = (byte)((int)Latitude>>24);
            b[p++] = (byte)(      Longitude);
            b[p++] = (byte)((int)Longitude>>8 );
            b[p++] = (byte)((int)Longitude>>16);
            b[p++] = (byte)((int)Longitude>>24);
            b[p++] = (byte)(      Altitude);
            b[p++] = (byte)((int)Altitude>>8 );
            b[p++] = (byte)((int)Altitude>>16);
            b[p++] = (byte)((int)Altitude>>24);
            b[p++] = (byte)(      TimeUsec);
            b[p++] = (byte)((long)TimeUsec>>8 );
            b[p++] = (byte)((long)TimeUsec>>16);
            b[p++] = (byte)((long)TimeUsec>>24);
            b[p++] = (byte)((long)TimeUsec>>32);
            b[p++] = (byte)((long)TimeUsec>>40);
            b[p++] = (byte)((long)TimeUsec>>48);
            b[p++] = (byte)((long)TimeUsec>>56);
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
            int l = 20;
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
