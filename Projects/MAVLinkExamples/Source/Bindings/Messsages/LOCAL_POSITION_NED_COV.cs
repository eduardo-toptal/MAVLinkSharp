        
using System.IO;
using System.Runtime.InteropServices;

#pragma warning disable CS0675

namespace MAVLinkBindings {

    /// <summary>
    /// The filtered local position (e.g. fused computer vision and accelerometers). Coordinate frame is right-handed, Z-axis down (aeronautical frame, NED / north-east-down convention)
    /// </summary>    
    public struct LocalPositionNedCovData : IMAVLinkMessageData {

        /// <summary>
        /// Message Id Associated w/ this Struct
        /// </summary>    
        public int GetId() { return 64; }

        public ulong                  TimeUsec;          //Timestamp (UNIX Epoch time or time since system boot). The receiving end can infer timestamp format (since 1.1.1970 or since system boot) by checking for the magnitude of the number.
        public float                  X;                 //X Position
        public float                  Y;                 //Y Position
        public float                  Z;                 //Z Position
        public float                  Vx;                //X Speed
        public float                  Vy;                //Y Speed
        public float                  Vz;                //Z Speed
        public float                  Ax;                //X Acceleration
        public float                  Ay;                //Y Acceleration
        public float                  Az;                //Z Acceleration
        public float[]                Covariance;        //Row-major representation of position, velocity and acceleration 9x9 cross-covariance matrix upper right triangle (states: x, y, z, vx, vy, vz, ax, ay, az; first nine entries are the first ROW, next eight entries are the second row, etc.). If unknown, assign NaN value to first element in the array.
        public MAVEstimatorTypeFlags  EstimatorType;     //Class id of the estimator this estimate originated from.    

        #region CTOR
        /// <summary>
        /// Instantiates a new LocalPositionNedCovData
        /// </summary>    
        public LocalPositionNedCovData() {
            TimeUsec            = default(ulong                );
            X                   = default(float                );
            Y                   = default(float                );
            Z                   = default(float                );
            Vx                  = default(float                );
            Vy                  = default(float                );
            Vz                  = default(float                );
            Ax                  = default(float                );
            Ay                  = default(float                );
            Az                  = default(float                );
            Covariance          = new float[ 45];
            EstimatorType       = default(MAVEstimatorTypeFlags);
        }
        #endregion

        #region Read Buffer
        /// <summary>
        /// Reads the data from Buffer into this struct
        /// </summary>    
        public int Read(byte[] p_buffer,int p_offset=0) {
            int    l = 225;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals
            int[]  LS8  = MAVLinkCRC.U8_LSH8, LS16 = MAVLinkCRC.U8_LSH16, LS24 = MAVLinkCRC.U8_LSH24, LS32 = MAVLinkCRC.U8_LSH32, LS40 = MAVLinkCRC.U8_LSH40, LS48 = MAVLinkCRC.U8_LSH48, LS56 = MAVLinkCRC.U8_LSH56;
            Span<byte> b = p_buffer.AsSpan(p_offset);            
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            TimeUsec            = (ulong                ) ((ulong)b[p++] | (ulong)LS8[b[p++]] | (ulong)LS16[b[p++]] | (ulong)LS24[b[p++]] | (ulong)LS32[b[p++]] | (ulong)LS40[b[p++]] | (ulong)LS48[b[p++]] | (ulong)LS56[b[p++]]);
            X                   = (float                ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Y                   = (float                ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Z                   = (float                ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Vx                  = (float                ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Vy                  = (float                ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Vz                  = (float                ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Ax                  = (float                ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Ay                  = (float                ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Az                  = (float                ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            for(int i=0;i<45 ;i++) { Covariance[i]       = (float                ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4; }
            EstimatorType       = (MAVEstimatorTypeFlags) (b[p++]);            
            return p;
        }
        #endregion

        #region Write Buffer
        /// <summary>
        /// Writes the message data into a Buffer
        /// </summary>    
        public int Write(byte[] p_buffer,int p_offset=0) {
            int    l = 225;
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
            MemoryMarshal.Write(b.Slice(p, 4), in X                  ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), in Y                  ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), in Z                  ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), in Vx                 ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), in Vy                 ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), in Vz                 ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), in Ax                 ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), in Ay                 ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), in Az                 ); p+=4;
            for(int i=0;i< 45;i++) {
                MemoryMarshal.Write(b.Slice(p, 4), in Covariance[i]      ); p+=4;
            }
            b[p++] = (byte)(EstimatorType);
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
            int l = 225;
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
