        
using System.IO;
using System.Runtime.InteropServices;
using MAVLinkSharp.Runtime;

#pragma warning disable CS0675

namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// High level message to control a gimbal's attitude. This message is to be sent to the gimbal manager (e.g. from a ground station). Angles and rates can be set to NaN according to use case.
    /// </summary>    
    public struct GimbalManagerSetAttitudeData : IMAVLinkMessageData {

        /// <summary>
        /// Message Id Associated w/ this Struct
        /// </summary>    
        public int GetId() { return 282; }

        public GimbalManagerFlags  Flags;                 //High level gimbal manager flags to use.
        public float[]             Q;                     //Quaternion components, w, x, y, z (1 0 0 0 is the null-rotation, the frame is depends on whether the flag GIMBAL_MANAGER_FLAGS_YAW_LOCK is set)
        public float               AngularVelocityX;      //X component of angular velocity, positive is rolling to the right, NaN to be ignored.
        public float               AngularVelocityY;      //Y component of angular velocity, positive is pitching up, NaN to be ignored.
        public float               AngularVelocityZ;      //Z component of angular velocity, positive is yawing to the right, NaN to be ignored.
        public byte                TargetSystem;          //System ID
        public byte                TargetComponent;       //Component ID
        public byte                GimbalDeviceId;        //Component ID of gimbal device to address (or 1-6 for non-MAVLink gimbal), 0 for all gimbal device components. Send command multiple times for more than one gimbal (but not all gimbals).    

        #region CTOR
        /// <summary>
        /// Instantiates a new GimbalManagerSetAttitudeData
        /// </summary>    
        public GimbalManagerSetAttitudeData() {
            Flags                   = default(GimbalManagerFlags);
            Q                       = new float[  4];
            AngularVelocityX        = default(float             );
            AngularVelocityY        = default(float             );
            AngularVelocityZ        = default(float             );
            TargetSystem            = default(byte              );
            TargetComponent         = default(byte              );
            GimbalDeviceId          = default(byte              );
        }
        #endregion

        #region Read Buffer
        /// <summary>
        /// Reads the data from Buffer into this struct
        /// </summary>    
        public int Read(byte[] p_buffer,int p_offset=0) {
            int    l = 35;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals
            int[]  LS8  = MAVLinkCRC.U8_LSH8, LS16 = MAVLinkCRC.U8_LSH16, LS24 = MAVLinkCRC.U8_LSH24, LS32 = MAVLinkCRC.U8_LSH32, LS40 = MAVLinkCRC.U8_LSH40, LS48 = MAVLinkCRC.U8_LSH48, LS56 = MAVLinkCRC.U8_LSH56;
            Span<byte> b = p_buffer.AsSpan(p_offset);            
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            Flags                   = (GimbalManagerFlags) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            for(int i=0;i<4  ;i++) { Q[i]                    = (float             ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4; }
            AngularVelocityX        = (float             ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            AngularVelocityY        = (float             ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            AngularVelocityZ        = (float             ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            TargetSystem            = (byte              ) (b[p++]);
            TargetComponent         = (byte              ) (b[p++]);
            GimbalDeviceId          = (byte              ) (b[p++]);            
            return p;
        }
        #endregion

        #region Write Buffer
        /// <summary>
        /// Writes the message data into a Buffer
        /// </summary>    
        public int Write(byte[] p_buffer,int p_offset=0) {
            int    l = 35;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals            
            Span<byte> b = p_buffer.AsSpan(p_offset);
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            b[p++] = (byte)(      Flags);
            b[p++] = (byte)((int)Flags>>8 );
            b[p++] = (byte)((int)Flags>>16);
            b[p++] = (byte)((int)Flags>>24);
            for(int i=0;i<  4;i++) {
                MemoryMarshal.Write(b.Slice(p, 4), ref Q[i]                   ); p+=4;
            }
            MemoryMarshal.Write(b.Slice(p, 4), ref AngularVelocityX       ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref AngularVelocityY       ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref AngularVelocityZ       ); p+=4;
            b[p++] = (byte)(TargetSystem);
            b[p++] = (byte)(TargetComponent);
            b[p++] = (byte)(GimbalDeviceId);
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
            int l = 35;
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
