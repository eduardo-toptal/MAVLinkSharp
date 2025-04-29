        
using System.IO;
using System.Runtime.InteropServices;
using MAVLinkSharp.Runtime;

#pragma warning disable CS0675

namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// Information about the field of view of a camera. Can be requested with a MAV_CMD_REQUEST_MESSAGE command.
    /// </summary>    
    public struct CameraFovStatusData : IMAVLinkMessageData {

        /// <summary>
        /// Message Id Associated w/ this Struct
        /// </summary>    
        public int GetId() { return 271; }

        public uint     TimeBootMs;      //Timestamp (time since system boot).
        public int      LatCamera;       //Latitude of camera (INT32_MAX if unknown).
        public int      LonCamera;       //Longitude of camera (INT32_MAX if unknown).
        public int      AltCamera;       //Altitude (MSL) of camera (INT32_MAX if unknown).
        public int      LatImage;        //Latitude of center of image (INT32_MAX if unknown, INT32_MIN if at infinity, not intersecting with horizon).
        public int      LonImage;        //Longitude of center of image (INT32_MAX if unknown, INT32_MIN if at infinity, not intersecting with horizon).
        public int      AltImage;        //Altitude (MSL) of center of image (INT32_MAX if unknown, INT32_MIN if at infinity, not intersecting with horizon).
        public float[]  Q;               //Quaternion of camera orientation (w, x, y, z order, zero-rotation is 1, 0, 0, 0)
        public float    Hfov;            //Horizontal field of view (NaN if unknown).
        public float    Vfov;            //Vertical field of view (NaN if unknown).    

        #region CTOR
        /// <summary>
        /// Instantiates a new CameraFovStatusData
        /// </summary>    
        public CameraFovStatusData() {
            TimeBootMs        = default(uint );
            LatCamera         = default(int  );
            LonCamera         = default(int  );
            AltCamera         = default(int  );
            LatImage          = default(int  );
            LonImage          = default(int  );
            AltImage          = default(int  );
            Q                 = new float[  4];
            Hfov              = default(float);
            Vfov              = default(float);
        }
        #endregion

        #region Read Buffer
        /// <summary>
        /// Reads the data from Buffer into this struct
        /// </summary>    
        public int Read(byte[] p_buffer,int p_offset=0) {
            int    l = 52;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals
            int[]  LS8  = MAVLinkCRC.U8_LSH8, LS16 = MAVLinkCRC.U8_LSH16, LS24 = MAVLinkCRC.U8_LSH24, LS32 = MAVLinkCRC.U8_LSH32, LS40 = MAVLinkCRC.U8_LSH40, LS48 = MAVLinkCRC.U8_LSH48, LS56 = MAVLinkCRC.U8_LSH56;
            Span<byte> b = p_buffer.AsSpan(p_offset);            
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            TimeBootMs        = (uint ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            LatCamera         = (int  ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            LonCamera         = (int  ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            AltCamera         = (int  ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            LatImage          = (int  ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            LonImage          = (int  ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            AltImage          = (int  ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            for(int i=0;i<4  ;i++) { Q[i]              = (float) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4; }
            Hfov              = (float) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Vfov              = (float) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;            
            return p;
        }
        #endregion

        #region Write Buffer
        /// <summary>
        /// Writes the message data into a Buffer
        /// </summary>    
        public int Write(byte[] p_buffer,int p_offset=0) {
            int    l = 52;
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
            b[p++] = (byte)(      LatCamera);
            b[p++] = (byte)((int)LatCamera>>8 );
            b[p++] = (byte)((int)LatCamera>>16);
            b[p++] = (byte)((int)LatCamera>>24);
            b[p++] = (byte)(      LonCamera);
            b[p++] = (byte)((int)LonCamera>>8 );
            b[p++] = (byte)((int)LonCamera>>16);
            b[p++] = (byte)((int)LonCamera>>24);
            b[p++] = (byte)(      AltCamera);
            b[p++] = (byte)((int)AltCamera>>8 );
            b[p++] = (byte)((int)AltCamera>>16);
            b[p++] = (byte)((int)AltCamera>>24);
            b[p++] = (byte)(      LatImage);
            b[p++] = (byte)((int)LatImage>>8 );
            b[p++] = (byte)((int)LatImage>>16);
            b[p++] = (byte)((int)LatImage>>24);
            b[p++] = (byte)(      LonImage);
            b[p++] = (byte)((int)LonImage>>8 );
            b[p++] = (byte)((int)LonImage>>16);
            b[p++] = (byte)((int)LonImage>>24);
            b[p++] = (byte)(      AltImage);
            b[p++] = (byte)((int)AltImage>>8 );
            b[p++] = (byte)((int)AltImage>>16);
            b[p++] = (byte)((int)AltImage>>24);
            for(int i=0;i<  4;i++) {
                MemoryMarshal.Write(b.Slice(p, 4), ref Q[i]             ); p+=4;
            }
            MemoryMarshal.Write(b.Slice(p, 4), ref Hfov             ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref Vfov             ); p+=4;
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
            int l = 52;
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
