        
using System.IO;
using System.Runtime.InteropServices;

#pragma warning disable CS0675

namespace MAVLinkBindings {

    /// <summary>
    /// Control a serial port. This can be used for raw access to an onboard serial peripheral such as a GPS or telemetry radio. It is designed to make it possible to update the devices firmware via MAVLink messages or change the devices settings. A message with zero bytes can be used to change just the baudrate.
    /// </summary>    
    public struct SerialControlData : IMAVLinkMessageData {

        /// <summary>
        /// Message Id Associated w/ this Struct
        /// </summary>    
        public int GetId() { return 126; }

        public uint                   Baudrate;            //Baudrate of transfer. Zero means no change.
        public ushort                 Timeout;             //Timeout for reply data
        public SerialControlDevFlags  Device;              //Serial control device type.
        public SerialControlFlag      Flags;               //Bitmap of serial control flags.
        public byte                   Count;               //how many bytes in this transfer
        public byte[]                 Data;                //serial data
        public byte                   TargetSystem;        //System ID
        public byte                   TargetComponent;     //Component ID    

        #region CTOR
        /// <summary>
        /// Instantiates a new SerialControlData
        /// </summary>    
        public SerialControlData() {
            Baudrate              = default(uint                 );
            Timeout               = default(ushort               );
            Device                = default(SerialControlDevFlags);
            Flags                 = default(SerialControlFlag    );
            Count                 = default(byte                 );
            Data                  = new byte[ 70];
            TargetSystem          = default(byte                 );
            TargetComponent       = default(byte                 );
        }
        #endregion

        #region Read Buffer
        /// <summary>
        /// Reads the data from Buffer into this struct
        /// </summary>    
        public int Read(byte[] p_buffer,int p_offset=0) {
            int    l = 81;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals
            int[]  LS8  = MAVLinkCRC.U8_LSH8, LS16 = MAVLinkCRC.U8_LSH16, LS24 = MAVLinkCRC.U8_LSH24, LS32 = MAVLinkCRC.U8_LSH32, LS40 = MAVLinkCRC.U8_LSH40, LS48 = MAVLinkCRC.U8_LSH48, LS56 = MAVLinkCRC.U8_LSH56;
            Span<byte> b = p_buffer.AsSpan(p_offset);            
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            Baudrate              = (uint                 ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            Timeout               = (ushort               ) (b[p++] | LS8[b[p++]]);
            Device                = (SerialControlDevFlags) (b[p++]);
            Flags                 = (SerialControlFlag    ) (b[p++]);
            Count                 = (byte                 ) (b[p++]);
            for(int i=0;i<70 ;i++) { Data[i]               = (byte                 ) (b[p++]); }
            TargetSystem          = (byte                 ) (b[p++]);
            TargetComponent       = (byte                 ) (b[p++]);            
            return p;
        }
        #endregion

        #region Write Buffer
        /// <summary>
        /// Writes the message data into a Buffer
        /// </summary>    
        public int Write(byte[] p_buffer,int p_offset=0) {
            int    l = 81;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals            
            Span<byte> b = p_buffer.AsSpan(p_offset);
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            b[p++] = (byte)(      Baudrate);
            b[p++] = (byte)((int)Baudrate>>8 );
            b[p++] = (byte)((int)Baudrate>>16);
            b[p++] = (byte)((int)Baudrate>>24);
            b[p++] = (byte)(      Timeout);
            b[p++] = (byte)((int)Timeout>>8 );
            b[p++] = (byte)(Device);
            b[p++] = (byte)(Flags);
            b[p++] = (byte)(Count);
            for(int i=0;i< 70;i++) {
                b[p++] = (byte)(Data[i]);
            }
            b[p++] = (byte)(TargetSystem);
            b[p++] = (byte)(TargetComponent);
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
            int l = 81;
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
