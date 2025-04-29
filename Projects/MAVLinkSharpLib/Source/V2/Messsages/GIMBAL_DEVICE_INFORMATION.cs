        
using System;
using System.IO;
using System.Runtime.InteropServices;
using MAVLinkSharp.Runtime;

#pragma warning disable CS0675

namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// Information about a low level gimbal. This message should be requested by the gimbal manager or a ground station using MAV_CMD_REQUEST_MESSAGE. The maximum angles and rates are the limits by hardware. However, the limits by software used are likely different/smaller and dependent on mode/settings/etc..
    /// </summary>    
    public struct GimbalDeviceInformationData : IMAVLinkMessageData {

        /// <summary>
        /// Message Id Associated w/ this Struct
        /// </summary>    
        public int GetId() { return 283; }

        public ulong                 Uid;                 //UID of gimbal hardware (0 if unknown).
        public uint                  TimeBootMs;          //Timestamp (time since system boot).
        public uint                  FirmwareVersion;     //Version of the gimbal firmware, encoded as: (Dev & 0xff) << 24 | (Patch & 0xff) << 16 | (Minor & 0xff) << 8 | (Major & 0xff).
        public uint                  HardwareVersion;     //Version of the gimbal hardware, encoded as: (Dev & 0xff) << 24 | (Patch & 0xff) << 16 | (Minor & 0xff) << 8 | (Major & 0xff).
        public float                 RollMin;             //Minimum hardware roll angle (positive: rolling to the right, negative: rolling to the left)
        public float                 RollMax;             //Maximum hardware roll angle (positive: rolling to the right, negative: rolling to the left)
        public float                 PitchMin;            //Minimum hardware pitch angle (positive: up, negative: down)
        public float                 PitchMax;            //Maximum hardware pitch angle (positive: up, negative: down)
        public float                 YawMin;              //Minimum hardware yaw angle (positive: to the right, negative: to the left)
        public float                 YawMax;              //Maximum hardware yaw angle (positive: to the right, negative: to the left)
        public GimbalDeviceCapFlags  CapFlags;            //Bitmap of gimbal capability flags.
        public ushort                CustomCapFlags;      //Bitmap for use for gimbal-specific capability flags.
        public char[]                VendorName;          //Name of the gimbal vendor.
        public char[]                ModelName;           //Name of the gimbal model.
        public char[]                CustomName;          //Custom name of the gimbal given to it by the user.    

        #region CTOR
        /// <summary>
        /// Instantiates a new GimbalDeviceInformationData
        /// </summary>    
        /*
        public GimbalDeviceInformationData() {
            Init();
        }
        */
        public void Init() {
            Uid                   = default(ulong               );
            TimeBootMs            = default(uint                );
            FirmwareVersion       = default(uint                );
            HardwareVersion       = default(uint                );
            RollMin               = default(float               );
            RollMax               = default(float               );
            PitchMin              = default(float               );
            PitchMax              = default(float               );
            YawMin                = default(float               );
            YawMax                = default(float               );
            CapFlags              = default(GimbalDeviceCapFlags);
            CustomCapFlags        = default(ushort              );
            VendorName            = new char[ 32];
            ModelName             = new char[ 32];
            CustomName            = new char[ 32];
        }
        #endregion

        #region Read Buffer
        /// <summary>
        /// Reads the data from Buffer into this struct
        /// </summary>    
        public int Read(byte[] p_buffer,int p_offset=0) {
            int    l = 144;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals
            int[]  LS8  = MAVLinkCRC.U8_LSH8, LS16 = MAVLinkCRC.U8_LSH16, LS24 = MAVLinkCRC.U8_LSH24, LS32 = MAVLinkCRC.U8_LSH32, LS40 = MAVLinkCRC.U8_LSH40, LS48 = MAVLinkCRC.U8_LSH48, LS56 = MAVLinkCRC.U8_LSH56;
            Span<byte> b = p_buffer.AsSpan(p_offset);            
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            Uid                   = (ulong               ) ((ulong)b[p++] | (ulong)LS8[b[p++]] | (ulong)LS16[b[p++]] | (ulong)LS24[b[p++]] | (ulong)LS32[b[p++]] | (ulong)LS40[b[p++]] | (ulong)LS48[b[p++]] | (ulong)LS56[b[p++]]);
            TimeBootMs            = (uint                ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            FirmwareVersion       = (uint                ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            HardwareVersion       = (uint                ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            RollMin               = (float               ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            RollMax               = (float               ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            PitchMin              = (float               ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            PitchMax              = (float               ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            YawMin                = (float               ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            YawMax                = (float               ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            CapFlags              = (GimbalDeviceCapFlags) (b[p++] | LS8[b[p++]]);
            CustomCapFlags        = (ushort              ) (b[p++] | LS8[b[p++]]);
            for(int i=0;i<32 ;i++) { VendorName[i]         = (char                ) (b[p++]); }
            for(int i=0;i<32 ;i++) { ModelName[i]          = (char                ) (b[p++]); }
            for(int i=0;i<32 ;i++) { CustomName[i]         = (char                ) (b[p++]); }            
            return p;
        }
        #endregion

        #region Write Buffer
        /// <summary>
        /// Writes the message data into a Buffer
        /// </summary>    
        public int Write(byte[] p_buffer,int p_offset=0) {
            int    l = 144;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals            
            Span<byte> b = p_buffer.AsSpan(p_offset);
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            b[p++] = (byte)(      Uid);
            b[p++] = (byte)((long)Uid>>8 );
            b[p++] = (byte)((long)Uid>>16);
            b[p++] = (byte)((long)Uid>>24);
            b[p++] = (byte)((long)Uid>>32);
            b[p++] = (byte)((long)Uid>>40);
            b[p++] = (byte)((long)Uid>>48);
            b[p++] = (byte)((long)Uid>>56);
            b[p++] = (byte)(      TimeBootMs);
            b[p++] = (byte)((int)TimeBootMs>>8 );
            b[p++] = (byte)((int)TimeBootMs>>16);
            b[p++] = (byte)((int)TimeBootMs>>24);
            b[p++] = (byte)(      FirmwareVersion);
            b[p++] = (byte)((int)FirmwareVersion>>8 );
            b[p++] = (byte)((int)FirmwareVersion>>16);
            b[p++] = (byte)((int)FirmwareVersion>>24);
            b[p++] = (byte)(      HardwareVersion);
            b[p++] = (byte)((int)HardwareVersion>>8 );
            b[p++] = (byte)((int)HardwareVersion>>16);
            b[p++] = (byte)((int)HardwareVersion>>24);
            MemoryMarshal.Write(b.Slice(p, 4), ref RollMin              ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref RollMax              ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref PitchMin             ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref PitchMax             ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref YawMin               ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref YawMax               ); p+=4;
            b[p++] = (byte)(      CapFlags);
            b[p++] = (byte)((int)CapFlags>>8 );
            b[p++] = (byte)(      CustomCapFlags);
            b[p++] = (byte)((int)CustomCapFlags>>8 );
            for(int i=0;i< 32;i++) {
                b[p++] = (byte)(VendorName[i]);
            }
            for(int i=0;i< 32;i++) {
                b[p++] = (byte)(ModelName[i]);
            }
            for(int i=0;i< 32;i++) {
                b[p++] = (byte)(CustomName[i]);
            }
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
            int l = 144;
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
