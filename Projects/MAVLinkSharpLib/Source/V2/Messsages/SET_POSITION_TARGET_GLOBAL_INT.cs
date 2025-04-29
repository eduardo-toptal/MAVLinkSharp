        
using System.IO;
using System.Runtime.InteropServices;
using MAVLinkSharp.Runtime;

#pragma warning disable CS0675

namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// Sets a desired vehicle position, velocity, and/or acceleration in a global coordinate system (WGS84). Used by an external controller to command the vehicle (manual controller or other system).
    /// </summary>    
    public struct SetPositionTargetGlobalIntData : IMAVLinkMessageData {

        /// <summary>
        /// Message Id Associated w/ this Struct
        /// </summary>    
        public int GetId() { return 86; }

        public uint                         TimeBootMs;          //Timestamp (time since system boot). The rationale for the timestamp in the setpoint is to allow the system to compensate for the transport delay of the setpoint. This allows the system to compensate processing latency.
        public int                          LatInt;              //X Position in WGS84 frame
        public int                          LonInt;              //Y Position in WGS84 frame
        public float                        Alt;                 //Altitude (MSL, Relative to home, or AGL - depending on frame)
        public float                        Vx;                  //X velocity in NED frame
        public float                        Vy;                  //Y velocity in NED frame
        public float                        Vz;                  //Z velocity in NED frame
        public float                        Afx;                 //X acceleration or force (if bit 10 of type_mask is set) in NED frame in meter / s^2 or N
        public float                        Afy;                 //Y acceleration or force (if bit 10 of type_mask is set) in NED frame in meter / s^2 or N
        public float                        Afz;                 //Z acceleration or force (if bit 10 of type_mask is set) in NED frame in meter / s^2 or N
        public float                        Yaw;                 //yaw setpoint
        public float                        YawRate;             //yaw rate setpoint
        public PositionTargetTypemaskFlags  TypeMask;            //Bitmap to indicate which dimensions should be ignored by the vehicle.
        public byte                         TargetSystem;        //System ID
        public byte                         TargetComponent;     //Component ID
        public MAVFrameFlags                CoordinateFrame;     //Valid options are: MAV_FRAME_GLOBAL_INT = 5, MAV_FRAME_GLOBAL_RELATIVE_ALT_INT = 6, MAV_FRAME_GLOBAL_TERRAIN_ALT_INT = 11    

        #region CTOR
        /// <summary>
        /// Instantiates a new SetPositionTargetGlobalIntData
        /// </summary>    
        public SetPositionTargetGlobalIntData() {
            TimeBootMs            = default(uint                       );
            LatInt                = default(int                        );
            LonInt                = default(int                        );
            Alt                   = default(float                      );
            Vx                    = default(float                      );
            Vy                    = default(float                      );
            Vz                    = default(float                      );
            Afx                   = default(float                      );
            Afy                   = default(float                      );
            Afz                   = default(float                      );
            Yaw                   = default(float                      );
            YawRate               = default(float                      );
            TypeMask              = default(PositionTargetTypemaskFlags);
            TargetSystem          = default(byte                       );
            TargetComponent       = default(byte                       );
            CoordinateFrame       = default(MAVFrameFlags              );
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
            TimeBootMs            = (uint                       ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            LatInt                = (int                        ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            LonInt                = (int                        ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            Alt                   = (float                      ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Vx                    = (float                      ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Vy                    = (float                      ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Vz                    = (float                      ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Afx                   = (float                      ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Afy                   = (float                      ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Afz                   = (float                      ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Yaw                   = (float                      ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            YawRate               = (float                      ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            TypeMask              = (PositionTargetTypemaskFlags) (b[p++] | LS8[b[p++]]);
            TargetSystem          = (byte                       ) (b[p++]);
            TargetComponent       = (byte                       ) (b[p++]);
            CoordinateFrame       = (MAVFrameFlags              ) (b[p++]);            
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
            b[p++] = (byte)(      TimeBootMs);
            b[p++] = (byte)((int)TimeBootMs>>8 );
            b[p++] = (byte)((int)TimeBootMs>>16);
            b[p++] = (byte)((int)TimeBootMs>>24);
            b[p++] = (byte)(      LatInt);
            b[p++] = (byte)((int)LatInt>>8 );
            b[p++] = (byte)((int)LatInt>>16);
            b[p++] = (byte)((int)LatInt>>24);
            b[p++] = (byte)(      LonInt);
            b[p++] = (byte)((int)LonInt>>8 );
            b[p++] = (byte)((int)LonInt>>16);
            b[p++] = (byte)((int)LonInt>>24);
            MemoryMarshal.Write(b.Slice(p, 4), ref Alt                  ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref Vx                   ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref Vy                   ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref Vz                   ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref Afx                  ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref Afy                  ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref Afz                  ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref Yaw                  ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref YawRate              ); p+=4;
            b[p++] = (byte)(      TypeMask);
            b[p++] = (byte)((int)TypeMask>>8 );
            b[p++] = (byte)(TargetSystem);
            b[p++] = (byte)(TargetComponent);
            b[p++] = (byte)(CoordinateFrame);
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
