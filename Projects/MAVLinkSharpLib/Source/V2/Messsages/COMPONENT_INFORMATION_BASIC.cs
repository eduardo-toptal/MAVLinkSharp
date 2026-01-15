        
using System;
using System.IO;
using System.Runtime.InteropServices;
using MAVLinkSharp.Runtime;

#pragma warning disable CS0675

namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// Basic component information data. Should be requested using MAV_CMD_REQUEST_MESSAGE on startup, or when required.
    /// </summary>    
    public struct ComponentInformationBasicData : IMAVLinkMessageData {

        /// <summary>
        /// Message Id Associated w/ this Struct
        /// </summary>    
        public int GetId() { return 396; }

        public MAVProtocolCapabilityFlags  Capabilities;          //Component capability flags
        public uint                        TimeBootMs;            //Timestamp (time since system boot).
        public uint                        TimeManufactureS;      //Date of manufacture as a UNIX Epoch time (since 1.1.1970) in seconds.
        public char[]                      VendorName;            //Name of the component vendor. Needs to be zero terminated. The field is optional and can be empty/all zeros.
        public char[]                      ModelName;             //Name of the component model. Needs to be zero terminated. The field is optional and can be empty/all zeros.
        public char[]                      SoftwareVersion;       //Software version. The recommended format is SEMVER: 'major.minor.patch'  (any format may be used). The field must be zero terminated if it has a value. The field is optional and can be empty/all zeros.
        public char[]                      HardwareVersion;       //Hardware version. The recommended format is SEMVER: 'major.minor.patch'  (any format may be used). The field must be zero terminated if it has a value. The field is optional and can be empty/all zeros.
        public char[]                      SerialNumber;          //Hardware serial number. The field must be zero terminated if it has a value. The field is optional and can be empty/all zeros.    

        #region CTOR
        /// <summary>
        /// Instantiates a new ComponentInformationBasicData
        /// </summary>    
        /*
        public ComponentInformationBasicData() {
            Init();
        }
        */
        public void Init() {
            Capabilities            = default(MAVProtocolCapabilityFlags);
            TimeBootMs              = default(uint                      );
            TimeManufactureS        = default(uint                      );
            VendorName              = new char[ 32];
            ModelName               = new char[ 32];
            SoftwareVersion         = new char[ 24];
            HardwareVersion         = new char[ 24];
            SerialNumber            = new char[ 32];
        }
        #endregion

        #region Read Buffer
        /// <summary>
        /// Reads the data from Buffer into this struct
        /// </summary>    
        public int Read(byte[] p_buffer,int p_offset=0) {
            int    l = 160;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals
            int[]  LS8  = MAVLinkCRC.U8_LSH8, LS16 = MAVLinkCRC.U8_LSH16, LS24 = MAVLinkCRC.U8_LSH24, LS32 = MAVLinkCRC.U8_LSH32, LS40 = MAVLinkCRC.U8_LSH40, LS48 = MAVLinkCRC.U8_LSH48, LS56 = MAVLinkCRC.U8_LSH56;
            Span<byte> b = p_buffer.AsSpan(p_offset);            
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            Capabilities            = (MAVProtocolCapabilityFlags) ((ulong)b[p++] | (ulong)LS8[b[p++]] | (ulong)LS16[b[p++]] | (ulong)LS24[b[p++]] | (ulong)LS32[b[p++]] | (ulong)LS40[b[p++]] | (ulong)LS48[b[p++]] | (ulong)LS56[b[p++]]);
            TimeBootMs              = (uint                      ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            TimeManufactureS        = (uint                      ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            for(int i=0;i<32 ;i++) { VendorName[i]           = (char                      ) (b[p++]); }
            for(int i=0;i<32 ;i++) { ModelName[i]            = (char                      ) (b[p++]); }
            for(int i=0;i<24 ;i++) { SoftwareVersion[i]      = (char                      ) (b[p++]); }
            for(int i=0;i<24 ;i++) { HardwareVersion[i]      = (char                      ) (b[p++]); }
            for(int i=0;i<32 ;i++) { SerialNumber[i]         = (char                      ) (b[p++]); }            
            return p;
        }
        #endregion

        #region Write Buffer
        /// <summary>
        /// Writes the message data into a Buffer
        /// </summary>    
        public int Write(byte[] p_buffer,int p_offset=0) {
            int    l = 160;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals            
            Span<byte> b = p_buffer.AsSpan(p_offset);
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            b[p++] = (byte)(      Capabilities);
            b[p++] = (byte)((long)Capabilities>>8 );
            b[p++] = (byte)((long)Capabilities>>16);
            b[p++] = (byte)((long)Capabilities>>24);
            b[p++] = (byte)((long)Capabilities>>32);
            b[p++] = (byte)((long)Capabilities>>40);
            b[p++] = (byte)((long)Capabilities>>48);
            b[p++] = (byte)((long)Capabilities>>56);
            b[p++] = (byte)(      TimeBootMs);
            b[p++] = (byte)((int)TimeBootMs>>8 );
            b[p++] = (byte)((int)TimeBootMs>>16);
            b[p++] = (byte)((int)TimeBootMs>>24);
            b[p++] = (byte)(      TimeManufactureS);
            b[p++] = (byte)((int)TimeManufactureS>>8 );
            b[p++] = (byte)((int)TimeManufactureS>>16);
            b[p++] = (byte)((int)TimeManufactureS>>24);
            for(int i=0;i< 32;i++) {
                b[p++] = (byte)(VendorName[i]);
            }
            for(int i=0;i< 32;i++) {
                b[p++] = (byte)(ModelName[i]);
            }
            for(int i=0;i< 24;i++) {
                b[p++] = (byte)(SoftwareVersion[i]);
            }
            for(int i=0;i< 24;i++) {
                b[p++] = (byte)(HardwareVersion[i]);
            }
            for(int i=0;i< 32;i++) {
                b[p++] = (byte)(SerialNumber[i]);
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
            int l = 160;
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
