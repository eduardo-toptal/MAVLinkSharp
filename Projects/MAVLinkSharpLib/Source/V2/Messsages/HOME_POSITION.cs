        
using System.IO;
using System.Runtime.InteropServices;
using MAVLinkSharp.Runtime;

#pragma warning disable CS0675

namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// 
    /// Contains the home position.
    /// The home position is the default position that the system will return to and land on.
    /// The position must be set automatically by the system during the takeoff, and may also be explicitly set using MAV_CMD_DO_SET_HOME.
    /// The global and local positions encode the position in the respective coordinate frames, while the q parameter encodes the orientation of the surface.
    /// Under normal conditions it describes the heading and terrain slope, which can be used by the aircraft to adjust the approach.
    /// The approach 3D vector describes the point to which the system should fly in normal flight mode and then perform a landing sequence along the vector.
    /// Note: this message can be requested by sending the MAV_CMD_REQUEST_MESSAGE with param1=242 (or the deprecated MAV_CMD_GET_HOME_POSITION command).
    /// 
    /// </summary>    
    public struct HomePositionData : IMAVLinkMessageData {

        /// <summary>
        /// Message Id Associated w/ this Struct
        /// </summary>    
        public int GetId() { return 242; }

        public int      Latitude;      //Latitude (WGS84)
        public int      Longitude;     //Longitude (WGS84)
        public int      Altitude;      //Altitude (MSL). Positive for up.
        public float    X;             //Local X position of this position in the local coordinate frame
        public float    Y;             //Local Y position of this position in the local coordinate frame
        public float    Z;             //Local Z position of this position in the local coordinate frame
        public float[]  Q;             //World to surface normal and heading transformation of the takeoff position. Used to indicate the heading and slope of the ground
        public float    ApproachX;     //Local X position of the end of the approach vector. Multicopters should set this position based on their takeoff path. Grass-landing fixed wing aircraft should set it the same way as multicopters. Runway-landing fixed wing aircraft should set it to the opposite direction of the takeoff, assuming the takeoff happened from the threshold / touchdown zone.
        public float    ApproachY;     //Local Y position of the end of the approach vector. Multicopters should set this position based on their takeoff path. Grass-landing fixed wing aircraft should set it the same way as multicopters. Runway-landing fixed wing aircraft should set it to the opposite direction of the takeoff, assuming the takeoff happened from the threshold / touchdown zone.
        public float    ApproachZ;     //Local Z position of the end of the approach vector. Multicopters should set this position based on their takeoff path. Grass-landing fixed wing aircraft should set it the same way as multicopters. Runway-landing fixed wing aircraft should set it to the opposite direction of the takeoff, assuming the takeoff happened from the threshold / touchdown zone.
        public ulong    TimeUsec;      //Timestamp (UNIX Epoch time or time since system boot). The receiving end can infer timestamp format (since 1.1.1970 or since system boot) by checking for the magnitude of the number.    

        #region CTOR
        /// <summary>
        /// Instantiates a new HomePositionData
        /// </summary>    
        public HomePositionData() {
            Latitude        = default(int  );
            Longitude       = default(int  );
            Altitude        = default(int  );
            X               = default(float);
            Y               = default(float);
            Z               = default(float);
            Q               = new float[  4];
            ApproachX       = default(float);
            ApproachY       = default(float);
            ApproachZ       = default(float);
            TimeUsec        = default(ulong);
        }
        #endregion

        #region Read Buffer
        /// <summary>
        /// Reads the data from Buffer into this struct
        /// </summary>    
        public int Read(byte[] p_buffer,int p_offset=0) {
            int    l = 60;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals
            int[]  LS8  = MAVLinkCRC.U8_LSH8, LS16 = MAVLinkCRC.U8_LSH16, LS24 = MAVLinkCRC.U8_LSH24, LS32 = MAVLinkCRC.U8_LSH32, LS40 = MAVLinkCRC.U8_LSH40, LS48 = MAVLinkCRC.U8_LSH48, LS56 = MAVLinkCRC.U8_LSH56;
            Span<byte> b = p_buffer.AsSpan(p_offset);            
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            Latitude        = (int  ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            Longitude       = (int  ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            Altitude        = (int  ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            X               = (float) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Y               = (float) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Z               = (float) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            for(int i=0;i<4  ;i++) { Q[i]            = (float) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4; }
            ApproachX       = (float) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            ApproachY       = (float) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            ApproachZ       = (float) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            TimeUsec        = (ulong) ((ulong)b[p++] | (ulong)LS8[b[p++]] | (ulong)LS16[b[p++]] | (ulong)LS24[b[p++]] | (ulong)LS32[b[p++]] | (ulong)LS40[b[p++]] | (ulong)LS48[b[p++]] | (ulong)LS56[b[p++]]);            
            return p;
        }
        #endregion

        #region Write Buffer
        /// <summary>
        /// Writes the message data into a Buffer
        /// </summary>    
        public int Write(byte[] p_buffer,int p_offset=0) {
            int    l = 60;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals            
            Span<byte> b = p_buffer.AsSpan(p_offset);
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            b[p++] = (byte)(      Latitude);
            b[p++] = (byte)((int)Latitude>>8 );
            b[p++] = (byte)((int)Latitude>>16);
            b[p++] = (byte)((int)Latitude>>24);
            b[p++] = (byte)(      Longitude);
            b[p++] = (byte)((int)Longitude>>8 );
            b[p++] = (byte)((int)Longitude>>16);
            b[p++] = (byte)((int)Longitude>>24);
            b[p++] = (byte)(      Altitude);
            b[p++] = (byte)((int)Altitude>>8 );
            b[p++] = (byte)((int)Altitude>>16);
            b[p++] = (byte)((int)Altitude>>24);
            MemoryMarshal.Write(b.Slice(p, 4), ref X              ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref Y              ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref Z              ); p+=4;
            for(int i=0;i<  4;i++) {
                MemoryMarshal.Write(b.Slice(p, 4), ref Q[i]           ); p+=4;
            }
            MemoryMarshal.Write(b.Slice(p, 4), ref ApproachX      ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref ApproachY      ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref ApproachZ      ); p+=4;
            b[p++] = (byte)(      TimeUsec);
            b[p++] = (byte)((long)TimeUsec>>8 );
            b[p++] = (byte)((long)TimeUsec>>16);
            b[p++] = (byte)((long)TimeUsec>>24);
            b[p++] = (byte)((long)TimeUsec>>32);
            b[p++] = (byte)((long)TimeUsec>>40);
            b[p++] = (byte)((long)TimeUsec>>48);
            b[p++] = (byte)((long)TimeUsec>>56);
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
            int l = 60;
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
