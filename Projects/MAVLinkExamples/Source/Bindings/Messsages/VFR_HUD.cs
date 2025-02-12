        
using System.IO;
using System.Runtime.InteropServices;

#pragma warning disable CS0675

namespace MAVLinkBindings {

    /// <summary>
    /// Metrics typically displayed on a HUD for fixed wing aircraft.
    /// </summary>    
    public struct VfrHudData : IMAVLinkMessageData {

        /// <summary>
        /// Message Id Associated w/ this Struct
        /// </summary>    
        public int GetId() { return 74; }

        public float   Airspeed;       //Vehicle speed in form appropriate for vehicle type. For standard aircraft this is typically calibrated airspeed (CAS) or indicated airspeed (IAS) - either of which can be used by a pilot to estimate stall speed.
        public float   Groundspeed;    //Current ground speed.
        public float   Alt;            //Current altitude (MSL).
        public float   Climb;          //Current climb rate.
        public short   Heading;        //Current heading in compass units (0-360, 0=north).
        public ushort  Throttle;       //Current throttle setting (0 to 100).    

        #region CTOR
        /// <summary>
        /// Instantiates a new VfrHudData
        /// </summary>    
        public VfrHudData() {
            Airspeed         = default(float );
            Groundspeed      = default(float );
            Alt              = default(float );
            Climb            = default(float );
            Heading          = default(short );
            Throttle         = default(ushort);
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
            Airspeed         = (float ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Groundspeed      = (float ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Alt              = (float ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Climb            = (float ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Heading          = (short ) (b[p++] | LS8[b[p++]]);
            Throttle         = (ushort) (b[p++] | LS8[b[p++]]);            
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
            MemoryMarshal.Write(b.Slice(p, 4), in Airspeed        ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), in Groundspeed     ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), in Alt             ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), in Climb           ); p+=4;
            b[p++] = (byte)(      Heading);
            b[p++] = (byte)((int)Heading>>8 );
            b[p++] = (byte)(      Throttle);
            b[p++] = (byte)((int)Throttle>>8 );
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
