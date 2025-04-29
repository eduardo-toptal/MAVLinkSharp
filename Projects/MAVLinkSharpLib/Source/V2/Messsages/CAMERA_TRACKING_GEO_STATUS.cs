        
using System;
using System.IO;
using System.Runtime.InteropServices;
using MAVLinkSharp.Runtime;

#pragma warning disable CS0675

namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// Camera tracking status, sent while in active tracking. Use MAV_CMD_SET_MESSAGE_INTERVAL to define message interval.
    /// </summary>    
    public struct CameraTrackingGeoStatusData : IMAVLinkMessageData {

        /// <summary>
        /// Message Id Associated w/ this Struct
        /// </summary>    
        public int GetId() { return 276; }

        public int                        Lat;                //Latitude of tracked object
        public int                        Lon;                //Longitude of tracked object
        public float                      Alt;                //Altitude of tracked object(AMSL, WGS84)
        public float                      HAcc;               //Horizontal accuracy. NAN if unknown
        public float                      VAcc;               //Vertical accuracy. NAN if unknown
        public float                      VelN;               //North velocity of tracked object. NAN if unknown
        public float                      VelE;               //East velocity of tracked object. NAN if unknown
        public float                      VelD;               //Down velocity of tracked object. NAN if unknown
        public float                      VelAcc;             //Velocity accuracy. NAN if unknown
        public float                      Dist;               //Distance between camera and tracked object. NAN if unknown
        public float                      Hdg;                //Heading in radians, in NED. NAN if unknown
        public float                      HdgAcc;             //Accuracy of heading, in NED. NAN if unknown
        public CameraTrackingStatusFlags  TrackingStatus;     //Current tracking status    

        #region CTOR
        /// <summary>
        /// Instantiates a new CameraTrackingGeoStatusData
        /// </summary>    
        /*
        public CameraTrackingGeoStatusData() {
            Init();
        }
        */
        public void Init() {
            Lat                  = default(int                      );
            Lon                  = default(int                      );
            Alt                  = default(float                    );
            HAcc                 = default(float                    );
            VAcc                 = default(float                    );
            VelN                 = default(float                    );
            VelE                 = default(float                    );
            VelD                 = default(float                    );
            VelAcc               = default(float                    );
            Dist                 = default(float                    );
            Hdg                  = default(float                    );
            HdgAcc               = default(float                    );
            TrackingStatus       = default(CameraTrackingStatusFlags);
        }
        #endregion

        #region Read Buffer
        /// <summary>
        /// Reads the data from Buffer into this struct
        /// </summary>    
        public int Read(byte[] p_buffer,int p_offset=0) {
            int    l = 49;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals
            int[]  LS8  = MAVLinkCRC.U8_LSH8, LS16 = MAVLinkCRC.U8_LSH16, LS24 = MAVLinkCRC.U8_LSH24, LS32 = MAVLinkCRC.U8_LSH32, LS40 = MAVLinkCRC.U8_LSH40, LS48 = MAVLinkCRC.U8_LSH48, LS56 = MAVLinkCRC.U8_LSH56;
            Span<byte> b = p_buffer.AsSpan(p_offset);            
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            Lat                  = (int                      ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            Lon                  = (int                      ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            Alt                  = (float                    ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            HAcc                 = (float                    ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            VAcc                 = (float                    ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            VelN                 = (float                    ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            VelE                 = (float                    ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            VelD                 = (float                    ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            VelAcc               = (float                    ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Dist                 = (float                    ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Hdg                  = (float                    ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            HdgAcc               = (float                    ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            TrackingStatus       = (CameraTrackingStatusFlags) (b[p++]);            
            return p;
        }
        #endregion

        #region Write Buffer
        /// <summary>
        /// Writes the message data into a Buffer
        /// </summary>    
        public int Write(byte[] p_buffer,int p_offset=0) {
            int    l = 49;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals            
            Span<byte> b = p_buffer.AsSpan(p_offset);
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            b[p++] = (byte)(      Lat);
            b[p++] = (byte)((int)Lat>>8 );
            b[p++] = (byte)((int)Lat>>16);
            b[p++] = (byte)((int)Lat>>24);
            b[p++] = (byte)(      Lon);
            b[p++] = (byte)((int)Lon>>8 );
            b[p++] = (byte)((int)Lon>>16);
            b[p++] = (byte)((int)Lon>>24);
            MemoryMarshal.Write(b.Slice(p, 4), ref Alt                 ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref HAcc                ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref VAcc                ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref VelN                ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref VelE                ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref VelD                ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref VelAcc              ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref Dist                ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref Hdg                 ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref HdgAcc              ); p+=4;
            b[p++] = (byte)(TrackingStatus);
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
            int l = 49;
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
