        
using System;
using System.IO;
using System.Runtime.InteropServices;
using MAVLinkSharp.Runtime;

#pragma warning disable CS0675

namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// Information about a camera. Can be requested with a MAV_CMD_REQUEST_MESSAGE command.
    /// </summary>    
    public struct CameraInformationData : IMAVLinkMessageData {

        /// <summary>
        /// Message Id Associated w/ this Struct
        /// </summary>    
        public int GetId() { return 259; }

        public uint            TimeBootMs;                //Timestamp (time since system boot).
        public uint            FirmwareVersion;           //Version of the camera firmware, encoded as: `(Dev & 0xff) << 24 + (Patch & 0xff) << 16 + (Minor & 0xff) << 8 + (Major & 0xff)`. Use 0 if not known.
        public float           FocalLength;               //Focal length. Use NaN if not known.
        public float           SensorSizeH;               //Image sensor size horizontal. Use NaN if not known.
        public float           SensorSizeV;               //Image sensor size vertical. Use NaN if not known.
        public CameraCapFlags  Flags;                     //Bitmap of camera capability flags.
        public ushort          ResolutionH;               //Horizontal image resolution. Use 0 if not known.
        public ushort          ResolutionV;               //Vertical image resolution. Use 0 if not known.
        public ushort          CamDefinitionVersion;      //Camera definition version (iteration).  Use 0 if not known.
        public byte[]          VendorName;                //Name of the camera vendor
        public byte[]          ModelName;                 //Name of the camera model
        public byte            LensId;                    //Reserved for a lens ID.  Use 0 if not known.
        public char[]          CamDefinitionUri;          //Camera definition URI (if any, otherwise only basic functions will be available). HTTP- (http://) and MAVLink FTP- (mavlinkftp://) formatted URIs are allowed (and both must be supported by any GCS that implements the Camera Protocol). The definition file may be xz compressed, which will be indicated by the file extension .xml.xz (a GCS that implements the protocol must support decompressing the file). The string needs to be zero terminated.  Use a zero-length string if not known.
        public byte            GimbalDeviceId;            //Gimbal id of a gimbal associated with this camera. This is the component id of the gimbal device, or 1-6 for non mavlink gimbals. Use 0 if no gimbal is associated with the camera.
        public byte            CameraDeviceId;            //Camera id of a non-MAVLink camera attached to an autopilot (1-6).  0 if the component is a MAVLink camera (with its own component id).    

        #region CTOR
        /// <summary>
        /// Instantiates a new CameraInformationData
        /// </summary>    
        /*
        public CameraInformationData() {
            Init();
        }
        */
        public void Init() {
            TimeBootMs                  = default(uint          );
            FirmwareVersion             = default(uint          );
            FocalLength                 = default(float         );
            SensorSizeH                 = default(float         );
            SensorSizeV                 = default(float         );
            Flags                       = default(CameraCapFlags);
            ResolutionH                 = default(ushort        );
            ResolutionV                 = default(ushort        );
            CamDefinitionVersion        = default(ushort        );
            VendorName                  = new byte[ 32];
            ModelName                   = new byte[ 32];
            LensId                      = default(byte          );
            CamDefinitionUri            = new char[140];
            GimbalDeviceId              = default(byte          );
            CameraDeviceId              = default(byte          );
        }
        #endregion

        #region Read Buffer
        /// <summary>
        /// Reads the data from Buffer into this struct
        /// </summary>    
        public int Read(byte[] p_buffer,int p_offset=0) {
            int    l = 237;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals
            int[]  LS8  = MAVLinkCRC.U8_LSH8, LS16 = MAVLinkCRC.U8_LSH16, LS24 = MAVLinkCRC.U8_LSH24, LS32 = MAVLinkCRC.U8_LSH32, LS40 = MAVLinkCRC.U8_LSH40, LS48 = MAVLinkCRC.U8_LSH48, LS56 = MAVLinkCRC.U8_LSH56;
            Span<byte> b = p_buffer.AsSpan(p_offset);            
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            TimeBootMs                  = (uint          ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            FirmwareVersion             = (uint          ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            FocalLength                 = (float         ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            SensorSizeH                 = (float         ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            SensorSizeV                 = (float         ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Flags                       = (CameraCapFlags) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            ResolutionH                 = (ushort        ) (b[p++] | LS8[b[p++]]);
            ResolutionV                 = (ushort        ) (b[p++] | LS8[b[p++]]);
            CamDefinitionVersion        = (ushort        ) (b[p++] | LS8[b[p++]]);
            for(int i=0;i<32 ;i++) { VendorName[i]               = (byte          ) (b[p++]); }
            for(int i=0;i<32 ;i++) { ModelName[i]                = (byte          ) (b[p++]); }
            LensId                      = (byte          ) (b[p++]);
            for(int i=0;i<140;i++) { CamDefinitionUri[i]         = (char          ) (b[p++]); }
            GimbalDeviceId              = (byte          ) (b[p++]);
            CameraDeviceId              = (byte          ) (b[p++]);            
            return p;
        }
        #endregion

        #region Write Buffer
        /// <summary>
        /// Writes the message data into a Buffer
        /// </summary>    
        public int Write(byte[] p_buffer,int p_offset=0) {
            int    l = 237;
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
            b[p++] = (byte)(      FirmwareVersion);
            b[p++] = (byte)((int)FirmwareVersion>>8 );
            b[p++] = (byte)((int)FirmwareVersion>>16);
            b[p++] = (byte)((int)FirmwareVersion>>24);
            MemoryMarshal.Write(b.Slice(p, 4), ref FocalLength                ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref SensorSizeH                ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref SensorSizeV                ); p+=4;
            b[p++] = (byte)(      Flags);
            b[p++] = (byte)((int)Flags>>8 );
            b[p++] = (byte)((int)Flags>>16);
            b[p++] = (byte)((int)Flags>>24);
            b[p++] = (byte)(      ResolutionH);
            b[p++] = (byte)((int)ResolutionH>>8 );
            b[p++] = (byte)(      ResolutionV);
            b[p++] = (byte)((int)ResolutionV>>8 );
            b[p++] = (byte)(      CamDefinitionVersion);
            b[p++] = (byte)((int)CamDefinitionVersion>>8 );
            for(int i=0;i< 32;i++) {
                b[p++] = (byte)(VendorName[i]);
            }
            for(int i=0;i< 32;i++) {
                b[p++] = (byte)(ModelName[i]);
            }
            b[p++] = (byte)(LensId);
            for(int i=0;i<140;i++) {
                b[p++] = (byte)(CamDefinitionUri[i]);
            }
            b[p++] = (byte)(GimbalDeviceId);
            b[p++] = (byte)(CameraDeviceId);
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
            int l = 237;
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
