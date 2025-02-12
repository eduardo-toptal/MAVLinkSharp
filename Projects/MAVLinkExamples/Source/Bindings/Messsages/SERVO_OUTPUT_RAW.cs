        
using System.IO;
using System.Runtime.InteropServices;

#pragma warning disable CS0675

namespace MAVLinkBindings {

    /// <summary>
    /// Superseded by ACTUATOR_OUTPUT_STATUS. The RAW values of the servo outputs (for RC input from the remote, use the RC_CHANNELS messages). The standard PPM modulation is as follows: 1000 microseconds: 0%, 2000 microseconds: 100%.
    /// </summary>    
    public struct ServoOutputRawData : IMAVLinkMessageData {

        /// <summary>
        /// Message Id Associated w/ this Struct
        /// </summary>    
        public int GetId() { return 36; }

        public uint    TimeUsec;       //Timestamp (UNIX Epoch time or time since system boot). The receiving end can infer timestamp format (since 1.1.1970 or since system boot) by checking for the magnitude of the number.
        public ushort  Servo1Raw;      //Servo output 1 value
        public ushort  Servo2Raw;      //Servo output 2 value
        public ushort  Servo3Raw;      //Servo output 3 value
        public ushort  Servo4Raw;      //Servo output 4 value
        public ushort  Servo5Raw;      //Servo output 5 value
        public ushort  Servo6Raw;      //Servo output 6 value
        public ushort  Servo7Raw;      //Servo output 7 value
        public ushort  Servo8Raw;      //Servo output 8 value
        public byte    Port;           //Servo output port (set of 8 outputs = 1 port). Flight stacks running on Pixhawk should use: 0 = MAIN, 1 = AUX.
        public ushort  Servo9Raw;      //Servo output 9 value
        public ushort  Servo10Raw;     //Servo output 10 value
        public ushort  Servo11Raw;     //Servo output 11 value
        public ushort  Servo12Raw;     //Servo output 12 value
        public ushort  Servo13Raw;     //Servo output 13 value
        public ushort  Servo14Raw;     //Servo output 14 value
        public ushort  Servo15Raw;     //Servo output 15 value
        public ushort  Servo16Raw;     //Servo output 16 value    

        #region CTOR
        /// <summary>
        /// Instantiates a new ServoOutputRawData
        /// </summary>    
        public ServoOutputRawData() {
            TimeUsec         = default(uint  );
            Servo1Raw        = default(ushort);
            Servo2Raw        = default(ushort);
            Servo3Raw        = default(ushort);
            Servo4Raw        = default(ushort);
            Servo5Raw        = default(ushort);
            Servo6Raw        = default(ushort);
            Servo7Raw        = default(ushort);
            Servo8Raw        = default(ushort);
            Port             = default(byte  );
            Servo9Raw        = default(ushort);
            Servo10Raw       = default(ushort);
            Servo11Raw       = default(ushort);
            Servo12Raw       = default(ushort);
            Servo13Raw       = default(ushort);
            Servo14Raw       = default(ushort);
            Servo15Raw       = default(ushort);
            Servo16Raw       = default(ushort);
        }
        #endregion

