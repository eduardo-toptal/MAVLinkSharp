        
using System;
using System.IO;
using System.Runtime.InteropServices;
using MAVLinkSharp.Runtime;

#pragma warning disable CS0675

namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// Camera absolute thermal range. This can be streamed when the associated VIDEO_STREAM_STATUS `flag` field bit VIDEO_STREAM_STATUS_FLAGS_THERMAL_RANGE_ENABLED is set, but a GCS may choose to only request it for the current active stream. Use MAV_CMD_SET_MESSAGE_INTERVAL to define message interval (param3 indicates the stream id of the current camera, or 0 for all streams, param4 indicates the target camera_device_id for autopilot-attached cameras or 0 for MAVLink cameras).
    /// </summary>    
    public struct CameraThermalRangeData : IMAVLinkMessageData {

        /// <summary>
        /// Message Id Associated w/ this Struct
        /// </summary>    
        public int GetId() { return 277; }

        public uint   TimeBootMs;          //Timestamp (time since system boot).
        public float  Max;                 //Temperature max.
        public float  MaxPointX;           //Temperature max point x value (normalized 0..1, 0 is left, 1 is right), NAN if unknown.
        public float  MaxPointY;           //Temperature max point y value (normalized 0..1, 0 is top, 1 is bottom), NAN if unknown.
        public float  Min;                 //Temperature min.
        public float  MinPointX;           //Temperature min point x value (normalized 0..1, 0 is left, 1 is right), NAN if unknown.
        public float  MinPointY;           //Temperature min point y value (normalized 0..1, 0 is top, 1 is bottom), NAN if unknown.
        public byte   StreamId;            //Video Stream ID (1 for first, 2 for second, etc.)
        public byte   CameraDeviceId;      //Camera id of a non-MAVLink camera attached to an autopilot (1-6).  0 if the component is a MAVLink camera (with its own component id).    

        #region CTOR
        /// <summary>
        /// Instantiates a new CameraThermalRangeData
        /// </summary>    
        /*
        public CameraThermalRangeData() {
            Init();
        }
        */
        public void Init() {
            TimeBootMs            = default(uint );
            Max                   = default(float);
            MaxPointX             = default(float);
            MaxPointY             = default(float);
            Min                   = default(float);
            MinPointX             = default(float);
            MinPointY             = default(float);
            StreamId              = default(byte );
            CameraDeviceId        = default(byte );
        }
        #endregion

        #region Read Buffer
        /// <summary>
        /// Reads the data from Buffer into this struct
        /// </summary>    
        public int Read(byte[] p_buffer,int p_offset=0) {
            int    l = 30;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals
            int[]  LS8  = MAVLinkCRC.U8_LSH8, LS16 = MAVLinkCRC.U8_LSH16, LS24 = MAVLinkCRC.U8_LSH24, LS32 = MAVLinkCRC.U8_LSH32, LS40 = MAVLinkCRC.U8_LSH40, LS48 = MAVLinkCRC.U8_LSH48, LS56 = MAVLinkCRC.U8_LSH56;
            Span<byte> b = p_buffer.AsSpan(p_offset);            
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            TimeBootMs            = (uint ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            Max                   = (float) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            MaxPointX             = (float) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            MaxPointY             = (float) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Min                   = (float) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            MinPointX             = (float) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            MinPointY             = (float) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            StreamId              = (byte ) (b[p++]);
            CameraDeviceId        = (byte ) (b[p++]);            
            return p;
        }
        #endregion

        #region Write Buffer
        /// <summary>
        /// Writes the message data into a Buffer
        /// </summary>    
        public int Write(byte[] p_buffer,int p_offset=0) {
            int    l = 30;
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
            MemoryMarshal.Write(b.Slice(p, 4), ref Max                  ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref MaxPointX            ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref MaxPointY            ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref Min                  ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref MinPointX            ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref MinPointY            ); p+=4;
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
            int l = 30;
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
