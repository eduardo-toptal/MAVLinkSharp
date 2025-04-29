        
using System;
using System.IO;
using System.Runtime.InteropServices;
using MAVLinkSharp.Runtime;

#pragma warning disable CS0675

namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// Low level message to control a gimbal device's attitude. This message is to be sent from the gimbal manager to the gimbal device component. Angles and rates can be set to NaN according to use case.
    /// </summary>    
    public struct GimbalDeviceSetAttitudeData : IMAVLinkMessageData {

        /// <summary>
        /// Message Id Associated w/ this Struct
        /// </summary>    
        public int GetId() { return 284; }

        public float[]            Q;                     //Quaternion components, w, x, y, z (1 0 0 0 is the null-rotation, the frame is depends on whether the flag GIMBAL_DEVICE_FLAGS_YAW_LOCK is set, set all fields to NaN if only angular velocity should be used)
        public float              AngularVelocityX;      //X component of angular velocity, positive is rolling to the right, NaN to be ignored.
        public float              AngularVelocityY;      //Y component of angular velocity, positive is pitching up, NaN to be ignored.
        public float              AngularVelocityZ;      //Z component of angular velocity, positive is yawing to the right, NaN to be ignored.
        public GimbalDeviceFlags  Flags;                 //Low level gimbal flags.
        public byte               TargetSystem;          //System ID
        public byte               TargetComponent;       //Component ID    

        #region CTOR
        /// <summary>
        /// Instantiates a new GimbalDeviceSetAttitudeData
        /// </summary>    
        /*
        public GimbalDeviceSetAttitudeData() {
            Init();
        }
        */
        public void Init() {
            Q                       = new float[  4];
            AngularVelocityX        = default(float            );
            AngularVelocityY        = default(float            );
            AngularVelocityZ        = default(float            );
            Flags                   = default(GimbalDeviceFlags);
            TargetSystem            = default(byte             );
            TargetComponent         = default(byte             );
        }
        #endregion

        #region Read Buffer
        /// <summary>
        /// Reads the data from Buffer into this struct
        /// </summary>    
        public int Read(byte[] p_buffer,int p_offset=0) {
            int    l = 32;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals
            int[]  LS8  = MAVLinkCRC.U8_LSH8, LS16 = MAVLinkCRC.U8_LSH16, LS24 = MAVLinkCRC.U8_LSH24, LS32 = MAVLinkCRC.U8_LSH32, LS40 = MAVLinkCRC.U8_LSH40, LS48 = MAVLinkCRC.U8_LSH48, LS56 = MAVLinkCRC.U8_LSH56;
            Span<byte> b = p_buffer.AsSpan(p_offset);            
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            for(int i=0;i<4  ;i++) { Q[i]                    = (float            ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4; }
            AngularVelocityX        = (float            ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            AngularVelocityY        = (float            ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            AngularVelocityZ        = (float            ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Flags                   = (GimbalDeviceFlags) (b[p++] | LS8[b[p++]]);
            TargetSystem            = (byte             ) (b[p++]);
            TargetComponent         = (byte             ) (b[p++]);            
            return p;
        }
        #endregion

        #region Write Buffer
        /// <summary>
        /// Writes the message data into a Buffer
        /// </summary>    
        public int Write(byte[] p_buffer,int p_offset=0) {
            int    l = 32;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals            
            Span<byte> b = p_buffer.AsSpan(p_offset);
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            for(int i=0;i<  4;i++) {
                MemoryMarshal.Write(b.Slice(p, 4), ref Q[i]                   ); p+=4;
            }
            MemoryMarshal.Write(b.Slice(p, 4), ref AngularVelocityX       ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref AngularVelocityY       ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref AngularVelocityZ       ); p+=4;
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
            int l = 32;
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
