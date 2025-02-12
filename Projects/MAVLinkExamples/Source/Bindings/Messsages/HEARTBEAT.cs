        
using System.IO;
using System.Runtime.InteropServices;

#pragma warning disable CS0675

namespace MAVLinkBindings {

    /// <summary>
    /// The heartbeat message shows that a system or component is present and responding. The type and autopilot fields (along with the message component id), allow the receiving system to treat further messages from this system appropriately (e.g. by laying out the user interface based on the autopilot). This microservice is documented at https://mavlink.io/en/services/heartbeat.html
    /// </summary>    
    public struct HeartbeatData : IMAVLinkMessageData {

        /// <summary>
        /// Message Id Associated w/ this Struct
        /// </summary>    
        public int GetId() { return 0; }

        public uint               CustomMode;         //A bitfield for use for autopilot-specific flags
        public MAVTypeFlags       Type;               //Vehicle or component type. For a flight controller component the vehicle type (quadrotor, helicopter, etc.). For other components the component type (e.g. camera, gimbal, etc.). This should be used in preference to component id for identifying the component type.
        public MAVAutopilotFlags  Autopilot;          //Autopilot type / class. Use MAV_AUTOPILOT_INVALID for components that are not flight controllers.
        public MAVModeFlag        BaseMode;           //System mode bitmap.
        public MAVStateFlags      SystemStatus;       //System status flag.
        public byte               MavlinkVersion;     //MAVLink version, not writable by user, gets added by protocol because of magic data type: uint8_t_mavlink_version    

        #region CTOR
        /// <summary>
        /// Instantiates a new HeartbeatData
        /// </summary>    
        public HeartbeatData() {
            CustomMode           = default(uint             );
            Type                 = default(MAVTypeFlags     );
            Autopilot            = default(MAVAutopilotFlags);
            BaseMode             = default(MAVModeFlag      );
            SystemStatus         = default(MAVStateFlags    );
            MavlinkVersion       = default(byte             );
        }
        #endregion

        #region Read Buffer
        /// <summary>
        /// Reads the data from Buffer into this struct
        /// </summary>    
        public int Read(byte[] p_buffer,int p_offset=0) {
            int    l = 9;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals
            int[]  LS8  = MAVLinkCRC.U8_LSH8, LS16 = MAVLinkCRC.U8_LSH16, LS24 = MAVLinkCRC.U8_LSH24, LS32 = MAVLinkCRC.U8_LSH32, LS40 = MAVLinkCRC.U8_LSH40, LS48 = MAVLinkCRC.U8_LSH48, LS56 = MAVLinkCRC.U8_LSH56;
            Span<byte> b = p_buffer.AsSpan(p_offset);            
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            CustomMode           = (uint             ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            Type                 = (MAVTypeFlags     ) (b[p++]);
            Autopilot            = (MAVAutopilotFlags) (b[p++]);
            BaseMode             = (MAVModeFlag      ) (b[p++]);
            SystemStatus         = (MAVStateFlags    ) (b[p++]);
            MavlinkVersion       = (byte             ) (b[p++]);            
            return p;
        }
        #endregion

        #region Write Buffer
        /// <summary>
        /// Writes the message data into a Buffer
        /// </summary>    
        public int Write(byte[] p_buffer,int p_offset=0) {
            int    l = 9;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals            
            Span<byte> b = p_buffer.AsSpan(p_offset);
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            b[p++] = (byte)(      CustomMode);
            b[p++] = (byte)((int)CustomMode>>8 );
            b[p++] = (byte)((int)CustomMode>>16);
            b[p++] = (byte)((int)CustomMode>>24);
            b[p++] = (byte)(Type);
            b[p++] = (byte)(Autopilot);
            b[p++] = (byte)(BaseMode);
            b[p++] = (byte)(SystemStatus);
            b[p++] = (byte)(MavlinkVersion);
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
            int l = 9;
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
