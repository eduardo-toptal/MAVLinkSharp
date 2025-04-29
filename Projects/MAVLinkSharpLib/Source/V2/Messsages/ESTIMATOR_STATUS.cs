        
using System.IO;
using System.Runtime.InteropServices;
using MAVLinkSharp.Runtime;

#pragma warning disable CS0675

namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// Estimator status message including flags, innovation test ratios and estimated accuracies. The flags message is an integer bitmask containing information on which EKF outputs are valid. See the ESTIMATOR_STATUS_FLAGS enum definition for further information. The innovation test ratios show the magnitude of the sensor innovation divided by the innovation check threshold. Under normal operation the innovation test ratios should be below 0.5 with occasional values up to 1.0. Values greater than 1.0 should be rare under normal operation and indicate that a measurement has been rejected by the filter. The user should be notified if an innovation test ratio greater than 1.0 is recorded. Notifications for values in the range between 0.5 and 1.0 should be optional and controllable by the user.
    /// </summary>    
    public struct EstimatorStatusData : IMAVLinkMessageData {

        /// <summary>
        /// Message Id Associated w/ this Struct
        /// </summary>    
        public int GetId() { return 230; }

        public ulong                 TimeUsec;              //Timestamp (UNIX Epoch time or time since system boot). The receiving end can infer timestamp format (since 1.1.1970 or since system boot) by checking for the magnitude of the number.
        public float                 VelRatio;              //Velocity innovation test ratio
        public float                 PosHorizRatio;         //Horizontal position innovation test ratio
        public float                 PosVertRatio;          //Vertical position innovation test ratio
        public float                 MagRatio;              //Magnetometer innovation test ratio
        public float                 HaglRatio;             //Height above terrain innovation test ratio
        public float                 TasRatio;              //True airspeed innovation test ratio
        public float                 PosHorizAccuracy;      //Horizontal position 1-STD accuracy relative to the EKF local origin
        public float                 PosVertAccuracy;       //Vertical position 1-STD accuracy relative to the EKF local origin
        public EstimatorStatusFlags  Flags;                 //Bitmap indicating which EKF outputs are valid.    

        #region CTOR
        /// <summary>
        /// Instantiates a new EstimatorStatusData
        /// </summary>    
        /*
        public EstimatorStatusData() {
            Init();
        }
        */
        public void Init() {
            TimeUsec                = default(ulong               );
            VelRatio                = default(float               );
            PosHorizRatio           = default(float               );
            PosVertRatio            = default(float               );
            MagRatio                = default(float               );
            HaglRatio               = default(float               );
            TasRatio                = default(float               );
            PosHorizAccuracy        = default(float               );
            PosVertAccuracy         = default(float               );
            Flags                   = default(EstimatorStatusFlags);
        }
        #endregion

        #region Read Buffer
        /// <summary>
        /// Reads the data from Buffer into this struct
        /// </summary>    
        public int Read(byte[] p_buffer,int p_offset=0) {
            int    l = 42;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals
            int[]  LS8  = MAVLinkCRC.U8_LSH8, LS16 = MAVLinkCRC.U8_LSH16, LS24 = MAVLinkCRC.U8_LSH24, LS32 = MAVLinkCRC.U8_LSH32, LS40 = MAVLinkCRC.U8_LSH40, LS48 = MAVLinkCRC.U8_LSH48, LS56 = MAVLinkCRC.U8_LSH56;
            Span<byte> b = p_buffer.AsSpan(p_offset);            
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            TimeUsec                = (ulong               ) ((ulong)b[p++] | (ulong)LS8[b[p++]] | (ulong)LS16[b[p++]] | (ulong)LS24[b[p++]] | (ulong)LS32[b[p++]] | (ulong)LS40[b[p++]] | (ulong)LS48[b[p++]] | (ulong)LS56[b[p++]]);
            VelRatio                = (float               ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            PosHorizRatio           = (float               ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            PosVertRatio            = (float               ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            MagRatio                = (float               ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            HaglRatio               = (float               ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            TasRatio                = (float               ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            PosHorizAccuracy        = (float               ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            PosVertAccuracy         = (float               ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Flags                   = (EstimatorStatusFlags) (b[p++] | LS8[b[p++]]);            
            return p;
        }
        #endregion

        #region Write Buffer
        /// <summary>
        /// Writes the message data into a Buffer
        /// </summary>    
        public int Write(byte[] p_buffer,int p_offset=0) {
            int    l = 42;
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
            MemoryMarshal.Write(b.Slice(p, 4), ref VelRatio               ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref PosHorizRatio          ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref PosVertRatio           ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref MagRatio               ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref HaglRatio              ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref TasRatio               ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref PosHorizAccuracy       ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref PosVertAccuracy        ); p+=4;
            b[p++] = (byte)(      Flags);
            b[p++] = (byte)((int)Flags>>8 );
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
            int l = 42;
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
