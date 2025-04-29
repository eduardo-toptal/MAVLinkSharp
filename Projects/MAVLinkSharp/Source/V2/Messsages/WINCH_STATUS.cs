        
using System.IO;
using System.Runtime.InteropServices;
using MAVLinkSharp.Runtime;

#pragma warning disable CS0675

namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// Winch status.
    /// </summary>    
    public struct WinchStatusData : IMAVLinkMessageData {

        /// <summary>
        /// Message Id Associated w/ this Struct
        /// </summary>    
        public int GetId() { return 9005; }

        public ulong               TimeUsec;       //Timestamp (synced to UNIX time or since system boot).
        public float               LineLength;     //Length of line released. NaN if unknown
        public float               Speed;          //Speed line is being released or retracted. Positive values if being released, negative values if being retracted, NaN if unknown
        public float               Tension;        //Tension on the line. NaN if unknown
        public float               Voltage;        //Voltage of the battery supplying the winch. NaN if unknown
        public float               Current;        //Current draw from the winch. NaN if unknown
        public MAVWinchStatusFlag  Status;         //Status flags
        public short               Temperature;    //Temperature of the motor. INT16_MAX if unknown    

        #region CTOR
        /// <summary>
        /// Instantiates a new WinchStatusData
        /// </summary>    
        public WinchStatusData() {
            TimeUsec         = default(ulong             );
            LineLength       = default(float             );
            Speed            = default(float             );
            Tension          = default(float             );
            Voltage          = default(float             );
            Current          = default(float             );
            Status           = default(MAVWinchStatusFlag);
            Temperature      = default(short             );
        }
        #endregion

        #region Read Buffer
        /// <summary>
        /// Reads the data from Buffer into this struct
        /// </summary>    
        public int Read(byte[] p_buffer,int p_offset=0) {
            int    l = 34;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals
            int[]  LS8  = MAVLinkCRC.U8_LSH8, LS16 = MAVLinkCRC.U8_LSH16, LS24 = MAVLinkCRC.U8_LSH24, LS32 = MAVLinkCRC.U8_LSH32, LS40 = MAVLinkCRC.U8_LSH40, LS48 = MAVLinkCRC.U8_LSH48, LS56 = MAVLinkCRC.U8_LSH56;
            Span<byte> b = p_buffer.AsSpan(p_offset);            
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            TimeUsec         = (ulong             ) ((ulong)b[p++] | (ulong)LS8[b[p++]] | (ulong)LS16[b[p++]] | (ulong)LS24[b[p++]] | (ulong)LS32[b[p++]] | (ulong)LS40[b[p++]] | (ulong)LS48[b[p++]] | (ulong)LS56[b[p++]]);
            LineLength       = (float             ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Speed            = (float             ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Tension          = (float             ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Voltage          = (float             ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Current          = (float             ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Status           = (MAVWinchStatusFlag) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            Temperature      = (short             ) (b[p++] | LS8[b[p++]]);            
            return p;
        }
        #endregion

        #region Write Buffer
        /// <summary>
        /// Writes the message data into a Buffer
        /// </summary>    
        public int Write(byte[] p_buffer,int p_offset=0) {
            int    l = 34;
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
            MemoryMarshal.Write(b.Slice(p, 4), ref LineLength      ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref Speed           ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref Tension         ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref Voltage         ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref Current         ); p+=4;
            b[p++] = (byte)(      Status);
            b[p++] = (byte)((int)Status>>8 );
            b[p++] = (byte)((int)Status>>16);
            b[p++] = (byte)((int)Status>>24);
            b[p++] = (byte)(      Temperature);
            b[p++] = (byte)((int)Temperature>>8 );
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
            int l = 34;
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
