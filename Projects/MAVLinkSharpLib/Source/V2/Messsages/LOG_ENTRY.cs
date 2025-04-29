        
using System.IO;
using System.Runtime.InteropServices;
using MAVLinkSharp.Runtime;

#pragma warning disable CS0675

namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// Reply to LOG_REQUEST_LIST
    /// </summary>    
    public struct LogEntryData : IMAVLinkMessageData {

        /// <summary>
        /// Message Id Associated w/ this Struct
        /// </summary>    
        public int GetId() { return 118; }

        public uint    TimeUtc;         //UTC timestamp of log since 1970, or 0 if not available
        public uint    Size;            //Size of the log (may be approximate)
        public ushort  Id;              //Log id
        public ushort  NumLogs;         //Total number of logs
        public ushort  LastLogNum;      //High log number    

        #region CTOR
        /// <summary>
        /// Instantiates a new LogEntryData
        /// </summary>    
        public LogEntryData() {
            TimeUtc           = default(uint  );
            Size              = default(uint  );
            Id                = default(ushort);
            NumLogs           = default(ushort);
            LastLogNum        = default(ushort);
        }
        #endregion

        #region Read Buffer
        /// <summary>
        /// Reads the data from Buffer into this struct
        /// </summary>    
        public int Read(byte[] p_buffer,int p_offset=0) {
            int    l = 14;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals
            int[]  LS8  = MAVLinkCRC.U8_LSH8, LS16 = MAVLinkCRC.U8_LSH16, LS24 = MAVLinkCRC.U8_LSH24, LS32 = MAVLinkCRC.U8_LSH32, LS40 = MAVLinkCRC.U8_LSH40, LS48 = MAVLinkCRC.U8_LSH48, LS56 = MAVLinkCRC.U8_LSH56;
            Span<byte> b = p_buffer.AsSpan(p_offset);            
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            TimeUtc           = (uint  ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            Size              = (uint  ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            Id                = (ushort) (b[p++] | LS8[b[p++]]);
            NumLogs           = (ushort) (b[p++] | LS8[b[p++]]);
            LastLogNum        = (ushort) (b[p++] | LS8[b[p++]]);            
            return p;
        }
        #endregion

        #region Write Buffer
        /// <summary>
        /// Writes the message data into a Buffer
        /// </summary>    
        public int Write(byte[] p_buffer,int p_offset=0) {
            int    l = 14;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals            
            Span<byte> b = p_buffer.AsSpan(p_offset);
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            b[p++] = (byte)(      TimeUtc);
            b[p++] = (byte)((int)TimeUtc>>8 );
            b[p++] = (byte)((int)TimeUtc>>16);
            b[p++] = (byte)((int)TimeUtc>>24);
            b[p++] = (byte)(      Size);
            b[p++] = (byte)((int)Size>>8 );
            b[p++] = (byte)((int)Size>>16);
            b[p++] = (byte)((int)Size>>24);
            b[p++] = (byte)(      Id);
            b[p++] = (byte)((int)Id>>8 );
            b[p++] = (byte)(      NumLogs);
            b[p++] = (byte)((int)NumLogs>>8 );
            b[p++] = (byte)(      LastLogNum);
            b[p++] = (byte)((int)LastLogNum>>8 );
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
            int l = 14;
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
