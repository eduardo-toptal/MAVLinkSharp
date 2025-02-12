        
using System.IO;
using System.Runtime.InteropServices;

#pragma warning disable CS0675

namespace MAVLinkBindings {

    /// <summary>
    /// Data for filling the OpenDroneID Authentication message. The Authentication Message defines a field that can provide a means of authenticity for the identity of the UAS (Unmanned Aircraft System). The Authentication message can have two different formats. For data page 0, the fields PageCount, Length and TimeStamp are present and AuthData is only 17 bytes. For data page 1 through 15, PageCount, Length and TimeStamp are not present and the size of AuthData is 23 bytes.
    /// </summary>    
    public struct OpenDroneIdAuthenticationData : IMAVLinkMessageData {

        /// <summary>
        /// Message Id Associated w/ this Struct
        /// </summary>    
        public int GetId() { return 12902; }

        public uint                  Timestamp;              //This field is only present for page 0. 32 bit Unix Timestamp in seconds since 00:00:00 01/01/2019.
        public byte                  TargetSystem;           //System ID (0 for broadcast).
        public byte                  TargetComponent;        //Component ID (0 for broadcast).
        public byte[]                IdOrMac;                //Only used for drone ID data received from other UAs. See detailed description at https://mavlink.io/en/services/opendroneid.html.
        public MAVOdidAuthTypeFlags  AuthenticationType;     //Indicates the type of authentication.
        public byte                  DataPage;               //Allowed range is 0 - 15.
        public byte                  LastPageIndex;          //This field is only present for page 0. Allowed range is 0 - 15. See the description of struct ODID_Auth_data at https://github.com/opendroneid/opendroneid-core-c/blob/master/libopendroneid/opendroneid.h.
        public byte                  Length;                 //This field is only present for page 0. Total bytes of authentication_data from all data pages. See the description of struct ODID_Auth_data at https://github.com/opendroneid/opendroneid-core-c/blob/master/libopendroneid/opendroneid.h.
        public byte[]                AuthenticationData;     //Opaque authentication data. For page 0, the size is only 17 bytes. For other pages, the size is 23 bytes. Shall be filled with nulls in the unused portion of the field.    

        #region CTOR
        /// <summary>
        /// Instantiates a new OpenDroneIdAuthenticationData
        /// </summary>    
        public OpenDroneIdAuthenticationData() {
            Timestamp                = default(uint                );
            TargetSystem             = default(byte                );
            TargetComponent          = default(byte                );
            IdOrMac                  = new byte[ 20];
            AuthenticationType       = default(MAVOdidAuthTypeFlags);
            DataPage                 = default(byte                );
            LastPageIndex            = default(byte                );
            Length                   = default(byte                );
            AuthenticationData       = new byte[ 23];
        }
        #endregion

        #region Read Buffer
        /// <summary>
        /// Reads the data from Buffer into this struct
        /// </summary>    
        public int Read(byte[] p_buffer,int p_offset=0) {
            int    l = 53;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals
            int[]  LS8  = MAVLinkCRC.U8_LSH8, LS16 = MAVLinkCRC.U8_LSH16, LS24 = MAVLinkCRC.U8_LSH24, LS32 = MAVLinkCRC.U8_LSH32, LS40 = MAVLinkCRC.U8_LSH40, LS48 = MAVLinkCRC.U8_LSH48, LS56 = MAVLinkCRC.U8_LSH56;
            Span<byte> b = p_buffer.AsSpan(p_offset);            
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            Timestamp                = (uint                ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            TargetSystem             = (byte                ) (b[p++]);
            TargetComponent          = (byte                ) (b[p++]);
            for(int i=0;i<20 ;i++) { IdOrMac[i]               = (byte                ) (b[p++]); }
            AuthenticationType       = (MAVOdidAuthTypeFlags) (b[p++]);
            DataPage                 = (byte                ) (b[p++]);
            LastPageIndex            = (byte                ) (b[p++]);
            Length                   = (byte                ) (b[p++]);
            for(int i=0;i<23 ;i++) { AuthenticationData[i]    = (byte                ) (b[p++]); }            
            return p;
        }
        #endregion

        #region Write Buffer
        /// <summary>
        /// Writes the message data into a Buffer
        /// </summary>    
        public int Write(byte[] p_buffer,int p_offset=0) {
            int    l = 53;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals            
            Span<byte> b = p_buffer.AsSpan(p_offset);
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            b[p++] = (byte)(      Timestamp);
            b[p++] = (byte)((int)Timestamp>>8 );
            b[p++] = (byte)((int)Timestamp>>16);
            b[p++] = (byte)((int)Timestamp>>24);
            b[p++] = (byte)(TargetSystem);
            b[p++] = (byte)(TargetComponent);
            for(int i=0;i< 20;i++) {
                b[p++] = (byte)(IdOrMac[i]);
            }
            b[p++] = (byte)(AuthenticationType);
            b[p++] = (byte)(DataPage);
            b[p++] = (byte)(LastPageIndex);
            b[p++] = (byte)(Length);
            for(int i=0;i< 23;i++) {
                b[p++] = (byte)(AuthenticationData[i]);
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
            int l = 53;
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
