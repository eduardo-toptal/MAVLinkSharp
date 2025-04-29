        
using System;
using System.IO;
using System.Runtime.InteropServices;
using MAVLinkSharp.Runtime;

#pragma warning disable CS0675

namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// The RAW IMU readings for the usual 9DOF sensor setup. This message should contain the scaled values to the described units
    /// </summary>    
    public struct ScaledImuData : IMAVLinkMessageData {

        /// <summary>
        /// Message Id Associated w/ this Struct
        /// </summary>    
        public int GetId() { return 26; }

        public uint   TimeBootMs;      //Timestamp (time since system boot).
        public short  Xacc;            //X acceleration
        public short  Yacc;            //Y acceleration
        public short  Zacc;            //Z acceleration
        public short  Xgyro;           //Angular speed around X axis
        public short  Ygyro;           //Angular speed around Y axis
        public short  Zgyro;           //Angular speed around Z axis
        public short  Xmag;            //X Magnetic field
        public short  Ymag;            //Y Magnetic field
        public short  Zmag;            //Z Magnetic field
        public short  Temperature;     //Temperature, 0: IMU does not provide temperature values. If the IMU is at 0C it must send 1 (0.01C).    

        #region CTOR
        /// <summary>
        /// Instantiates a new ScaledImuData
        /// </summary>    
        /*
        public ScaledImuData() {
            Init();
        }
        */
        public void Init() {
            TimeBootMs        = default(uint );
            Xacc              = default(short);
            Yacc              = default(short);
            Zacc              = default(short);
            Xgyro             = default(short);
            Ygyro             = default(short);
            Zgyro             = default(short);
            Xmag              = default(short);
            Ymag              = default(short);
            Zmag              = default(short);
            Temperature       = default(short);
        }
        #endregion

        #region Read Buffer
        /// <summary>
        /// Reads the data from Buffer into this struct
        /// </summary>    
        public int Read(byte[] p_buffer,int p_offset=0) {
            int    l = 24;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals
            int[]  LS8  = MAVLinkCRC.U8_LSH8, LS16 = MAVLinkCRC.U8_LSH16, LS24 = MAVLinkCRC.U8_LSH24, LS32 = MAVLinkCRC.U8_LSH32, LS40 = MAVLinkCRC.U8_LSH40, LS48 = MAVLinkCRC.U8_LSH48, LS56 = MAVLinkCRC.U8_LSH56;
            Span<byte> b = p_buffer.AsSpan(p_offset);            
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            TimeBootMs        = (uint ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            Xacc              = (short) (b[p++] | LS8[b[p++]]);
            Yacc              = (short) (b[p++] | LS8[b[p++]]);
            Zacc              = (short) (b[p++] | LS8[b[p++]]);
            Xgyro             = (short) (b[p++] | LS8[b[p++]]);
            Ygyro             = (short) (b[p++] | LS8[b[p++]]);
            Zgyro             = (short) (b[p++] | LS8[b[p++]]);
            Xmag              = (short) (b[p++] | LS8[b[p++]]);
            Ymag              = (short) (b[p++] | LS8[b[p++]]);
            Zmag              = (short) (b[p++] | LS8[b[p++]]);
            Temperature       = (short) (b[p++] | LS8[b[p++]]);            
            return p;
        }
        #endregion

        #region Write Buffer
        /// <summary>
        /// Writes the message data into a Buffer
        /// </summary>    
        public int Write(byte[] p_buffer,int p_offset=0) {
            int    l = 24;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals            
            Span<byte> b = p_buffer.AsSpan(p_offset);
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            b[p++] = (byte)(      TimeBootMs);
            b[p++] = (byte)((int)TimeBootMs>>8 );
            b[p++] = (byte)((int)TimeBootMs>>16);
            b[p++] = (byte)((int)TimeBootMs>>24);
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
            int l = 24;
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
