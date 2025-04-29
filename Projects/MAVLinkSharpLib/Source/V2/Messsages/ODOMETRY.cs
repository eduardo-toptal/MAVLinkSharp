        
using System;
using System.IO;
using System.Runtime.InteropServices;
using MAVLinkSharp.Runtime;

#pragma warning disable CS0675

namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// Odometry message to communicate odometry information with an external interface. Fits ROS REP 147 standard for aerial vehicles (http://www.ros.org/reps/rep-0147.html).
    /// </summary>    
    public struct OdometryData : IMAVLinkMessageData {

        /// <summary>
        /// Message Id Associated w/ this Struct
        /// </summary>    
        public int GetId() { return 331; }

        public ulong                  TimeUsec;               //Timestamp (UNIX Epoch time or time since system boot). The receiving end can infer timestamp format (since 1.1.1970 or since system boot) by checking for the magnitude of the number.
        public float                  X;                      //X Position
        public float                  Y;                      //Y Position
        public float                  Z;                      //Z Position
        public float[]                Q;                      //Quaternion components, w, x, y, z (1 0 0 0 is the null-rotation)
        public float                  Vx;                     //X linear speed
        public float                  Vy;                     //Y linear speed
        public float                  Vz;                     //Z linear speed
        public float                  Rollspeed;              //Roll angular speed
        public float                  Pitchspeed;             //Pitch angular speed
        public float                  Yawspeed;               //Yaw angular speed
        public float[]                PoseCovariance;         //Row-major representation of a 6x6 pose cross-covariance matrix upper right triangle (states: x, y, z, roll, pitch, yaw; first six entries are the first ROW, next five entries are the second ROW, etc.). If unknown, assign NaN value to first element in the array.
        public float[]                VelocityCovariance;     //Row-major representation of a 6x6 velocity cross-covariance matrix upper right triangle (states: vx, vy, vz, rollspeed, pitchspeed, yawspeed; first six entries are the first ROW, next five entries are the second ROW, etc.). If unknown, assign NaN value to first element in the array.
        public MAVFrameFlags          FrameId;                //Coordinate frame of reference for the pose data.
        public MAVFrameFlags          ChildFrameId;           //Coordinate frame of reference for the velocity in free space (twist) data.
        public byte                   ResetCounter;           //Estimate reset counter. This should be incremented when the estimate resets in any of the dimensions (position, velocity, attitude, angular speed). This is designed to be used when e.g an external SLAM system detects a loop-closure and the estimate jumps.
        public MAVEstimatorTypeFlags  EstimatorType;          //Type of estimator that is providing the odometry.
        public sbyte                  Quality;                //Optional odometry quality metric as a percentage. -1 = odometry has failed, 0 = unknown/unset quality, 1 = worst quality, 100 = best quality    

        #region CTOR
        /// <summary>
        /// Instantiates a new OdometryData
        /// </summary>    
        /*
        public OdometryData() {
            Init();
        }
        */
        public void Init() {
            TimeUsec                 = default(ulong                );
            X                        = default(float                );
            Y                        = default(float                );
            Z                        = default(float                );
            Q                        = new float[  4];
            Vx                       = default(float                );
            Vy                       = default(float                );
            Vz                       = default(float                );
            Rollspeed                = default(float                );
            Pitchspeed               = default(float                );
            Yawspeed                 = default(float                );
            PoseCovariance           = new float[ 21];
            VelocityCovariance       = new float[ 21];
            FrameId                  = default(MAVFrameFlags        );
            ChildFrameId             = default(MAVFrameFlags        );
            ResetCounter             = default(byte                 );
            EstimatorType            = default(MAVEstimatorTypeFlags);
            Quality                  = default(sbyte                );
        }
        #endregion

        #region Read Buffer
        /// <summary>
        /// Reads the data from Buffer into this struct
        /// </summary>    
        public int Read(byte[] p_buffer,int p_offset=0) {
            int    l = 233;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals
            int[]  LS8  = MAVLinkCRC.U8_LSH8, LS16 = MAVLinkCRC.U8_LSH16, LS24 = MAVLinkCRC.U8_LSH24, LS32 = MAVLinkCRC.U8_LSH32, LS40 = MAVLinkCRC.U8_LSH40, LS48 = MAVLinkCRC.U8_LSH48, LS56 = MAVLinkCRC.U8_LSH56;
            Span<byte> b = p_buffer.AsSpan(p_offset);            
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            TimeUsec                 = (ulong                ) ((ulong)b[p++] | (ulong)LS8[b[p++]] | (ulong)LS16[b[p++]] | (ulong)LS24[b[p++]] | (ulong)LS32[b[p++]] | (ulong)LS40[b[p++]] | (ulong)LS48[b[p++]] | (ulong)LS56[b[p++]]);
            X                        = (float                ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Y                        = (float                ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Z                        = (float                ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            for(int i=0;i<4  ;i++) { Q[i]                     = (float                ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4; }
            Vx                       = (float                ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Vy                       = (float                ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Vz                       = (float                ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Rollspeed                = (float                ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Pitchspeed               = (float                ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Yawspeed                 = (float                ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            for(int i=0;i<21 ;i++) { PoseCovariance[i]        = (float                ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4; }
            for(int i=0;i<21 ;i++) { VelocityCovariance[i]    = (float                ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4; }
            FrameId                  = (MAVFrameFlags        ) (b[p++]);
            ChildFrameId             = (MAVFrameFlags        ) (b[p++]);
            ResetCounter             = (byte                 ) (b[p++]);
            EstimatorType            = (MAVEstimatorTypeFlags) (b[p++]);
            Quality                  = (sbyte                ) (b[p++]);            
            return p;
        }
        #endregion

        #region Write Buffer
        /// <summary>
        /// Writes the message data into a Buffer
        /// </summary>    
        public int Write(byte[] p_buffer,int p_offset=0) {
            int    l = 233;
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
            MemoryMarshal.Write(b.Slice(p, 4), ref X                       ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref Y                       ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref Z                       ); p+=4;
            for(int i=0;i<  4;i++) {
                MemoryMarshal.Write(b.Slice(p, 4), ref Q[i]                    ); p+=4;
            }
            MemoryMarshal.Write(b.Slice(p, 4), ref Vx                      ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref Vy                      ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref Vz                      ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref Rollspeed               ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref Pitchspeed              ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref Yawspeed                ); p+=4;
            for(int i=0;i< 21;i++) {
                MemoryMarshal.Write(b.Slice(p, 4), ref PoseCovariance[i]       ); p+=4;
            }
            for(int i=0;i< 21;i++) {
                MemoryMarshal.Write(b.Slice(p, 4), ref VelocityCovariance[i]   ); p+=4;
            }
            b[p++] = (byte)(FrameId);
            b[p++] = (byte)(ChildFrameId);
            b[p++] = (byte)(ResetCounter);
            b[p++] = (byte)(EstimatorType);
            b[p++] = (byte)(Quality);
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
            int l = 233;
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
