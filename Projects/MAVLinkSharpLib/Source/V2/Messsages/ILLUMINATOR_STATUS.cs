        
using System;
using System.IO;
using System.Runtime.InteropServices;
using MAVLinkSharp.Runtime;

#pragma warning disable CS0675

namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// Illuminator status
    /// </summary>    
    public struct IlluminatorStatusData : IMAVLinkMessageData {

        /// <summary>
        /// Message Id Associated w/ this Struct
        /// </summary>    
        public int GetId() { return 440; }

        public uint                   UptimeMs;             //Time since the start-up of the illuminator in ms
        public IlluminatorErrorFlags  ErrorStatus;          //Errors
        public float                  Brightness;           //Illuminator brightness
        public float                  StrobePeriod;         //Illuminator strobing period in seconds
        public float                  StrobeDutyCycle;      //Illuminator strobing duty cycle
        public float                  TempC;                //Temperature in Celsius
        public float                  MinStrobePeriod;      //Minimum strobing period in seconds
        public float                  MaxStrobePeriod;      //Maximum strobing period in seconds
        public byte                   Enable;               //0: Illuminators OFF, 1: Illuminators ON
        public IlluminatorModeFlags   ModeBitmask;          //Supported illuminator modes
        public IlluminatorModeFlags   Mode;                 //Illuminator mode    

        #region CTOR
        /// <summary>
        /// Instantiates a new IlluminatorStatusData
        /// </summary>    
        /*
        public IlluminatorStatusData() {
            Init();
        }
        */
        public void Init() {
            UptimeMs               = default(uint                 );
            ErrorStatus            = default(IlluminatorErrorFlags);
            Brightness             = default(float                );
            StrobePeriod           = default(float                );
            StrobeDutyCycle        = default(float                );
            TempC                  = default(float                );
            MinStrobePeriod        = default(float                );
            MaxStrobePeriod        = default(float                );
            Enable                 = default(byte                 );
            ModeBitmask            = default(IlluminatorModeFlags );
            Mode                   = default(IlluminatorModeFlags );
        }
        #endregion

        #region Read Buffer
        /// <summary>
        /// Reads the data from Buffer into this struct
        /// </summary>    
        public int Read(byte[] p_buffer,int p_offset=0) {
            int    l = 35;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals
            int[]  LS8  = MAVLinkCRC.U8_LSH8, LS16 = MAVLinkCRC.U8_LSH16, LS24 = MAVLinkCRC.U8_LSH24, LS32 = MAVLinkCRC.U8_LSH32, LS40 = MAVLinkCRC.U8_LSH40, LS48 = MAVLinkCRC.U8_LSH48, LS56 = MAVLinkCRC.U8_LSH56;
            Span<byte> b = p_buffer.AsSpan(p_offset);            
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            UptimeMs               = (uint                 ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            ErrorStatus            = (IlluminatorErrorFlags) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            Brightness             = (float                ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            StrobePeriod           = (float                ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            StrobeDutyCycle        = (float                ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            TempC                  = (float                ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            MinStrobePeriod        = (float                ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            MaxStrobePeriod        = (float                ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Enable                 = (byte                 ) (b[p++]);
            ModeBitmask            = (IlluminatorModeFlags ) (b[p++]);
            Mode                   = (IlluminatorModeFlags ) (b[p++]);            
            return p;
        }
        #endregion

        #region Write Buffer
        /// <summary>
        /// Writes the message data into a Buffer
        /// </summary>    
        public int Write(byte[] p_buffer,int p_offset=0) {
            int    l = 35;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals            
            Span<byte> b = p_buffer.AsSpan(p_offset);
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            b[p++] = (byte)(      UptimeMs);
            b[p++] = (byte)((int)UptimeMs>>8 );
            b[p++] = (byte)((int)UptimeMs>>16);
            b[p++] = (byte)((int)UptimeMs>>24);
            b[p++] = (byte)(      ErrorStatus);
            b[p++] = (byte)((int)ErrorStatus>>8 );
            b[p++] = (byte)((int)ErrorStatus>>16);
            b[p++] = (byte)((int)ErrorStatus>>24);
            MemoryMarshal.Write(b.Slice(p, 4), ref Brightness            ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref StrobePeriod          ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref StrobeDutyCycle       ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref TempC                 ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref MinStrobePeriod       ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref MaxStrobePeriod       ); p+=4;
            b[p++] = (byte)(Enable);
            b[p++] = (byte)(ModeBitmask);
            b[p++] = (byte)(Mode);
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
            int l = 35;
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
