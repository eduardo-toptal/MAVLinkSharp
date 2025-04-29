        
using System.IO;
using System.Runtime.InteropServices;
using MAVLinkSharp.Runtime;

#pragma warning disable CS0675

namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// Global position estimate from a Vicon motion system source.
    /// </summary>    
    public struct ViconPositionEstimateData : IMAVLinkMessageData {

        /// <summary>
        /// Message Id Associated w/ this Struct
        /// </summary>    
        public int GetId() { return 104; }

        public ulong    Usec;          //Timestamp (UNIX time or time since system boot)
        public float    X;             //Global X position
        public float    Y;             //Global Y position
        public float    Z;             //Global Z position
        public float    Roll;          //Roll angle
        public float    Pitch;         //Pitch angle
        public float    Yaw;           //Yaw angle
        public float[]  Covariance;    //Row-major representation of 6x6 pose cross-covariance matrix upper right triangle (states: x, y, z, roll, pitch, yaw; first six entries are the first ROW, next five entries are the second ROW, etc.). If unknown, assign NaN value to first element in the array.    

        #region CTOR
        /// <summary>
        /// Instantiates a new ViconPositionEstimateData
        /// </summary>    
        /*
        public ViconPositionEstimateData() {
            Init();
        }
        */
        public void Init() {
            Usec            = default(ulong);
            X               = default(float);
            Y               = default(float);
            Z               = default(float);
            Roll            = default(float);
            Pitch           = default(float);
            Yaw             = default(float);
            Covariance      = new float[ 21];
        }
        #endregion

        #region Read Buffer
        /// <summary>
        /// Reads the data from Buffer into this struct
        /// </summary>    
        public int Read(byte[] p_buffer,int p_offset=0) {
            int    l = 116;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals
            int[]  LS8  = MAVLinkCRC.U8_LSH8, LS16 = MAVLinkCRC.U8_LSH16, LS24 = MAVLinkCRC.U8_LSH24, LS32 = MAVLinkCRC.U8_LSH32, LS40 = MAVLinkCRC.U8_LSH40, LS48 = MAVLinkCRC.U8_LSH48, LS56 = MAVLinkCRC.U8_LSH56;
            Span<byte> b = p_buffer.AsSpan(p_offset);            
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            Usec            = (ulong) ((ulong)b[p++] | (ulong)LS8[b[p++]] | (ulong)LS16[b[p++]] | (ulong)LS24[b[p++]] | (ulong)LS32[b[p++]] | (ulong)LS40[b[p++]] | (ulong)LS48[b[p++]] | (ulong)LS56[b[p++]]);
            X               = (float) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Y               = (float) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Z               = (float) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Roll            = (float) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Pitch           = (float) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Yaw             = (float) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            for(int i=0;i<21 ;i++) { Covariance[i]   = (float) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4; }            
            return p;
        }
        #endregion

        #region Write Buffer
        /// <summary>
        /// Writes the message data into a Buffer
        /// </summary>    
        public int Write(byte[] p_buffer,int p_offset=0) {
            int    l = 116;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals            
            Span<byte> b = p_buffer.AsSpan(p_offset);
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            b[p++] = (byte)(      Usec);
            b[p++] = (byte)((long)Usec>>8 );
            b[p++] = (byte)((long)Usec>>16);
            b[p++] = (byte)((long)Usec>>24);
            b[p++] = (byte)((long)Usec>>32);
            b[p++] = (byte)((long)Usec>>40);
            b[p++] = (byte)((long)Usec>>48);
            b[p++] = (byte)((long)Usec>>56);
            MemoryMarshal.Write(b.Slice(p, 4), ref X              ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref Y              ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref Z              ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref Roll           ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref Pitch          ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref Yaw            ); p+=4;
            for(int i=0;i< 21;i++) {
                MemoryMarshal.Write(b.Slice(p, 4), ref Covariance[i]  ); p+=4;
            }
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
            int l = 116;
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
