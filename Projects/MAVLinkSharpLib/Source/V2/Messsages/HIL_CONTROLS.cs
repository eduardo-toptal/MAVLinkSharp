        
using System;
using System.IO;
using System.Runtime.InteropServices;
using MAVLinkSharp.Runtime;

#pragma warning disable CS0675

namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// Sent from autopilot to simulation. Hardware in the loop control outputs
    /// </summary>    
    public struct HilControlsData : IMAVLinkMessageData {

        /// <summary>
        /// Message Id Associated w/ this Struct
        /// </summary>    
        public int GetId() { return 91; }

        public ulong         TimeUsec;          //Timestamp (UNIX Epoch time or time since system boot). The receiving end can infer timestamp format (since 1.1.1970 or since system boot) by checking for the magnitude of the number.
        public float         RollAilerons;      //Control output -1 .. 1
        public float         PitchElevator;     //Control output -1 .. 1
        public float         YawRudder;         //Control output -1 .. 1
        public float         Throttle;          //Throttle 0 .. 1
        public float         Aux1;              //Aux 1, -1 .. 1
        public float         Aux2;              //Aux 2, -1 .. 1
        public float         Aux3;              //Aux 3, -1 .. 1
        public float         Aux4;              //Aux 4, -1 .. 1
        public MAVModeFlags  Mode;              //System mode.
        public byte          NavMode;           //Navigation mode (MAV_NAV_MODE)    

        #region CTOR
        /// <summary>
        /// Instantiates a new HilControlsData
        /// </summary>    
        /*
        public HilControlsData() {
            Init();
        }
        */
        public void Init() {
            TimeUsec            = default(ulong       );
            RollAilerons        = default(float       );
            PitchElevator       = default(float       );
            YawRudder           = default(float       );
            Throttle            = default(float       );
            Aux1                = default(float       );
            Aux2                = default(float       );
            Aux3                = default(float       );
            Aux4                = default(float       );
            Mode                = default(MAVModeFlags);
            NavMode             = default(byte        );
        }
        #endregion

        #region Read Buffer
        /// <summary>
        /// Reads the data from Buffer into this struct
        /// </summary>    
        public int Read(byte[] p_buffer,int p_offset=0) {
            int    l = 42;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals
            int[]  LS8  = MAVLinkCRC.U8_LSH8, LS16 = MAVLinkCRC.U8_LSH16, LS24 = MAVLinkCRC.U8_LSH24, LS32 = MAVLinkCRC.U8_LSH32, LS40 = MAVLinkCRC.U8_LSH40, LS48 = MAVLinkCRC.U8_LSH48, LS56 = MAVLinkCRC.U8_LSH56;
            Span<byte> b = p_buffer.AsSpan(p_offset);            
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            TimeUsec            = (ulong       ) ((ulong)b[p++] | (ulong)LS8[b[p++]] | (ulong)LS16[b[p++]] | (ulong)LS24[b[p++]] | (ulong)LS32[b[p++]] | (ulong)LS40[b[p++]] | (ulong)LS48[b[p++]] | (ulong)LS56[b[p++]]);
            RollAilerons        = (float       ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            PitchElevator       = (float       ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            YawRudder           = (float       ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Throttle            = (float       ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Aux1                = (float       ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Aux2                = (float       ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Aux3                = (float       ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Aux4                = (float       ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Mode                = (MAVModeFlags) (b[p++]);
            NavMode             = (byte        ) (b[p++]);            
            return p;
        }
        #endregion

        #region Write Buffer
        /// <summary>
        /// Writes the message data into a Buffer
        /// </summary>    
        public int Write(byte[] p_buffer,int p_offset=0) {
            int    l = 42;
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
            MemoryMarshal.Write(b.Slice(p, 4), ref RollAilerons       ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref PitchElevator      ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref YawRudder          ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref Throttle           ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref Aux1               ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref Aux2               ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref Aux3               ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref Aux4               ); p+=4;
            b[p++] = (byte)(Mode);
            b[p++] = (byte)(NavMode);
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
            int l = 42;
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
