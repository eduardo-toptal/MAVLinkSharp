        
using System;
using System.IO;
using System.Runtime.InteropServices;
using MAVLinkSharp.Runtime;

#pragma warning disable CS0675

namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// The autopilot is requesting a resource (file, binary, other type of data)
    /// </summary>    
    public struct ResourceRequestData : IMAVLinkMessageData {

        /// <summary>
        /// Message Id Associated w/ this Struct
        /// </summary>    
        public int GetId() { return 142; }

        public byte    RequestId;        //Request ID. This ID should be reused when sending back URI contents
        public byte    UriType;          //The type of requested URI. 0 = a file via URL. 1 = a UAVCAN binary
        public byte[]  Uri;              //The requested unique resource identifier (URI). It is not necessarily a straight domain name (depends on the URI type enum)
        public byte    TransferType;     //The way the autopilot wants to receive the URI. 0 = MAVLink FTP. 1 = binary stream.
        public byte[]  Storage;          //The storage path the autopilot wants the URI to be stored in. Will only be valid if the transfer_type has a storage associated (e.g. MAVLink FTP).    

        #region CTOR
        /// <summary>
        /// Instantiates a new ResourceRequestData
        /// </summary>    
        /*
        public ResourceRequestData() {
            Init();
        }
        */
        public void Init() {
            RequestId          = default(byte);
            UriType            = default(byte);
            Uri                = new byte[120];
            TransferType       = default(byte);
            Storage            = new byte[120];
        }
        #endregion

        #region Read Buffer
        /// <summary>
        /// Reads the data from Buffer into this struct
        /// </summary>    
        public int Read(byte[] p_buffer,int p_offset=0) {
            int    l = 243;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals
            int[]  LS8  = MAVLinkCRC.U8_LSH8, LS16 = MAVLinkCRC.U8_LSH16, LS24 = MAVLinkCRC.U8_LSH24, LS32 = MAVLinkCRC.U8_LSH32, LS40 = MAVLinkCRC.U8_LSH40, LS48 = MAVLinkCRC.U8_LSH48, LS56 = MAVLinkCRC.U8_LSH56;
            Span<byte> b = p_buffer.AsSpan(p_offset);            
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            RequestId          = (byte) (b[p++]);
            UriType            = (byte) (b[p++]);
            for(int i=0;i<120;i++) { Uri[i]             = (byte) (b[p++]); }
            TransferType       = (byte) (b[p++]);
            for(int i=0;i<120;i++) { Storage[i]         = (byte) (b[p++]); }            
            return p;
        }
        #endregion

        #region Write Buffer
        /// <summary>
        /// Writes the message data into a Buffer
        /// </summary>    
        public int Write(byte[] p_buffer,int p_offset=0) {
            int    l = 243;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals            
            Span<byte> b = p_buffer.AsSpan(p_offset);
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            b[p++] = (byte)(RequestId);
            b[p++] = (byte)(UriType);
            for(int i=0;i<120;i++) {
                b[p++] = (byte)(Uri[i]);
            }
            b[p++] = (byte)(TransferType);
            for(int i=0;i<120;i++) {
                b[p++] = (byte)(Storage[i]);
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
            int l = 243;
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
