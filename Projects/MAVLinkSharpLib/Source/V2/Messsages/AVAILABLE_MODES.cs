        
using System;
using System.IO;
using System.Runtime.InteropServices;
using MAVLinkSharp.Runtime;

#pragma warning disable CS0675

namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// Information about a flight mode.
    /// 
    /// The message can be enumerated to get information for all modes, or requested for a particular mode, using MAV_CMD_REQUEST_MESSAGE.
    /// Specify 0 in param2 to request that the message is emitted for all available modes or the specific index for just one mode.
    /// The modes must be available/settable for the current vehicle/frame type.
    /// Each mode should only be emitted once (even if it is both standard and custom).
    /// Note that the current mode should be emitted in CURRENT_MODE, and that if the mode list can change then AVAILABLE_MODES_MONITOR must be emitted on first change and subsequently streamed.
    /// See https://mavlink.io/en/services/standard_modes.html
    /// 
    /// </summary>    
    public struct AvailableModesData : IMAVLinkMessageData {

        /// <summary>
        /// Message Id Associated w/ this Struct
        /// </summary>    
        public int GetId() { return 435; }

        public uint                  CustomMode;       //A bitfield for use for autopilot-specific flags
        public MAVModePropertyFlags  Properties;       //Mode properties.
        public byte                  NumberModes;      //The total number of available modes for the current vehicle type.
        public byte                  ModeIndex;        //The current mode index within number_modes, indexed from 1. The index is not guaranteed to be persistent, and may change between reboots or if the set of modes change.
        public MAVStandardModeFlags  StandardMode;     //Standard mode.
        public char[]                ModeName;         //Name of custom mode, with null termination character. Should be omitted for standard modes.    

        #region CTOR
        /// <summary>
        /// Instantiates a new AvailableModesData
        /// </summary>    
        /*
        public AvailableModesData() {
            Init();
        }
        */
        public void Init() {
            CustomMode         = default(uint                );
            Properties         = default(MAVModePropertyFlags);
            NumberModes        = default(byte                );
            ModeIndex          = default(byte                );
            StandardMode       = default(MAVStandardModeFlags);
            ModeName           = new char[ 35];
        }
        #endregion

        #region Read Buffer
        /// <summary>
        /// Reads the data from Buffer into this struct
        /// </summary>    
        public int Read(byte[] p_buffer,int p_offset=0) {
            int    l = 46;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals
            int[]  LS8  = MAVLinkCRC.U8_LSH8, LS16 = MAVLinkCRC.U8_LSH16, LS24 = MAVLinkCRC.U8_LSH24, LS32 = MAVLinkCRC.U8_LSH32, LS40 = MAVLinkCRC.U8_LSH40, LS48 = MAVLinkCRC.U8_LSH48, LS56 = MAVLinkCRC.U8_LSH56;
            Span<byte> b = p_buffer.AsSpan(p_offset);            
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            CustomMode         = (uint                ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            Properties         = (MAVModePropertyFlags) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            NumberModes        = (byte                ) (b[p++]);
            ModeIndex          = (byte                ) (b[p++]);
            StandardMode       = (MAVStandardModeFlags) (b[p++]);
            for(int i=0;i<35 ;i++) { ModeName[i]        = (char                ) (b[p++]); }            
            return p;
        }
        #endregion

        #region Write Buffer
        /// <summary>
        /// Writes the message data into a Buffer
        /// </summary>    
        public int Write(byte[] p_buffer,int p_offset=0) {
            int    l = 46;
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
            b[p++] = (byte)(      Properties);
            b[p++] = (byte)((int)Properties>>8 );
            b[p++] = (byte)((int)Properties>>16);
            b[p++] = (byte)((int)Properties>>24);
            b[p++] = (byte)(NumberModes);
            b[p++] = (byte)(ModeIndex);
            b[p++] = (byte)(StandardMode);
            for(int i=0;i< 35;i++) {
                b[p++] = (byte)(ModeName[i]);
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
            int l = 46;
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
