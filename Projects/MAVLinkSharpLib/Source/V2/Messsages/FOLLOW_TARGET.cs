        
using System.IO;
using System.Runtime.InteropServices;
using MAVLinkSharp.Runtime;

#pragma warning disable CS0675

namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// Current motion information from a designated system
    /// </summary>    
    public struct FollowTargetData : IMAVLinkMessageData {

        /// <summary>
        /// Message Id Associated w/ this Struct
        /// </summary>    
        public int GetId() { return 144; }

        public ulong    Timestamp;           //Timestamp (time since system boot).
        public ulong    CustomState;         //button states or switches of a tracker device
        public int      Lat;                 //Latitude (WGS84)
        public int      Lon;                 //Longitude (WGS84)
        public float    Alt;                 //Altitude (MSL)
        public float[]  Vel;                 //target velocity (0,0,0) for unknown
        public float[]  Acc;                 //linear target acceleration (0,0,0) for unknown
        public float[]  AttitudeQ;           //(0 0 0 0 for unknown)
        public float[]  Rates;               //(0 0 0 for unknown)
        public float[]  PositionCov;         //eph epv
        public byte     EstCapabilities;     //bit positions for tracker reporting capabilities (POS = 0, VEL = 1, ACCEL = 2, ATT + RATES = 3)    

        #region CTOR
        /// <summary>
        /// Instantiates a new FollowTargetData
        /// </summary>    
        /*
        public FollowTargetData() {
            Init();
        }
        */
        public void Init() {
            Timestamp             = default(ulong);
            CustomState           = default(ulong);
            Lat                   = default(int  );
            Lon                   = default(int  );
            Alt                   = default(float);
            Vel                   = new float[  3];
            Acc                   = new float[  3];
            AttitudeQ             = new float[  4];
            Rates                 = new float[  3];
            PositionCov           = new float[  3];
            EstCapabilities       = default(byte );
        }
        #endregion

        #region Read Buffer
        /// <summary>
        /// Reads the data from Buffer into this struct
        /// </summary>    
        public int Read(byte[] p_buffer,int p_offset=0) {
            int    l = 93;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals
            int[]  LS8  = MAVLinkCRC.U8_LSH8, LS16 = MAVLinkCRC.U8_LSH16, LS24 = MAVLinkCRC.U8_LSH24, LS32 = MAVLinkCRC.U8_LSH32, LS40 = MAVLinkCRC.U8_LSH40, LS48 = MAVLinkCRC.U8_LSH48, LS56 = MAVLinkCRC.U8_LSH56;
            Span<byte> b = p_buffer.AsSpan(p_offset);            
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            Timestamp             = (ulong) ((ulong)b[p++] | (ulong)LS8[b[p++]] | (ulong)LS16[b[p++]] | (ulong)LS24[b[p++]] | (ulong)LS32[b[p++]] | (ulong)LS40[b[p++]] | (ulong)LS48[b[p++]] | (ulong)LS56[b[p++]]);
            CustomState           = (ulong) ((ulong)b[p++] | (ulong)LS8[b[p++]] | (ulong)LS16[b[p++]] | (ulong)LS24[b[p++]] | (ulong)LS32[b[p++]] | (ulong)LS40[b[p++]] | (ulong)LS48[b[p++]] | (ulong)LS56[b[p++]]);
            Lat                   = (int  ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            Lon                   = (int  ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            Alt                   = (float) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            for(int i=0;i<3  ;i++) { Vel[i]                = (float) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4; }
            for(int i=0;i<3  ;i++) { Acc[i]                = (float) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4; }
            for(int i=0;i<4  ;i++) { AttitudeQ[i]          = (float) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4; }
            for(int i=0;i<3  ;i++) { Rates[i]              = (float) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4; }
            for(int i=0;i<3  ;i++) { PositionCov[i]        = (float) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4; }
            EstCapabilities       = (byte ) (b[p++]);            
            return p;
        }
        #endregion

        #region Write Buffer
        /// <summary>
        /// Writes the message data into a Buffer
        /// </summary>    
        public int Write(byte[] p_buffer,int p_offset=0) {
            int    l = 93;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals            
            Span<byte> b = p_buffer.AsSpan(p_offset);
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            b[p++] = (byte)(      Timestamp);
            b[p++] = (byte)((long)Timestamp>>8 );
            b[p++] = (byte)((long)Timestamp>>16);
            b[p++] = (byte)((long)Timestamp>>24);
            b[p++] = (byte)((long)Timestamp>>32);
            b[p++] = (byte)((long)Timestamp>>40);
            b[p++] = (byte)((long)Timestamp>>48);
            b[p++] = (byte)((long)Timestamp>>56);
            b[p++] = (byte)(      CustomState);
            b[p++] = (byte)((long)CustomState>>8 );
            b[p++] = (byte)((long)CustomState>>16);
            b[p++] = (byte)((long)CustomState>>24);
            b[p++] = (byte)((long)CustomState>>32);
            b[p++] = (byte)((long)CustomState>>40);
            b[p++] = (byte)((long)CustomState>>48);
            b[p++] = (byte)((long)CustomState>>56);
            b[p++] = (byte)(      Lat);
            b[p++] = (byte)((int)Lat>>8 );
            b[p++] = (byte)((int)Lat>>16);
            b[p++] = (byte)((int)Lat>>24);
            b[p++] = (byte)(      Lon);
            b[p++] = (byte)((int)Lon>>8 );
            b[p++] = (byte)((int)Lon>>16);
            b[p++] = (byte)((int)Lon>>24);
            MemoryMarshal.Write(b.Slice(p, 4), ref Alt                  ); p+=4;
            for(int i=0;i<  3;i++) {
                MemoryMarshal.Write(b.Slice(p, 4), ref Vel[i]               ); p+=4;
            }
            for(int i=0;i<  3;i++) {
                MemoryMarshal.Write(b.Slice(p, 4), ref Acc[i]               ); p+=4;
            }
            for(int i=0;i<  4;i++) {
                MemoryMarshal.Write(b.Slice(p, 4), ref AttitudeQ[i]         ); p+=4;
            }
            for(int i=0;i<  3;i++) {
                MemoryMarshal.Write(b.Slice(p, 4), ref Rates[i]             ); p+=4;
            }
            for(int i=0;i<  3;i++) {
                MemoryMarshal.Write(b.Slice(p, 4), ref PositionCov[i]       ); p+=4;
            }
            b[p++] = (byte)(EstCapabilities);
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
            int l = 93;
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
