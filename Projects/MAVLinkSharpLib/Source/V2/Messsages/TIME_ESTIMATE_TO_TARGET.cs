        
using System;
using System.IO;
using System.Runtime.InteropServices;
using MAVLinkSharp.Runtime;

#pragma warning disable CS0675

namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// Time/duration estimates for various events and actions given the current vehicle state and position.
    /// </summary>    
    public struct TimeEstimateToTargetData : IMAVLinkMessageData {

        /// <summary>
        /// Message Id Associated w/ this Struct
        /// </summary>    
        public int GetId() { return 380; }

        public int  SafeReturn;           //Estimated time to complete the vehicle's configured "safe return" action from its current position (e.g. RTL, Smart RTL, etc.). -1 indicates that the vehicle is landed, or that no time estimate available.
        public int  Land;                 //Estimated time for vehicle to complete the LAND action from its current position. -1 indicates that the vehicle is landed, or that no time estimate available.
        public int  MissionNextItem;      //Estimated time for reaching/completing the currently active mission item. -1 means no time estimate available.
        public int  MissionEnd;           //Estimated time for completing the current mission. -1 means no mission active and/or no estimate available.
        public int  CommandedAction;      //Estimated time for completing the current commanded action (i.e. Go To, Takeoff, Land, etc.). -1 means no action active and/or no estimate available.    

        #region CTOR
        /// <summary>
        /// Instantiates a new TimeEstimateToTargetData
        /// </summary>    
        /*
        public TimeEstimateToTargetData() {
            Init();
        }
        */
        public void Init() {
            SafeReturn             = default(int);
            Land                   = default(int);
            MissionNextItem        = default(int);
            MissionEnd             = default(int);
            CommandedAction        = default(int);
        }
        #endregion

        #region Read Buffer
        /// <summary>
        /// Reads the data from Buffer into this struct
        /// </summary>    
        public int Read(byte[] p_buffer,int p_offset=0) {
            int    l = 20;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals
            int[]  LS8  = MAVLinkCRC.U8_LSH8, LS16 = MAVLinkCRC.U8_LSH16, LS24 = MAVLinkCRC.U8_LSH24, LS32 = MAVLinkCRC.U8_LSH32, LS40 = MAVLinkCRC.U8_LSH40, LS48 = MAVLinkCRC.U8_LSH48, LS56 = MAVLinkCRC.U8_LSH56;
            Span<byte> b = p_buffer.AsSpan(p_offset);            
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            SafeReturn             = (int) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            Land                   = (int) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            MissionNextItem        = (int) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            MissionEnd             = (int) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            CommandedAction        = (int) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);            
            return p;
        }
        #endregion

        #region Write Buffer
        /// <summary>
        /// Writes the message data into a Buffer
        /// </summary>    
        public int Write(byte[] p_buffer,int p_offset=0) {
            int    l = 20;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals            
            Span<byte> b = p_buffer.AsSpan(p_offset);
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            b[p++] = (byte)(      SafeReturn);
            b[p++] = (byte)((int)SafeReturn>>8 );
            b[p++] = (byte)((int)SafeReturn>>16);
            b[p++] = (byte)((int)SafeReturn>>24);
            b[p++] = (byte)(      Land);
            b[p++] = (byte)((int)Land>>8 );
            b[p++] = (byte)((int)Land>>16);
            b[p++] = (byte)((int)Land>>24);
            b[p++] = (byte)(      MissionNextItem);
            b[p++] = (byte)((int)MissionNextItem>>8 );
            b[p++] = (byte)((int)MissionNextItem>>16);
            b[p++] = (byte)((int)MissionNextItem>>24);
            b[p++] = (byte)(      MissionEnd);
            b[p++] = (byte)((int)MissionEnd>>8 );
            b[p++] = (byte)((int)MissionEnd>>16);
            b[p++] = (byte)((int)MissionEnd>>24);
            b[p++] = (byte)(      CommandedAction);
            b[p++] = (byte)((int)CommandedAction>>8 );
            b[p++] = (byte)((int)CommandedAction>>16);
            b[p++] = (byte)((int)CommandedAction>>24);
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
            int l = 20;
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
