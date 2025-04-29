        
using System.IO;
using System.Runtime.InteropServices;
using MAVLinkSharp.Runtime;

#pragma warning disable CS0675

namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// Version and capability of autopilot software. This should be emitted in response to a request with MAV_CMD_REQUEST_MESSAGE.
    /// </summary>    
    public struct AutopilotVersionData : IMAVLinkMessageData {

        /// <summary>
        /// Message Id Associated w/ this Struct
        /// </summary>    
        public int GetId() { return 148; }

        public MAVProtocolCapabilityFlags  Capabilities;                 //Bitmap of capabilities
        public ulong                       Uid;                          //UID if provided by hardware (see uid2)
        public uint                        FlightSwVersion;              //Firmware version number
        public uint                        MiddlewareSwVersion;          //Middleware version number
        public uint                        OsSwVersion;                  //Operating system version number
        public uint                        BoardVersion;                 //HW / board version (last 8 bits should be silicon ID, if any). The first 16 bits of this field specify https://github.com/PX4/PX4-Bootloader/blob/master/board_types.txt
        public ushort                      VendorId;                     //ID of the board vendor
        public ushort                      ProductId;                    //ID of the product
        public byte[]                      FlightCustomVersion;          //Custom version field, commonly the first 8 bytes of the git hash. This is not an unique identifier, but should allow to identify the commit using the main version number even for very large code bases.
        public byte[]                      MiddlewareCustomVersion;      //Custom version field, commonly the first 8 bytes of the git hash. This is not an unique identifier, but should allow to identify the commit using the main version number even for very large code bases.
        public byte[]                      OsCustomVersion;              //Custom version field, commonly the first 8 bytes of the git hash. This is not an unique identifier, but should allow to identify the commit using the main version number even for very large code bases.
        public byte[]                      Uid2;                         //UID if provided by hardware (supersedes the uid field. If this is non-zero, use this field, otherwise use uid)    

        #region CTOR
        /// <summary>
        /// Instantiates a new AutopilotVersionData
        /// </summary>    
        public AutopilotVersionData() {
            Capabilities                   = default(MAVProtocolCapabilityFlags);
            Uid                            = default(ulong                     );
            FlightSwVersion                = default(uint                      );
            MiddlewareSwVersion            = default(uint                      );
            OsSwVersion                    = default(uint                      );
            BoardVersion                   = default(uint                      );
            VendorId                       = default(ushort                    );
            ProductId                      = default(ushort                    );
            FlightCustomVersion            = new byte[  8];
            MiddlewareCustomVersion        = new byte[  8];
            OsCustomVersion                = new byte[  8];
            Uid2                           = new byte[ 18];
        }
        #endregion

        #region Read Buffer
        /// <summary>
        /// Reads the data from Buffer into this struct
        /// </summary>    
        public int Read(byte[] p_buffer,int p_offset=0) {
            int    l = 78;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals
            int[]  LS8  = MAVLinkCRC.U8_LSH8, LS16 = MAVLinkCRC.U8_LSH16, LS24 = MAVLinkCRC.U8_LSH24, LS32 = MAVLinkCRC.U8_LSH32, LS40 = MAVLinkCRC.U8_LSH40, LS48 = MAVLinkCRC.U8_LSH48, LS56 = MAVLinkCRC.U8_LSH56;
            Span<byte> b = p_buffer.AsSpan(p_offset);            
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            Capabilities                   = (MAVProtocolCapabilityFlags) ((ulong)b[p++] | (ulong)LS8[b[p++]] | (ulong)LS16[b[p++]] | (ulong)LS24[b[p++]] | (ulong)LS32[b[p++]] | (ulong)LS40[b[p++]] | (ulong)LS48[b[p++]] | (ulong)LS56[b[p++]]);
            Uid                            = (ulong                     ) ((ulong)b[p++] | (ulong)LS8[b[p++]] | (ulong)LS16[b[p++]] | (ulong)LS24[b[p++]] | (ulong)LS32[b[p++]] | (ulong)LS40[b[p++]] | (ulong)LS48[b[p++]] | (ulong)LS56[b[p++]]);
            FlightSwVersion                = (uint                      ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            MiddlewareSwVersion            = (uint                      ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            OsSwVersion                    = (uint                      ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            BoardVersion                   = (uint                      ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            VendorId                       = (ushort                    ) (b[p++] | LS8[b[p++]]);
            ProductId                      = (ushort                    ) (b[p++] | LS8[b[p++]]);
            for(int i=0;i<8  ;i++) { FlightCustomVersion[i]         = (byte                      ) (b[p++]); }
            for(int i=0;i<8  ;i++) { MiddlewareCustomVersion[i]     = (byte                      ) (b[p++]); }
            for(int i=0;i<8  ;i++) { OsCustomVersion[i]             = (byte                      ) (b[p++]); }
            for(int i=0;i<18 ;i++) { Uid2[i]                        = (byte                      ) (b[p++]); }            
            return p;
        }
        #endregion

        #region Write Buffer
        /// <summary>
        /// Writes the message data into a Buffer
        /// </summary>    
        public int Write(byte[] p_buffer,int p_offset=0) {
            int    l = 78;
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
            b[p++] = (byte)(      Uid);
            b[p++] = (byte)((long)Uid>>8 );
            b[p++] = (byte)((long)Uid>>16);
            b[p++] = (byte)((long)Uid>>24);
            b[p++] = (byte)((long)Uid>>32);
            b[p++] = (byte)((long)Uid>>40);
            b[p++] = (byte)((long)Uid>>48);
            b[p++] = (byte)((long)Uid>>56);
            b[p++] = (byte)(      FlightSwVersion);
            b[p++] = (byte)((int)FlightSwVersion>>8 );
            b[p++] = (byte)((int)FlightSwVersion>>16);
            b[p++] = (byte)((int)FlightSwVersion>>24);
            b[p++] = (byte)(      MiddlewareSwVersion);
            b[p++] = (byte)((int)MiddlewareSwVersion>>8 );
            b[p++] = (byte)((int)MiddlewareSwVersion>>16);
            b[p++] = (byte)((int)MiddlewareSwVersion>>24);
            b[p++] = (byte)(      OsSwVersion);
            b[p++] = (byte)((int)OsSwVersion>>8 );
            b[p++] = (byte)((int)OsSwVersion>>16);
            b[p++] = (byte)((int)OsSwVersion>>24);
            b[p++] = (byte)(      BoardVersion);
            b[p++] = (byte)((int)BoardVersion>>8 );
            b[p++] = (byte)((int)BoardVersion>>16);
            b[p++] = (byte)((int)BoardVersion>>24);
            b[p++] = (byte)(      VendorId);
            b[p++] = (byte)((int)VendorId>>8 );
            b[p++] = (byte)(      ProductId);
            b[p++] = (byte)((int)ProductId>>8 );
            for(int i=0;i<  8;i++) {
                b[p++] = (byte)(FlightCustomVersion[i]);
            }
            for(int i=0;i<  8;i++) {
                b[p++] = (byte)(MiddlewareCustomVersion[i]);
            }
            for(int i=0;i<  8;i++) {
                b[p++] = (byte)(OsCustomVersion[i]);
            }
            for(int i=0;i< 18;i++) {
                b[p++] = (byte)(Uid2[i]);
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
            int l = 78;
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
