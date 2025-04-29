        
using System.IO;
using System.Runtime.InteropServices;
using MAVLinkSharp.Runtime;

#pragma warning disable CS0675

namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// 
    /// Component information message, which may be requested using MAV_CMD_REQUEST_MESSAGE.
    /// 
    /// </summary>    
    public struct ComponentInformationData : IMAVLinkMessageData {

        /// <summary>
        /// Message Id Associated w/ this Struct
        /// </summary>    
        public int GetId() { return 395; }

        public uint    TimeBootMs;                       //Timestamp (time since system boot).
        public uint    GeneralMetadataFileCrc;           //CRC32 of the general metadata file (general_metadata_uri).
        public uint    PeripheralsMetadataFileCrc;       //CRC32 of peripherals metadata file (peripherals_metadata_uri).
        public char[]  GeneralMetadataUri;               //MAVLink FTP URI for the general metadata file (COMP_METADATA_TYPE_GENERAL), which may be compressed with xz. The file contains general component metadata, and may contain URI links for additional metadata (see COMP_METADATA_TYPE). The information is static from boot, and may be generated at compile time. The string needs to be zero terminated.
        public char[]  PeripheralsMetadataUri;           //(Optional) MAVLink FTP URI for the peripherals metadata file (COMP_METADATA_TYPE_PERIPHERALS), which may be compressed with xz. This contains data about "attached components" such as UAVCAN nodes. The peripherals are in a separate file because the information must be generated dynamically at runtime. The string needs to be zero terminated.    

        #region CTOR
        /// <summary>
        /// Instantiates a new ComponentInformationData
        /// </summary>    
        /*
        public ComponentInformationData() {
            Init();
        }
        */
        public void Init() {
            TimeBootMs                         = default(uint);
            GeneralMetadataFileCrc             = default(uint);
            PeripheralsMetadataFileCrc         = default(uint);
            GeneralMetadataUri                 = new char[100];
            PeripheralsMetadataUri             = new char[100];
        }
        #endregion

        #region Read Buffer
        /// <summary>
        /// Reads the data from Buffer into this struct
        /// </summary>    
        public int Read(byte[] p_buffer,int p_offset=0) {
            int    l = 212;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals
            int[]  LS8  = MAVLinkCRC.U8_LSH8, LS16 = MAVLinkCRC.U8_LSH16, LS24 = MAVLinkCRC.U8_LSH24, LS32 = MAVLinkCRC.U8_LSH32, LS40 = MAVLinkCRC.U8_LSH40, LS48 = MAVLinkCRC.U8_LSH48, LS56 = MAVLinkCRC.U8_LSH56;
            Span<byte> b = p_buffer.AsSpan(p_offset);            
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            TimeBootMs                         = (uint) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            GeneralMetadataFileCrc             = (uint) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            PeripheralsMetadataFileCrc         = (uint) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            for(int i=0;i<100;i++) { GeneralMetadataUri[i]              = (char) (b[p++]); }
            for(int i=0;i<100;i++) { PeripheralsMetadataUri[i]          = (char) (b[p++]); }            
            return p;
        }
        #endregion

        #region Write Buffer
        /// <summary>
        /// Writes the message data into a Buffer
        /// </summary>    
        public int Write(byte[] p_buffer,int p_offset=0) {
            int    l = 212;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals            
            Span<byte> b = p_buffer.AsSpan(p_offset);
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            b[p++] = (byte)(      TimeBootMs);
            b[p++] = (byte)((int)TimeBootMs>>8 );
            b[p++] = (byte)((int)TimeBootMs>>16);
            b[p++] = (byte)((int)TimeBootMs>>24);
            b[p++] = (byte)(      GeneralMetadataFileCrc);
            b[p++] = (byte)((int)GeneralMetadataFileCrc>>8 );
            b[p++] = (byte)((int)GeneralMetadataFileCrc>>16);
            b[p++] = (byte)((int)GeneralMetadataFileCrc>>24);
            b[p++] = (byte)(      PeripheralsMetadataFileCrc);
            b[p++] = (byte)((int)PeripheralsMetadataFileCrc>>8 );
            b[p++] = (byte)((int)PeripheralsMetadataFileCrc>>16);
            b[p++] = (byte)((int)PeripheralsMetadataFileCrc>>24);
            for(int i=0;i<100;i++) {
                b[p++] = (byte)(GeneralMetadataUri[i]);
            }
            for(int i=0;i<100;i++) {
                b[p++] = (byte)(PeripheralsMetadataUri[i]);
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
            int l = 212;
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
