        
using System.IO;
using System.Runtime.InteropServices;

#pragma warning disable CS0675

namespace MAVLinkBindings {

    /// <summary>
    /// Sent from simulation to autopilot, avoids in contrast to HIL_STATE singularities. This packet is useful for high throughput applications such as hardware in the loop simulations.
    /// </summary>    
    public struct HilStateQuaternionData : IMAVLinkMessageData {

        /// <summary>
        /// Message Id Associated w/ this Struct
        /// </summary>    
        public int GetId() { return 115; }

        public ulong    TimeUsec;               //Timestamp (UNIX Epoch time or time since system boot). The receiving end can infer timestamp format (since 1.1.1970 or since system boot) by checking for the magnitude of the number.
        public float[]  AttitudeQuaternion;     //Vehicle attitude expressed as normalized quaternion in w, x, y, z order (with 1 0 0 0 being the null-rotation)
        public float    Rollspeed;              //Body frame roll / phi angular speed
        public float    Pitchspeed;             //Body frame pitch / theta angular speed
        public float    Yawspeed;               //Body frame yaw / psi angular speed
        public int      Lat;                    //Latitude
        public int      Lon;                    //Longitude
        public int      Alt;                    //Altitude
        public short    Vx;                     //Ground X Speed (Latitude)
        public short    Vy;                     //Ground Y Speed (Longitude)
        public short    Vz;                     //Ground Z Speed (Altitude)
        public ushort   IndAirspeed;            //Indicated airspeed
        public ushort   TrueAirspeed;           //True airspeed
        public short    Xacc;                   //X acceleration
        public short    Yacc;                   //Y acceleration
        public short    Zacc;                   //Z acceleration    

        #region CTOR
        /// <summary>
        /// Instantiates a new HilStateQuaternionData
        /// </summary>    
        public HilStateQuaternionData() {
            TimeUsec                 = default(ulong );
            AttitudeQuaternion       = new float[  4];
            Rollspeed                = default(float );
            Pitchspeed               = default(float );
            Yawspeed                 = default(float );
            Lat                      = default(int   );
            Lon                      = default(int   );
            Alt                      = default(int   );
            Vx                       = default(short );
            Vy                       = default(short );
            Vz                       = default(short );
            IndAirspeed              = default(ushort);
            TrueAirspeed             = default(ushort);
            Xacc                     = default(short );
            Yacc                     = default(short );
            Zacc                     = default(short );
        }
        #endregion

        #region Read Buffer
        /// <summary>
        /// Reads the data from Buffer into this struct
        /// </summary>    
        public int Read(byte[] p_buffer,int p_offset=0) {
            int    l = 64;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals
            int[]  LS8  = MAVLinkCRC.U8_LSH8, LS16 = MAVLinkCRC.U8_LSH16, LS24 = MAVLinkCRC.U8_LSH24, LS32 = MAVLinkCRC.U8_LSH32, LS40 = MAVLinkCRC.U8_LSH40, LS48 = MAVLinkCRC.U8_LSH48, LS56 = MAVLinkCRC.U8_LSH56;
            Span<byte> b = p_buffer.AsSpan(p_offset);            
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            TimeUsec                 = (ulong ) ((ulong)b[p++] | (ulong)LS8[b[p++]] | (ulong)LS16[b[p++]] | (ulong)LS24[b[p++]] | (ulong)LS32[b[p++]] | (ulong)LS40[b[p++]] | (ulong)LS48[b[p++]] | (ulong)LS56[b[p++]]);
            for(int i=0;i<4  ;i++) { AttitudeQuaternion[i]    = (float ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4; }
            Rollspeed                = (float ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Pitchspeed               = (float ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Yawspeed                 = (float ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Lat                      = (int   ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            Lon                      = (int   ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            Alt                      = (int   ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            Vx                       = (short ) (b[p++] | LS8[b[p++]]);
            Vy                       = (short ) (b[p++] | LS8[b[p++]]);
            Vz                       = (short ) (b[p++] | LS8[b[p++]]);
            IndAirspeed              = (ushort) (b[p++] | LS8[b[p++]]);
            TrueAirspeed             = (ushort) (b[p++] | LS8[b[p++]]);
            Xacc                     = (short ) (b[p++] | LS8[b[p++]]);
            Yacc                     = (short ) (b[p++] | LS8[b[p++]]);
            Zacc                     = (short ) (b[p++] | LS8[b[p++]]);            
            return p;
        }
        #endregion

        #region Write Buffer
        /// <summary>
        /// Writes the message data into a Buffer
        /// </summary>    
        public int Write(byte[] p_buffer,int p_offset=0) {
            int    l = 64;
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
            for(int i=0;i<  4;i++) {
                MemoryMarshal.Write(b.Slice(p, 4), in AttitudeQuaternion[i]   ); p+=4;
            }
            MemoryMarshal.Write(b.Slice(p, 4), in Rollspeed               ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), in Pitchspeed              ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), in Yawspeed                ); p+=4;
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
            b[p++] = (byte)(      Vx);
            b[p++] = (byte)((int)Vx>>8 );
            b[p++] = (byte)(      Vy);
            b[p++] = (byte)((int)Vy>>8 );
            b[p++] = (byte)(      Vz);
            b[p++] = (byte)((int)Vz>>8 );
            b[p++] = (byte)(      IndAirspeed);
            b[p++] = (byte)((int)IndAirspeed>>8 );
            b[p++] = (byte)(      TrueAirspeed);
            b[p++] = (byte)((int)TrueAirspeed>>8 );
            b[p++] = (byte)(      Xacc);
            b[p++] = (byte)((int)Xacc>>8 );
            b[p++] = (byte)(      Yacc);
            b[p++] = (byte)((int)Yacc>>8 );
            b[p++] = (byte)(      Zacc);
            b[p++] = (byte)((int)Zacc>>8 );
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
            int l = 64;
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
