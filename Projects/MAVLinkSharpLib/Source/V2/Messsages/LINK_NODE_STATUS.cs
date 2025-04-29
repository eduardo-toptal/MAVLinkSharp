        
using System;
using System.IO;
using System.Runtime.InteropServices;
using MAVLinkSharp.Runtime;

#pragma warning disable CS0675

namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// Status generated in each node in the communication chain and injected into MAVLink stream.
    /// </summary>    
    public struct LinkNodeStatusData : IMAVLinkMessageData {

        /// <summary>
        /// Message Id Associated w/ this Struct
        /// </summary>    
        public int GetId() { return 8; }

        public ulong   Timestamp;            //Timestamp (time since system boot).
        public uint    TxRate;               //Transmit rate
        public uint    RxRate;               //Receive rate
        public uint    MessagesSent;         //Messages sent
        public uint    MessagesReceived;     //Messages received (estimated from counting seq)
        public uint    MessagesLost;         //Messages lost (estimated from counting seq)
        public ushort  RxParseErr;           //Number of bytes that could not be parsed correctly.
        public ushort  TxOverflows;          //Transmit buffer overflows. This number wraps around as it reaches UINT16_MAX
        public ushort  RxOverflows;          //Receive buffer overflows. This number wraps around as it reaches UINT16_MAX
        public byte    TxBuf;                //Remaining free transmit buffer space
        public byte    RxBuf;                //Remaining free receive buffer space    

        #region CTOR
        /// <summary>
        /// Instantiates a new LinkNodeStatusData
        /// </summary>    
        /*
        public LinkNodeStatusData() {
            Init();
        }
        */
        public void Init() {
            Timestamp              = default(ulong );
            TxRate                 = default(uint  );
            RxRate                 = default(uint  );
            MessagesSent           = default(uint  );
            MessagesReceived       = default(uint  );
            MessagesLost           = default(uint  );
            RxParseErr             = default(ushort);
            TxOverflows            = default(ushort);
            RxOverflows            = default(ushort);
            TxBuf                  = default(byte  );
            RxBuf                  = default(byte  );
        }
        #endregion

        #region Read Buffer
        /// <summary>
        /// Reads the data from Buffer into this struct
        /// </summary>    
        public int Read(byte[] p_buffer,int p_offset=0) {
            int    l = 36;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals
            int[]  LS8  = MAVLinkCRC.U8_LSH8, LS16 = MAVLinkCRC.U8_LSH16, LS24 = MAVLinkCRC.U8_LSH24, LS32 = MAVLinkCRC.U8_LSH32, LS40 = MAVLinkCRC.U8_LSH40, LS48 = MAVLinkCRC.U8_LSH48, LS56 = MAVLinkCRC.U8_LSH56;
            Span<byte> b = p_buffer.AsSpan(p_offset);            
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            Timestamp              = (ulong ) ((ulong)b[p++] | (ulong)LS8[b[p++]] | (ulong)LS16[b[p++]] | (ulong)LS24[b[p++]] | (ulong)LS32[b[p++]] | (ulong)LS40[b[p++]] | (ulong)LS48[b[p++]] | (ulong)LS56[b[p++]]);
            TxRate                 = (uint  ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            RxRate                 = (uint  ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            MessagesSent           = (uint  ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            MessagesReceived       = (uint  ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            MessagesLost           = (uint  ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            RxParseErr             = (ushort) (b[p++] | LS8[b[p++]]);
            TxOverflows            = (ushort) (b[p++] | LS8[b[p++]]);
            RxOverflows            = (ushort) (b[p++] | LS8[b[p++]]);
            TxBuf                  = (byte  ) (b[p++]);
            RxBuf                  = (byte  ) (b[p++]);            
            return p;
        }
        #endregion

        #region Write Buffer
        /// <summary>
        /// Writes the message data into a Buffer
        /// </summary>    
        public int Write(byte[] p_buffer,int p_offset=0) {
            int    l = 36;
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
            b[p++] = (byte)(      TxRate);
            b[p++] = (byte)((int)TxRate>>8 );
            b[p++] = (byte)((int)TxRate>>16);
            b[p++] = (byte)((int)TxRate>>24);
            b[p++] = (byte)(      RxRate);
            b[p++] = (byte)((int)RxRate>>8 );
            b[p++] = (byte)((int)RxRate>>16);
            b[p++] = (byte)((int)RxRate>>24);
            b[p++] = (byte)(      MessagesSent);
            b[p++] = (byte)((int)MessagesSent>>8 );
            b[p++] = (byte)((int)MessagesSent>>16);
            b[p++] = (byte)((int)MessagesSent>>24);
            b[p++] = (byte)(      MessagesReceived);
            b[p++] = (byte)((int)MessagesReceived>>8 );
            b[p++] = (byte)((int)MessagesReceived>>16);
            b[p++] = (byte)((int)MessagesReceived>>24);
            b[p++] = (byte)(      MessagesLost);
            b[p++] = (byte)((int)MessagesLost>>8 );
            b[p++] = (byte)((int)MessagesLost>>16);
            b[p++] = (byte)((int)MessagesLost>>24);
            b[p++] = (byte)(      RxParseErr);
            b[p++] = (byte)((int)RxParseErr>>8 );
            b[p++] = (byte)(      TxOverflows);
            b[p++] = (byte)((int)TxOverflows>>8 );
            b[p++] = (byte)(      RxOverflows);
            b[p++] = (byte)((int)RxOverflows>>8 );
            b[p++] = (byte)(TxBuf);
            b[p++] = (byte)(RxBuf);
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
            int l = 36;
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
