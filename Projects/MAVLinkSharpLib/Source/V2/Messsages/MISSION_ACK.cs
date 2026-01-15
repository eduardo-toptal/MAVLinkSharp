        
using System;
using System.IO;
using System.Runtime.InteropServices;
using MAVLinkSharp.Runtime;

#pragma warning disable CS0675

namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// Acknowledgment message during waypoint handling. The type field states if this message is a positive ack (type=0) or if an error happened (type=non-zero).
    /// </summary>    
    public struct MissionAckData : IMAVLinkMessageData {

        /// <summary>
        /// Message Id Associated w/ this Struct
        /// </summary>    
        public int GetId() { return 47; }

        public byte                   TargetSystem;        //System ID
        public byte                   TargetComponent;     //Component ID
        public MAVMissionResultFlags  Type;                //Mission result.
        public MAVMissionTypeFlags    MissionType;         //Mission type.
        public uint                   OpaqueId;            //Id of new on-vehicle mission, fence, or rally point plan (on upload to vehicle). | The id is calculated and returned by a vehicle when a new plan is uploaded by a GCS. | The only requirement on the id is that it must change when there is any change to the on-vehicle plan type (there is no requirement that the id be globally unique). | 0 on download from the vehicle to the GCS (on download the ID is set in MISSION_COUNT). | 0 if plan ids are not supported. | The current on-vehicle plan ids are streamed in `MISSION_CURRENT`, allowing a GCS to determine if any part of the plan has changed and needs to be re-uploaded.    

        #region CTOR
        /// <summary>
        /// Instantiates a new MissionAckData
        /// </summary>    
        /*
        public MissionAckData() {
            Init();
        }
        */
        public void Init() {
            TargetSystem          = default(byte                 );
            TargetComponent       = default(byte                 );
            Type                  = default(MAVMissionResultFlags);
            MissionType           = default(MAVMissionTypeFlags  );
            OpaqueId              = default(uint                 );
        }
        #endregion

        #region Read Buffer
        /// <summary>
        /// Reads the data from Buffer into this struct
        /// </summary>    
        public int Read(byte[] p_buffer,int p_offset=0) {
            int    l = 8;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals
            int[]  LS8  = MAVLinkCRC.U8_LSH8, LS16 = MAVLinkCRC.U8_LSH16, LS24 = MAVLinkCRC.U8_LSH24, LS32 = MAVLinkCRC.U8_LSH32, LS40 = MAVLinkCRC.U8_LSH40, LS48 = MAVLinkCRC.U8_LSH48, LS56 = MAVLinkCRC.U8_LSH56;
            Span<byte> b = p_buffer.AsSpan(p_offset);            
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            TargetSystem          = (byte                 ) (b[p++]);
            TargetComponent       = (byte                 ) (b[p++]);
            Type                  = (MAVMissionResultFlags) (b[p++]);
            MissionType           = (MAVMissionTypeFlags  ) (b[p++]);
            OpaqueId              = (uint                 ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);            
            return p;
        }
        #endregion

        #region Write Buffer
        /// <summary>
        /// Writes the message data into a Buffer
        /// </summary>    
        public int Write(byte[] p_buffer,int p_offset=0) {
            int    l = 8;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals            
            Span<byte> b = p_buffer.AsSpan(p_offset);
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            b[p++] = (byte)(TargetSystem);
            b[p++] = (byte)(TargetComponent);
            b[p++] = (byte)(Type);
            b[p++] = (byte)(MissionType);
            b[p++] = (byte)(      OpaqueId);
            b[p++] = (byte)((int)OpaqueId>>8 );
            b[p++] = (byte)((int)OpaqueId>>16);
            b[p++] = (byte)((int)OpaqueId>>24);
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
            int l = 8;
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
