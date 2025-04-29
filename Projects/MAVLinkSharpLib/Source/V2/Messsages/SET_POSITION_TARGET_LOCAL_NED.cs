        
using System;
using System.IO;
using System.Runtime.InteropServices;
using MAVLinkSharp.Runtime;

#pragma warning disable CS0675

namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// Sets a desired vehicle position in a local north-east-down coordinate frame. Used by an external controller to command the vehicle (manual controller or other system).
    /// </summary>    
    public struct SetPositionTargetLocalNedData : IMAVLinkMessageData {

        /// <summary>
        /// Message Id Associated w/ this Struct
        /// </summary>    
        public int GetId() { return 84; }

        public uint                         TimeBootMs;          //Timestamp (time since system boot).
        public float                        X;                   //X Position in NED frame
        public float                        Y;                   //Y Position in NED frame
        public float                        Z;                   //Z Position in NED frame (note, altitude is negative in NED)
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
        public MAVFrameFlags                CoordinateFrame;     //Valid options are: MAV_FRAME_LOCAL_NED = 1, MAV_FRAME_LOCAL_OFFSET_NED = 7, MAV_FRAME_BODY_NED = 8, MAV_FRAME_BODY_OFFSET_NED = 9    

        #region CTOR
        /// <summary>
        /// Instantiates a new SetPositionTargetLocalNedData
        /// </summary>    
        /*
        public SetPositionTargetLocalNedData() {
            Init();
        }
        */
        public void Init() {
            TimeBootMs            = default(uint                       );
            X                     = default(float                      );
            Y                     = default(float                      );
            Z                     = default(float                      );
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
            X                     = (float                      ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Y                     = (float                      ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Z                     = (float                      ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
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
            MemoryMarshal.Write(b.Slice(p, 4), ref X                    ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref Y                    ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref Z                    ); p+=4;
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
