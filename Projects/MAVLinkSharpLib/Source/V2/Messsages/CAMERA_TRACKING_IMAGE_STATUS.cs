        
using System;
using System.IO;
using System.Runtime.InteropServices;
using MAVLinkSharp.Runtime;

#pragma warning disable CS0675

namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// Camera tracking status, sent while in active tracking. Use MAV_CMD_SET_MESSAGE_INTERVAL to define message interval.
    /// </summary>    
    public struct CameraTrackingImageStatusData : IMAVLinkMessageData {

        /// <summary>
        /// Message Id Associated w/ this Struct
        /// </summary>    
        public int GetId() { return 275; }

        public float                          PointX;              //Current tracked point x value if CAMERA_TRACKING_MODE_POINT (normalized 0..1, 0 is left, 1 is right), NAN if unknown
        public float                          PointY;              //Current tracked point y value if CAMERA_TRACKING_MODE_POINT (normalized 0..1, 0 is top, 1 is bottom), NAN if unknown
        public float                          Radius;              //Current tracked radius if CAMERA_TRACKING_MODE_POINT (normalized 0..1, 0 is image left, 1 is image right), NAN if unknown
        public float                          RecTopX;             //Current tracked rectangle top x value if CAMERA_TRACKING_MODE_RECTANGLE (normalized 0..1, 0 is left, 1 is right), NAN if unknown
        public float                          RecTopY;             //Current tracked rectangle top y value if CAMERA_TRACKING_MODE_RECTANGLE (normalized 0..1, 0 is top, 1 is bottom), NAN if unknown
        public float                          RecBottomX;          //Current tracked rectangle bottom x value if CAMERA_TRACKING_MODE_RECTANGLE (normalized 0..1, 0 is left, 1 is right), NAN if unknown
        public float                          RecBottomY;          //Current tracked rectangle bottom y value if CAMERA_TRACKING_MODE_RECTANGLE (normalized 0..1, 0 is top, 1 is bottom), NAN if unknown
        public CameraTrackingStatusFlags      TrackingStatus;      //Current tracking status
        public CameraTrackingModeFlags        TrackingMode;        //Current tracking mode
        public CameraTrackingTargetDataFlags  TargetData;          //Defines location of target data
        public byte                           CameraDeviceId;      //Camera id of a non-MAVLink camera attached to an autopilot (1-6).  0 if the component is a MAVLink camera (with its own component id).    

        #region CTOR
        /// <summary>
        /// Instantiates a new CameraTrackingImageStatusData
        /// </summary>    
        /*
        public CameraTrackingImageStatusData() {
            Init();
        }
        */
        public void Init() {
            PointX                = default(float                        );
            PointY                = default(float                        );
            Radius                = default(float                        );
            RecTopX               = default(float                        );
            RecTopY               = default(float                        );
            RecBottomX            = default(float                        );
            RecBottomY            = default(float                        );
            TrackingStatus        = default(CameraTrackingStatusFlags    );
            TrackingMode          = default(CameraTrackingModeFlags      );
            TargetData            = default(CameraTrackingTargetDataFlags);
            CameraDeviceId        = default(byte                         );
        }
        #endregion

        #region Read Buffer
        /// <summary>
        /// Reads the data from Buffer into this struct
        /// </summary>    
        public int Read(byte[] p_buffer,int p_offset=0) {
            int    l = 32;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals
            int[]  LS8  = MAVLinkCRC.U8_LSH8, LS16 = MAVLinkCRC.U8_LSH16, LS24 = MAVLinkCRC.U8_LSH24, LS32 = MAVLinkCRC.U8_LSH32, LS40 = MAVLinkCRC.U8_LSH40, LS48 = MAVLinkCRC.U8_LSH48, LS56 = MAVLinkCRC.U8_LSH56;
            Span<byte> b = p_buffer.AsSpan(p_offset);            
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            PointX                = (float                        ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            PointY                = (float                        ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Radius                = (float                        ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            RecTopX               = (float                        ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            RecTopY               = (float                        ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            RecBottomX            = (float                        ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            RecBottomY            = (float                        ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            TrackingStatus        = (CameraTrackingStatusFlags    ) (b[p++]);
            TrackingMode          = (CameraTrackingModeFlags      ) (b[p++]);
            TargetData            = (CameraTrackingTargetDataFlags) (b[p++]);
            CameraDeviceId        = (byte                         ) (b[p++]);            
            return p;
        }
        #endregion

        #region Write Buffer
        /// <summary>
        /// Writes the message data into a Buffer
        /// </summary>    
        public int Write(byte[] p_buffer,int p_offset=0) {
            int    l = 32;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals            
            Span<byte> b = p_buffer.AsSpan(p_offset);
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            MemoryMarshal.Write(b.Slice(p, 4), ref PointX               ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref PointY               ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref Radius               ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref RecTopX              ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref RecTopY              ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref RecBottomX           ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref RecBottomY           ); p+=4;
            b[p++] = (byte)(TrackingStatus);
            b[p++] = (byte)(TrackingMode);
            b[p++] = (byte)(TargetData);
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
            int l = 32;
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
