        
using System.IO;
using System.Runtime.InteropServices;
using MAVLinkSharp.Runtime;

#pragma warning disable CS0675

namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// The attitude in the aeronautical frame (right-handed, Z-down, X-front, Y-right).
    /// </summary>    
    public struct AttitudeData : IMAVLinkMessageData {

        /// <summary>
        /// Message Id Associated w/ this Struct
        /// </summary>    
        public int GetId() { return 30; }

        public uint   TimeBootMs;      //Timestamp (time since system boot).
        public float  Roll;            //Roll angle (-pi..+pi)
        public float  Pitch;           //Pitch angle (-pi..+pi)
        public float  Yaw;             //Yaw angle (-pi..+pi)
        public float  Rollspeed;       //Roll angular speed
        public float  Pitchspeed;      //Pitch angular speed
        public float  Yawspeed;        //Yaw angular speed    

        #region CTOR
        /// <summary>
        /// Instantiates a new AttitudeData
        /// </summary>    
        public AttitudeData() {
            TimeBootMs        = default(uint );
            Roll              = default(float);
            Pitch             = default(float);
            Yaw               = default(float);
            Rollspeed         = default(float);
            Pitchspeed        = default(float);
            Yawspeed          = default(float);
        }
        #endregion

        #region Read Buffer
        /// <summary>
        /// Reads the data from Buffer into this struct
        /// </summary>    
        public int Read(byte[] p_buffer,int p_offset=0) {
            int    l = 28;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals
            int[]  LS8  = MAVLinkCRC.U8_LSH8, LS16 = MAVLinkCRC.U8_LSH16, LS24 = MAVLinkCRC.U8_LSH24, LS32 = MAVLinkCRC.U8_LSH32, LS40 = MAVLinkCRC.U8_LSH40, LS48 = MAVLinkCRC.U8_LSH48, LS56 = MAVLinkCRC.U8_LSH56;
            Span<byte> b = p_buffer.AsSpan(p_offset);            
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            TimeBootMs        = (uint ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            Roll              = (float) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Pitch             = (float) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Yaw               = (float) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Rollspeed         = (float) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Pitchspeed        = (float) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Yawspeed          = (float) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;            
            return p;
        }
        #endregion

        #region Write Buffer
        /// <summary>
        /// Writes the message data into a Buffer
        /// </summary>    
        public int Write(byte[] p_buffer,int p_offset=0) {
            int    l = 28;
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
            MemoryMarshal.Write(b.Slice(p, 4), ref Roll             ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref Pitch            ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref Yaw              ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref Rollspeed        ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref Pitchspeed       ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref Yawspeed         ); p+=4;
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
            int l = 28;
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
