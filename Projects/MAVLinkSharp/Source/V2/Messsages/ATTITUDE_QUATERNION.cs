        
using System.IO;
using System.Runtime.InteropServices;
using MAVLinkSharp.Runtime;

#pragma warning disable CS0675

namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// The attitude in the aeronautical frame (right-handed, Z-down, X-front, Y-right), expressed as quaternion. Quaternion order is w, x, y, z and a zero rotation would be expressed as (1 0 0 0).
    /// </summary>    
    public struct AttitudeQuaternionData : IMAVLinkMessageData {

        /// <summary>
        /// Message Id Associated w/ this Struct
        /// </summary>    
        public int GetId() { return 31; }

        public uint     TimeBootMs;       //Timestamp (time since system boot).
        public float    Q1;               //Quaternion component 1, w (1 in null-rotation)
        public float    Q2;               //Quaternion component 2, x (0 in null-rotation)
        public float    Q3;               //Quaternion component 3, y (0 in null-rotation)
        public float    Q4;               //Quaternion component 4, z (0 in null-rotation)
        public float    Rollspeed;        //Roll angular speed
        public float    Pitchspeed;       //Pitch angular speed
        public float    Yawspeed;         //Yaw angular speed
        public float[]  ReprOffsetQ;      //Rotation offset by which the attitude quaternion and angular speed vector should be rotated for user display (quaternion with [w, x, y, z] order, zero-rotation is [1, 0, 0, 0], send [0, 0, 0, 0] if field not supported). This field is intended for systems in which the reference attitude may change during flight. For example, tailsitters VTOLs rotate their reference attitude by 90 degrees between hover mode and fixed wing mode, thus repr_offset_q is equal to [1, 0, 0, 0] in hover mode and equal to [0.7071, 0, 0.7071, 0] in fixed wing mode.    

        #region CTOR
        /// <summary>
        /// Instantiates a new AttitudeQuaternionData
        /// </summary>    
        public AttitudeQuaternionData() {
            TimeBootMs         = default(uint );
            Q1                 = default(float);
            Q2                 = default(float);
            Q3                 = default(float);
            Q4                 = default(float);
            Rollspeed          = default(float);
            Pitchspeed         = default(float);
            Yawspeed           = default(float);
            ReprOffsetQ        = new float[  4];
        }
        #endregion

        #region Read Buffer
        /// <summary>
        /// Reads the data from Buffer into this struct
        /// </summary>    
        public int Read(byte[] p_buffer,int p_offset=0) {
            int    l = 48;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals
            int[]  LS8  = MAVLinkCRC.U8_LSH8, LS16 = MAVLinkCRC.U8_LSH16, LS24 = MAVLinkCRC.U8_LSH24, LS32 = MAVLinkCRC.U8_LSH32, LS40 = MAVLinkCRC.U8_LSH40, LS48 = MAVLinkCRC.U8_LSH48, LS56 = MAVLinkCRC.U8_LSH56;
            Span<byte> b = p_buffer.AsSpan(p_offset);            
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            TimeBootMs         = (uint ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            Q1                 = (float) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Q2                 = (float) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Q3                 = (float) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Q4                 = (float) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Rollspeed          = (float) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Pitchspeed         = (float) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Yawspeed           = (float) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            for(int i=0;i<4  ;i++) { ReprOffsetQ[i]     = (float) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4; }            
            return p;
        }
        #endregion

        #region Write Buffer
        /// <summary>
        /// Writes the message data into a Buffer
        /// </summary>    
        public int Write(byte[] p_buffer,int p_offset=0) {
            int    l = 48;
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
            MemoryMarshal.Write(b.Slice(p, 4), ref Q1                ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref Q2                ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref Q3                ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref Q4                ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref Rollspeed         ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref Pitchspeed        ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref Yawspeed          ); p+=4;
            for(int i=0;i<  4;i++) {
                MemoryMarshal.Write(b.Slice(p, 4), ref ReprOffsetQ[i]    ); p+=4;
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
            int l = 48;
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
