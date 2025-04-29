        
using System;
using System.IO;
using System.Runtime.InteropServices;
using MAVLinkSharp.Runtime;

#pragma warning disable CS0675

namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// Information about a high level gimbal manager. This message should be requested by a ground station using MAV_CMD_REQUEST_MESSAGE.
    /// </summary>    
    public struct GimbalManagerInformationData : IMAVLinkMessageData {

        /// <summary>
        /// Message Id Associated w/ this Struct
        /// </summary>    
        public int GetId() { return 280; }

        public uint                   TimeBootMs;          //Timestamp (time since system boot).
        public GimbalManagerCapFlags  CapFlags;            //Bitmap of gimbal capability flags.
        public float                  RollMin;             //Minimum hardware roll angle (positive: rolling to the right, negative: rolling to the left)
        public float                  RollMax;             //Maximum hardware roll angle (positive: rolling to the right, negative: rolling to the left)
        public float                  PitchMin;            //Minimum pitch angle (positive: up, negative: down)
        public float                  PitchMax;            //Maximum pitch angle (positive: up, negative: down)
        public float                  YawMin;              //Minimum yaw angle (positive: to the right, negative: to the left)
        public float                  YawMax;              //Maximum yaw angle (positive: to the right, negative: to the left)
        public byte                   GimbalDeviceId;      //Gimbal device ID that this gimbal manager is responsible for.    

        #region CTOR
        /// <summary>
        /// Instantiates a new GimbalManagerInformationData
        /// </summary>    
        /*
        public GimbalManagerInformationData() {
            Init();
        }
        */
        public void Init() {
            TimeBootMs            = default(uint                 );
            CapFlags              = default(GimbalManagerCapFlags);
            RollMin               = default(float                );
            RollMax               = default(float                );
            PitchMin              = default(float                );
            PitchMax              = default(float                );
            YawMin                = default(float                );
            YawMax                = default(float                );
            GimbalDeviceId        = default(byte                 );
        }
        #endregion

        #region Read Buffer
        /// <summary>
        /// Reads the data from Buffer into this struct
        /// </summary>    
        public int Read(byte[] p_buffer,int p_offset=0) {
            int    l = 33;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals
            int[]  LS8  = MAVLinkCRC.U8_LSH8, LS16 = MAVLinkCRC.U8_LSH16, LS24 = MAVLinkCRC.U8_LSH24, LS32 = MAVLinkCRC.U8_LSH32, LS40 = MAVLinkCRC.U8_LSH40, LS48 = MAVLinkCRC.U8_LSH48, LS56 = MAVLinkCRC.U8_LSH56;
            Span<byte> b = p_buffer.AsSpan(p_offset);            
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            TimeBootMs            = (uint                 ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            CapFlags              = (GimbalManagerCapFlags) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            RollMin               = (float                ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            RollMax               = (float                ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            PitchMin              = (float                ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            PitchMax              = (float                ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            YawMin                = (float                ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            YawMax                = (float                ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            GimbalDeviceId        = (byte                 ) (b[p++]);            
            return p;
        }
        #endregion

        #region Write Buffer
        /// <summary>
        /// Writes the message data into a Buffer
        /// </summary>    
        public int Write(byte[] p_buffer,int p_offset=0) {
            int    l = 33;
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
            b[p++] = (byte)(      CapFlags);
            b[p++] = (byte)((int)CapFlags>>8 );
            b[p++] = (byte)((int)CapFlags>>16);
            b[p++] = (byte)((int)CapFlags>>24);
            MemoryMarshal.Write(b.Slice(p, 4), ref RollMin              ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref RollMax              ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref PitchMin             ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref PitchMax             ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref YawMin               ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref YawMax               ); p+=4;
            b[p++] = (byte)(GimbalDeviceId);
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
            int l = 33;
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
