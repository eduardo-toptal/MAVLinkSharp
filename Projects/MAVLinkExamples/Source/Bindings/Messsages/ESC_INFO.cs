        
using System.IO;
using System.Runtime.InteropServices;

#pragma warning disable CS0675

namespace MAVLinkBindings {

    /// <summary>
    /// ESC information for lower rate streaming. Recommended streaming rate 1Hz. See ESC_STATUS for higher-rate ESC data.
    /// </summary>    
    public struct EscInfoData : IMAVLinkMessageData {

        /// <summary>
        /// Message Id Associated w/ this Struct
        /// </summary>    
        public int GetId() { return 290; }

        public ulong                   TimeUsec;           //Timestamp (UNIX Epoch time or time since system boot). The receiving end can infer timestamp format (since 1.1.1970 or since system boot) by checking for the magnitude the number.
        public uint[]                  ErrorCount;         //Number of reported errors by each ESC since boot.
        public ushort                  Counter;            //Counter of data packets received.
        public EscFailureFlags[]       FailureFlags;       //Bitmap of ESC failure flags.
        public short[]                 Temperature;        //Temperature of each ESC. INT16_MAX: if data not supplied by ESC.
        public byte                    Index;              //Index of the first ESC in this message. minValue = 0, maxValue = 60, increment = 4.
        public byte                    Count;              //Total number of ESCs in all messages of this type. Message fields with an index higher than this should be ignored because they contain invalid data.
        public EscConnectionTypeFlags  ConnectionType;     //Connection type protocol for all ESC.
        public byte                    Info;               //Information regarding online/offline status of each ESC.    

        #region CTOR
        /// <summary>
        /// Instantiates a new EscInfoData
        /// </summary>    
        public EscInfoData() {
            TimeUsec             = default(ulong                 );
            ErrorCount           = new uint[  4];
            Counter              = default(ushort                );
            FailureFlags         = new EscFailureFlags[  4];
            Temperature          = new short[  4];
            Index                = default(byte                  );
            Count                = default(byte                  );
            ConnectionType       = default(EscConnectionTypeFlags);
            Info                 = default(byte                  );
        }
        #endregion

        #region Read Buffer
        /// <summary>
        /// Reads the data from Buffer into this struct
        /// </summary>    
        public int Read(byte[] p_buffer,int p_offset=0) {
            int    l = 46;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals
            int[]  LS8  = MAVLinkCRC.U8_LSH8, LS16 = MAVLinkCRC.U8_LSH16, LS24 = MAVLinkCRC.U8_LSH24, LS32 = MAVLinkCRC.U8_LSH32, LS40 = MAVLinkCRC.U8_LSH40, LS48 = MAVLinkCRC.U8_LSH48, LS56 = MAVLinkCRC.U8_LSH56;
            Span<byte> b = p_buffer.AsSpan(p_offset);            
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            TimeUsec             = (ulong                 ) ((ulong)b[p++] | (ulong)LS8[b[p++]] | (ulong)LS16[b[p++]] | (ulong)LS24[b[p++]] | (ulong)LS32[b[p++]] | (ulong)LS40[b[p++]] | (ulong)LS48[b[p++]] | (ulong)LS56[b[p++]]);
            for(int i=0;i<4  ;i++) { ErrorCount[i]        = (uint                  ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]); }
            Counter              = (ushort                ) (b[p++] | LS8[b[p++]]);
            for(int i=0;i<4  ;i++) { FailureFlags[i]      = (EscFailureFlags       ) (b[p++] | LS8[b[p++]]); }
            for(int i=0;i<4  ;i++) { Temperature[i]       = (short                 ) (b[p++] | LS8[b[p++]]); }
            Index                = (byte                  ) (b[p++]);
            Count                = (byte                  ) (b[p++]);
            ConnectionType       = (EscConnectionTypeFlags) (b[p++]);
            Info                 = (byte                  ) (b[p++]);            
            return p;
        }
        #endregion

        #region Write Buffer
        /// <summary>
        /// Writes the message data into a Buffer
        /// </summary>    
        public int Write(byte[] p_buffer,int p_offset=0) {
            int    l = 46;
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
            for(int i=0;i<  4;i++) {
                b[p++] = (byte)(      ErrorCount[i]);
                b[p++] = (byte)((int)ErrorCount[i]>>8 );
                b[p++] = (byte)((int)ErrorCount[i]>>16);
                b[p++] = (byte)((int)ErrorCount[i]>>24);
            }
            b[p++] = (byte)(      Counter);
            b[p++] = (byte)((int)Counter>>8 );
            for(int i=0;i<  4;i++) {
                b[p++] = (byte)(      FailureFlags[i]);
                b[p++] = (byte)((int)FailureFlags[i]>>8 );
            }
            for(int i=0;i<  4;i++) {
                b[p++] = (byte)(      Temperature[i]);
                b[p++] = (byte)((int)Temperature[i]>>8 );
            }
            b[p++] = (byte)(Index);
            b[p++] = (byte)(Count);
            b[p++] = (byte)(ConnectionType);
            b[p++] = (byte)(Info);
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
            int l = 46;
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
