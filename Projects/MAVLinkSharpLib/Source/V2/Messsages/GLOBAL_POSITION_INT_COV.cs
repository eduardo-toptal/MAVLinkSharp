        
using System.IO;
using System.Runtime.InteropServices;
using MAVLinkSharp.Runtime;

#pragma warning disable CS0675

namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// The filtered global position (e.g. fused GPS and accelerometers). The position is in GPS-frame (right-handed, Z-up). It  is designed as scaled integer message since the resolution of float is not sufficient. NOTE: This message is intended for onboard networks / companion computers and higher-bandwidth links and optimized for accuracy and completeness. Please use the GLOBAL_POSITION_INT message for a minimal subset.
    /// </summary>    
    public struct GlobalPositionIntCovData : IMAVLinkMessageData {

        /// <summary>
        /// Message Id Associated w/ this Struct
        /// </summary>    
        public int GetId() { return 63; }

        public ulong                  TimeUsec;          //Timestamp (UNIX Epoch time or time since system boot). The receiving end can infer timestamp format (since 1.1.1970 or since system boot) by checking for the magnitude of the number.
        public int                    Lat;               //Latitude
        public int                    Lon;               //Longitude
        public int                    Alt;               //Altitude in meters above MSL
        public int                    RelativeAlt;       //Altitude above ground
        public float                  Vx;                //Ground X Speed (Latitude)
        public float                  Vy;                //Ground Y Speed (Longitude)
        public float                  Vz;                //Ground Z Speed (Altitude)
        public float[]                Covariance;        //Row-major representation of a 6x6 position and velocity 6x6 cross-covariance matrix (states: lat, lon, alt, vx, vy, vz; first six entries are the first ROW, next six entries are the second row, etc.). If unknown, assign NaN value to first element in the array.
        public MAVEstimatorTypeFlags  EstimatorType;     //Class id of the estimator this estimate originated from.    

        #region CTOR
        /// <summary>
        /// Instantiates a new GlobalPositionIntCovData
        /// </summary>    
        /*
        public GlobalPositionIntCovData() {
            Init();
        }
        */
        public void Init() {
            TimeUsec            = default(ulong                );
            Lat                 = default(int                  );
            Lon                 = default(int                  );
            Alt                 = default(int                  );
            RelativeAlt         = default(int                  );
            Vx                  = default(float                );
            Vy                  = default(float                );
            Vz                  = default(float                );
            Covariance          = new float[ 36];
            EstimatorType       = default(MAVEstimatorTypeFlags);
        }
        #endregion

        #region Read Buffer
        /// <summary>
        /// Reads the data from Buffer into this struct
        /// </summary>    
        public int Read(byte[] p_buffer,int p_offset=0) {
            int    l = 181;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals
            int[]  LS8  = MAVLinkCRC.U8_LSH8, LS16 = MAVLinkCRC.U8_LSH16, LS24 = MAVLinkCRC.U8_LSH24, LS32 = MAVLinkCRC.U8_LSH32, LS40 = MAVLinkCRC.U8_LSH40, LS48 = MAVLinkCRC.U8_LSH48, LS56 = MAVLinkCRC.U8_LSH56;
            Span<byte> b = p_buffer.AsSpan(p_offset);            
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            TimeUsec            = (ulong                ) ((ulong)b[p++] | (ulong)LS8[b[p++]] | (ulong)LS16[b[p++]] | (ulong)LS24[b[p++]] | (ulong)LS32[b[p++]] | (ulong)LS40[b[p++]] | (ulong)LS48[b[p++]] | (ulong)LS56[b[p++]]);
            Lat                 = (int                  ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            Lon                 = (int                  ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            Alt                 = (int                  ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            RelativeAlt         = (int                  ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            Vx                  = (float                ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Vy                  = (float                ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Vz                  = (float                ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            for(int i=0;i<36 ;i++) { Covariance[i]       = (float                ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4; }
            EstimatorType       = (MAVEstimatorTypeFlags) (b[p++]);            
            return p;
        }
        #endregion

        #region Write Buffer
        /// <summary>
        /// Writes the message data into a Buffer
        /// </summary>    
        public int Write(byte[] p_buffer,int p_offset=0) {
            int    l = 181;
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
            b[p++] = (byte)(      RelativeAlt);
            b[p++] = (byte)((int)RelativeAlt>>8 );
            b[p++] = (byte)((int)RelativeAlt>>16);
            b[p++] = (byte)((int)RelativeAlt>>24);
            MemoryMarshal.Write(b.Slice(p, 4), ref Vx                 ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref Vy                 ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref Vz                 ); p+=4;
            for(int i=0;i< 36;i++) {
                MemoryMarshal.Write(b.Slice(p, 4), ref Covariance[i]      ); p+=4;
            }
            b[p++] = (byte)(EstimatorType);
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
            int l = 181;
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
