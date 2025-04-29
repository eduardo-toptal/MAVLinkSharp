        
using System.IO;
using System.Runtime.InteropServices;
using MAVLinkSharp.Runtime;

#pragma warning disable CS0675

namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// The location of a landing target. See: https://mavlink.io/en/services/landing_target.html
    /// </summary>    
    public struct LandingTargetData : IMAVLinkMessageData {

        /// <summary>
        /// Message Id Associated w/ this Struct
        /// </summary>    
        public int GetId() { return 149; }

        public ulong                   TimeUsec;          //Timestamp (UNIX Epoch time or time since system boot). The receiving end can infer timestamp format (since 1.1.1970 or since system boot) by checking for the magnitude of the number.
        public float                   AngleX;            //X-axis angular offset of the target from the center of the image
        public float                   AngleY;            //Y-axis angular offset of the target from the center of the image
        public float                   Distance;          //Distance to the target from the vehicle
        public float                   SizeX;             //Size of target along x-axis
        public float                   SizeY;             //Size of target along y-axis
        public byte                    TargetNum;         //The ID of the target if multiple targets are present
        public MAVFrameFlags           Frame;             //Coordinate frame used for following fields.
        public float                   X;                 //X Position of the landing target in MAV_FRAME
        public float                   Y;                 //Y Position of the landing target in MAV_FRAME
        public float                   Z;                 //Z Position of the landing target in MAV_FRAME
        public float[]                 Q;                 //Quaternion of landing target orientation (w, x, y, z order, zero-rotation is 1, 0, 0, 0)
        public LandingTargetTypeFlags  Type;              //Type of landing target
        public byte                    PositionValid;     //Boolean indicating whether the position fields (x, y, z, q, type) contain valid target position information (valid: 1, invalid: 0). Default is 0 (invalid).    

        #region CTOR
        /// <summary>
        /// Instantiates a new LandingTargetData
        /// </summary>    
        /*
        public LandingTargetData() {
            Init();
        }
        */
        public void Init() {
            TimeUsec            = default(ulong                 );
            AngleX              = default(float                 );
            AngleY              = default(float                 );
            Distance            = default(float                 );
            SizeX               = default(float                 );
            SizeY               = default(float                 );
            TargetNum           = default(byte                  );
            Frame               = default(MAVFrameFlags         );
            X                   = default(float                 );
            Y                   = default(float                 );
            Z                   = default(float                 );
            Q                   = new float[  4];
            Type                = default(LandingTargetTypeFlags);
            PositionValid       = default(byte                  );
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
            TimeUsec            = (ulong                 ) ((ulong)b[p++] | (ulong)LS8[b[p++]] | (ulong)LS16[b[p++]] | (ulong)LS24[b[p++]] | (ulong)LS32[b[p++]] | (ulong)LS40[b[p++]] | (ulong)LS48[b[p++]] | (ulong)LS56[b[p++]]);
            AngleX              = (float                 ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            AngleY              = (float                 ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Distance            = (float                 ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            SizeX               = (float                 ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            SizeY               = (float                 ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            TargetNum           = (byte                  ) (b[p++]);
            Frame               = (MAVFrameFlags         ) (b[p++]);
            X                   = (float                 ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Y                   = (float                 ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Z                   = (float                 ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            for(int i=0;i<4  ;i++) { Q[i]                = (float                 ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4; }
            Type                = (LandingTargetTypeFlags) (b[p++]);
            PositionValid       = (byte                  ) (b[p++]);            
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
            b[p++] = (byte)(      TimeUsec);
            b[p++] = (byte)((long)TimeUsec>>8 );
            b[p++] = (byte)((long)TimeUsec>>16);
            b[p++] = (byte)((long)TimeUsec>>24);
            b[p++] = (byte)((long)TimeUsec>>32);
            b[p++] = (byte)((long)TimeUsec>>40);
            b[p++] = (byte)((long)TimeUsec>>48);
            b[p++] = (byte)((long)TimeUsec>>56);
            MemoryMarshal.Write(b.Slice(p, 4), ref AngleX             ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref AngleY             ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref Distance           ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref SizeX              ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref SizeY              ); p+=4;
            b[p++] = (byte)(TargetNum);
            b[p++] = (byte)(Frame);
            MemoryMarshal.Write(b.Slice(p, 4), ref X                  ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref Y                  ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref Z                  ); p+=4;
            for(int i=0;i<  4;i++) {
                MemoryMarshal.Write(b.Slice(p, 4), ref Q[i]               ); p+=4;
            }
            b[p++] = (byte)(Type);
            b[p++] = (byte)(PositionValid);
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
