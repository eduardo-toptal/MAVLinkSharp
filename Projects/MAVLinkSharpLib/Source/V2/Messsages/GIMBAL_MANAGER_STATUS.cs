        
using System;
using System.IO;
using System.Runtime.InteropServices;
using MAVLinkSharp.Runtime;

#pragma warning disable CS0675

namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// Current status about a high level gimbal manager. This message should be broadcast at a low regular rate (e.g. 5Hz).
    /// </summary>    
    public struct GimbalManagerStatusData : IMAVLinkMessageData {

        /// <summary>
        /// Message Id Associated w/ this Struct
        /// </summary>    
        public int GetId() { return 281; }

        public uint                TimeBootMs;                  //Timestamp (time since system boot).
        public GimbalManagerFlags  Flags;                       //High level gimbal manager flags currently applied.
        public byte                GimbalDeviceId;              //Gimbal device ID that this gimbal manager is responsible for. Component ID of gimbal device (or 1-6 for non-MAVLink gimbal).
        public byte                PrimaryControlSysid;         //System ID of MAVLink component with primary control, 0 for none.
        public byte                PrimaryControlCompid;        //Component ID of MAVLink component with primary control, 0 for none.
        public byte                SecondaryControlSysid;       //System ID of MAVLink component with secondary control, 0 for none.
        public byte                SecondaryControlCompid;      //Component ID of MAVLink component with secondary control, 0 for none.    

        #region CTOR
        /// <summary>
        /// Instantiates a new GimbalManagerStatusData
        /// </summary>    
        /*
        public GimbalManagerStatusData() {
            Init();
        }
        */
        public void Init() {
            TimeBootMs                    = default(uint              );
            Flags                         = default(GimbalManagerFlags);
            GimbalDeviceId                = default(byte              );
            PrimaryControlSysid           = default(byte              );
            PrimaryControlCompid          = default(byte              );
            SecondaryControlSysid         = default(byte              );
            SecondaryControlCompid        = default(byte              );
        }
        #endregion

        #region Read Buffer
        /// <summary>
        /// Reads the data from Buffer into this struct
        /// </summary>    
        public int Read(byte[] p_buffer,int p_offset=0) {
            int    l = 13;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals
            int[]  LS8  = MAVLinkCRC.U8_LSH8, LS16 = MAVLinkCRC.U8_LSH16, LS24 = MAVLinkCRC.U8_LSH24, LS32 = MAVLinkCRC.U8_LSH32, LS40 = MAVLinkCRC.U8_LSH40, LS48 = MAVLinkCRC.U8_LSH48, LS56 = MAVLinkCRC.U8_LSH56;
            Span<byte> b = p_buffer.AsSpan(p_offset);            
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            TimeBootMs                    = (uint              ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            Flags                         = (GimbalManagerFlags) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            GimbalDeviceId                = (byte              ) (b[p++]);
            PrimaryControlSysid           = (byte              ) (b[p++]);
            PrimaryControlCompid          = (byte              ) (b[p++]);
            SecondaryControlSysid         = (byte              ) (b[p++]);
            SecondaryControlCompid        = (byte              ) (b[p++]);            
            return p;
        }
        #endregion

        #region Write Buffer
        /// <summary>
        /// Writes the message data into a Buffer
        /// </summary>    
        public int Write(byte[] p_buffer,int p_offset=0) {
            int    l = 13;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals            
            Span<byte> b = p_buffer.AsSpan(p_offset);
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            b[p++] = (byte)(      TimeBootMs);
            b[p++] = (byte)((int)TimeBootMs>>8 );
            b[p++] = (byte)((int)TimeBootMs>>16);
            b[p++] = (byte)((int)TimeBootMs>>24);
            b[p++] = (byte)(      Flags);
            b[p++] = (byte)((int)Flags>>8 );
            b[p++] = (byte)((int)Flags>>16);
            b[p++] = (byte)((int)Flags>>24);
            b[p++] = (byte)(GimbalDeviceId);
            b[p++] = (byte)(PrimaryControlSysid);
            b[p++] = (byte)(PrimaryControlCompid);
            b[p++] = (byte)(SecondaryControlSysid);
            b[p++] = (byte)(SecondaryControlCompid);
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
            int l = 13;
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
