        
using System.IO;
using System.Runtime.InteropServices;

#pragma warning disable CS0675

namespace MAVLinkBindings {

    /// <summary>
    /// The scaled values of the RC channels received: (-100%) -10000, (0%) 0, (100%) 10000. Channels that are inactive should be set to UINT16_MAX.
    /// </summary>    
    public struct RcChannelsScaledData : IMAVLinkMessageData {

        /// <summary>
        /// Message Id Associated w/ this Struct
        /// </summary>    
        public int GetId() { return 34; }

        public uint   TimeBootMs;      //Timestamp (time since system boot).
        public short  Chan1Scaled;     //RC channel 1 value scaled.
        public short  Chan2Scaled;     //RC channel 2 value scaled.
        public short  Chan3Scaled;     //RC channel 3 value scaled.
        public short  Chan4Scaled;     //RC channel 4 value scaled.
        public short  Chan5Scaled;     //RC channel 5 value scaled.
        public short  Chan6Scaled;     //RC channel 6 value scaled.
        public short  Chan7Scaled;     //RC channel 7 value scaled.
        public short  Chan8Scaled;     //RC channel 8 value scaled.
        public byte   Port;            //Servo output port (set of 8 outputs = 1 port). Flight stacks running on Pixhawk should use: 0 = MAIN, 1 = AUX.
        public byte   Rssi;            //Receive signal strength indicator in device-dependent units/scale. Values: [0-254], UINT8_MAX: invalid/unknown.    

        #region CTOR
        /// <summary>
        /// Instantiates a new RcChannelsScaledData
        /// </summary>    
        public RcChannelsScaledData() {
            TimeBootMs        = default(uint );
            Chan1Scaled       = default(short);
            Chan2Scaled       = default(short);
            Chan3Scaled       = default(short);
            Chan4Scaled       = default(short);
            Chan5Scaled       = default(short);
            Chan6Scaled       = default(short);
            Chan7Scaled       = default(short);
            Chan8Scaled       = default(short);
            Port              = default(byte );
            Rssi              = default(byte );
        }
        #endregion

        #region Read Buffer
        /// <summary>
        /// Reads the data from Buffer into this struct
        /// </summary>    
        public int Read(byte[] p_buffer,int p_offset=0) {
            int    l = 22;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals
            int[]  LS8  = MAVLinkCRC.U8_LSH8, LS16 = MAVLinkCRC.U8_LSH16, LS24 = MAVLinkCRC.U8_LSH24, LS32 = MAVLinkCRC.U8_LSH32, LS40 = MAVLinkCRC.U8_LSH40, LS48 = MAVLinkCRC.U8_LSH48, LS56 = MAVLinkCRC.U8_LSH56;
            Span<byte> b = p_buffer.AsSpan(p_offset);            
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            TimeBootMs        = (uint ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            Chan1Scaled       = (short) (b[p++] | LS8[b[p++]]);
            Chan2Scaled       = (short) (b[p++] | LS8[b[p++]]);
            Chan3Scaled       = (short) (b[p++] | LS8[b[p++]]);
            Chan4Scaled       = (short) (b[p++] | LS8[b[p++]]);
            Chan5Scaled       = (short) (b[p++] | LS8[b[p++]]);
            Chan6Scaled       = (short) (b[p++] | LS8[b[p++]]);
            Chan7Scaled       = (short) (b[p++] | LS8[b[p++]]);
            Chan8Scaled       = (short) (b[p++] | LS8[b[p++]]);
            Port              = (byte ) (b[p++]);
            Rssi              = (byte ) (b[p++]);            
            return p;
        }
        #endregion

        #region Write Buffer
        /// <summary>
        /// Writes the message data into a Buffer
        /// </summary>    
        public int Write(byte[] p_buffer,int p_offset=0) {
            int    l = 22;
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
            b[p++] = (byte)(      Chan1Scaled);
            b[p++] = (byte)((int)Chan1Scaled>>8 );
            b[p++] = (byte)(      Chan2Scaled);
            b[p++] = (byte)((int)Chan2Scaled>>8 );
            b[p++] = (byte)(      Chan3Scaled);
            b[p++] = (byte)((int)Chan3Scaled>>8 );
            b[p++] = (byte)(      Chan4Scaled);
            b[p++] = (byte)((int)Chan4Scaled>>8 );
            b[p++] = (byte)(      Chan5Scaled);
            b[p++] = (byte)((int)Chan5Scaled>>8 );
            b[p++] = (byte)(      Chan6Scaled);
            b[p++] = (byte)((int)Chan6Scaled>>8 );
            b[p++] = (byte)(      Chan7Scaled);
            b[p++] = (byte)((int)Chan7Scaled>>8 );
            b[p++] = (byte)(      Chan8Scaled);
            b[p++] = (byte)((int)Chan8Scaled>>8 );
            b[p++] = (byte)(Port);
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
            int l = 22;
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
