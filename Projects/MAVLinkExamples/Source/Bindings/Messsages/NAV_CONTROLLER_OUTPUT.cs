        
using System.IO;
using System.Runtime.InteropServices;

#pragma warning disable CS0675

namespace MAVLinkBindings {

    /// <summary>
    /// The state of the navigation and position controller.
    /// </summary>    
    public struct NavControllerOutputData : IMAVLinkMessageData {

        /// <summary>
        /// Message Id Associated w/ this Struct
        /// </summary>    
        public int GetId() { return 62; }

        public float   NavRoll;           //Current desired roll
        public float   NavPitch;          //Current desired pitch
        public float   AltError;          //Current altitude error
        public float   AspdError;         //Current airspeed error
        public float   XtrackError;       //Current crosstrack error on x-y plane
        public short   NavBearing;        //Current desired heading
        public short   TargetBearing;     //Bearing to current waypoint/target
        public ushort  WpDist;            //Distance to active waypoint    

        #region CTOR
        /// <summary>
        /// Instantiates a new NavControllerOutputData
        /// </summary>    
        public NavControllerOutputData() {
            NavRoll             = default(float );
            NavPitch            = default(float );
            AltError            = default(float );
            AspdError           = default(float );
            XtrackError         = default(float );
            NavBearing          = default(short );
            TargetBearing       = default(short );
            WpDist              = default(ushort);
        }
        #endregion

        #region Read Buffer
        /// <summary>
        /// Reads the data from Buffer into this struct
        /// </summary>    
        public int Read(byte[] p_buffer,int p_offset=0) {
            int    l = 26;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals
            int[]  LS8  = MAVLinkCRC.U8_LSH8, LS16 = MAVLinkCRC.U8_LSH16, LS24 = MAVLinkCRC.U8_LSH24, LS32 = MAVLinkCRC.U8_LSH32, LS40 = MAVLinkCRC.U8_LSH40, LS48 = MAVLinkCRC.U8_LSH48, LS56 = MAVLinkCRC.U8_LSH56;
            Span<byte> b = p_buffer.AsSpan(p_offset);            
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            NavRoll             = (float ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            NavPitch            = (float ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            AltError            = (float ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            AspdError           = (float ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            XtrackError         = (float ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            NavBearing          = (short ) (b[p++] | LS8[b[p++]]);
            TargetBearing       = (short ) (b[p++] | LS8[b[p++]]);
            WpDist              = (ushort) (b[p++] | LS8[b[p++]]);            
            return p;
        }
        #endregion

        #region Write Buffer
        /// <summary>
        /// Writes the message data into a Buffer
        /// </summary>    
        public int Write(byte[] p_buffer,int p_offset=0) {
            int    l = 26;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals            
            Span<byte> b = p_buffer.AsSpan(p_offset);
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            MemoryMarshal.Write(b.Slice(p, 4), in NavRoll            ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), in NavPitch           ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), in AltError           ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), in AspdError          ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), in XtrackError        ); p+=4;
            b[p++] = (byte)(      NavBearing);
            b[p++] = (byte)((int)NavBearing>>8 );
            b[p++] = (byte)(      TargetBearing);
            b[p++] = (byte)((int)TargetBearing>>8 );
            b[p++] = (byte)(      WpDist);
            b[p++] = (byte)((int)WpDist>>8 );
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
            int l = 26;
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
