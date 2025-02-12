        
using System.IO;
using System.Runtime.InteropServices;

#pragma warning disable CS0675

namespace MAVLinkBindings {

    /// <summary>
    /// Report current used cellular network status
    /// </summary>    
    public struct CellularStatusData : IMAVLinkMessageData {

        /// <summary>
        /// Message Id Associated w/ this Struct
        /// </summary>    
        public int GetId() { return 334; }

        public ushort                            Mcc;               //Mobile country code. If unknown, set to UINT16_MAX
        public ushort                            Mnc;               //Mobile network code. If unknown, set to UINT16_MAX
        public ushort                            Lac;               //Location area code. If unknown, set to 0
        public CellularStatusFlag                Status;            //Cellular modem status
        public CellularNetworkFailedReasonFlags  FailureReason;     //Failure reason when status in in CELLUAR_STATUS_FAILED
        public CellularNetworkRadioTypeFlags     Type;              //Cellular network radio type: gsm, cdma, lte...
        public byte                              Quality;           //Signal quality in percent. If unknown, set to UINT8_MAX    

        #region CTOR
        /// <summary>
        /// Instantiates a new CellularStatusData
        /// </summary>    
        public CellularStatusData() {
            Mcc                 = default(ushort                          );
            Mnc                 = default(ushort                          );
            Lac                 = default(ushort                          );
            Status              = default(CellularStatusFlag              );
            FailureReason       = default(CellularNetworkFailedReasonFlags);
            Type                = default(CellularNetworkRadioTypeFlags   );
            Quality             = default(byte                            );
        }
        #endregion

        #region Read Buffer
        /// <summary>
        /// Reads the data from Buffer into this struct
        /// </summary>    
        public int Read(byte[] p_buffer,int p_offset=0) {
            int    l = 10;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals
            int[]  LS8  = MAVLinkCRC.U8_LSH8, LS16 = MAVLinkCRC.U8_LSH16, LS24 = MAVLinkCRC.U8_LSH24, LS32 = MAVLinkCRC.U8_LSH32, LS40 = MAVLinkCRC.U8_LSH40, LS48 = MAVLinkCRC.U8_LSH48, LS56 = MAVLinkCRC.U8_LSH56;
            Span<byte> b = p_buffer.AsSpan(p_offset);            
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            Mcc                 = (ushort                          ) (b[p++] | LS8[b[p++]]);
            Mnc                 = (ushort                          ) (b[p++] | LS8[b[p++]]);
            Lac                 = (ushort                          ) (b[p++] | LS8[b[p++]]);
            Status              = (CellularStatusFlag              ) (b[p++]);
            FailureReason       = (CellularNetworkFailedReasonFlags) (b[p++]);
            Type                = (CellularNetworkRadioTypeFlags   ) (b[p++]);
            Quality             = (byte                            ) (b[p++]);            
            return p;
        }
        #endregion

        #region Write Buffer
        /// <summary>
        /// Writes the message data into a Buffer
        /// </summary>    
        public int Write(byte[] p_buffer,int p_offset=0) {
            int    l = 10;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals            
            Span<byte> b = p_buffer.AsSpan(p_offset);
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            b[p++] = (byte)(      Mcc);
            b[p++] = (byte)((int)Mcc>>8 );
            b[p++] = (byte)(      Mnc);
            b[p++] = (byte)((int)Mnc>>8 );
            b[p++] = (byte)(      Lac);
            b[p++] = (byte)((int)Lac>>8 );
            b[p++] = (byte)(Status);
            b[p++] = (byte)(FailureReason);
            b[p++] = (byte)(Type);
            b[p++] = (byte)(Quality);
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
            int l = 10;
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
