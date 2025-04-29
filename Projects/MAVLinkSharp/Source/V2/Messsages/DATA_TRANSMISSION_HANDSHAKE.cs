        
using System.IO;
using System.Runtime.InteropServices;
using MAVLinkSharp.Runtime;

#pragma warning disable CS0675

namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// Handshake message to initiate, control and stop image streaming when using the Image Transmission Protocol: https://mavlink.io/en/services/image_transmission.html.
    /// </summary>    
    public struct DataTransmissionHandshakeData : IMAVLinkMessageData {

        /// <summary>
        /// Message Id Associated w/ this Struct
        /// </summary>    
        public int GetId() { return 130; }

        public uint                        Size;           //total data size (set on ACK only).
        public ushort                      Width;          //Width of a matrix or image.
        public ushort                      Height;         //Height of a matrix or image.
        public ushort                      Packets;        //Number of packets being sent (set on ACK only).
        public MAVLinkDataStreamTypeFlags  Type;           //Type of requested/acknowledged data.
        public byte                        Payload;        //Payload size per packet (normally 253 byte, see DATA field size in message ENCAPSULATED_DATA) (set on ACK only).
        public byte                        JpgQuality;     //JPEG quality. Values: [1-100].    

        #region CTOR
        /// <summary>
        /// Instantiates a new DataTransmissionHandshakeData
        /// </summary>    
        public DataTransmissionHandshakeData() {
            Size             = default(uint                      );
            Width            = default(ushort                    );
            Height           = default(ushort                    );
            Packets          = default(ushort                    );
            Type             = default(MAVLinkDataStreamTypeFlags);
            Payload          = default(byte                      );
            JpgQuality       = default(byte                      );
        }
        #endregion

        #region Read Buffer
        /// <summary>
        /// Reads the data from Buffer into this struct
        /// </summary>    
        public int Read(byte[] p_buffer,int p_offset=0) {
            int    l = 13;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals
            int[]  LS8  = MAVLinkCRC.U8_LSH8, LS16 = MAVLinkCRC.U8_LSH16, LS24 = MAVLinkCRC.U8_LSH24, LS32 = MAVLinkCRC.U8_LSH32, LS40 = MAVLinkCRC.U8_LSH40, LS48 = MAVLinkCRC.U8_LSH48, LS56 = MAVLinkCRC.U8_LSH56;
            Span<byte> b = p_buffer.AsSpan(p_offset);            
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            Size             = (uint                      ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            Width            = (ushort                    ) (b[p++] | LS8[b[p++]]);
            Height           = (ushort                    ) (b[p++] | LS8[b[p++]]);
            Packets          = (ushort                    ) (b[p++] | LS8[b[p++]]);
            Type             = (MAVLinkDataStreamTypeFlags) (b[p++]);
            Payload          = (byte                      ) (b[p++]);
            JpgQuality       = (byte                      ) (b[p++]);            
            return p;
        }
        #endregion

        #region Write Buffer
        /// <summary>
        /// Writes the message data into a Buffer
        /// </summary>    
        public int Write(byte[] p_buffer,int p_offset=0) {
            int    l = 13;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals            
            Span<byte> b = p_buffer.AsSpan(p_offset);
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            b[p++] = (byte)(      Size);
            b[p++] = (byte)((int)Size>>8 );
            b[p++] = (byte)((int)Size>>16);
            b[p++] = (byte)((int)Size>>24);
            b[p++] = (byte)(      Width);
            b[p++] = (byte)((int)Width>>8 );
            b[p++] = (byte)(      Height);
            b[p++] = (byte)((int)Height>>8 );
            b[p++] = (byte)(      Packets);
            b[p++] = (byte)((int)Packets>>8 );
            b[p++] = (byte)(Type);
            b[p++] = (byte)(Payload);
            b[p++] = (byte)(JpgQuality);
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
            int l = 13;
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
