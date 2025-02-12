        
using System.IO;
using System.Runtime.InteropServices;

#pragma warning disable CS0675

namespace MAVLinkBindings {

    /// <summary>
    /// Message reporting the status of a gimbal device. This message should be broadcasted by a gimbal device component. The angles encoded in the quaternion are relative to absolute North if the flag GIMBAL_DEVICE_FLAGS_YAW_LOCK is set (roll: positive is rolling to the right, pitch: positive is pitching up, yaw is turn to the right) or relative to the vehicle heading if the flag is not set. This message should be broadcast at a low regular rate (e.g. 10Hz).
    /// </summary>    
    public struct GimbalDeviceAttitudeStatusData : IMAVLinkMessageData {

        /// <summary>
        /// Message Id Associated w/ this Struct
        /// </summary>    
        public int GetId() { return 285; }

        public uint                    TimeBootMs;            //Timestamp (time since system boot).
        public float[]                 Q;                     //Quaternion components, w, x, y, z (1 0 0 0 is the null-rotation, the frame is depends on whether the flag GIMBAL_DEVICE_FLAGS_YAW_LOCK is set)
        public float                   AngularVelocityX;      //X component of angular velocity (NaN if unknown)
        public float                   AngularVelocityY;      //Y component of angular velocity (NaN if unknown)
        public float                   AngularVelocityZ;      //Z component of angular velocity (NaN if unknown)
        public GimbalDeviceErrorFlags  FailureFlags;          //Failure flags (0 for no failure)
        public GimbalDeviceFlags       Flags;                 //Current gimbal flags set.
        public byte                    TargetSystem;          //System ID
        public byte                    TargetComponent;       //Component ID    

        #region CTOR
        /// <summary>
        /// Instantiates a new GimbalDeviceAttitudeStatusData
        /// </summary>    
        public GimbalDeviceAttitudeStatusData() {
            TimeBootMs              = default(uint                  );
            Q                       = new float[  4];
            AngularVelocityX        = default(float                 );
            AngularVelocityY        = default(float                 );
            AngularVelocityZ        = default(float                 );
            FailureFlags            = default(GimbalDeviceErrorFlags);
            Flags                   = default(GimbalDeviceFlags     );
            TargetSystem            = default(byte                  );
            TargetComponent         = default(byte                  );
        }
        #endregion

        #region Read Buffer
        /// <summary>
        /// Reads the data from Buffer into this struct
        /// </summary>    
        public int Read(byte[] p_buffer,int p_offset=0) {
            int    l = 40;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals
            int[]  LS8  = MAVLinkCRC.U8_LSH8, LS16 = MAVLinkCRC.U8_LSH16, LS24 = MAVLinkCRC.U8_LSH24, LS32 = MAVLinkCRC.U8_LSH32, LS40 = MAVLinkCRC.U8_LSH40, LS48 = MAVLinkCRC.U8_LSH48, LS56 = MAVLinkCRC.U8_LSH56;
            Span<byte> b = p_buffer.AsSpan(p_offset);            
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            TimeBootMs              = (uint                  ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            for(int i=0;i<4  ;i++) { Q[i]                    = (float                 ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4; }
            AngularVelocityX        = (float                 ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            AngularVelocityY        = (float                 ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            AngularVelocityZ        = (float                 ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            FailureFlags            = (GimbalDeviceErrorFlags) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            Flags                   = (GimbalDeviceFlags     ) (b[p++] | LS8[b[p++]]);
            TargetSystem            = (byte                  ) (b[p++]);
            TargetComponent         = (byte                  ) (b[p++]);            
            return p;
        }
        #endregion

        #region Write Buffer
        /// <summary>
        /// Writes the message data into a Buffer
        /// </summary>    
        public int Write(byte[] p_buffer,int p_offset=0) {
            int    l = 40;
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
            for(int i=0;i<  4;i++) {
                MemoryMarshal.Write(b.Slice(p, 4), in Q[i]                   ); p+=4;
            }
            MemoryMarshal.Write(b.Slice(p, 4), in AngularVelocityX       ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), in AngularVelocityY       ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), in AngularVelocityZ       ); p+=4;
            b[p++] = (byte)(      FailureFlags);
            b[p++] = (byte)((int)FailureFlags>>8 );
            b[p++] = (byte)((int)FailureFlags>>16);
            b[p++] = (byte)((int)FailureFlags>>24);
            b[p++] = (byte)(      Flags);
            b[p++] = (byte)((int)Flags>>8 );
            b[p++] = (byte)(TargetSystem);
            b[p++] = (byte)(TargetComponent);
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
            int l = 40;
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
