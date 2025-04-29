        
using System.IO;
using System.Runtime.InteropServices;
using MAVLinkSharp.Runtime;

#pragma warning disable CS0675

namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// Optical flow from an angular rate flow sensor (e.g. PX4FLOW or mouse sensor)
    /// </summary>    
    public struct OpticalFlowRadData : IMAVLinkMessageData {

        /// <summary>
        /// Message Id Associated w/ this Struct
        /// </summary>    
        public int GetId() { return 106; }

        public ulong  TimeUsec;                  //Timestamp (UNIX Epoch time or time since system boot). The receiving end can infer timestamp format (since 1.1.1970 or since system boot) by checking for the magnitude of the number.
        public uint   IntegrationTimeUs;         //Integration time. Divide integrated_x and integrated_y by the integration time to obtain average flow. The integration time also indicates the.
        public float  IntegratedX;               //Flow around X axis (Sensor RH rotation about the X axis induces a positive flow. Sensor linear motion along the positive Y axis induces a negative flow.)
        public float  IntegratedY;               //Flow around Y axis (Sensor RH rotation about the Y axis induces a positive flow. Sensor linear motion along the positive X axis induces a positive flow.)
        public float  IntegratedXgyro;           //RH rotation around X axis
        public float  IntegratedYgyro;           //RH rotation around Y axis
        public float  IntegratedZgyro;           //RH rotation around Z axis
        public uint   TimeDeltaDistanceUs;       //Time since the distance was sampled.
        public float  Distance;                  //Distance to the center of the flow field. Positive value (including zero): distance known. Negative value: Unknown distance.
        public short  Temperature;               //Temperature
        public byte   SensorId;                  //Sensor ID
        public byte   Quality;                   //Optical flow quality / confidence. 0: no valid flow, 255: maximum quality    

        #region CTOR
        /// <summary>
        /// Instantiates a new OpticalFlowRadData
        /// </summary>    
        /*
        public OpticalFlowRadData() {
            Init();
        }
        */
        public void Init() {
            TimeUsec                    = default(ulong);
            IntegrationTimeUs           = default(uint );
            IntegratedX                 = default(float);
            IntegratedY                 = default(float);
            IntegratedXgyro             = default(float);
            IntegratedYgyro             = default(float);
            IntegratedZgyro             = default(float);
            TimeDeltaDistanceUs         = default(uint );
            Distance                    = default(float);
            Temperature                 = default(short);
            SensorId                    = default(byte );
            Quality                     = default(byte );
        }
        #endregion

        #region Read Buffer
        /// <summary>
        /// Reads the data from Buffer into this struct
        /// </summary>    
        public int Read(byte[] p_buffer,int p_offset=0) {
            int    l = 44;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals
            int[]  LS8  = MAVLinkCRC.U8_LSH8, LS16 = MAVLinkCRC.U8_LSH16, LS24 = MAVLinkCRC.U8_LSH24, LS32 = MAVLinkCRC.U8_LSH32, LS40 = MAVLinkCRC.U8_LSH40, LS48 = MAVLinkCRC.U8_LSH48, LS56 = MAVLinkCRC.U8_LSH56;
            Span<byte> b = p_buffer.AsSpan(p_offset);            
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            TimeUsec                    = (ulong) ((ulong)b[p++] | (ulong)LS8[b[p++]] | (ulong)LS16[b[p++]] | (ulong)LS24[b[p++]] | (ulong)LS32[b[p++]] | (ulong)LS40[b[p++]] | (ulong)LS48[b[p++]] | (ulong)LS56[b[p++]]);
            IntegrationTimeUs           = (uint ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            IntegratedX                 = (float) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            IntegratedY                 = (float) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            IntegratedXgyro             = (float) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            IntegratedYgyro             = (float) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            IntegratedZgyro             = (float) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            TimeDeltaDistanceUs         = (uint ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            Distance                    = (float) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Temperature                 = (short) (b[p++] | LS8[b[p++]]);
            SensorId                    = (byte ) (b[p++]);
            Quality                     = (byte ) (b[p++]);            
            return p;
        }
        #endregion

        #region Write Buffer
        /// <summary>
        /// Writes the message data into a Buffer
        /// </summary>    
        public int Write(byte[] p_buffer,int p_offset=0) {
            int    l = 44;
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
            b[p++] = (byte)(      IntegrationTimeUs);
            b[p++] = (byte)((int)IntegrationTimeUs>>8 );
            b[p++] = (byte)((int)IntegrationTimeUs>>16);
            b[p++] = (byte)((int)IntegrationTimeUs>>24);
            MemoryMarshal.Write(b.Slice(p, 4), ref IntegratedX                ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref IntegratedY                ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref IntegratedXgyro            ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref IntegratedYgyro            ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref IntegratedZgyro            ); p+=4;
            b[p++] = (byte)(      TimeDeltaDistanceUs);
            b[p++] = (byte)((int)TimeDeltaDistanceUs>>8 );
            b[p++] = (byte)((int)TimeDeltaDistanceUs>>16);
            b[p++] = (byte)((int)TimeDeltaDistanceUs>>24);
            MemoryMarshal.Write(b.Slice(p, 4), ref Distance                   ); p+=4;
            b[p++] = (byte)(      Temperature);
            b[p++] = (byte)((int)Temperature>>8 );
            b[p++] = (byte)(SensorId);
            b[p++] = (byte)(Quality);
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
            int l = 44;
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
