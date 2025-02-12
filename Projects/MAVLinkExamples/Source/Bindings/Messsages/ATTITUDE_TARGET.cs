        
using System.IO;
using System.Runtime.InteropServices;

#pragma warning disable CS0675

namespace MAVLinkBindings {

    /// <summary>
    /// Reports the current commanded attitude of the vehicle as specified by the autopilot. This should match the commands sent in a SET_ATTITUDE_TARGET message if the vehicle is being controlled this way.
    /// </summary>    
    public struct AttitudeTargetData : IMAVLinkMessageData {

        /// <summary>
        /// Message Id Associated w/ this Struct
        /// </summary>    
        public int GetId() { return 83; }

        public uint                         TimeBootMs;         //Timestamp (time since system boot).
        public float[]                      Q;                  //Attitude quaternion (w, x, y, z order, zero-rotation is 1, 0, 0, 0)
        public float                        BodyRollRate;       //Body roll rate
        public float                        BodyPitchRate;      //Body pitch rate
        public float                        BodyYawRate;        //Body yaw rate
        public float                        Thrust;             //Collective thrust, normalized to 0 .. 1 (-1 .. 1 for vehicles capable of reverse trust)
        public AttitudeTargetTypemaskFlags  TypeMask;           //Bitmap to indicate which dimensions should be ignored by the vehicle.    

        #region CTOR
        /// <summary>
        /// Instantiates a new AttitudeTargetData
        /// </summary>    
        public AttitudeTargetData() {
            TimeBootMs           = default(uint                       );
            Q                    = new float[  4];
            BodyRollRate         = default(float                      );
            BodyPitchRate        = default(float                      );
            BodyYawRate          = default(float                      );
            Thrust               = default(float                      );
            TypeMask             = default(AttitudeTargetTypemaskFlags);
        }
        #endregion

        #region Read Buffer
        /// <summary>
        /// Reads the data from Buffer into this struct
        /// </summary>    
        public int Read(byte[] p_buffer,int p_offset=0) {
            int    l = 37;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals
            int[]  LS8  = MAVLinkCRC.U8_LSH8, LS16 = MAVLinkCRC.U8_LSH16, LS24 = MAVLinkCRC.U8_LSH24, LS32 = MAVLinkCRC.U8_LSH32, LS40 = MAVLinkCRC.U8_LSH40, LS48 = MAVLinkCRC.U8_LSH48, LS56 = MAVLinkCRC.U8_LSH56;
            Span<byte> b = p_buffer.AsSpan(p_offset);            
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            TimeBootMs           = (uint                       ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            for(int i=0;i<4  ;i++) { Q[i]                 = (float                      ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4; }
            BodyRollRate         = (float                      ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            BodyPitchRate        = (float                      ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            BodyYawRate          = (float                      ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Thrust               = (float                      ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            TypeMask             = (AttitudeTargetTypemaskFlags) (b[p++]);            
            return p;
        }
        #endregion

        #region Write Buffer
        /// <summary>
        /// Writes the message data into a Buffer
        /// </summary>    
        public int Write(byte[] p_buffer,int p_offset=0) {
            int    l = 37;
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
                MemoryMarshal.Write(b.Slice(p, 4), in Q[i]                ); p+=4;
            }
            MemoryMarshal.Write(b.Slice(p, 4), in BodyRollRate        ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), in BodyPitchRate       ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), in BodyYawRate         ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), in Thrust              ); p+=4;
            b[p++] = (byte)(TypeMask);
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
            int l = 37;
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
