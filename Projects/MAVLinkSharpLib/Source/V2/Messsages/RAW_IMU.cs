        
using System.IO;
using System.Runtime.InteropServices;
using MAVLinkSharp.Runtime;

#pragma warning disable CS0675

namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// The RAW IMU readings for a 9DOF sensor, which is identified by the id (default IMU1). This message should always contain the true raw values without any scaling to allow data capture and system debugging.
    /// </summary>    
    public struct RawImuData : IMAVLinkMessageData {

        /// <summary>
        /// Message Id Associated w/ this Struct
        /// </summary>    
        public int GetId() { return 27; }

        public ulong  TimeUsec;       //Timestamp (UNIX Epoch time or time since system boot). The receiving end can infer timestamp format (since 1.1.1970 or since system boot) by checking for the magnitude of the number.
        public short  Xacc;           //X acceleration (raw)
        public short  Yacc;           //Y acceleration (raw)
        public short  Zacc;           //Z acceleration (raw)
        public short  Xgyro;          //Angular speed around X axis (raw)
        public short  Ygyro;          //Angular speed around Y axis (raw)
        public short  Zgyro;          //Angular speed around Z axis (raw)
        public short  Xmag;           //X Magnetic field (raw)
        public short  Ymag;           //Y Magnetic field (raw)
        public short  Zmag;           //Z Magnetic field (raw)
        public byte   Id;             //Id. Ids are numbered from 0 and map to IMUs numbered from 1 (e.g. IMU1 will have a message with id=0)
        public short  Temperature;    //Temperature, 0: IMU does not provide temperature values. If the IMU is at 0C it must send 1 (0.01C).    

        #region CTOR
        /// <summary>
        /// Instantiates a new RawImuData
        /// </summary>    
        /*
        public RawImuData() {
            Init();
        }
        */
        public void Init() {
            TimeUsec         = default(ulong);
            Xacc             = default(short);
            Yacc             = default(short);
            Zacc             = default(short);
            Xgyro            = default(short);
            Ygyro            = default(short);
            Zgyro            = default(short);
            Xmag             = default(short);
            Ymag             = default(short);
            Zmag             = default(short);
            Id               = default(byte );
            Temperature      = default(short);
        }
        #endregion

        #region Read Buffer
        /// <summary>
        /// Reads the data from Buffer into this struct
        /// </summary>    
        public int Read(byte[] p_buffer,int p_offset=0) {
            int    l = 29;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals
            int[]  LS8  = MAVLinkCRC.U8_LSH8, LS16 = MAVLinkCRC.U8_LSH16, LS24 = MAVLinkCRC.U8_LSH24, LS32 = MAVLinkCRC.U8_LSH32, LS40 = MAVLinkCRC.U8_LSH40, LS48 = MAVLinkCRC.U8_LSH48, LS56 = MAVLinkCRC.U8_LSH56;
            Span<byte> b = p_buffer.AsSpan(p_offset);            
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            TimeUsec         = (ulong) ((ulong)b[p++] | (ulong)LS8[b[p++]] | (ulong)LS16[b[p++]] | (ulong)LS24[b[p++]] | (ulong)LS32[b[p++]] | (ulong)LS40[b[p++]] | (ulong)LS48[b[p++]] | (ulong)LS56[b[p++]]);
            Xacc             = (short) (b[p++] | LS8[b[p++]]);
            Yacc             = (short) (b[p++] | LS8[b[p++]]);
            Zacc             = (short) (b[p++] | LS8[b[p++]]);
            Xgyro            = (short) (b[p++] | LS8[b[p++]]);
            Ygyro            = (short) (b[p++] | LS8[b[p++]]);
            Zgyro            = (short) (b[p++] | LS8[b[p++]]);
            Xmag             = (short) (b[p++] | LS8[b[p++]]);
            Ymag             = (short) (b[p++] | LS8[b[p++]]);
            Zmag             = (short) (b[p++] | LS8[b[p++]]);
            Id               = (byte ) (b[p++]);
            Temperature      = (short) (b[p++] | LS8[b[p++]]);            
            return p;
        }
        #endregion

        #region Write Buffer
        /// <summary>
        /// Writes the message data into a Buffer
        /// </summary>    
        public int Write(byte[] p_buffer,int p_offset=0) {
            int    l = 29;
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
            b[p++] = (byte)(      Xacc);
            b[p++] = (byte)((int)Xacc>>8 );
            b[p++] = (byte)(      Yacc);
            b[p++] = (byte)((int)Yacc>>8 );
            b[p++] = (byte)(      Zacc);
            b[p++] = (byte)((int)Zacc>>8 );
            b[p++] = (byte)(      Xgyro);
            b[p++] = (byte)((int)Xgyro>>8 );
            b[p++] = (byte)(      Ygyro);
            b[p++] = (byte)((int)Ygyro>>8 );
            b[p++] = (byte)(      Zgyro);
            b[p++] = (byte)((int)Zgyro>>8 );
            b[p++] = (byte)(      Xmag);
            b[p++] = (byte)((int)Xmag>>8 );
            b[p++] = (byte)(      Ymag);
            b[p++] = (byte)((int)Ymag>>8 );
            b[p++] = (byte)(      Zmag);
            b[p++] = (byte)((int)Zmag>>8 );
            b[p++] = (byte)(Id);
            b[p++] = (byte)(      Temperature);
            b[p++] = (byte)((int)Temperature>>8 );
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
            int l = 29;
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
