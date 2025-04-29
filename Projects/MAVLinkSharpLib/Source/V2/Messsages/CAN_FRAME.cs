        
using System;
using System.IO;
using System.Runtime.InteropServices;
using MAVLinkSharp.Runtime;

#pragma warning disable CS0675

namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// A forwarded CAN frame as requested by MAV_CMD_CAN_FORWARD.
    /// </summary>    
    public struct CanFrameData : IMAVLinkMessageData {

        /// <summary>
        /// Message Id Associated w/ this Struct
        /// </summary>    
        public int GetId() { return 386; }

        public uint    Id;                  //Frame ID
        public byte    TargetSystem;        //System ID.
        public byte    TargetComponent;     //Component ID.
        public byte    Bus;                 //Bus number
        public byte    Len;                 //Frame length
        public byte[]  Data;                //Frame data    

        #region CTOR
        /// <summary>
        /// Instantiates a new CanFrameData
        /// </summary>    
        /*
        public CanFrameData() {
            Init();
        }
        */
        public void Init() {
            Id                    = default(uint);
            TargetSystem          = default(byte);
            TargetComponent       = default(byte);
            Bus                   = default(byte);
            Len                   = default(byte);
            Data                  = new byte[  8];
        }
        #endregion

        #region Read Buffer
        /// <summary>
        /// Reads the data from Buffer into this struct
        /// </summary>    
        public int Read(byte[] p_buffer,int p_offset=0) {
            int    l = 16;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals
            int[]  LS8  = MAVLinkCRC.U8_LSH8, LS16 = MAVLinkCRC.U8_LSH16, LS24 = MAVLinkCRC.U8_LSH24, LS32 = MAVLinkCRC.U8_LSH32, LS40 = MAVLinkCRC.U8_LSH40, LS48 = MAVLinkCRC.U8_LSH48, LS56 = MAVLinkCRC.U8_LSH56;
            Span<byte> b = p_buffer.AsSpan(p_offset);            
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            Id                    = (uint) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            TargetSystem          = (byte) (b[p++]);
            TargetComponent       = (byte) (b[p++]);
            Bus                   = (byte) (b[p++]);
            Len                   = (byte) (b[p++]);
            for(int i=0;i<8  ;i++) { Data[i]               = (byte) (b[p++]); }            
            return p;
        }
        #endregion

        #region Write Buffer
        /// <summary>
        /// Writes the message data into a Buffer
        /// </summary>    
        public int Write(byte[] p_buffer,int p_offset=0) {
            int    l = 16;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals            
            Span<byte> b = p_buffer.AsSpan(p_offset);
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            b[p++] = (byte)(      Id);
            b[p++] = (byte)((int)Id>>8 );
            b[p++] = (byte)((int)Id>>16);
            b[p++] = (byte)((int)Id>>24);
            b[p++] = (byte)(TargetSystem);
            b[p++] = (byte)(TargetComponent);
            b[p++] = (byte)(Bus);
            b[p++] = (byte)(Len);
            for(int i=0;i<  8;i++) {
                b[p++] = (byte)(Data[i]);
            }
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
            int l = 16;
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
