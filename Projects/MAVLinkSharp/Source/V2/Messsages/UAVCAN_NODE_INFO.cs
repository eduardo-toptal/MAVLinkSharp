        
using System.IO;
using System.Runtime.InteropServices;
using MAVLinkSharp.Runtime;

#pragma warning disable CS0675

namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// General information describing a particular UAVCAN node. Please refer to the definition of the UAVCAN service "uavcan.protocol.GetNodeInfo" for the background information. This message should be emitted by the system whenever a new node appears online, or an existing node reboots. Additionally, it can be emitted upon request from the other end of the MAVLink channel (see MAV_CMD_UAVCAN_GET_NODE_INFO). It is also not prohibited to emit this message unconditionally at a low frequency. The UAVCAN specification is available at http://uavcan.org.
    /// </summary>    
    public struct UavcanNodeInfoData : IMAVLinkMessageData {

        /// <summary>
        /// Message Id Associated w/ this Struct
        /// </summary>    
        public int GetId() { return 311; }

        public ulong   TimeUsec;            //Timestamp (UNIX Epoch time or time since system boot). The receiving end can infer timestamp format (since 1.1.1970 or since system boot) by checking for the magnitude of the number.
        public uint    UptimeSec;           //Time since the start-up of the node.
        public uint    SwVcsCommit;         //Version control system (VCS) revision identifier (e.g. git short commit hash). 0 if unknown.
        public char[]  Name;                //Node name string. For example, "sapog.px4.io".
        public byte    HwVersionMajor;      //Hardware major version number.
        public byte    HwVersionMinor;      //Hardware minor version number.
        public byte[]  HwUniqueId;          //Hardware unique 128-bit ID.
        public byte    SwVersionMajor;      //Software major version number.
        public byte    SwVersionMinor;      //Software minor version number.    

        #region CTOR
        /// <summary>
        /// Instantiates a new UavcanNodeInfoData
        /// </summary>    
        public UavcanNodeInfoData() {
            TimeUsec              = default(ulong);
            UptimeSec             = default(uint );
            SwVcsCommit           = default(uint );
            Name                  = new char[ 80];
            HwVersionMajor        = default(byte );
            HwVersionMinor        = default(byte );
            HwUniqueId            = new byte[ 16];
            SwVersionMajor        = default(byte );
            SwVersionMinor        = default(byte );
        }
        #endregion

        #region Read Buffer
        /// <summary>
        /// Reads the data from Buffer into this struct
        /// </summary>    
        public int Read(byte[] p_buffer,int p_offset=0) {
            int    l = 116;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals
            int[]  LS8  = MAVLinkCRC.U8_LSH8, LS16 = MAVLinkCRC.U8_LSH16, LS24 = MAVLinkCRC.U8_LSH24, LS32 = MAVLinkCRC.U8_LSH32, LS40 = MAVLinkCRC.U8_LSH40, LS48 = MAVLinkCRC.U8_LSH48, LS56 = MAVLinkCRC.U8_LSH56;
            Span<byte> b = p_buffer.AsSpan(p_offset);            
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            TimeUsec              = (ulong) ((ulong)b[p++] | (ulong)LS8[b[p++]] | (ulong)LS16[b[p++]] | (ulong)LS24[b[p++]] | (ulong)LS32[b[p++]] | (ulong)LS40[b[p++]] | (ulong)LS48[b[p++]] | (ulong)LS56[b[p++]]);
            UptimeSec             = (uint ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            SwVcsCommit           = (uint ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            for(int i=0;i<80 ;i++) { Name[i]               = (char ) (b[p++]); }
            HwVersionMajor        = (byte ) (b[p++]);
            HwVersionMinor        = (byte ) (b[p++]);
            for(int i=0;i<16 ;i++) { HwUniqueId[i]         = (byte ) (b[p++]); }
            SwVersionMajor        = (byte ) (b[p++]);
            SwVersionMinor        = (byte ) (b[p++]);            
            return p;
        }
        #endregion

        #region Write Buffer
        /// <summary>
        /// Writes the message data into a Buffer
        /// </summary>    
        public int Write(byte[] p_buffer,int p_offset=0) {
            int    l = 116;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals            
            Span<byte> b = p_buffer.AsSpan(p_offset);
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            b[p++] = (byte)(      TimeUsec);
            b[p++] = (byte)((long)TimeUsec>>8 );
            b[p++] = (byte)((long)TimeUsec>>16);
            b[p++] = (byte)((long)TimeUsec>>24);
            b[p++] = (byte)((long)TimeUsec>>32);
            b[p++] = (byte)((long)TimeUsec>>40);
            b[p++] = (byte)((long)TimeUsec>>48);
            b[p++] = (byte)((long)TimeUsec>>56);
            b[p++] = (byte)(      UptimeSec);
            b[p++] = (byte)((int)UptimeSec>>8 );
            b[p++] = (byte)((int)UptimeSec>>16);
            b[p++] = (byte)((int)UptimeSec>>24);
            b[p++] = (byte)(      SwVcsCommit);
            b[p++] = (byte)((int)SwVcsCommit>>8 );
            b[p++] = (byte)((int)SwVcsCommit>>16);
            b[p++] = (byte)((int)SwVcsCommit>>24);
            for(int i=0;i< 80;i++) {
                b[p++] = (byte)(Name[i]);
            }
            b[p++] = (byte)(HwVersionMajor);
            b[p++] = (byte)(HwVersionMinor);
            for(int i=0;i< 16;i++) {
                b[p++] = (byte)(HwUniqueId[i]);
            }
            b[p++] = (byte)(SwVersionMajor);
            b[p++] = (byte)(SwVersionMinor);
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
            int l = 116;
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
