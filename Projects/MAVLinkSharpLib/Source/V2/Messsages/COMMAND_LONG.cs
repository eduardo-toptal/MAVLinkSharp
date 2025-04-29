        
using System;
using System.IO;
using System.Runtime.InteropServices;
using MAVLinkSharp.Runtime;

#pragma warning disable CS0675

namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// Send a command with up to seven parameters to the MAV. The command microservice is documented at https://mavlink.io/en/services/command.html
    /// </summary>    
    public struct CommandLongData : IMAVLinkMessageData {

        /// <summary>
        /// Message Id Associated w/ this Struct
        /// </summary>    
        public int GetId() { return 76; }

        public float        Param1;              //Parameter 1 (for the specific command).
        public float        Param2;              //Parameter 2 (for the specific command).
        public float        Param3;              //Parameter 3 (for the specific command).
        public float        Param4;              //Parameter 4 (for the specific command).
        public float        Param5;              //Parameter 5 (for the specific command).
        public float        Param6;              //Parameter 6 (for the specific command).
        public float        Param7;              //Parameter 7 (for the specific command).
        public MAVCmdFlags  Command;             //Command ID (of command to send).
        public byte         TargetSystem;        //System which should execute the command
        public byte         TargetComponent;     //Component which should execute the command, 0 for all components
        public byte         Confirmation;        //0: First transmission of this command. 1-255: Confirmation transmissions (e.g. for kill command)    

        #region CTOR
        /// <summary>
        /// Instantiates a new CommandLongData
        /// </summary>    
        /*
        public CommandLongData() {
            Init();
        }
        */
        public void Init() {
            Param1                = default(float      );
            Param2                = default(float      );
            Param3                = default(float      );
            Param4                = default(float      );
            Param5                = default(float      );
            Param6                = default(float      );
            Param7                = default(float      );
            Command               = default(MAVCmdFlags);
            TargetSystem          = default(byte       );
            TargetComponent       = default(byte       );
            Confirmation          = default(byte       );
        }
        #endregion

        #region Read Buffer
        /// <summary>
        /// Reads the data from Buffer into this struct
        /// </summary>    
        public int Read(byte[] p_buffer,int p_offset=0) {
            int    l = 33;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals
            int[]  LS8  = MAVLinkCRC.U8_LSH8, LS16 = MAVLinkCRC.U8_LSH16, LS24 = MAVLinkCRC.U8_LSH24, LS32 = MAVLinkCRC.U8_LSH32, LS40 = MAVLinkCRC.U8_LSH40, LS48 = MAVLinkCRC.U8_LSH48, LS56 = MAVLinkCRC.U8_LSH56;
            Span<byte> b = p_buffer.AsSpan(p_offset);            
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            Param1                = (float      ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Param2                = (float      ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Param3                = (float      ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Param4                = (float      ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Param5                = (float      ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Param6                = (float      ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Param7                = (float      ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Command               = (MAVCmdFlags) (b[p++] | LS8[b[p++]]);
            TargetSystem          = (byte       ) (b[p++]);
            TargetComponent       = (byte       ) (b[p++]);
            Confirmation          = (byte       ) (b[p++]);            
            return p;
        }
        #endregion

        #region Write Buffer
        /// <summary>
        /// Writes the message data into a Buffer
        /// </summary>    
        public int Write(byte[] p_buffer,int p_offset=0) {
            int    l = 33;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals            
            Span<byte> b = p_buffer.AsSpan(p_offset);
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            MemoryMarshal.Write(b.Slice(p, 4), ref Param1               ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref Param2               ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref Param3               ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref Param4               ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref Param5               ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref Param6               ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref Param7               ); p+=4;
            b[p++] = (byte)(      Command);
            b[p++] = (byte)((int)Command>>8 );
            b[p++] = (byte)(TargetSystem);
            b[p++] = (byte)(TargetComponent);
            b[p++] = (byte)(Confirmation);
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
            int l = 33;
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
