        
using System.IO;
using System.Runtime.InteropServices;

#pragma warning disable CS0675

namespace MAVLinkBindings {

    /// <summary>
    /// The IMU readings in SI units in NED body frame
    /// </summary>    
    public struct HilSensorData : IMAVLinkMessageData {

        /// <summary>
        /// Message Id Associated w/ this Struct
        /// </summary>    
        public int GetId() { return 107; }

        public ulong                  TimeUsec;          //Timestamp (UNIX Epoch time or time since system boot). The receiving end can infer timestamp format (since 1.1.1970 or since system boot) by checking for the magnitude of the number.
        public float                  Xacc;              //X acceleration
        public float                  Yacc;              //Y acceleration
        public float                  Zacc;              //Z acceleration
        public float                  Xgyro;             //Angular speed around X axis in body frame
        public float                  Ygyro;             //Angular speed around Y axis in body frame
        public float                  Zgyro;             //Angular speed around Z axis in body frame
        public float                  Xmag;              //X Magnetic field
        public float                  Ymag;              //Y Magnetic field
        public float                  Zmag;              //Z Magnetic field
        public float                  AbsPressure;       //Absolute pressure
        public float                  DiffPressure;      //Differential pressure (airspeed)
        public float                  PressureAlt;       //Altitude calculated from pressure
        public float                  Temperature;       //Temperature
        public HilSensorUpdatedFlags  FieldsUpdated;     //Bitmap for fields that have updated since last message
        public byte                   Id;                //Sensor ID (zero indexed). Used for multiple sensor inputs    

        #region CTOR
        /// <summary>
        /// Instantiates a new HilSensorData
        /// </summary>    
        public HilSensorData() {
            TimeUsec            = default(ulong                );
            Xacc                = default(float                );
            Yacc                = default(float                );
            Zacc                = default(float                );
            Xgyro               = default(float                );
            Ygyro               = default(float                );
            Zgyro               = default(float                );
            Xmag                = default(float                );
            Ymag                = default(float                );
            Zmag                = default(float                );
            AbsPressure         = default(float                );
            DiffPressure        = default(float                );
            PressureAlt         = default(float                );
            Temperature         = default(float                );
            FieldsUpdated       = default(HilSensorUpdatedFlags);
            Id                  = default(byte                 );
        }
        #endregion

        #region Read Buffer
        /// <summary>
        /// Reads the data from Buffer into this struct
        /// </summary>    
        public int Read(byte[] p_buffer,int p_offset=0) {
            int    l = 65;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals
            int[]  LS8  = MAVLinkCRC.U8_LSH8, LS16 = MAVLinkCRC.U8_LSH16, LS24 = MAVLinkCRC.U8_LSH24, LS32 = MAVLinkCRC.U8_LSH32, LS40 = MAVLinkCRC.U8_LSH40, LS48 = MAVLinkCRC.U8_LSH48, LS56 = MAVLinkCRC.U8_LSH56;
            Span<byte> b = p_buffer.AsSpan(p_offset);            
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            TimeUsec            = (ulong                ) ((ulong)b[p++] | (ulong)LS8[b[p++]] | (ulong)LS16[b[p++]] | (ulong)LS24[b[p++]] | (ulong)LS32[b[p++]] | (ulong)LS40[b[p++]] | (ulong)LS48[b[p++]] | (ulong)LS56[b[p++]]);
            Xacc                = (float                ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Yacc                = (float                ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Zacc                = (float                ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Xgyro               = (float                ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Ygyro               = (float                ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Zgyro               = (float                ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Xmag                = (float                ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Ymag                = (float                ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Zmag                = (float                ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            AbsPressure         = (float                ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            DiffPressure        = (float                ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            PressureAlt         = (float                ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Temperature         = (float                ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            FieldsUpdated       = (HilSensorUpdatedFlags) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            Id                  = (byte                 ) (b[p++]);            
            return p;
        }
        #endregion

        #region Write Buffer
        /// <summary>
        /// Writes the message data into a Buffer
        /// </summary>    
        public int Write(byte[] p_buffer,int p_offset=0) {
            int    l = 65;
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
            MemoryMarshal.Write(b.Slice(p, 4), in Xacc               ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), in Yacc               ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), in Zacc               ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), in Xgyro              ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), in Ygyro              ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), in Zgyro              ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), in Xmag               ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), in Ymag               ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), in Zmag               ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), in AbsPressure        ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), in DiffPressure       ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), in PressureAlt        ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), in Temperature        ); p+=4;
            b[p++] = (byte)(      FieldsUpdated);
            b[p++] = (byte)((int)FieldsUpdated>>8 );
            b[p++] = (byte)((int)FieldsUpdated>>16);
            b[p++] = (byte)((int)FieldsUpdated>>24);
            b[p++] = (byte)(Id);
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
            int l = 65;
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
