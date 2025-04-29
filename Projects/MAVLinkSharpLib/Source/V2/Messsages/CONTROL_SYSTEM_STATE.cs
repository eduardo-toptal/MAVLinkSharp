        
using System.IO;
using System.Runtime.InteropServices;
using MAVLinkSharp.Runtime;

#pragma warning disable CS0675

namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// The smoothed, monotonic system state used to feed the control loops of the system.
    /// </summary>    
    public struct ControlSystemStateData : IMAVLinkMessageData {

        /// <summary>
        /// Message Id Associated w/ this Struct
        /// </summary>    
        public int GetId() { return 146; }

        public ulong    TimeUsec;        //Timestamp (UNIX Epoch time or time since system boot). The receiving end can infer timestamp format (since 1.1.1970 or since system boot) by checking for the magnitude of the number.
        public float    XAcc;            //X acceleration in body frame
        public float    YAcc;            //Y acceleration in body frame
        public float    ZAcc;            //Z acceleration in body frame
        public float    XVel;            //X velocity in body frame
        public float    YVel;            //Y velocity in body frame
        public float    ZVel;            //Z velocity in body frame
        public float    XPos;            //X position in local frame
        public float    YPos;            //Y position in local frame
        public float    ZPos;            //Z position in local frame
        public float    Airspeed;        //Airspeed, set to -1 if unknown
        public float[]  VelVariance;     //Variance of body velocity estimate
        public float[]  PosVariance;     //Variance in local position
        public float[]  Q;               //The attitude, represented as Quaternion
        public float    RollRate;        //Angular rate in roll axis
        public float    PitchRate;       //Angular rate in pitch axis
        public float    YawRate;         //Angular rate in yaw axis    

        #region CTOR
        /// <summary>
        /// Instantiates a new ControlSystemStateData
        /// </summary>    
        /*
        public ControlSystemStateData() {
            Init();
        }
        */
        public void Init() {
            TimeUsec          = default(ulong);
            XAcc              = default(float);
            YAcc              = default(float);
            ZAcc              = default(float);
            XVel              = default(float);
            YVel              = default(float);
            ZVel              = default(float);
            XPos              = default(float);
            YPos              = default(float);
            ZPos              = default(float);
            Airspeed          = default(float);
            VelVariance       = new float[  3];
            PosVariance       = new float[  3];
            Q                 = new float[  4];
            RollRate          = default(float);
            PitchRate         = default(float);
            YawRate           = default(float);
        }
        #endregion

        #region Read Buffer
        /// <summary>
        /// Reads the data from Buffer into this struct
        /// </summary>    
        public int Read(byte[] p_buffer,int p_offset=0) {
            int    l = 100;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals
            int[]  LS8  = MAVLinkCRC.U8_LSH8, LS16 = MAVLinkCRC.U8_LSH16, LS24 = MAVLinkCRC.U8_LSH24, LS32 = MAVLinkCRC.U8_LSH32, LS40 = MAVLinkCRC.U8_LSH40, LS48 = MAVLinkCRC.U8_LSH48, LS56 = MAVLinkCRC.U8_LSH56;
            Span<byte> b = p_buffer.AsSpan(p_offset);            
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            TimeUsec          = (ulong) ((ulong)b[p++] | (ulong)LS8[b[p++]] | (ulong)LS16[b[p++]] | (ulong)LS24[b[p++]] | (ulong)LS32[b[p++]] | (ulong)LS40[b[p++]] | (ulong)LS48[b[p++]] | (ulong)LS56[b[p++]]);
            XAcc              = (float) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            YAcc              = (float) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            ZAcc              = (float) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            XVel              = (float) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            YVel              = (float) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            ZVel              = (float) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            XPos              = (float) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            YPos              = (float) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            ZPos              = (float) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Airspeed          = (float) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            for(int i=0;i<3  ;i++) { VelVariance[i]    = (float) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4; }
            for(int i=0;i<3  ;i++) { PosVariance[i]    = (float) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4; }
            for(int i=0;i<4  ;i++) { Q[i]              = (float) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4; }
            RollRate          = (float) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            PitchRate         = (float) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            YawRate           = (float) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;            
            return p;
        }
        #endregion

        #region Write Buffer
        /// <summary>
        /// Writes the message data into a Buffer
        /// </summary>    
        public int Write(byte[] p_buffer,int p_offset=0) {
            int    l = 100;
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
            MemoryMarshal.Write(b.Slice(p, 4), ref XAcc             ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref YAcc             ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref ZAcc             ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref XVel             ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref YVel             ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref ZVel             ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref XPos             ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref YPos             ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref ZPos             ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref Airspeed         ); p+=4;
            for(int i=0;i<  3;i++) {
                MemoryMarshal.Write(b.Slice(p, 4), ref VelVariance[i]   ); p+=4;
            }
            for(int i=0;i<  3;i++) {
                MemoryMarshal.Write(b.Slice(p, 4), ref PosVariance[i]   ); p+=4;
            }
            for(int i=0;i<  4;i++) {
                MemoryMarshal.Write(b.Slice(p, 4), ref Q[i]             ); p+=4;
            }
            MemoryMarshal.Write(b.Slice(p, 4), ref RollRate         ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref PitchRate        ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref YawRate          ); p+=4;
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
            int l = 100;
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
