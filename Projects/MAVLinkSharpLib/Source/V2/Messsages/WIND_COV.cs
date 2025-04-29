        
using System.IO;
using System.Runtime.InteropServices;
using MAVLinkSharp.Runtime;

#pragma warning disable CS0675

namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// Wind estimate from vehicle. Note that despite the name, this message does not actually contain any covariances but instead variability and accuracy fields in terms of standard deviation (1-STD).
    /// </summary>    
    public struct WindCovData : IMAVLinkMessageData {

        /// <summary>
        /// Message Id Associated w/ this Struct
        /// </summary>    
        public int GetId() { return 231; }

        public ulong  TimeUsec;          //Timestamp (UNIX Epoch time or time since system boot). The receiving end can infer timestamp format (since 1.1.1970 or since system boot) by checking for the magnitude of the number.
        public float  WindX;             //Wind in North (NED) direction (NAN if unknown)
        public float  WindY;             //Wind in East (NED) direction (NAN if unknown)
        public float  WindZ;             //Wind in down (NED) direction (NAN if unknown)
        public float  VarHoriz;          //Variability of wind in XY, 1-STD estimated from a 1 Hz lowpassed wind estimate (NAN if unknown)
        public float  VarVert;           //Variability of wind in Z, 1-STD estimated from a 1 Hz lowpassed wind estimate (NAN if unknown)
        public float  WindAlt;           //Altitude (MSL) that this measurement was taken at (NAN if unknown)
        public float  HorizAccuracy;     //Horizontal speed 1-STD accuracy (0 if unknown)
        public float  VertAccuracy;      //Vertical speed 1-STD accuracy (0 if unknown)    

        #region CTOR
        /// <summary>
        /// Instantiates a new WindCovData
        /// </summary>    
        public WindCovData() {
            TimeUsec            = default(ulong);
            WindX               = default(float);
            WindY               = default(float);
            WindZ               = default(float);
            VarHoriz            = default(float);
            VarVert             = default(float);
            WindAlt             = default(float);
            HorizAccuracy       = default(float);
            VertAccuracy        = default(float);
        }
        #endregion

        #region Read Buffer
        /// <summary>
        /// Reads the data from Buffer into this struct
        /// </summary>    
        public int Read(byte[] p_buffer,int p_offset=0) {
            int    l = 40;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals
            int[]  LS8  = MAVLinkCRC.U8_LSH8, LS16 = MAVLinkCRC.U8_LSH16, LS24 = MAVLinkCRC.U8_LSH24, LS32 = MAVLinkCRC.U8_LSH32, LS40 = MAVLinkCRC.U8_LSH40, LS48 = MAVLinkCRC.U8_LSH48, LS56 = MAVLinkCRC.U8_LSH56;
            Span<byte> b = p_buffer.AsSpan(p_offset);            
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            TimeUsec            = (ulong) ((ulong)b[p++] | (ulong)LS8[b[p++]] | (ulong)LS16[b[p++]] | (ulong)LS24[b[p++]] | (ulong)LS32[b[p++]] | (ulong)LS40[b[p++]] | (ulong)LS48[b[p++]] | (ulong)LS56[b[p++]]);
            WindX               = (float) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            WindY               = (float) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            WindZ               = (float) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            VarHoriz            = (float) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            VarVert             = (float) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            WindAlt             = (float) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            HorizAccuracy       = (float) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            VertAccuracy        = (float) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;            
            return p;
        }
        #endregion

        #region Write Buffer
        /// <summary>
        /// Writes the message data into a Buffer
        /// </summary>    
        public int Write(byte[] p_buffer,int p_offset=0) {
            int    l = 40;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals            
            Span<byte> b = p_buffer.AsSpan(p_offset);
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            b[p++] = (byte)(      TimeUsec);
            b[p++] = (byte)((long)TimeUsec>>8 );
            b[p++] = (byte)((long)TimeUsec>>16);
            b[p++] = (byte)((long)TimeUsec>>24);
            b[p++] = (byte)((long)TimeUsec>>32);
            b[p++] = (byte)((long)TimeUsec>>40);
            b[p++] = (byte)((long)TimeUsec>>48);
            b[p++] = (byte)((long)TimeUsec>>56);
            MemoryMarshal.Write(b.Slice(p, 4), ref WindX              ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref WindY              ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref WindZ              ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref VarHoriz           ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref VarVert            ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref WindAlt            ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref HorizAccuracy      ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref VertAccuracy       ); p+=4;
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
            int l = 40;
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
