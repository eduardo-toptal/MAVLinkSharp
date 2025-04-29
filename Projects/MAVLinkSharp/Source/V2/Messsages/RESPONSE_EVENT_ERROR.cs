        
using System.IO;
using System.Runtime.InteropServices;
using MAVLinkSharp.Runtime;

#pragma warning disable CS0675

namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// Response to a REQUEST_EVENT in case of an error (e.g. the event is not available anymore).
    /// </summary>    
    public struct ResponseEventErrorData : IMAVLinkMessageData {

        /// <summary>
        /// Message Id Associated w/ this Struct
        /// </summary>    
        public int GetId() { return 413; }

        public ushort                    Sequence;                     //Sequence number.
        public ushort                    SequenceOldestAvailable;      //Oldest Sequence number that is still available after the sequence set in REQUEST_EVENT.
        public byte                      TargetSystem;                 //System ID
        public byte                      TargetComponent;              //Component ID
        public MAVEventErrorReasonFlags  Reason;                       //Error reason.    

        #region CTOR
        /// <summary>
        /// Instantiates a new ResponseEventErrorData
        /// </summary>    
        public ResponseEventErrorData() {
            Sequence                       = default(ushort                  );
            SequenceOldestAvailable        = default(ushort                  );
            TargetSystem                   = default(byte                    );
            TargetComponent                = default(byte                    );
            Reason                         = default(MAVEventErrorReasonFlags);
        }
        #endregion

        #region Read Buffer
        /// <summary>
        /// Reads the data from Buffer into this struct
        /// </summary>    
        public int Read(byte[] p_buffer,int p_offset=0) {
            int    l = 7;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals
            int[]  LS8  = MAVLinkCRC.U8_LSH8, LS16 = MAVLinkCRC.U8_LSH16, LS24 = MAVLinkCRC.U8_LSH24, LS32 = MAVLinkCRC.U8_LSH32, LS40 = MAVLinkCRC.U8_LSH40, LS48 = MAVLinkCRC.U8_LSH48, LS56 = MAVLinkCRC.U8_LSH56;
            Span<byte> b = p_buffer.AsSpan(p_offset);            
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            Sequence                       = (ushort                  ) (b[p++] | LS8[b[p++]]);
            SequenceOldestAvailable        = (ushort                  ) (b[p++] | LS8[b[p++]]);
            TargetSystem                   = (byte                    ) (b[p++]);
            TargetComponent                = (byte                    ) (b[p++]);
            Reason                         = (MAVEventErrorReasonFlags) (b[p++]);            
            return p;
        }
        #endregion

        #region Write Buffer
        /// <summary>
        /// Writes the message data into a Buffer
        /// </summary>    
        public int Write(byte[] p_buffer,int p_offset=0) {
            int    l = 7;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals            
            Span<byte> b = p_buffer.AsSpan(p_offset);
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            b[p++] = (byte)(      Sequence);
            b[p++] = (byte)((int)Sequence>>8 );
            b[p++] = (byte)(      SequenceOldestAvailable);
            b[p++] = (byte)((int)SequenceOldestAvailable>>8 );
            b[p++] = (byte)(TargetSystem);
            b[p++] = (byte)(TargetComponent);
            b[p++] = (byte)(Reason);
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
            int l = 7;
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
