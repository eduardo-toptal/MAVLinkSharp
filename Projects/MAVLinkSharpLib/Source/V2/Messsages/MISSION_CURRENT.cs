        
using System;
using System.IO;
using System.Runtime.InteropServices;
using MAVLinkSharp.Runtime;

#pragma warning disable CS0675

namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// 
    /// Message that announces the sequence number of the current target mission item (that the system will fly towards/execute when the mission is running).
    /// This message should be streamed all the time (nominally at 1Hz).
    /// This message should be emitted following a call to MAV_CMD_DO_SET_MISSION_CURRENT or MISSION_SET_CURRENT.
    /// 
    /// </summary>    
    public struct MissionCurrentData : IMAVLinkMessageData {

        /// <summary>
        /// Message Id Associated w/ this Struct
        /// </summary>    
        public int GetId() { return 42; }

        public ushort             Seq;                //Sequence
        public ushort             Total;              //Total number of mission items on vehicle (on last item, sequence == total). If the autopilot stores its home location as part of the mission this will be excluded from the total. 0: Not supported, UINT16_MAX if no mission is present on the vehicle.
        public MissionStateFlags  MissionState;       //Mission state machine state. MISSION_STATE_UNKNOWN if state reporting not supported.
        public byte               MissionMode;        //Vehicle is in a mode that can execute mission items or suspended. 0: Unknown, 1: In mission mode, 2: Suspended (not in mission mode).
        public uint               MissionId;          //Id of current on-vehicle mission plan, or 0 if IDs are not supported or there is no mission loaded. GCS can use this to track changes to the mission plan type. The same value is returned on mission upload (in the MISSION_ACK).
        public uint               FenceId;            //Id of current on-vehicle fence plan, or 0 if IDs are not supported or there is no fence loaded. GCS can use this to track changes to the fence plan type. The same value is returned on fence upload (in the MISSION_ACK).
        public uint               RallyPointsId;      //Id of current on-vehicle rally point plan, or 0 if IDs are not supported or there are no rally points loaded. GCS can use this to track changes to the rally point plan type. The same value is returned on rally point upload (in the MISSION_ACK).    

        #region CTOR
        /// <summary>
        /// Instantiates a new MissionCurrentData
        /// </summary>    
        /*
        public MissionCurrentData() {
            Init();
        }
        */
        public void Init() {
            Seq                  = default(ushort           );
            Total                = default(ushort           );
            MissionState         = default(MissionStateFlags);
            MissionMode          = default(byte             );
            MissionId            = default(uint             );
            FenceId              = default(uint             );
            RallyPointsId        = default(uint             );
        }
        #endregion

        #region Read Buffer
        /// <summary>
        /// Reads the data from Buffer into this struct
        /// </summary>    
        public int Read(byte[] p_buffer,int p_offset=0) {
            int    l = 18;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals
            int[]  LS8  = MAVLinkCRC.U8_LSH8, LS16 = MAVLinkCRC.U8_LSH16, LS24 = MAVLinkCRC.U8_LSH24, LS32 = MAVLinkCRC.U8_LSH32, LS40 = MAVLinkCRC.U8_LSH40, LS48 = MAVLinkCRC.U8_LSH48, LS56 = MAVLinkCRC.U8_LSH56;
            Span<byte> b = p_buffer.AsSpan(p_offset);            
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            Seq                  = (ushort           ) (b[p++] | LS8[b[p++]]);
            Total                = (ushort           ) (b[p++] | LS8[b[p++]]);
            MissionState         = (MissionStateFlags) (b[p++]);
            MissionMode          = (byte             ) (b[p++]);
            MissionId            = (uint             ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            FenceId              = (uint             ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            RallyPointsId        = (uint             ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);            
            return p;
        }
        #endregion

        #region Write Buffer
        /// <summary>
        /// Writes the message data into a Buffer
        /// </summary>    
        public int Write(byte[] p_buffer,int p_offset=0) {
            int    l = 18;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals            
            Span<byte> b = p_buffer.AsSpan(p_offset);
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            b[p++] = (byte)(      Seq);
            b[p++] = (byte)((int)Seq>>8 );
            b[p++] = (byte)(      Total);
            b[p++] = (byte)((int)Total>>8 );
            b[p++] = (byte)(MissionState);
            b[p++] = (byte)(MissionMode);
            b[p++] = (byte)(      MissionId);
            b[p++] = (byte)((int)MissionId>>8 );
            b[p++] = (byte)((int)MissionId>>16);
            b[p++] = (byte)((int)MissionId>>24);
            b[p++] = (byte)(      FenceId);
            b[p++] = (byte)((int)FenceId>>8 );
            b[p++] = (byte)((int)FenceId>>16);
            b[p++] = (byte)((int)FenceId>>24);
            b[p++] = (byte)(      RallyPointsId);
            b[p++] = (byte)((int)RallyPointsId>>8 );
            b[p++] = (byte)((int)RallyPointsId>>16);
            b[p++] = (byte)((int)RallyPointsId>>24);
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
            int l = 18;
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
