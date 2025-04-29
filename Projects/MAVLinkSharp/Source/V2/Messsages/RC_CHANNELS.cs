        
using System.IO;
using System.Runtime.InteropServices;
using MAVLinkSharp.Runtime;

#pragma warning disable CS0675

namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// The PPM values of the RC channels received. The standard PPM modulation is as follows: 1000 microseconds: 0%, 2000 microseconds: 100%.  A value of UINT16_MAX implies the channel is unused. Individual receivers/transmitters might violate this specification.
    /// </summary>    
    public struct RcChannelsData : IMAVLinkMessageData {

        /// <summary>
        /// Message Id Associated w/ this Struct
        /// </summary>    
        public int GetId() { return 65; }

        public uint    TimeBootMs;      //Timestamp (time since system boot).
        public ushort  Chan1Raw;        //RC channel 1 value.
        public ushort  Chan2Raw;        //RC channel 2 value.
        public ushort  Chan3Raw;        //RC channel 3 value.
        public ushort  Chan4Raw;        //RC channel 4 value.
        public ushort  Chan5Raw;        //RC channel 5 value.
        public ushort  Chan6Raw;        //RC channel 6 value.
        public ushort  Chan7Raw;        //RC channel 7 value.
        public ushort  Chan8Raw;        //RC channel 8 value.
        public ushort  Chan9Raw;        //RC channel 9 value.
        public ushort  Chan10Raw;       //RC channel 10 value.
        public ushort  Chan11Raw;       //RC channel 11 value.
        public ushort  Chan12Raw;       //RC channel 12 value.
        public ushort  Chan13Raw;       //RC channel 13 value.
        public ushort  Chan14Raw;       //RC channel 14 value.
        public ushort  Chan15Raw;       //RC channel 15 value.
        public ushort  Chan16Raw;       //RC channel 16 value.
        public ushort  Chan17Raw;       //RC channel 17 value.
        public ushort  Chan18Raw;       //RC channel 18 value.
        public byte    Chancount;       //Total number of RC channels being received. This can be larger than 18, indicating that more channels are available but not given in this message. This value should be 0 when no RC channels are available.
        public byte    Rssi;            //Receive signal strength indicator in device-dependent units/scale. Values: [0-254], UINT8_MAX: invalid/unknown.    

        #region CTOR
        /// <summary>
        /// Instantiates a new RcChannelsData
        /// </summary>    
        public RcChannelsData() {
            TimeBootMs        = default(uint  );
            Chan1Raw          = default(ushort);
            Chan2Raw          = default(ushort);
            Chan3Raw          = default(ushort);
            Chan4Raw          = default(ushort);
            Chan5Raw          = default(ushort);
            Chan6Raw          = default(ushort);
            Chan7Raw          = default(ushort);
            Chan8Raw          = default(ushort);
            Chan9Raw          = default(ushort);
            Chan10Raw         = default(ushort);
            Chan11Raw         = default(ushort);
            Chan12Raw         = default(ushort);
            Chan13Raw         = default(ushort);
            Chan14Raw         = default(ushort);
            Chan15Raw         = default(ushort);
            Chan16Raw         = default(ushort);
            Chan17Raw         = default(ushort);
            Chan18Raw         = default(ushort);
            Chancount         = default(byte  );
            Rssi              = default(byte  );
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
            TimeBootMs        = (uint  ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            Chan1Raw          = (ushort) (b[p++] | LS8[b[p++]]);
            Chan2Raw          = (ushort) (b[p++] | LS8[b[p++]]);
            Chan3Raw          = (ushort) (b[p++] | LS8[b[p++]]);
            Chan4Raw          = (ushort) (b[p++] | LS8[b[p++]]);
            Chan5Raw          = (ushort) (b[p++] | LS8[b[p++]]);
            Chan6Raw          = (ushort) (b[p++] | LS8[b[p++]]);
            Chan7Raw          = (ushort) (b[p++] | LS8[b[p++]]);
            Chan8Raw          = (ushort) (b[p++] | LS8[b[p++]]);
            Chan9Raw          = (ushort) (b[p++] | LS8[b[p++]]);
            Chan10Raw         = (ushort) (b[p++] | LS8[b[p++]]);
            Chan11Raw         = (ushort) (b[p++] | LS8[b[p++]]);
            Chan12Raw         = (ushort) (b[p++] | LS8[b[p++]]);
            Chan13Raw         = (ushort) (b[p++] | LS8[b[p++]]);
            Chan14Raw         = (ushort) (b[p++] | LS8[b[p++]]);
            Chan15Raw         = (ushort) (b[p++] | LS8[b[p++]]);
            Chan16Raw         = (ushort) (b[p++] | LS8[b[p++]]);
            Chan17Raw         = (ushort) (b[p++] | LS8[b[p++]]);
            Chan18Raw         = (ushort) (b[p++] | LS8[b[p++]]);
            Chancount         = (byte  ) (b[p++]);
            Rssi              = (byte  ) (b[p++]);            
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
            b[p++] = (byte)(      TimeBootMs);
            b[p++] = (byte)((int)TimeBootMs>>8 );
            b[p++] = (byte)((int)TimeBootMs>>16);
            b[p++] = (byte)((int)TimeBootMs>>24);
            b[p++] = (byte)(      Chan1Raw);
            b[p++] = (byte)((int)Chan1Raw>>8 );
            b[p++] = (byte)(      Chan2Raw);
            b[p++] = (byte)((int)Chan2Raw>>8 );
            b[p++] = (byte)(      Chan3Raw);
            b[p++] = (byte)((int)Chan3Raw>>8 );
            b[p++] = (byte)(      Chan4Raw);
            b[p++] = (byte)((int)Chan4Raw>>8 );
            b[p++] = (byte)(      Chan5Raw);
            b[p++] = (byte)((int)Chan5Raw>>8 );
            b[p++] = (byte)(      Chan6Raw);
            b[p++] = (byte)((int)Chan6Raw>>8 );
            b[p++] = (byte)(      Chan7Raw);
            b[p++] = (byte)((int)Chan7Raw>>8 );
            b[p++] = (byte)(      Chan8Raw);
            b[p++] = (byte)((int)Chan8Raw>>8 );
            b[p++] = (byte)(      Chan9Raw);
            b[p++] = (byte)((int)Chan9Raw>>8 );
            b[p++] = (byte)(      Chan10Raw);
            b[p++] = (byte)((int)Chan10Raw>>8 );
            b[p++] = (byte)(      Chan11Raw);
            b[p++] = (byte)((int)Chan11Raw>>8 );
            b[p++] = (byte)(      Chan12Raw);
            b[p++] = (byte)((int)Chan12Raw>>8 );
            b[p++] = (byte)(      Chan13Raw);
            b[p++] = (byte)((int)Chan13Raw>>8 );
            b[p++] = (byte)(      Chan14Raw);
            b[p++] = (byte)((int)Chan14Raw>>8 );
            b[p++] = (byte)(      Chan15Raw);
            b[p++] = (byte)((int)Chan15Raw>>8 );
            b[p++] = (byte)(      Chan16Raw);
            b[p++] = (byte)((int)Chan16Raw>>8 );
            b[p++] = (byte)(      Chan17Raw);
            b[p++] = (byte)((int)Chan17Raw>>8 );
            b[p++] = (byte)(      Chan18Raw);
            b[p++] = (byte)((int)Chan18Raw>>8 );
            b[p++] = (byte)(Chancount);
            b[p++] = (byte)(Rssi);
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
