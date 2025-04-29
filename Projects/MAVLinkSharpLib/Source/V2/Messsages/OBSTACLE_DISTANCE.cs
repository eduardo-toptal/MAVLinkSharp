        
using System.IO;
using System.Runtime.InteropServices;
using MAVLinkSharp.Runtime;

#pragma warning disable CS0675

namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// Obstacle distances in front of the sensor, starting from the left in increment degrees to the right
    /// </summary>    
    public struct ObstacleDistanceData : IMAVLinkMessageData {

        /// <summary>
        /// Message Id Associated w/ this Struct
        /// </summary>    
        public int GetId() { return 330; }

        public ulong                   TimeUsec;        //Timestamp (UNIX Epoch time or time since system boot). The receiving end can infer timestamp format (since 1.1.1970 or since system boot) by checking for the magnitude of the number.
        public ushort[]                Distances;       //Distance of obstacles around the vehicle with index 0 corresponding to north + angle_offset, unless otherwise specified in the frame. A value of 0 is valid and means that the obstacle is practically touching the sensor. A value of max_distance +1 means no obstacle is present. A value of UINT16_MAX for unknown/not used. In a array element, one unit corresponds to 1cm.
        public ushort                  MinDistance;     //Minimum distance the sensor can measure.
        public ushort                  MaxDistance;     //Maximum distance the sensor can measure.
        public MAVDistanceSensorFlags  SensorType;      //Class id of the distance sensor type.
        public byte                    Increment;       //Angular width in degrees of each array element. Increment direction is clockwise. This field is ignored if increment_f is non-zero.
        public float                   IncrementF;      //Angular width in degrees of each array element as a float. If non-zero then this value is used instead of the uint8_t increment field. Positive is clockwise direction, negative is counter-clockwise.
        public float                   AngleOffset;     //Relative angle offset of the 0-index element in the distances array. Value of 0 corresponds to forward. Positive is clockwise direction, negative is counter-clockwise.
        public MAVFrameFlags           Frame;           //Coordinate frame of reference for the yaw rotation and offset of the sensor data. Defaults to MAV_FRAME_GLOBAL, which is north aligned. For body-mounted sensors use MAV_FRAME_BODY_FRD, which is vehicle front aligned.    

        #region CTOR
        /// <summary>
        /// Instantiates a new ObstacleDistanceData
        /// </summary>    
        /*
        public ObstacleDistanceData() {
            Init();
        }
        */
        public void Init() {
            TimeUsec          = default(ulong                 );
            Distances         = new ushort[ 72];
            MinDistance       = default(ushort                );
            MaxDistance       = default(ushort                );
            SensorType        = default(MAVDistanceSensorFlags);
            Increment         = default(byte                  );
            IncrementF        = default(float                 );
            AngleOffset       = default(float                 );
            Frame             = default(MAVFrameFlags         );
        }
        #endregion

        #region Read Buffer
        /// <summary>
        /// Reads the data from Buffer into this struct
        /// </summary>    
        public int Read(byte[] p_buffer,int p_offset=0) {
            int    l = 167;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals
            int[]  LS8  = MAVLinkCRC.U8_LSH8, LS16 = MAVLinkCRC.U8_LSH16, LS24 = MAVLinkCRC.U8_LSH24, LS32 = MAVLinkCRC.U8_LSH32, LS40 = MAVLinkCRC.U8_LSH40, LS48 = MAVLinkCRC.U8_LSH48, LS56 = MAVLinkCRC.U8_LSH56;
            Span<byte> b = p_buffer.AsSpan(p_offset);            
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            TimeUsec          = (ulong                 ) ((ulong)b[p++] | (ulong)LS8[b[p++]] | (ulong)LS16[b[p++]] | (ulong)LS24[b[p++]] | (ulong)LS32[b[p++]] | (ulong)LS40[b[p++]] | (ulong)LS48[b[p++]] | (ulong)LS56[b[p++]]);
            for(int i=0;i<72 ;i++) { Distances[i]      = (ushort                ) (b[p++] | LS8[b[p++]]); }
            MinDistance       = (ushort                ) (b[p++] | LS8[b[p++]]);
            MaxDistance       = (ushort                ) (b[p++] | LS8[b[p++]]);
            SensorType        = (MAVDistanceSensorFlags) (b[p++]);
            Increment         = (byte                  ) (b[p++]);
            IncrementF        = (float                 ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            AngleOffset       = (float                 ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Frame             = (MAVFrameFlags         ) (b[p++]);            
            return p;
        }
        #endregion

        #region Write Buffer
        /// <summary>
        /// Writes the message data into a Buffer
        /// </summary>    
        public int Write(byte[] p_buffer,int p_offset=0) {
            int    l = 167;
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
            for(int i=0;i< 72;i++) {
                b[p++] = (byte)(      Distances[i]);
                b[p++] = (byte)((int)Distances[i]>>8 );
            }
            b[p++] = (byte)(      MinDistance);
            b[p++] = (byte)((int)MinDistance>>8 );
            b[p++] = (byte)(      MaxDistance);
            b[p++] = (byte)((int)MaxDistance>>8 );
            b[p++] = (byte)(SensorType);
            b[p++] = (byte)(Increment);
            MemoryMarshal.Write(b.Slice(p, 4), ref IncrementF       ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref AngleOffset      ); p+=4;
            b[p++] = (byte)(Frame);
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
            int l = 167;
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
