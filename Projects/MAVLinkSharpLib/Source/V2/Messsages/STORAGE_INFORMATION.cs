        
using System.IO;
using System.Runtime.InteropServices;
using MAVLinkSharp.Runtime;

#pragma warning disable CS0675

namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// Information about a storage medium. This message is sent in response to a request with MAV_CMD_REQUEST_MESSAGE and whenever the status of the storage changes (STORAGE_STATUS). Use MAV_CMD_REQUEST_MESSAGE.param2 to indicate the index/id of requested storage: 0 for all, 1 for first, 2 for second, etc.
    /// </summary>    
    public struct StorageInformationData : IMAVLinkMessageData {

        /// <summary>
        /// Message Id Associated w/ this Struct
        /// </summary>    
        public int GetId() { return 261; }

        public uint                TimeBootMs;            //Timestamp (time since system boot).
        public float               TotalCapacity;         //Total capacity. If storage is not ready (STORAGE_STATUS_READY) value will be ignored.
        public float               UsedCapacity;          //Used capacity. If storage is not ready (STORAGE_STATUS_READY) value will be ignored.
        public float               AvailableCapacity;     //Available storage capacity. If storage is not ready (STORAGE_STATUS_READY) value will be ignored.
        public float               ReadSpeed;             //Read speed.
        public float               WriteSpeed;            //Write speed.
        public byte                StorageId;             //Storage ID (1 for first, 2 for second, etc.)
        public byte                StorageCount;          //Number of storage devices
        public StorageStatusFlags  Status;                //Status of storage
        public StorageTypeFlags    Type;                  //Type of storage
        public char[]              Name;                  //Textual storage name to be used in UI (microSD 1, Internal Memory, etc.) This is a NULL terminated string. If it is exactly 32 characters long, add a terminating NULL. If this string is empty, the generic type is shown to the user.
        public StorageUsageFlag    StorageUsage;          //Flags indicating whether this instance is preferred storage for photos, videos, etc. | Note: Implementations should initially set the flags on the system-default storage id used for saving media (if possible/supported). | This setting can then be overridden using MAV_CMD_SET_STORAGE_USAGE. | If the media usage flags are not set, a GCS may assume storage ID 1 is the default storage for all media types.    

        #region CTOR
        /// <summary>
        /// Instantiates a new StorageInformationData
        /// </summary>    
        public StorageInformationData() {
            TimeBootMs              = default(uint              );
            TotalCapacity           = default(float             );
            UsedCapacity            = default(float             );
            AvailableCapacity       = default(float             );
            ReadSpeed               = default(float             );
            WriteSpeed              = default(float             );
            StorageId               = default(byte              );
            StorageCount            = default(byte              );
            Status                  = default(StorageStatusFlags);
            Type                    = default(StorageTypeFlags  );
            Name                    = new char[ 32];
            StorageUsage            = default(StorageUsageFlag  );
        }
        #endregion

        #region Read Buffer
        /// <summary>
        /// Reads the data from Buffer into this struct
        /// </summary>    
        public int Read(byte[] p_buffer,int p_offset=0) {
            int    l = 61;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals
            int[]  LS8  = MAVLinkCRC.U8_LSH8, LS16 = MAVLinkCRC.U8_LSH16, LS24 = MAVLinkCRC.U8_LSH24, LS32 = MAVLinkCRC.U8_LSH32, LS40 = MAVLinkCRC.U8_LSH40, LS48 = MAVLinkCRC.U8_LSH48, LS56 = MAVLinkCRC.U8_LSH56;
            Span<byte> b = p_buffer.AsSpan(p_offset);            
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            TimeBootMs              = (uint              ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            TotalCapacity           = (float             ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            UsedCapacity            = (float             ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            AvailableCapacity       = (float             ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            ReadSpeed               = (float             ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            WriteSpeed              = (float             ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            StorageId               = (byte              ) (b[p++]);
            StorageCount            = (byte              ) (b[p++]);
            Status                  = (StorageStatusFlags) (b[p++]);
            Type                    = (StorageTypeFlags  ) (b[p++]);
            for(int i=0;i<32 ;i++) { Name[i]                 = (char              ) (b[p++]); }
            StorageUsage            = (StorageUsageFlag  ) (b[p++]);            
            return p;
        }
        #endregion

        #region Write Buffer
        /// <summary>
        /// Writes the message data into a Buffer
        /// </summary>    
        public int Write(byte[] p_buffer,int p_offset=0) {
            int    l = 61;
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
            MemoryMarshal.Write(b.Slice(p, 4), ref TotalCapacity          ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref UsedCapacity           ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref AvailableCapacity      ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref ReadSpeed              ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref WriteSpeed             ); p+=4;
            b[p++] = (byte)(StorageId);
            b[p++] = (byte)(StorageCount);
            b[p++] = (byte)(Status);
            b[p++] = (byte)(Type);
            for(int i=0;i< 32;i++) {
                b[p++] = (byte)(Name[i]);
            }
            b[p++] = (byte)(StorageUsage);
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
            int l = 61;
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
