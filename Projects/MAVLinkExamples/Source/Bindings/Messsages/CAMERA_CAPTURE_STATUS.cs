        
using System.IO;
using System.Runtime.InteropServices;

#pragma warning disable CS0675

namespace MAVLinkBindings {

    /// <summary>
    /// Information about the status of a capture. Can be requested with a MAV_CMD_REQUEST_MESSAGE command.
    /// </summary>    
    public struct CameraCaptureStatusData : IMAVLinkMessageData {

        /// <summary>
        /// Message Id Associated w/ this Struct
        /// </summary>    
        public int GetId() { return 262; }

        public uint   TimeBootMs;            //Timestamp (time since system boot).
        public float  ImageInterval;         //Image capture interval
        public uint   RecordingTimeMs;       //Elapsed time since recording started (0: Not supported/available). A GCS should compute recording time and use non-zero values of this field to correct any discrepancy.
        public float  AvailableCapacity;     //Available storage capacity.
        public byte   ImageStatus;           //Current status of image capturing (0: idle, 1: capture in progress, 2: interval set but idle, 3: interval set and capture in progress)
        public byte   VideoStatus;           //Current status of video capturing (0: idle, 1: capture in progress)
        public int    ImageCount;            //Total number of images captured ('forever', or until reset using MAV_CMD_STORAGE_FORMAT).    

        #region CTOR
        /// <summary>
        /// Instantiates a new CameraCaptureStatusData
        /// </summary>    
        public CameraCaptureStatusData() {
            TimeBootMs              = default(uint );
            ImageInterval           = default(float);
            RecordingTimeMs         = default(uint );
            AvailableCapacity       = default(float);
            ImageStatus             = default(byte );
            VideoStatus             = default(byte );
            ImageCount              = default(int  );
        }
        #endregion

        #region Read Buffer
        /// <summary>
        /// Reads the data from Buffer into this struct
        /// </summary>    
        public int Read(byte[] p_buffer,int p_offset=0) {
            int    l = 22;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals
            int[]  LS8  = MAVLinkCRC.U8_LSH8, LS16 = MAVLinkCRC.U8_LSH16, LS24 = MAVLinkCRC.U8_LSH24, LS32 = MAVLinkCRC.U8_LSH32, LS40 = MAVLinkCRC.U8_LSH40, LS48 = MAVLinkCRC.U8_LSH48, LS56 = MAVLinkCRC.U8_LSH56;
            Span<byte> b = p_buffer.AsSpan(p_offset);            
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            TimeBootMs              = (uint ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            ImageInterval           = (float) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            RecordingTimeMs         = (uint ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            AvailableCapacity       = (float) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            ImageStatus             = (byte ) (b[p++]);
            VideoStatus             = (byte ) (b[p++]);
            ImageCount              = (int  ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);            
            return p;
        }
        #endregion

        #region Write Buffer
        /// <summary>
        /// Writes the message data into a Buffer
        /// </summary>    
        public int Write(byte[] p_buffer,int p_offset=0) {
            int    l = 22;
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
            MemoryMarshal.Write(b.Slice(p, 4), in ImageInterval          ); p+=4;
            b[p++] = (byte)(      RecordingTimeMs);
            b[p++] = (byte)((int)RecordingTimeMs>>8 );
            b[p++] = (byte)((int)RecordingTimeMs>>16);
            b[p++] = (byte)((int)RecordingTimeMs>>24);
            MemoryMarshal.Write(b.Slice(p, 4), in AvailableCapacity      ); p+=4;
            b[p++] = (byte)(ImageStatus);
            b[p++] = (byte)(VideoStatus);
            b[p++] = (byte)(      ImageCount);
            b[p++] = (byte)((int)ImageCount>>8 );
            b[p++] = (byte)((int)ImageCount>>16);
            b[p++] = (byte)((int)ImageCount>>24);
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
            int l = 22;
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
