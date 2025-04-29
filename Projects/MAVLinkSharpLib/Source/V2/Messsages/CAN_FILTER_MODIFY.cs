        
using System;
using System.IO;
using System.Runtime.InteropServices;
using MAVLinkSharp.Runtime;

#pragma warning disable CS0675

namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// Modify the filter of what CAN messages to forward over the mavlink. This can be used to make CAN forwarding work well on low bandwidth links. The filtering is applied on bits 8 to 24 of the CAN id (2nd and 3rd bytes) which corresponds to the DroneCAN message ID for DroneCAN. Filters with more than 16 IDs can be constructed by sending multiple CAN_FILTER_MODIFY messages.
    /// </summary>    
    public struct CanFilterModifyData : IMAVLinkMessageData {

        /// <summary>
        /// Message Id Associated w/ this Struct
        /// </summary>    
        public int GetId() { return 388; }

        public ushort[]          Ids;                 //filter IDs, length num_ids
        public byte              TargetSystem;        //System ID.
        public byte              TargetComponent;     //Component ID.
        public byte              Bus;                 //bus number
        public CanFilterOpFlags  Operation;           //what operation to perform on the filter list. See CAN_FILTER_OP enum.
        public byte              NumIds;              //number of IDs in filter list    

        #region CTOR
        /// <summary>
        /// Instantiates a new CanFilterModifyData
        /// </summary>    
        /*
        public CanFilterModifyData() {
            Init();
        }
        */
        public void Init() {
            Ids                   = new ushort[ 16];
            TargetSystem          = default(byte            );
            TargetComponent       = default(byte            );
            Bus                   = default(byte            );
            Operation             = default(CanFilterOpFlags);
            NumIds                = default(byte            );
        }
        #endregion

        #region Read Buffer
        /// <summary>
        /// Reads the data from Buffer into this struct
        /// </summary>    
        public int Read(byte[] p_buffer,int p_offset=0) {
            int    l = 37;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals
            int[]  LS8  = MAVLinkCRC.U8_LSH8, LS16 = MAVLinkCRC.U8_LSH16, LS24 = MAVLinkCRC.U8_LSH24, LS32 = MAVLinkCRC.U8_LSH32, LS40 = MAVLinkCRC.U8_LSH40, LS48 = MAVLinkCRC.U8_LSH48, LS56 = MAVLinkCRC.U8_LSH56;
            Span<byte> b = p_buffer.AsSpan(p_offset);            
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            for(int i=0;i<16 ;i++) { Ids[i]                = (ushort          ) (b[p++] | LS8[b[p++]]); }
            TargetSystem          = (byte            ) (b[p++]);
            TargetComponent       = (byte            ) (b[p++]);
            Bus                   = (byte            ) (b[p++]);
            Operation             = (CanFilterOpFlags) (b[p++]);
            NumIds                = (byte            ) (b[p++]);            
            return p;
        }
        #endregion

        #region Write Buffer
        /// <summary>
        /// Writes the message data into a Buffer
        /// </summary>    
        public int Write(byte[] p_buffer,int p_offset=0) {
            int    l = 37;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals            
            Span<byte> b = p_buffer.AsSpan(p_offset);
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            for(int i=0;i< 16;i++) {
                b[p++] = (byte)(      Ids[i]);
                b[p++] = (byte)((int)Ids[i]>>8 );
            }
            b[p++] = (byte)(TargetSystem);
            b[p++] = (byte)(TargetComponent);
            b[p++] = (byte)(Bus);
            b[p++] = (byte)(Operation);
            b[p++] = (byte)(NumIds);
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
            int l = 37;
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
