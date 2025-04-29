        
using System.IO;
using System.Runtime.InteropServices;
using MAVLinkSharp.Runtime;

#pragma warning disable CS0675

namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// Sets a desired vehicle attitude. Used by an external controller to command the vehicle (manual controller or other system).
    /// </summary>    
    public struct SetAttitudeTargetData : IMAVLinkMessageData {

        /// <summary>
        /// Message Id Associated w/ this Struct
        /// </summary>    
        public int GetId() { return 82; }

        public uint                         TimeBootMs;          //Timestamp (time since system boot).
        public float[]                      Q;                   //Attitude quaternion (w, x, y, z order, zero-rotation is 1, 0, 0, 0)
        public float                        BodyRollRate;        //Body roll rate
        public float                        BodyPitchRate;       //Body pitch rate
        public float                        BodyYawRate;         //Body yaw rate
        public float                        Thrust;              //Collective thrust, normalized to 0 .. 1 (-1 .. 1 for vehicles capable of reverse trust)
        public byte                         TargetSystem;        //System ID
        public byte                         TargetComponent;     //Component ID
        public AttitudeTargetTypemaskFlags  TypeMask;            //Bitmap to indicate which dimensions should be ignored by the vehicle.
        public float[]                      ThrustBody;          //3D thrust setpoint in the body NED frame, normalized to -1 .. 1    

        #region CTOR
        /// <summary>
        /// Instantiates a new SetAttitudeTargetData
        /// </summary>    
        /*
        public SetAttitudeTargetData() {
            Init();
        }
        */
        public void Init() {
            TimeBootMs            = default(uint                       );
            Q                     = new float[  4];
            BodyRollRate          = default(float                      );
            BodyPitchRate         = default(float                      );
            BodyYawRate           = default(float                      );
            Thrust                = default(float                      );
            TargetSystem          = default(byte                       );
            TargetComponent       = default(byte                       );
            TypeMask              = default(AttitudeTargetTypemaskFlags);
            ThrustBody            = new float[  3];
        }
        #endregion

        #region Read Buffer
        /// <summary>
        /// Reads the data from Buffer into this struct
        /// </summary>    
        public int Read(byte[] p_buffer,int p_offset=0) {
            int    l = 51;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals
            int[]  LS8  = MAVLinkCRC.U8_LSH8, LS16 = MAVLinkCRC.U8_LSH16, LS24 = MAVLinkCRC.U8_LSH24, LS32 = MAVLinkCRC.U8_LSH32, LS40 = MAVLinkCRC.U8_LSH40, LS48 = MAVLinkCRC.U8_LSH48, LS56 = MAVLinkCRC.U8_LSH56;
            Span<byte> b = p_buffer.AsSpan(p_offset);            
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            TimeBootMs            = (uint                       ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            for(int i=0;i<4  ;i++) { Q[i]                  = (float                      ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4; }
            BodyRollRate          = (float                      ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            BodyPitchRate         = (float                      ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            BodyYawRate           = (float                      ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Thrust                = (float                      ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            TargetSystem          = (byte                       ) (b[p++]);
            TargetComponent       = (byte                       ) (b[p++]);
            TypeMask              = (AttitudeTargetTypemaskFlags) (b[p++]);
            for(int i=0;i<3  ;i++) { ThrustBody[i]         = (float                      ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4; }            
            return p;
        }
        #endregion

        #region Write Buffer
        /// <summary>
        /// Writes the message data into a Buffer
        /// </summary>    
        public int Write(byte[] p_buffer,int p_offset=0) {
            int    l = 51;
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
                MemoryMarshal.Write(b.Slice(p, 4), ref Q[i]                 ); p+=4;
            }
            MemoryMarshal.Write(b.Slice(p, 4), ref BodyRollRate         ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref BodyPitchRate        ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref BodyYawRate          ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref Thrust               ); p+=4;
            b[p++] = (byte)(TargetSystem);
            b[p++] = (byte)(TargetComponent);
            b[p++] = (byte)(TypeMask);
            for(int i=0;i<  3;i++) {
                MemoryMarshal.Write(b.Slice(p, 4), ref ThrustBody[i]        ); p+=4;
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
            int l = 51;
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
