        
using System.IO;
using System.Runtime.InteropServices;

#pragma warning disable CS0675

namespace MAVLinkBindings {

    /// <summary>
    /// Configure cellular modems.
    /// This message is re-emitted as an acknowledgement by the modem.
    /// The message may also be explicitly requested using MAV_CMD_REQUEST_MESSAGE.
    /// </summary>    
    public struct CellularConfigData : IMAVLinkMessageData {

        /// <summary>
        /// Message Id Associated w/ this Struct
        /// </summary>    
        public int GetId() { return 336; }

        public byte                         EnableLte;     //Enable/disable LTE. 0: setting unchanged, 1: disabled, 2: enabled. Current setting when sent back as a response.
        public byte                         EnablePin;     //Enable/disable PIN on the SIM card. 0: setting unchanged, 1: disabled, 2: enabled. Current setting when sent back as a response.
        public char[]                       Pin;           //PIN sent to the SIM card. Blank when PIN is disabled. Empty when message is sent back as a response.
        public char[]                       NewPin;        //New PIN when changing the PIN. Blank to leave it unchanged. Empty when message is sent back as a response.
        public char[]                       Apn;           //Name of the cellular APN. Blank to leave it unchanged. Current APN when sent back as a response.
        public char[]                       Puk;           //Required PUK code in case the user failed to authenticate 3 times with the PIN. Empty when message is sent back as a response.
        public byte                         Roaming;       //Enable/disable roaming. 0: setting unchanged, 1: disabled, 2: enabled. Current setting when sent back as a response.
        public CellularConfigResponseFlags  Response;      //Message acceptance response (sent back to GS).    

        #region CTOR
        /// <summary>
        /// Instantiates a new CellularConfigData
        /// </summary>    
        public CellularConfigData() {
            EnableLte       = default(byte                       );
            EnablePin       = default(byte                       );
            Pin             = new char[ 16];
            NewPin          = new char[ 16];
            Apn             = new char[ 32];
            Puk             = new char[ 16];
            Roaming         = default(byte                       );
            Response        = default(CellularConfigResponseFlags);
        }
        #endregion

        #region Read Buffer
        /// <summary>
        /// Reads the data from Buffer into this struct
        /// </summary>    
        public int Read(byte[] p_buffer,int p_offset=0) {
            int    l = 84;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals
            int[]  LS8  = MAVLinkCRC.U8_LSH8, LS16 = MAVLinkCRC.U8_LSH16, LS24 = MAVLinkCRC.U8_LSH24, LS32 = MAVLinkCRC.U8_LSH32, LS40 = MAVLinkCRC.U8_LSH40, LS48 = MAVLinkCRC.U8_LSH48, LS56 = MAVLinkCRC.U8_LSH56;
            Span<byte> b = p_buffer.AsSpan(p_offset);            
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            EnableLte       = (byte                       ) (b[p++]);
            EnablePin       = (byte                       ) (b[p++]);
            for(int i=0;i<16 ;i++) { Pin[i]          = (char                       ) (b[p++]); }
            for(int i=0;i<16 ;i++) { NewPin[i]       = (char                       ) (b[p++]); }
            for(int i=0;i<32 ;i++) { Apn[i]          = (char                       ) (b[p++]); }
            for(int i=0;i<16 ;i++) { Puk[i]          = (char                       ) (b[p++]); }
            Roaming         = (byte                       ) (b[p++]);
            Response        = (CellularConfigResponseFlags) (b[p++]);            
            return p;
        }
        #endregion

        #region Write Buffer
        /// <summary>
        /// Writes the message data into a Buffer
        /// </summary>    
        public int Write(byte[] p_buffer,int p_offset=0) {
            int    l = 84;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals            
            Span<byte> b = p_buffer.AsSpan(p_offset);
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            b[p++] = (byte)(EnableLte);
            b[p++] = (byte)(EnablePin);
            for(int i=0;i< 16;i++) {
                b[p++] = (byte)(Pin[i]);
            }
            for(int i=0;i< 16;i++) {
                b[p++] = (byte)(NewPin[i]);
            }
            for(int i=0;i< 32;i++) {
                b[p++] = (byte)(Apn[i]);
            }
            for(int i=0;i< 16;i++) {
                b[p++] = (byte)(Puk[i]);
            }
            b[p++] = (byte)(Roaming);
            b[p++] = (byte)(Response);
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
            int l = 84;
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
