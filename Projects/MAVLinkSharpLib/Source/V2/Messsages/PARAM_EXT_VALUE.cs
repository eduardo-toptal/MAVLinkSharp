        
using System.IO;
using System.Runtime.InteropServices;
using MAVLinkSharp.Runtime;

#pragma warning disable CS0675

namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// Emit the value of a parameter. The inclusion of param_count and param_index in the message allows the recipient to keep track of received parameters and allows them to re-request missing parameters after a loss or timeout.
    /// </summary>    
    public struct ParamExtValueData : IMAVLinkMessageData {

        /// <summary>
        /// Message Id Associated w/ this Struct
        /// </summary>    
        public int GetId() { return 322; }

        public ushort                ParamCount;     //Total number of parameters
        public ushort                ParamIndex;     //Index of this parameter
        public char[]                ParamId;        //Parameter id, terminated by NULL if the length is less than 16 human-readable chars and WITHOUT null termination (NULL) byte if the length is exactly 16 chars - applications have to provide 16+1 bytes storage if the ID is stored as string
        public char[]                ParamValue;     //Parameter value
        public MAVParamExtTypeFlags  ParamType;      //Parameter type.    

        #region CTOR
        /// <summary>
        /// Instantiates a new ParamExtValueData
        /// </summary>    
        /*
        public ParamExtValueData() {
            Init();
        }
        */
        public void Init() {
            ParamCount       = default(ushort              );
            ParamIndex       = default(ushort              );
            ParamId          = new char[ 16];
            ParamValue       = new char[128];
            ParamType        = default(MAVParamExtTypeFlags);
        }
        #endregion

        #region Read Buffer
        /// <summary>
        /// Reads the data from Buffer into this struct
        /// </summary>    
        public int Read(byte[] p_buffer,int p_offset=0) {
            int    l = 149;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals
            int[]  LS8  = MAVLinkCRC.U8_LSH8, LS16 = MAVLinkCRC.U8_LSH16, LS24 = MAVLinkCRC.U8_LSH24, LS32 = MAVLinkCRC.U8_LSH32, LS40 = MAVLinkCRC.U8_LSH40, LS48 = MAVLinkCRC.U8_LSH48, LS56 = MAVLinkCRC.U8_LSH56;
            Span<byte> b = p_buffer.AsSpan(p_offset);            
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            ParamCount       = (ushort              ) (b[p++] | LS8[b[p++]]);
            ParamIndex       = (ushort              ) (b[p++] | LS8[b[p++]]);
            for(int i=0;i<16 ;i++) { ParamId[i]       = (char                ) (b[p++]); }
            for(int i=0;i<128;i++) { ParamValue[i]    = (char                ) (b[p++]); }
            ParamType        = (MAVParamExtTypeFlags) (b[p++]);            
            return p;
        }
        #endregion

        #region Write Buffer
        /// <summary>
        /// Writes the message data into a Buffer
        /// </summary>    
        public int Write(byte[] p_buffer,int p_offset=0) {
            int    l = 149;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals            
            Span<byte> b = p_buffer.AsSpan(p_offset);
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            b[p++] = (byte)(      ParamCount);
            b[p++] = (byte)((int)ParamCount>>8 );
            b[p++] = (byte)(      ParamIndex);
            b[p++] = (byte)((int)ParamIndex>>8 );
            for(int i=0;i< 16;i++) {
                b[p++] = (byte)(ParamId[i]);
            }
            for(int i=0;i<128;i++) {
                b[p++] = (byte)(ParamValue[i]);
            }
            b[p++] = (byte)(ParamType);
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
            int l = 149;
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
