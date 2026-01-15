        
using System;
using System.IO;
using System.Runtime.InteropServices;
using MAVLinkSharp.Runtime;

#pragma warning disable CS0675

namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// Information about the status of a video stream. It may be requested using MAV_CMD_REQUEST_MESSAGE.
    /// </summary>    
    public struct VideoStreamStatusData : IMAVLinkMessageData {

        /// <summary>
        /// Message Id Associated w/ this Struct
        /// </summary>    
        public int GetId() { return 270; }

        public float                   Framerate;           //Frame rate
        public uint                    Bitrate;             //Bit rate
        public VideoStreamStatusFlags  Flags;               //Bitmap of stream status flags
        public ushort                  ResolutionH;         //Horizontal resolution
        public ushort                  ResolutionV;         //Vertical resolution
        public ushort                  Rotation;            //Video image rotation clockwise
        public ushort                  Hfov;                //Horizontal Field of view
        public byte                    StreamId;            //Video Stream ID (1 for first, 2 for second, etc.)
        public byte                    CameraDeviceId;      //Camera id of a non-MAVLink camera attached to an autopilot (1-6).  0 if the component is a MAVLink camera (with its own component id).    

        #region CTOR
        /// <summary>
        /// Instantiates a new VideoStreamStatusData
        /// </summary>    
        /*
        public VideoStreamStatusData() {
            Init();
        }
        */
        public void Init() {
            Framerate             = default(float                 );
            Bitrate               = default(uint                  );
            Flags                 = default(VideoStreamStatusFlags);
            ResolutionH           = default(ushort                );
            ResolutionV           = default(ushort                );
            Rotation              = default(ushort                );
            Hfov                  = default(ushort                );
            StreamId              = default(byte                  );
            CameraDeviceId        = default(byte                  );
        }
        #endregion

        #region Read Buffer
        /// <summary>
        /// Reads the data from Buffer into this struct
        /// </summary>    
        public int Read(byte[] p_buffer,int p_offset=0) {
            int    l = 20;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals
            int[]  LS8  = MAVLinkCRC.U8_LSH8, LS16 = MAVLinkCRC.U8_LSH16, LS24 = MAVLinkCRC.U8_LSH24, LS32 = MAVLinkCRC.U8_LSH32, LS40 = MAVLinkCRC.U8_LSH40, LS48 = MAVLinkCRC.U8_LSH48, LS56 = MAVLinkCRC.U8_LSH56;
            Span<byte> b = p_buffer.AsSpan(p_offset);            
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            Framerate             = (float                 ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Bitrate               = (uint                  ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            Flags                 = (VideoStreamStatusFlags) (b[p++] | LS8[b[p++]]);
            ResolutionH           = (ushort                ) (b[p++] | LS8[b[p++]]);
            ResolutionV           = (ushort                ) (b[p++] | LS8[b[p++]]);
            Rotation              = (ushort                ) (b[p++] | LS8[b[p++]]);
            Hfov                  = (ushort                ) (b[p++] | LS8[b[p++]]);
            StreamId              = (byte                  ) (b[p++]);
            CameraDeviceId        = (byte                  ) (b[p++]);            
            return p;
        }
        #endregion

        #region Write Buffer
        /// <summary>
        /// Writes the message data into a Buffer
        /// </summary>    
        public int Write(byte[] p_buffer,int p_offset=0) {
            int    l = 20;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals            
            Span<byte> b = p_buffer.AsSpan(p_offset);
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            MemoryMarshal.Write(b.Slice(p, 4), ref Framerate            ); p+=4;
            b[p++] = (byte)(      Bitrate);
            b[p++] = (byte)((int)Bitrate>>8 );
            b[p++] = (byte)((int)Bitrate>>16);
            b[p++] = (byte)((int)Bitrate>>24);
            b[p++] = (byte)(      Flags);
            b[p++] = (byte)((int)Flags>>8 );
            b[p++] = (byte)(      ResolutionH);
            b[p++] = (byte)((int)ResolutionH>>8 );
            b[p++] = (byte)(      ResolutionV);
            b[p++] = (byte)((int)ResolutionV>>8 );
            b[p++] = (byte)(      Rotation);
            b[p++] = (byte)((int)Rotation>>8 );
            b[p++] = (byte)(      Hfov);
            b[p++] = (byte)((int)Hfov>>8 );
            b[p++] = (byte)(StreamId);
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
            int l = 20;
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