        #region Read Buffer
        /// <summary>
        /// Reads the data from Buffer into this struct
        /// </summary>    
        public int Read(byte[] p_buffer,int p_offset=0) {
            int    l = 37;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals
            int[]  LS8  = MAVLinkCRC.U8_LSH8, LS16 = MAVLinkCRC.U8_LSH16, LS24 = MAVLinkCRC.U8_LSH24, LS32 = MAVLinkCRC.U8_LSH32, LS40 = MAVLinkCRC.U8_LSH40, LS48 = MAVLinkCRC.U8_LSH48, LS56 = MAVLinkCRC.U8_LSH56;
            Span<byte> b = p_buffer.AsSpan(p_offset);            
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            TimeUsec         = (uint  ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            Servo1Raw        = (ushort) (b[p++] | LS8[b[p++]]);
            Servo2Raw        = (ushort) (b[p++] | LS8[b[p++]]);
            Servo3Raw        = (ushort) (b[p++] | LS8[b[p++]]);
            Servo4Raw        = (ushort) (b[p++] | LS8[b[p++]]);
            Servo5Raw        = (ushort) (b[p++] | LS8[b[p++]]);
            Servo6Raw        = (ushort) (b[p++] | LS8[b[p++]]);
            Servo7Raw        = (ushort) (b[p++] | LS8[b[p++]]);
            Servo8Raw        = (ushort) (b[p++] | LS8[b[p++]]);
            Port             = (byte  ) (b[p++]);
            Servo9Raw        = (ushort) (b[p++] | LS8[b[p++]]);
            Servo10Raw       = (ushort) (b[p++] | LS8[b[p++]]);
            Servo11Raw       = (ushort) (b[p++] | LS8[b[p++]]);
            Servo12Raw       = (ushort) (b[p++] | LS8[b[p++]]);
            Servo13Raw       = (ushort) (b[p++] | LS8[b[p++]]);
            Servo14Raw       = (ushort) (b[p++] | LS8[b[p++]]);
            Servo15Raw       = (ushort) (b[p++] | LS8[b[p++]]);
            Servo16Raw       = (ushort) (b[p++] | LS8[b[p++]]);            
            return p;
        }
        #endregion

        #region Write Buffer
        /// <summary>
        /// Writes the message data into a Buffer
        /// </summary>    
        public int Write(byte[] p_buffer,int p_offset=0) {
            int    l = 37;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals            
            Span<byte> b = p_buffer.AsSpan(p_offset);
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            b[p++] = (byte)(      TimeUsec);
            b[p++] = (byte)((int)TimeUsec>>8 );
            b[p++] = (byte)((int)TimeUsec>>16);
            b[p++] = (byte)((int)TimeUsec>>24);
            b[p++] = (byte)(      Servo1Raw);
            b[p++] = (byte)((int)Servo1Raw>>8 );
            b[p++] = (byte)(      Servo2Raw);
            b[p++] = (byte)((int)Servo2Raw>>8 );
            b[p++] = (byte)(      Servo3Raw);
            b[p++] = (byte)((int)Servo3Raw>>8 );
            b[p++] = (byte)(      Servo4Raw);
            b[p++] = (byte)((int)Servo4Raw>>8 );
            b[p++] = (byte)(      Servo5Raw);
            b[p++] = (byte)((int)Servo5Raw>>8 );
            b[p++] = (byte)(      Servo6Raw);
            b[p++] = (byte)((int)Servo6Raw>>8 );
            b[p++] = (byte)(      Servo7Raw);
            b[p++] = (byte)((int)Servo7Raw>>8 );
            b[p++] = (byte)(      Servo8Raw);
            b[p++] = (byte)((int)Servo8Raw>>8 );
            b[p++] = (byte)(Port);
            b[p++] = (byte)(      Servo9Raw);
            b[p++] = (byte)((int)Servo9Raw>>8 );
            b[p++] = (byte)(      Servo10Raw);
            b[p++] = (byte)((int)Servo10Raw>>8 );
            b[p++] = (byte)(      Servo11Raw);
            b[p++] = (byte)((int)Servo11Raw>>8 );
            b[p++] = (byte)(      Servo12Raw);
            b[p++] = (byte)((int)Servo12Raw>>8 );
            b[p++] = (byte)(      Servo13Raw);
            b[p++] = (byte)((int)Servo13Raw>>8 );
            b[p++] = (byte)(      Servo14Raw);
            b[p++] = (byte)((int)Servo14Raw>>8 );
            b[p++] = (byte)(      Servo15Raw);
            b[p++] = (byte)((int)Servo15Raw>>8 );
            b[p++] = (byte)(      Servo16Raw);
            b[p++] = (byte)((int)Servo16Raw>>8 );
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
            int l = 37;
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
