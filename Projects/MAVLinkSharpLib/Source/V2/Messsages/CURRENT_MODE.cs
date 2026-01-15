        
using System;
using System.IO;
using System.Runtime.InteropServices;
using MAVLinkSharp.Runtime;

#pragma warning disable CS0675

namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// Get the current mode.
    /// This should be emitted on any mode change, and broadcast at low rate (nominally 0.5 Hz).
    /// It may be requested using MAV_CMD_REQUEST_MESSAGE.
    /// See https://mavlink.io/en/services/standard_modes.html
    /// 
    /// </summary>    
    public struct CurrentModeData : IMAVLinkMessageData {

        /// <summary>
        /// Message Id Associated w/ this Struct
        /// </summary>    
        public int GetId() { return 436; }

        public uint                  CustomMode;              //A bitfield for use for autopilot-specific flags
        public uint                  IntendedCustomMode;      //The custom_mode of the mode that was last commanded by the user (for example, with MAV_CMD_DO_SET_STANDARD_MODE, MAV_CMD_DO_SET_MODE or via RC). This should usually be the same as custom_mode. It will be different if the vehicle is unable to enter the intended mode, or has left that mode due to a failsafe condition. 0 indicates the intended custom mode is unknown/not supplied
        public MAVStandardModeFlags  StandardMode;            //Standard mode.    

        #region CTOR
        /// <summary>
        /// Instantiates a new CurrentModeData
        /// </summary>    
        /*
        public CurrentModeData() {
            Init();
        }
        */
        public void Init() {
            CustomMode                = default(uint                );
            IntendedCustomMode        = default(uint                );
            StandardMode              = default(MAVStandardModeFlags);
        }
        #endregion

        #region Read Buffer
        /// <summary>
        /// Reads the data from Buffer into this struct
        /// </summary>    
        public int Read(byte[] p_buffer,int p_offset=0) {
            int    l = 9;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals
            int[]  LS8  = MAVLinkCRC.U8_LSH8, LS16 = MAVLinkCRC.U8_LSH16, LS24 = MAVLinkCRC.U8_LSH24, LS32 = MAVLinkCRC.U8_LSH32, LS40 = MAVLinkCRC.U8_LSH40, LS48 = MAVLinkCRC.U8_LSH48, LS56 = MAVLinkCRC.U8_LSH56;
            Span<byte> b = p_buffer.AsSpan(p_offset);            
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            CustomMode                = (uint                ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            IntendedCustomMode        = (uint                ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            StandardMode              = (MAVStandardModeFlags) (b[p++]);            
            return p;
        }
        #endregion

        #region Write Buffer
        /// <summary>
        /// Writes the message data into a Buffer
        /// </summary>    
        public int Write(byte[] p_buffer,int p_offset=0) {
            int    l = 9;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals            
            Span<byte> b = p_buffer.AsSpan(p_offset);
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            b[p++] = (byte)(      CustomMode);
            b[p++] = (byte)((int)CustomMode>>8 );
            b[p++] = (byte)((int)CustomMode>>16);
            b[p++] = (byte)((int)CustomMode>>24);
            b[p++] = (byte)(      IntendedCustomMode);
            b[p++] = (byte)((int)IntendedCustomMode>>8 );
            b[p++] = (byte)((int)IntendedCustomMode>>16);
            b[p++] = (byte)((int)IntendedCustomMode>>24);
            b[p++] = (byte)(StandardMode);
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
            int l = 9;
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
