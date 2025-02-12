        
using System.IO;
using System.Runtime.InteropServices;

#pragma warning disable CS0675

namespace MAVLinkBindings {

    /// <summary>
    /// Describe a trajectory using an array of up-to 5 bezier control points in the local frame (MAV_FRAME_LOCAL_NED).
    /// </summary>    
    public struct TrajectoryRepresentationBezierData : IMAVLinkMessageData {

        /// <summary>
        /// Message Id Associated w/ this Struct
        /// </summary>    
        public int GetId() { return 333; }

        public ulong    TimeUsec;        //Timestamp (UNIX Epoch time or time since system boot). The receiving end can infer timestamp format (since 1.1.1970 or since system boot) by checking for the magnitude of the number.
        public float[]  PosX;            //X-coordinate of bezier control points. Set to NaN if not being used
        public float[]  PosY;            //Y-coordinate of bezier control points. Set to NaN if not being used
        public float[]  PosZ;            //Z-coordinate of bezier control points. Set to NaN if not being used
        public float[]  Delta;           //Bezier time horizon. Set to NaN if velocity/acceleration should not be incorporated
        public float[]  PosYaw;          //Yaw. Set to NaN for unchanged
        public byte     ValidPoints;     //Number of valid control points (up-to 5 points are possible)    

        #region CTOR
        /// <summary>
        /// Instantiates a new TrajectoryRepresentationBezierData
        /// </summary>    
        public TrajectoryRepresentationBezierData() {
            TimeUsec          = default(ulong);
            PosX              = new float[  5];
            PosY              = new float[  5];
            PosZ              = new float[  5];
            Delta             = new float[  5];
            PosYaw            = new float[  5];
            ValidPoints       = default(byte );
        }
        #endregion

        #region Read Buffer
        /// <summary>
        /// Reads the data from Buffer into this struct
        /// </summary>    
        public int Read(byte[] p_buffer,int p_offset=0) {
            int    l = 109;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals
            int[]  LS8  = MAVLinkCRC.U8_LSH8, LS16 = MAVLinkCRC.U8_LSH16, LS24 = MAVLinkCRC.U8_LSH24, LS32 = MAVLinkCRC.U8_LSH32, LS40 = MAVLinkCRC.U8_LSH40, LS48 = MAVLinkCRC.U8_LSH48, LS56 = MAVLinkCRC.U8_LSH56;
            Span<byte> b = p_buffer.AsSpan(p_offset);            
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            TimeUsec          = (ulong) ((ulong)b[p++] | (ulong)LS8[b[p++]] | (ulong)LS16[b[p++]] | (ulong)LS24[b[p++]] | (ulong)LS32[b[p++]] | (ulong)LS40[b[p++]] | (ulong)LS48[b[p++]] | (ulong)LS56[b[p++]]);
            for(int i=0;i<5  ;i++) { PosX[i]           = (float) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4; }
            for(int i=0;i<5  ;i++) { PosY[i]           = (float) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4; }
            for(int i=0;i<5  ;i++) { PosZ[i]           = (float) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4; }
            for(int i=0;i<5  ;i++) { Delta[i]          = (float) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4; }
            for(int i=0;i<5  ;i++) { PosYaw[i]         = (float) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4; }
            ValidPoints       = (byte ) (b[p++]);            
            return p;
        }
        #endregion

        #region Write Buffer
        /// <summary>
        /// Writes the message data into a Buffer
        /// </summary>    
        public int Write(byte[] p_buffer,int p_offset=0) {
            int    l = 109;
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
            for(int i=0;i<  5;i++) {
                MemoryMarshal.Write(b.Slice(p, 4), in PosX[i]          ); p+=4;
            }
            for(int i=0;i<  5;i++) {
                MemoryMarshal.Write(b.Slice(p, 4), in PosY[i]          ); p+=4;
            }
            for(int i=0;i<  5;i++) {
                MemoryMarshal.Write(b.Slice(p, 4), in PosZ[i]          ); p+=4;
            }
            for(int i=0;i<  5;i++) {
                MemoryMarshal.Write(b.Slice(p, 4), in Delta[i]         ); p+=4;
            }
            for(int i=0;i<  5;i++) {
                MemoryMarshal.Write(b.Slice(p, 4), in PosYaw[i]        ); p+=4;
            }
            b[p++] = (byte)(ValidPoints);
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
            int l = 109;
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
