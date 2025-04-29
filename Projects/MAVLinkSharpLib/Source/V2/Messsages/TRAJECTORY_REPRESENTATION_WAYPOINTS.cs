        
using System.IO;
using System.Runtime.InteropServices;
using MAVLinkSharp.Runtime;

#pragma warning disable CS0675

namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// Describe a trajectory using an array of up-to 5 waypoints in the local frame (MAV_FRAME_LOCAL_NED).
    /// </summary>    
    public struct TrajectoryRepresentationWaypointsData : IMAVLinkMessageData {

        /// <summary>
        /// Message Id Associated w/ this Struct
        /// </summary>    
        public int GetId() { return 332; }

        public ulong          TimeUsec;        //Timestamp (UNIX Epoch time or time since system boot). The receiving end can infer timestamp format (since 1.1.1970 or since system boot) by checking for the magnitude of the number.
        public float[]        PosX;            //X-coordinate of waypoint, set to NaN if not being used
        public float[]        PosY;            //Y-coordinate of waypoint, set to NaN if not being used
        public float[]        PosZ;            //Z-coordinate of waypoint, set to NaN if not being used
        public float[]        VelX;            //X-velocity of waypoint, set to NaN if not being used
        public float[]        VelY;            //Y-velocity of waypoint, set to NaN if not being used
        public float[]        VelZ;            //Z-velocity of waypoint, set to NaN if not being used
        public float[]        AccX;            //X-acceleration of waypoint, set to NaN if not being used
        public float[]        AccY;            //Y-acceleration of waypoint, set to NaN if not being used
        public float[]        AccZ;            //Z-acceleration of waypoint, set to NaN if not being used
        public float[]        PosYaw;          //Yaw angle, set to NaN if not being used
        public float[]        VelYaw;          //Yaw rate, set to NaN if not being used
        public MAVCmdFlags[]  Command;         //MAV_CMD command id of waypoint, set to UINT16_MAX if not being used.
        public byte           ValidPoints;     //Number of valid points (up-to 5 waypoints are possible)    

        #region CTOR
        /// <summary>
        /// Instantiates a new TrajectoryRepresentationWaypointsData
        /// </summary>    
        /*
        public TrajectoryRepresentationWaypointsData() {
            Init();
        }
        */
        public void Init() {
            TimeUsec          = default(ulong      );
            PosX              = new float[  5];
            PosY              = new float[  5];
            PosZ              = new float[  5];
            VelX              = new float[  5];
            VelY              = new float[  5];
            VelZ              = new float[  5];
            AccX              = new float[  5];
            AccY              = new float[  5];
            AccZ              = new float[  5];
            PosYaw            = new float[  5];
            VelYaw            = new float[  5];
            Command           = new MAVCmdFlags[  5];
            ValidPoints       = default(byte       );
        }
        #endregion

        #region Read Buffer
        /// <summary>
        /// Reads the data from Buffer into this struct
        /// </summary>    
        public int Read(byte[] p_buffer,int p_offset=0) {
            int    l = 239;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals
            int[]  LS8  = MAVLinkCRC.U8_LSH8, LS16 = MAVLinkCRC.U8_LSH16, LS24 = MAVLinkCRC.U8_LSH24, LS32 = MAVLinkCRC.U8_LSH32, LS40 = MAVLinkCRC.U8_LSH40, LS48 = MAVLinkCRC.U8_LSH48, LS56 = MAVLinkCRC.U8_LSH56;
            Span<byte> b = p_buffer.AsSpan(p_offset);            
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            TimeUsec          = (ulong      ) ((ulong)b[p++] | (ulong)LS8[b[p++]] | (ulong)LS16[b[p++]] | (ulong)LS24[b[p++]] | (ulong)LS32[b[p++]] | (ulong)LS40[b[p++]] | (ulong)LS48[b[p++]] | (ulong)LS56[b[p++]]);
            for(int i=0;i<5  ;i++) { PosX[i]           = (float      ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4; }
            for(int i=0;i<5  ;i++) { PosY[i]           = (float      ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4; }
            for(int i=0;i<5  ;i++) { PosZ[i]           = (float      ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4; }
            for(int i=0;i<5  ;i++) { VelX[i]           = (float      ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4; }
            for(int i=0;i<5  ;i++) { VelY[i]           = (float      ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4; }
            for(int i=0;i<5  ;i++) { VelZ[i]           = (float      ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4; }
            for(int i=0;i<5  ;i++) { AccX[i]           = (float      ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4; }
            for(int i=0;i<5  ;i++) { AccY[i]           = (float      ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4; }
            for(int i=0;i<5  ;i++) { AccZ[i]           = (float      ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4; }
            for(int i=0;i<5  ;i++) { PosYaw[i]         = (float      ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4; }
            for(int i=0;i<5  ;i++) { VelYaw[i]         = (float      ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4; }
            for(int i=0;i<5  ;i++) { Command[i]        = (MAVCmdFlags) (b[p++] | LS8[b[p++]]); }
            ValidPoints       = (byte       ) (b[p++]);            
            return p;
        }
        #endregion

        #region Write Buffer
        /// <summary>
        /// Writes the message data into a Buffer
        /// </summary>    
        public int Write(byte[] p_buffer,int p_offset=0) {
            int    l = 239;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals            
            Span<byte> b = p_buffer.AsSpan(p_offset);
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            b[p++] = (byte)(      TimeUsec);
            b[p++] = (byte)((long)TimeUsec>>8 );
            b[p++] = (byte)((long)TimeUsec>>16);
            b[p++] = (byte)((long)TimeUsec>>24);
            b[p++] = (byte)((long)TimeUsec>>32);
            b[p++] = (byte)((long)TimeUsec>>40);
            b[p++] = (byte)((long)TimeUsec>>48);
            b[p++] = (byte)((long)TimeUsec>>56);
            for(int i=0;i<  5;i++) {
                MemoryMarshal.Write(b.Slice(p, 4), ref PosX[i]          ); p+=4;
            }
            for(int i=0;i<  5;i++) {
                MemoryMarshal.Write(b.Slice(p, 4), ref PosY[i]          ); p+=4;
            }
            for(int i=0;i<  5;i++) {
                MemoryMarshal.Write(b.Slice(p, 4), ref PosZ[i]          ); p+=4;
            }
            for(int i=0;i<  5;i++) {
                MemoryMarshal.Write(b.Slice(p, 4), ref VelX[i]          ); p+=4;
            }
            for(int i=0;i<  5;i++) {
                MemoryMarshal.Write(b.Slice(p, 4), ref VelY[i]          ); p+=4;
            }
            for(int i=0;i<  5;i++) {
                MemoryMarshal.Write(b.Slice(p, 4), ref VelZ[i]          ); p+=4;
            }
            for(int i=0;i<  5;i++) {
                MemoryMarshal.Write(b.Slice(p, 4), ref AccX[i]          ); p+=4;
            }
            for(int i=0;i<  5;i++) {
                MemoryMarshal.Write(b.Slice(p, 4), ref AccY[i]          ); p+=4;
            }
            for(int i=0;i<  5;i++) {
                MemoryMarshal.Write(b.Slice(p, 4), ref AccZ[i]          ); p+=4;
            }
            for(int i=0;i<  5;i++) {
                MemoryMarshal.Write(b.Slice(p, 4), ref PosYaw[i]        ); p+=4;
            }
            for(int i=0;i<  5;i++) {
                MemoryMarshal.Write(b.Slice(p, 4), ref VelYaw[i]        ); p+=4;
            }
            for(int i=0;i<  5;i++) {
                b[p++] = (byte)(      Command[i]);
                b[p++] = (byte)((int)Command[i]>>8 );
            }
            b[p++] = (byte)(ValidPoints);
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
            int l = 239;
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
