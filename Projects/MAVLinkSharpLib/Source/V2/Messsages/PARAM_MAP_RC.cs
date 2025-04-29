        
using System.IO;
using System.Runtime.InteropServices;
using MAVLinkSharp.Runtime;

#pragma warning disable CS0675

namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// Bind a RC channel to a parameter. The parameter should change according to the RC channel value.
    /// </summary>    
    public struct ParamMapRcData : IMAVLinkMessageData {

        /// <summary>
        /// Message Id Associated w/ this Struct
        /// </summary>    
        public int GetId() { return 50; }

        public float   ParamValue0;                   //Initial parameter value
        public float   Scale;                         //Scale, maps the RC range [-1, 1] to a parameter value
        public float   ParamValueMin;                 //Minimum param value. The protocol does not define if this overwrites an onboard minimum value. (Depends on implementation)
        public float   ParamValueMax;                 //Maximum param value. The protocol does not define if this overwrites an onboard maximum value. (Depends on implementation)
        public short   ParamIndex;                    //Parameter index. Send -1 to use the param ID field as identifier (else the param id will be ignored), send -2 to disable any existing map for this rc_channel_index.
        public byte    TargetSystem;                  //System ID
        public byte    TargetComponent;               //Component ID
        public char[]  ParamId;                       //Onboard parameter id, terminated by NULL if the length is less than 16 human-readable chars and WITHOUT null termination (NULL) byte if the length is exactly 16 chars - applications have to provide 16+1 bytes storage if the ID is stored as string
        public byte    ParameterRcChannelIndex;       //Index of parameter RC channel. Not equal to the RC channel id. Typically corresponds to a potentiometer-knob on the RC.    

        #region CTOR
        /// <summary>
        /// Instantiates a new ParamMapRcData
        /// </summary>    
        /*
        public ParamMapRcData() {
            Init();
        }
        */
        public void Init() {
            ParamValue0                     = default(float);
            Scale                           = default(float);
            ParamValueMin                   = default(float);
            ParamValueMax                   = default(float);
            ParamIndex                      = default(short);
            TargetSystem                    = default(byte );
            TargetComponent                 = default(byte );
            ParamId                         = new char[ 16];
            ParameterRcChannelIndex         = default(byte );
        }
        #endregion

        #region Read Buffer
        /// <summary>
        /// Reads the data from Buffer into this struct
        /// </summary>    
        public int Read(byte[] p_buffer,int p_offset=0) {
            int    l = 37;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals
            int[]  LS8  = MAVLinkCRC.U8_LSH8, LS16 = MAVLinkCRC.U8_LSH16, LS24 = MAVLinkCRC.U8_LSH24, LS32 = MAVLinkCRC.U8_LSH32, LS40 = MAVLinkCRC.U8_LSH40, LS48 = MAVLinkCRC.U8_LSH48, LS56 = MAVLinkCRC.U8_LSH56;
            Span<byte> b = p_buffer.AsSpan(p_offset);            
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            ParamValue0                     = (float) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Scale                           = (float) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            ParamValueMin                   = (float) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            ParamValueMax                   = (float) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            ParamIndex                      = (short) (b[p++] | LS8[b[p++]]);
            TargetSystem                    = (byte ) (b[p++]);
            TargetComponent                 = (byte ) (b[p++]);
            for(int i=0;i<16 ;i++) { ParamId[i]                      = (char ) (b[p++]); }
            ParameterRcChannelIndex         = (byte ) (b[p++]);            
            return p;
        }
        #endregion

        #region Write Buffer
        /// <summary>
        /// Writes the message data into a Buffer
        /// </summary>    
        public int Write(byte[] p_buffer,int p_offset=0) {
            int    l = 37;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals            
            Span<byte> b = p_buffer.AsSpan(p_offset);
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            MemoryMarshal.Write(b.Slice(p, 4), ref ParamValue0                    ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref Scale                          ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref ParamValueMin                  ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref ParamValueMax                  ); p+=4;
            b[p++] = (byte)(      ParamIndex);
            b[p++] = (byte)((int)ParamIndex>>8 );
            b[p++] = (byte)(TargetSystem);
            b[p++] = (byte)(TargetComponent);
            for(int i=0;i< 16;i++) {
                b[p++] = (byte)(ParamId[i]);
            }
            b[p++] = (byte)(ParameterRcChannelIndex);
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
            int l = 37;
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
