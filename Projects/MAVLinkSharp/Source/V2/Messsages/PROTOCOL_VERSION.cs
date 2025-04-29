        
using System.IO;
using System.Runtime.InteropServices;
using MAVLinkSharp.Runtime;

#pragma warning disable CS0675

namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// Version and capability of protocol version. This message can be requested with MAV_CMD_REQUEST_MESSAGE and is used as part of the handshaking to establish which MAVLink version should be used on the network. Every node should respond to a request for PROTOCOL_VERSION to enable the handshaking. Library implementers should consider adding this into the default decoding state machine to allow the protocol core to respond directly.
    /// </summary>    
    public struct ProtocolVersionData : IMAVLinkMessageData {

        /// <summary>
        /// Message Id Associated w/ this Struct
        /// </summary>    
        public int GetId() { return 300; }

        public ushort  Version;                 //Currently active MAVLink version number * 100: v1.0 is 100, v2.0 is 200, etc.
        public ushort  MinVersion;              //Minimum MAVLink version supported
        public ushort  MaxVersion;              //Maximum MAVLink version supported (set to the same value as version by default)
        public byte[]  SpecVersionHash;         //The first 8 bytes (not characters printed in hex!) of the git hash.
        public byte[]  LibraryVersionHash;      //The first 8 bytes (not characters printed in hex!) of the git hash.    

        #region CTOR
        /// <summary>
        /// Instantiates a new ProtocolVersionData
        /// </summary>    
        public ProtocolVersionData() {
            Version                   = default(ushort);
            MinVersion                = default(ushort);
            MaxVersion                = default(ushort);
            SpecVersionHash           = new byte[  8];
            LibraryVersionHash        = new byte[  8];
        }
        #endregion

        #region Read Buffer
        /// <summary>
        /// Reads the data from Buffer into this struct
        /// </summary>    
        public int Read(byte[] p_buffer,int p_offset=0) {
            int    l = 22;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals
            int[]  LS8  = MAVLinkCRC.U8_LSH8, LS16 = MAVLinkCRC.U8_LSH16, LS24 = MAVLinkCRC.U8_LSH24, LS32 = MAVLinkCRC.U8_LSH32, LS40 = MAVLinkCRC.U8_LSH40, LS48 = MAVLinkCRC.U8_LSH48, LS56 = MAVLinkCRC.U8_LSH56;
            Span<byte> b = p_buffer.AsSpan(p_offset);            
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            Version                   = (ushort) (b[p++] | LS8[b[p++]]);
            MinVersion                = (ushort) (b[p++] | LS8[b[p++]]);
            MaxVersion                = (ushort) (b[p++] | LS8[b[p++]]);
            for(int i=0;i<8  ;i++) { SpecVersionHash[i]        = (byte  ) (b[p++]); }
            for(int i=0;i<8  ;i++) { LibraryVersionHash[i]     = (byte  ) (b[p++]); }            
            return p;
        }
        #endregion

        #region Write Buffer
        /// <summary>
        /// Writes the message data into a Buffer
        /// </summary>    
        public int Write(byte[] p_buffer,int p_offset=0) {
            int    l = 22;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals            
            Span<byte> b = p_buffer.AsSpan(p_offset);
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            b[p++] = (byte)(      Version);
            b[p++] = (byte)((int)Version>>8 );
            b[p++] = (byte)(      MinVersion);
            b[p++] = (byte)((int)MinVersion>>8 );
            b[p++] = (byte)(      MaxVersion);
            b[p++] = (byte)((int)MaxVersion>>8 );
            for(int i=0;i<  8;i++) {
                b[p++] = (byte)(SpecVersionHash[i]);
            }
            for(int i=0;i<  8;i++) {
                b[p++] = (byte)(LibraryVersionHash[i]);
            }
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
            int l = 22;
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
