        
using System.IO;
using System.Runtime.InteropServices;

#pragma warning disable CS0675

namespace MAVLinkBindings {

    /// <summary>
    /// Message encoding a command with parameters as scaled integers. Scaling depends on the actual command value. NaN or INT32_MAX may be used in float/integer params (respectively) to indicate optional/default values (e.g. to use the component's current latitude, yaw rather than a specific value). The command microservice is documented at https://mavlink.io/en/services/command.html
    /// </summary>    
    public struct CommandIntData : IMAVLinkMessageData {

        /// <summary>
        /// Message Id Associated w/ this Struct
        /// </summary>    
        public int GetId() { return 75; }

        public float          Param1;              //PARAM1, see MAV_CMD enum
        public float          Param2;              //PARAM2, see MAV_CMD enum
        public float          Param3;              //PARAM3, see MAV_CMD enum
        public float          Param4;              //PARAM4, see MAV_CMD enum
        public int            X;                   //PARAM5 / local: x position in meters * 1e4, global: latitude in degrees * 10^7
        public int            Y;                   //PARAM6 / local: y position in meters * 1e4, global: longitude in degrees * 10^7
        public float          Z;                   //PARAM7 / z position: global: altitude in meters (relative or absolute, depending on frame).
        public MAVCmdFlags    Command;             //The scheduled action for the mission item.
        public byte           TargetSystem;        //System ID
        public byte           TargetComponent;     //Component ID
        public MAVFrameFlags  Frame;               //The coordinate system of the COMMAND.
        public byte           Current;             //Not used.
        public byte           Autocontinue;        //Not used (set 0).    

        #region CTOR
        /// <summary>
        /// Instantiates a new CommandIntData
        /// </summary>    
        public CommandIntData() {
            Param1                = default(float        );
            Param2                = default(float        );
            Param3                = default(float        );
            Param4                = default(float        );
            X                     = default(int          );
            Y                     = default(int          );
            Z                     = default(float        );
            Command               = default(MAVCmdFlags  );
            TargetSystem          = default(byte         );
            TargetComponent       = default(byte         );
            Frame                 = default(MAVFrameFlags);
            Current               = default(byte         );
            Autocontinue          = default(byte         );
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
            Param1                = (float        ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Param2                = (float        ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Param3                = (float        ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Param4                = (float        ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            X                     = (int          ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            Y                     = (int          ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            Z                     = (float        ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Command               = (MAVCmdFlags  ) (b[p++] | LS8[b[p++]]);
            TargetSystem          = (byte         ) (b[p++]);
            TargetComponent       = (byte         ) (b[p++]);
            Frame                 = (MAVFrameFlags) (b[p++]);
            Current               = (byte         ) (b[p++]);
            Autocontinue          = (byte         ) (b[p++]);            
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
            MemoryMarshal.Write(b.Slice(p, 4), in Param1               ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), in Param2               ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), in Param3               ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), in Param4               ); p+=4;
            b[p++] = (byte)(      X);
            b[p++] = (byte)((int)X>>8 );
            b[p++] = (byte)((int)X>>16);
            b[p++] = (byte)((int)X>>24);
            b[p++] = (byte)(      Y);
            b[p++] = (byte)((int)Y>>8 );
            b[p++] = (byte)((int)Y>>16);
            b[p++] = (byte)((int)Y>>24);
            MemoryMarshal.Write(b.Slice(p, 4), in Z                    ); p+=4;
            b[p++] = (byte)(      Command);
            b[p++] = (byte)((int)Command>>8 );
            b[p++] = (byte)(TargetSystem);
            b[p++] = (byte)(TargetComponent);
            b[p++] = (byte)(Frame);
            b[p++] = (byte)(Current);
            b[p++] = (byte)(Autocontinue);
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
