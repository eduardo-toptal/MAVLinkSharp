        
using System;
using System.IO;
using System.Runtime.InteropServices;
using MAVLinkSharp.Runtime;

#pragma warning disable CS0675

namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// Sent from autopilot to simulation. Hardware in the loop control outputs (replacement for HIL_CONTROLS)
    /// </summary>    
    public struct HilActuatorControlsData : IMAVLinkMessageData {

        /// <summary>
        /// Message Id Associated w/ this Struct
        /// </summary>    
        public int GetId() { return 93; }

        public ulong        TimeUsec;     //Timestamp (UNIX Epoch time or time since system boot). The receiving end can infer timestamp format (since 1.1.1970 or since system boot) by checking for the magnitude of the number.
        public ulong        Flags;        //Flags as bitfield, 1: indicate simulation using lockstep.
        public float[]      Controls;     //Control outputs -1 .. 1. Channel assignment depends on the simulated hardware.
        public MAVModeFlag  Mode;         //System mode. Includes arming state.    

        #region CTOR
        /// <summary>
        /// Instantiates a new HilActuatorControlsData
        /// </summary>    
        /*
        public HilActuatorControlsData() {
            Init();
        }
        */
        public void Init() {
            TimeUsec       = default(ulong      );
            Flags          = default(ulong      );
            Controls       = new float[ 16];
            Mode           = default(MAVModeFlag);
        }
        #endregion

        #region Read Buffer
        /// <summary>
        /// Reads the data from Buffer into this struct
        /// </summary>    
        public int Read(byte[] p_buffer,int p_offset=0) {
            int    l = 81;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals
            int[]  LS8  = MAVLinkCRC.U8_LSH8, LS16 = MAVLinkCRC.U8_LSH16, LS24 = MAVLinkCRC.U8_LSH24, LS32 = MAVLinkCRC.U8_LSH32, LS40 = MAVLinkCRC.U8_LSH40, LS48 = MAVLinkCRC.U8_LSH48, LS56 = MAVLinkCRC.U8_LSH56;
            Span<byte> b = p_buffer.AsSpan(p_offset);            
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            TimeUsec       = (ulong      ) ((ulong)b[p++] | (ulong)LS8[b[p++]] | (ulong)LS16[b[p++]] | (ulong)LS24[b[p++]] | (ulong)LS32[b[p++]] | (ulong)LS40[b[p++]] | (ulong)LS48[b[p++]] | (ulong)LS56[b[p++]]);
            Flags          = (ulong      ) ((ulong)b[p++] | (ulong)LS8[b[p++]] | (ulong)LS16[b[p++]] | (ulong)LS24[b[p++]] | (ulong)LS32[b[p++]] | (ulong)LS40[b[p++]] | (ulong)LS48[b[p++]] | (ulong)LS56[b[p++]]);
            for(int i=0;i<16 ;i++) { Controls[i]    = (float      ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4; }
            Mode           = (MAVModeFlag) (b[p++]);            
            return p;
        }
        #endregion

        #region Write Buffer
        /// <summary>
        /// Writes the message data into a Buffer
        /// </summary>    
        public int Write(byte[] p_buffer,int p_offset=0) {
            int    l = 81;
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
            b[p++] = (byte)(      Flags);
            b[p++] = (byte)((long)Flags>>8 );
            b[p++] = (byte)((long)Flags>>16);
            b[p++] = (byte)((long)Flags>>24);
            b[p++] = (byte)((long)Flags>>32);
            b[p++] = (byte)((long)Flags>>40);
            b[p++] = (byte)((long)Flags>>48);
            b[p++] = (byte)((long)Flags>>56);
            for(int i=0;i< 16;i++) {
                MemoryMarshal.Write(b.Slice(p, 4), ref Controls[i]   ); p+=4;
            }
            b[p++] = (byte)(Mode);
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
            int l = 81;
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
