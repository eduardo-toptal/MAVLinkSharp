        
using System;
using System.IO;
using System.Runtime.InteropServices;
using MAVLinkSharp.Runtime;

#pragma warning disable CS0675

namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// The system time is the time of the sender's master clock.
    /// This can be emitted by flight controllers, onboard computers, or other components in the MAVLink network.
    /// Components that are using a less reliable time source, such as a battery-backed real time clock, can choose to match their system clock to that of a system that indicates a more recent time.
    /// This allows more broadly accurate date stamping of logs, and so on.
    /// If precise time synchronization is needed then use TIMESYNC instead.
    /// </summary>    
    public struct SystemTimeData : IMAVLinkMessageData {

        /// <summary>
        /// Message Id Associated w/ this Struct
        /// </summary>    
        public int GetId() { return 2; }

        public ulong  TimeUnixUsec;      //Timestamp (UNIX epoch time).
        public uint   TimeBootMs;        //Timestamp (time since system boot).    

        #region CTOR
        /// <summary>
        /// Instantiates a new SystemTimeData
        /// </summary>    
        /*
        public SystemTimeData() {
            Init();
        }
        */
        public void Init() {
            TimeUnixUsec        = default(ulong);
            TimeBootMs          = default(uint );
        }
        #endregion

        #region Read Buffer
        /// <summary>
        /// Reads the data from Buffer into this struct
        /// </summary>    
        public int Read(byte[] p_buffer,int p_offset=0) {
            int    l = 12;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals
            int[]  LS8  = MAVLinkCRC.U8_LSH8, LS16 = MAVLinkCRC.U8_LSH16, LS24 = MAVLinkCRC.U8_LSH24, LS32 = MAVLinkCRC.U8_LSH32, LS40 = MAVLinkCRC.U8_LSH40, LS48 = MAVLinkCRC.U8_LSH48, LS56 = MAVLinkCRC.U8_LSH56;
            Span<byte> b = p_buffer.AsSpan(p_offset);            
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            TimeUnixUsec        = (ulong) ((ulong)b[p++] | (ulong)LS8[b[p++]] | (ulong)LS16[b[p++]] | (ulong)LS24[b[p++]] | (ulong)LS32[b[p++]] | (ulong)LS40[b[p++]] | (ulong)LS48[b[p++]] | (ulong)LS56[b[p++]]);
            TimeBootMs          = (uint ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);            
            return p;
        }
        #endregion

        #region Write Buffer
        /// <summary>
        /// Writes the message data into a Buffer
        /// </summary>    
        public int Write(byte[] p_buffer,int p_offset=0) {
            int    l = 12;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals            
            Span<byte> b = p_buffer.AsSpan(p_offset);
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            b[p++] = (byte)(      TimeUnixUsec);
            b[p++] = (byte)((long)TimeUnixUsec>>8 );
            b[p++] = (byte)((long)TimeUnixUsec>>16);
            b[p++] = (byte)((long)TimeUnixUsec>>24);
            b[p++] = (byte)((long)TimeUnixUsec>>32);
            b[p++] = (byte)((long)TimeUnixUsec>>40);
            b[p++] = (byte)((long)TimeUnixUsec>>48);
            b[p++] = (byte)((long)TimeUnixUsec>>56);
            b[p++] = (byte)(      TimeBootMs);
            b[p++] = (byte)((int)TimeBootMs>>8 );
            b[p++] = (byte)((int)TimeBootMs>>16);
            b[p++] = (byte)((int)TimeBootMs>>24);
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
            int l = 12;
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
