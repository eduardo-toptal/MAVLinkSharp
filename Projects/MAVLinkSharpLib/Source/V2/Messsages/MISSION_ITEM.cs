        
using System;
using System.IO;
using System.Runtime.InteropServices;
using MAVLinkSharp.Runtime;

#pragma warning disable CS0675

namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// Message encoding a mission item. This message is emitted to announce
    /// the presence of a mission item and to set a mission item on the system. The mission item can be either in x, y, z meters (type: LOCAL) or x:lat, y:lon, z:altitude. Local frame is Z-down, right handed (NED), global frame is Z-up, right handed (ENU). NaN may be used to indicate an optional/default value (e.g. to use the system's current latitude or yaw rather than a specific value). See also https://mavlink.io/en/services/mission.html.
    /// </summary>    
    public struct MissionItemData : IMAVLinkMessageData {

        /// <summary>
        /// Message Id Associated w/ this Struct
        /// </summary>    
        public int GetId() { return 39; }

        public float                Param1;              //PARAM1, see MAV_CMD enum
        public float                Param2;              //PARAM2, see MAV_CMD enum
        public float                Param3;              //PARAM3, see MAV_CMD enum
        public float                Param4;              //PARAM4, see MAV_CMD enum
        public float                X;                   //PARAM5 / local: X coordinate, global: latitude
        public float                Y;                   //PARAM6 / local: Y coordinate, global: longitude
        public float                Z;                   //PARAM7 / local: Z coordinate, global: altitude (relative or absolute, depending on frame).
        public ushort               Seq;                 //Sequence
        public MAVCmdFlags          Command;             //The scheduled action for the waypoint.
        public byte                 TargetSystem;        //System ID
        public byte                 TargetComponent;     //Component ID
        public MAVFrameFlags        Frame;               //The coordinate system of the waypoint.
        public byte                 Current;             //false:0, true:1
        public byte                 Autocontinue;        //Autocontinue to next waypoint
        public MAVMissionTypeFlags  MissionType;         //Mission type.    

        #region CTOR
        /// <summary>
        /// Instantiates a new MissionItemData
        /// </summary>    
        /*
        public MissionItemData() {
            Init();
        }
        */
        public void Init() {
            Param1                = default(float              );
            Param2                = default(float              );
            Param3                = default(float              );
            Param4                = default(float              );
            X                     = default(float              );
            Y                     = default(float              );
            Z                     = default(float              );
            Seq                   = default(ushort             );
            Command               = default(MAVCmdFlags        );
            TargetSystem          = default(byte               );
            TargetComponent       = default(byte               );
            Frame                 = default(MAVFrameFlags      );
            Current               = default(byte               );
            Autocontinue          = default(byte               );
            MissionType           = default(MAVMissionTypeFlags);
        }
        #endregion

        #region Read Buffer
        /// <summary>
        /// Reads the data from Buffer into this struct
        /// </summary>    
        public int Read(byte[] p_buffer,int p_offset=0) {
            int    l = 38;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals
            int[]  LS8  = MAVLinkCRC.U8_LSH8, LS16 = MAVLinkCRC.U8_LSH16, LS24 = MAVLinkCRC.U8_LSH24, LS32 = MAVLinkCRC.U8_LSH32, LS40 = MAVLinkCRC.U8_LSH40, LS48 = MAVLinkCRC.U8_LSH48, LS56 = MAVLinkCRC.U8_LSH56;
            Span<byte> b = p_buffer.AsSpan(p_offset);            
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            Param1                = (float              ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Param2                = (float              ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Param3                = (float              ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Param4                = (float              ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            X                     = (float              ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Y                     = (float              ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Z                     = (float              ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Seq                   = (ushort             ) (b[p++] | LS8[b[p++]]);
            Command               = (MAVCmdFlags        ) (b[p++] | LS8[b[p++]]);
            TargetSystem          = (byte               ) (b[p++]);
            TargetComponent       = (byte               ) (b[p++]);
            Frame                 = (MAVFrameFlags      ) (b[p++]);
            Current               = (byte               ) (b[p++]);
            Autocontinue          = (byte               ) (b[p++]);
            MissionType           = (MAVMissionTypeFlags) (b[p++]);            
            return p;
        }
        #endregion

        #region Write Buffer
        /// <summary>
        /// Writes the message data into a Buffer
        /// </summary>    
        public int Write(byte[] p_buffer,int p_offset=0) {
            int    l = 38;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals            
            Span<byte> b = p_buffer.AsSpan(p_offset);
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            MemoryMarshal.Write(b.Slice(p, 4), ref Param1               ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref Param2               ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref Param3               ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref Param4               ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref X                    ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref Y                    ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref Z                    ); p+=4;
            b[p++] = (byte)(      Seq);
            b[p++] = (byte)((int)Seq>>8 );
            b[p++] = (byte)(      Command);
            b[p++] = (byte)((int)Command>>8 );
            b[p++] = (byte)(TargetSystem);
            b[p++] = (byte)(TargetComponent);
            b[p++] = (byte)(Frame);
            b[p++] = (byte)(Current);
            b[p++] = (byte)(Autocontinue);
            b[p++] = (byte)(MissionType);
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
            int l = 38;
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
