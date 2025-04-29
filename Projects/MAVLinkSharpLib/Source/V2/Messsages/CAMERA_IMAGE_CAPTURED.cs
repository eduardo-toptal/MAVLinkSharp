        
using System;
using System.IO;
using System.Runtime.InteropServices;
using MAVLinkSharp.Runtime;

#pragma warning disable CS0675

namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// Information about a captured image. This is emitted every time a message is captured.
    /// MAV_CMD_REQUEST_MESSAGE can be used to (re)request this message for a specific sequence number or range of sequence numbers:
    /// MAV_CMD_REQUEST_MESSAGE.param2 indicates the sequence number the first image to send, or set to -1 to send the message for all sequence numbers.
    /// MAV_CMD_REQUEST_MESSAGE.param3 is used to specify a range of messages to send:
    /// set to 0 (default) to send just the the message for the sequence number in param 2,
    /// set to -1 to send the message for the sequence number in param 2 and all the following sequence numbers,
    /// set to the sequence number of the final message in the range.
    /// </summary>    
    public struct CameraImageCapturedData : IMAVLinkMessageData {

        /// <summary>
        /// Message Id Associated w/ this Struct
        /// </summary>    
        public int GetId() { return 263; }

        public ulong    TimeUtc;           //Timestamp (time since UNIX epoch) in UTC. 0 for unknown.
        public uint     TimeBootMs;        //Timestamp (time since system boot).
        public int      Lat;               //Latitude where image was taken
        public int      Lon;               //Longitude where capture was taken
        public int      Alt;               //Altitude (MSL) where image was taken
        public int      RelativeAlt;       //Altitude above ground
        public float[]  Q;                 //Quaternion of camera orientation (w, x, y, z order, zero-rotation is 1, 0, 0, 0)
        public int      ImageIndex;        //Zero based index of this image (i.e. a new image will have index CAMERA_CAPTURE_STATUS.image count -1)
        public byte     CameraId;          //Deprecated/unused. Component IDs are used to differentiate multiple cameras.
        public sbyte    CaptureResult;     //Boolean indicating success (1) or failure (0) while capturing this image.
        public char[]   FileUrl;           //URL of image taken. Either local storage or http://foo.jpg if camera provides an HTTP interface.    

        #region CTOR
        /// <summary>
        /// Instantiates a new CameraImageCapturedData
        /// </summary>    
        /*
        public CameraImageCapturedData() {
            Init();
        }
        */
        public void Init() {
            TimeUtc             = default(ulong);
            TimeBootMs          = default(uint );
            Lat                 = default(int  );
            Lon                 = default(int  );
            Alt                 = default(int  );
            RelativeAlt         = default(int  );
            Q                   = new float[  4];
            ImageIndex          = default(int  );
            CameraId            = default(byte );
            CaptureResult       = default(sbyte);
            FileUrl             = new char[205];
        }
        #endregion

        #region Read Buffer
        /// <summary>
        /// Reads the data from Buffer into this struct
        /// </summary>    
        public int Read(byte[] p_buffer,int p_offset=0) {
            int    l = 255;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals
            int[]  LS8  = MAVLinkCRC.U8_LSH8, LS16 = MAVLinkCRC.U8_LSH16, LS24 = MAVLinkCRC.U8_LSH24, LS32 = MAVLinkCRC.U8_LSH32, LS40 = MAVLinkCRC.U8_LSH40, LS48 = MAVLinkCRC.U8_LSH48, LS56 = MAVLinkCRC.U8_LSH56;
            Span<byte> b = p_buffer.AsSpan(p_offset);            
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            TimeUtc             = (ulong) ((ulong)b[p++] | (ulong)LS8[b[p++]] | (ulong)LS16[b[p++]] | (ulong)LS24[b[p++]] | (ulong)LS32[b[p++]] | (ulong)LS40[b[p++]] | (ulong)LS48[b[p++]] | (ulong)LS56[b[p++]]);
            TimeBootMs          = (uint ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            Lat                 = (int  ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            Lon                 = (int  ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            Alt                 = (int  ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            RelativeAlt         = (int  ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            for(int i=0;i<4  ;i++) { Q[i]                = (float) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4; }
            ImageIndex          = (int  ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            CameraId            = (byte ) (b[p++]);
            CaptureResult       = (sbyte) (b[p++]);
            for(int i=0;i<205;i++) { FileUrl[i]          = (char ) (b[p++]); }            
            return p;
        }
        #endregion

        #region Write Buffer
        /// <summary>
        /// Writes the message data into a Buffer
        /// </summary>    
        public int Write(byte[] p_buffer,int p_offset=0) {
            int    l = 255;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals            
            Span<byte> b = p_buffer.AsSpan(p_offset);
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            b[p++] = (byte)(      TimeUtc);
            b[p++] = (byte)((long)TimeUtc>>8 );
            b[p++] = (byte)((long)TimeUtc>>16);
            b[p++] = (byte)((long)TimeUtc>>24);
            b[p++] = (byte)((long)TimeUtc>>32);
            b[p++] = (byte)((long)TimeUtc>>40);
            b[p++] = (byte)((long)TimeUtc>>48);
            b[p++] = (byte)((long)TimeUtc>>56);
            b[p++] = (byte)(      TimeBootMs);
            b[p++] = (byte)((int)TimeBootMs>>8 );
            b[p++] = (byte)((int)TimeBootMs>>16);
            b[p++] = (byte)((int)TimeBootMs>>24);
            b[p++] = (byte)(      Lat);
            b[p++] = (byte)((int)Lat>>8 );
            b[p++] = (byte)((int)Lat>>16);
            b[p++] = (byte)((int)Lat>>24);
            b[p++] = (byte)(      Lon);
            b[p++] = (byte)((int)Lon>>8 );
            b[p++] = (byte)((int)Lon>>16);
            b[p++] = (byte)((int)Lon>>24);
            b[p++] = (byte)(      Alt);
            b[p++] = (byte)((int)Alt>>8 );
            b[p++] = (byte)((int)Alt>>16);
            b[p++] = (byte)((int)Alt>>24);
            b[p++] = (byte)(      RelativeAlt);
            b[p++] = (byte)((int)RelativeAlt>>8 );
            b[p++] = (byte)((int)RelativeAlt>>16);
            b[p++] = (byte)((int)RelativeAlt>>24);
            for(int i=0;i<  4;i++) {
                MemoryMarshal.Write(b.Slice(p, 4), ref Q[i]               ); p+=4;
            }
            b[p++] = (byte)(      ImageIndex);
            b[p++] = (byte)((int)ImageIndex>>8 );
            b[p++] = (byte)((int)ImageIndex>>16);
            b[p++] = (byte)((int)ImageIndex>>24);
            b[p++] = (byte)(CameraId);
            b[p++] = (byte)(CaptureResult);
            for(int i=0;i<205;i++) {
                b[p++] = (byte)(FileUrl[i]);
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
            int l = 255;
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
