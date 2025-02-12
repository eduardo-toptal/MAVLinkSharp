        
using System.IO;
using System.Runtime.InteropServices;

#pragma warning disable CS0675

namespace MAVLinkBindings {

    /// <summary>
    /// Status of the Iridium SBD link.
    /// </summary>    
    public struct IsbdLinkStatusData : IMAVLinkMessageData {

        /// <summary>
        /// Message Id Associated w/ this Struct
        /// </summary>    
        public int GetId() { return 335; }

        public ulong   Timestamp;              //Timestamp (UNIX Epoch time or time since system boot). The receiving end can infer timestamp format (since 1.1.1970 or since system boot) by checking for the magnitude of the number.
        public ulong   LastHeartbeat;          //Timestamp of the last successful sbd session. The receiving end can infer timestamp format (since 1.1.1970 or since system boot) by checking for the magnitude of the number.
        public ushort  FailedSessions;         //Number of failed SBD sessions.
        public ushort  SuccessfulSessions;     //Number of successful SBD sessions.
        public byte    SignalQuality;          //Signal quality equal to the number of bars displayed on the ISU signal strength indicator. Range is 0 to 5, where 0 indicates no signal and 5 indicates maximum signal strength.
        public byte    RingPending;            //1: Ring call pending, 0: No call pending.
        public byte    TxSessionPending;       //1: Transmission session pending, 0: No transmission session pending.
        public byte    RxSessionPending;       //1: Receiving session pending, 0: No receiving session pending.    

        #region CTOR
        /// <summary>
        /// Instantiates a new IsbdLinkStatusData
        /// </summary>    
        public IsbdLinkStatusData() {
            Timestamp                = default(ulong );
            LastHeartbeat            = default(ulong );
            FailedSessions           = default(ushort);
            SuccessfulSessions       = default(ushort);
            SignalQuality            = default(byte  );
            RingPending              = default(byte  );
            TxSessionPending         = default(byte  );
            RxSessionPending         = default(byte  );
        }
        #endregion

        #region Read Buffer
        /// <summary>
        /// Reads the data from Buffer into this struct
        /// </summary>    
        public int Read(byte[] p_buffer,int p_offset=0) {
            int    l = 24;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals
            int[]  LS8  = MAVLinkCRC.U8_LSH8, LS16 = MAVLinkCRC.U8_LSH16, LS24 = MAVLinkCRC.U8_LSH24, LS32 = MAVLinkCRC.U8_LSH32, LS40 = MAVLinkCRC.U8_LSH40, LS48 = MAVLinkCRC.U8_LSH48, LS56 = MAVLinkCRC.U8_LSH56;
            Span<byte> b = p_buffer.AsSpan(p_offset);            
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            Timestamp                = (ulong ) ((ulong)b[p++] | (ulong)LS8[b[p++]] | (ulong)LS16[b[p++]] | (ulong)LS24[b[p++]] | (ulong)LS32[b[p++]] | (ulong)LS40[b[p++]] | (ulong)LS48[b[p++]] | (ulong)LS56[b[p++]]);
            LastHeartbeat            = (ulong ) ((ulong)b[p++] | (ulong)LS8[b[p++]] | (ulong)LS16[b[p++]] | (ulong)LS24[b[p++]] | (ulong)LS32[b[p++]] | (ulong)LS40[b[p++]] | (ulong)LS48[b[p++]] | (ulong)LS56[b[p++]]);
            FailedSessions           = (ushort) (b[p++] | LS8[b[p++]]);
            SuccessfulSessions       = (ushort) (b[p++] | LS8[b[p++]]);
            SignalQuality            = (byte  ) (b[p++]);
            RingPending              = (byte  ) (b[p++]);
            TxSessionPending         = (byte  ) (b[p++]);
            RxSessionPending         = (byte  ) (b[p++]);            
            return p;
        }
        #endregion

        #region Write Buffer
        /// <summary>
        /// Writes the message data into a Buffer
        /// </summary>    
        public int Write(byte[] p_buffer,int p_offset=0) {
            int    l = 24;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals            
            Span<byte> b = p_buffer.AsSpan(p_offset);
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            b[p++] = (byte)(      Timestamp);
            b[p++] = (byte)((long)Timestamp>>8 );
            b[p++] = (byte)((long)Timestamp>>16);
            b[p++] = (byte)((long)Timestamp>>24);
            b[p++] = (byte)((long)Timestamp>>32);
            b[p++] = (byte)((long)Timestamp>>40);
            b[p++] = (byte)((long)Timestamp>>48);
            b[p++] = (byte)((long)Timestamp>>56);
            b[p++] = (byte)(      LastHeartbeat);
            b[p++] = (byte)((long)LastHeartbeat>>8 );
            b[p++] = (byte)((long)LastHeartbeat>>16);
            b[p++] = (byte)((long)LastHeartbeat>>24);
            b[p++] = (byte)((long)LastHeartbeat>>32);
            b[p++] = (byte)((long)LastHeartbeat>>40);
            b[p++] = (byte)((long)LastHeartbeat>>48);
            b[p++] = (byte)((long)LastHeartbeat>>56);
            b[p++] = (byte)(      FailedSessions);
            b[p++] = (byte)((int)FailedSessions>>8 );
            b[p++] = (byte)(      SuccessfulSessions);
            b[p++] = (byte)((int)SuccessfulSessions>>8 );
            b[p++] = (byte)(SignalQuality);
            b[p++] = (byte)(RingPending);
            b[p++] = (byte)(TxSessionPending);
            b[p++] = (byte)(RxSessionPending);
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
            int l = 24;
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
