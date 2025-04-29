        
using System.IO;
using System.Runtime.InteropServices;
using MAVLinkSharp.Runtime;

#pragma warning disable CS0675

namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// Information about flight since last arming.
    /// This can be requested using MAV_CMD_REQUEST_MESSAGE.
    /// 
    /// </summary>    
    public struct FlightInformationData : IMAVLinkMessageData {

        /// <summary>
        /// Message Id Associated w/ this Struct
        /// </summary>    
        public int GetId() { return 264; }

        public ulong  ArmingTimeUtc;       //Timestamp at arming (time since UNIX epoch) in UTC, 0 for unknown
        public ulong  TakeoffTimeUtc;      //Timestamp at takeoff (time since UNIX epoch) in UTC, 0 for unknown
        public ulong  FlightUuid;          //Universally unique identifier (UUID) of flight, should correspond to name of log files
        public uint   TimeBootMs;          //Timestamp (time since system boot).    

        #region CTOR
        /// <summary>
        /// Instantiates a new FlightInformationData
        /// </summary>    
        /*
        public FlightInformationData() {
            Init();
        }
        */
        public void Init() {
            ArmingTimeUtc         = default(ulong);
            TakeoffTimeUtc        = default(ulong);
            FlightUuid            = default(ulong);
            TimeBootMs            = default(uint );
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
            ArmingTimeUtc         = (ulong) ((ulong)b[p++] | (ulong)LS8[b[p++]] | (ulong)LS16[b[p++]] | (ulong)LS24[b[p++]] | (ulong)LS32[b[p++]] | (ulong)LS40[b[p++]] | (ulong)LS48[b[p++]] | (ulong)LS56[b[p++]]);
            TakeoffTimeUtc        = (ulong) ((ulong)b[p++] | (ulong)LS8[b[p++]] | (ulong)LS16[b[p++]] | (ulong)LS24[b[p++]] | (ulong)LS32[b[p++]] | (ulong)LS40[b[p++]] | (ulong)LS48[b[p++]] | (ulong)LS56[b[p++]]);
            FlightUuid            = (ulong) ((ulong)b[p++] | (ulong)LS8[b[p++]] | (ulong)LS16[b[p++]] | (ulong)LS24[b[p++]] | (ulong)LS32[b[p++]] | (ulong)LS40[b[p++]] | (ulong)LS48[b[p++]] | (ulong)LS56[b[p++]]);
            TimeBootMs            = (uint ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);            
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
            b[p++] = (byte)(      ArmingTimeUtc);
            b[p++] = (byte)((long)ArmingTimeUtc>>8 );
            b[p++] = (byte)((long)ArmingTimeUtc>>16);
            b[p++] = (byte)((long)ArmingTimeUtc>>24);
            b[p++] = (byte)((long)ArmingTimeUtc>>32);
            b[p++] = (byte)((long)ArmingTimeUtc>>40);
            b[p++] = (byte)((long)ArmingTimeUtc>>48);
            b[p++] = (byte)((long)ArmingTimeUtc>>56);
            b[p++] = (byte)(      TakeoffTimeUtc);
            b[p++] = (byte)((long)TakeoffTimeUtc>>8 );
            b[p++] = (byte)((long)TakeoffTimeUtc>>16);
            b[p++] = (byte)((long)TakeoffTimeUtc>>24);
            b[p++] = (byte)((long)TakeoffTimeUtc>>32);
            b[p++] = (byte)((long)TakeoffTimeUtc>>40);
            b[p++] = (byte)((long)TakeoffTimeUtc>>48);
            b[p++] = (byte)((long)TakeoffTimeUtc>>56);
            b[p++] = (byte)(      FlightUuid);
            b[p++] = (byte)((long)FlightUuid>>8 );
            b[p++] = (byte)((long)FlightUuid>>16);
            b[p++] = (byte)((long)FlightUuid>>24);
            b[p++] = (byte)((long)FlightUuid>>32);
            b[p++] = (byte)((long)FlightUuid>>40);
            b[p++] = (byte)((long)FlightUuid>>48);
            b[p++] = (byte)((long)FlightUuid>>56);
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
