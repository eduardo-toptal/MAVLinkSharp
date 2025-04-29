        
using System.IO;
using System.Runtime.InteropServices;
using MAVLinkSharp.Runtime;

#pragma warning disable CS0675

namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// Low level message containing autopilot state relevant for a gimbal device. This message is to be sent from the gimbal manager to the gimbal device component. The data of this message server for the gimbal's estimator corrections in particular horizon compensation, as well as the autopilot's control intention e.g. feed forward angular control in z-axis.
    /// </summary>    
    public struct AutopilotStateForGimbalDeviceData : IMAVLinkMessageData {

        /// <summary>
        /// Message Id Associated w/ this Struct
        /// </summary>    
        public int GetId() { return 286; }

        public ulong                 TimeBootUs;                         //Timestamp (time since system boot).
        public float[]               Q;                                  //Quaternion components of autopilot attitude: w, x, y, z (1 0 0 0 is the null-rotation, Hamilton convention).
        public uint                  QEstimatedDelayUs;                  //Estimated delay of the attitude data.
        public float                 Vx;                                 //X Speed in NED (North, East, Down).
        public float                 Vy;                                 //Y Speed in NED (North, East, Down).
        public float                 Vz;                                 //Z Speed in NED (North, East, Down).
        public uint                  VEstimatedDelayUs;                  //Estimated delay of the speed data.
        public float                 FeedForwardAngularVelocityZ;        //Feed forward Z component of angular velocity, positive is yawing to the right, NaN to be ignored. This is to indicate if the autopilot is actively yawing.
        public EstimatorStatusFlags  EstimatorStatus;                    //Bitmap indicating which estimator outputs are valid.
        public byte                  TargetSystem;                       //System ID
        public byte                  TargetComponent;                    //Component ID
        public MAVLandedStateFlags   LandedState;                        //The landed state. Is set to MAV_LANDED_STATE_UNDEFINED if landed state is unknown.    

        #region CTOR
        /// <summary>
        /// Instantiates a new AutopilotStateForGimbalDeviceData
        /// </summary>    
        /*
        public AutopilotStateForGimbalDeviceData() {
            Init();
        }
        */
        public void Init() {
            TimeBootUs                           = default(ulong               );
            Q                                    = new float[  4];
            QEstimatedDelayUs                    = default(uint                );
            Vx                                   = default(float               );
            Vy                                   = default(float               );
            Vz                                   = default(float               );
            VEstimatedDelayUs                    = default(uint                );
            FeedForwardAngularVelocityZ          = default(float               );
            EstimatorStatus                      = default(EstimatorStatusFlags);
            TargetSystem                         = default(byte                );
            TargetComponent                      = default(byte                );
            LandedState                          = default(MAVLandedStateFlags );
        }
        #endregion

        #region Read Buffer
        /// <summary>
        /// Reads the data from Buffer into this struct
        /// </summary>    
        public int Read(byte[] p_buffer,int p_offset=0) {
            int    l = 53;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals
            int[]  LS8  = MAVLinkCRC.U8_LSH8, LS16 = MAVLinkCRC.U8_LSH16, LS24 = MAVLinkCRC.U8_LSH24, LS32 = MAVLinkCRC.U8_LSH32, LS40 = MAVLinkCRC.U8_LSH40, LS48 = MAVLinkCRC.U8_LSH48, LS56 = MAVLinkCRC.U8_LSH56;
            Span<byte> b = p_buffer.AsSpan(p_offset);            
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            TimeBootUs                           = (ulong               ) ((ulong)b[p++] | (ulong)LS8[b[p++]] | (ulong)LS16[b[p++]] | (ulong)LS24[b[p++]] | (ulong)LS32[b[p++]] | (ulong)LS40[b[p++]] | (ulong)LS48[b[p++]] | (ulong)LS56[b[p++]]);
            for(int i=0;i<4  ;i++) { Q[i]                                 = (float               ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4; }
            QEstimatedDelayUs                    = (uint                ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            Vx                                   = (float               ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Vy                                   = (float               ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Vz                                   = (float               ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            VEstimatedDelayUs                    = (uint                ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            FeedForwardAngularVelocityZ          = (float               ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            EstimatorStatus                      = (EstimatorStatusFlags) (b[p++] | LS8[b[p++]]);
            TargetSystem                         = (byte                ) (b[p++]);
            TargetComponent                      = (byte                ) (b[p++]);
            LandedState                          = (MAVLandedStateFlags ) (b[p++]);            
            return p;
        }
        #endregion

        #region Write Buffer
        /// <summary>
        /// Writes the message data into a Buffer
        /// </summary>    
        public int Write(byte[] p_buffer,int p_offset=0) {
            int    l = 53;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals            
            Span<byte> b = p_buffer.AsSpan(p_offset);
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            b[p++] = (byte)(      TimeBootUs);
            b[p++] = (byte)((long)TimeBootUs>>8 );
            b[p++] = (byte)((long)TimeBootUs>>16);
            b[p++] = (byte)((long)TimeBootUs>>24);
            b[p++] = (byte)((long)TimeBootUs>>32);
            b[p++] = (byte)((long)TimeBootUs>>40);
            b[p++] = (byte)((long)TimeBootUs>>48);
            b[p++] = (byte)((long)TimeBootUs>>56);
            for(int i=0;i<  4;i++) {
                MemoryMarshal.Write(b.Slice(p, 4), ref Q[i]                                ); p+=4;
            }
            b[p++] = (byte)(      QEstimatedDelayUs);
            b[p++] = (byte)((int)QEstimatedDelayUs>>8 );
            b[p++] = (byte)((int)QEstimatedDelayUs>>16);
            b[p++] = (byte)((int)QEstimatedDelayUs>>24);
            MemoryMarshal.Write(b.Slice(p, 4), ref Vx                                  ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref Vy                                  ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref Vz                                  ); p+=4;
            b[p++] = (byte)(      VEstimatedDelayUs);
            b[p++] = (byte)((int)VEstimatedDelayUs>>8 );
            b[p++] = (byte)((int)VEstimatedDelayUs>>16);
            b[p++] = (byte)((int)VEstimatedDelayUs>>24);
            MemoryMarshal.Write(b.Slice(p, 4), ref FeedForwardAngularVelocityZ         ); p+=4;
            b[p++] = (byte)(      EstimatorStatus);
            b[p++] = (byte)((int)EstimatorStatus>>8 );
            b[p++] = (byte)(TargetSystem);
            b[p++] = (byte)(TargetComponent);
            b[p++] = (byte)(LandedState);
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
            int l = 53;
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
