        
using System.IO;
using System.Runtime.InteropServices;
using MAVLinkSharp.Runtime;

#pragma warning disable CS0675

namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// Optical flow from a flow sensor (e.g. optical mouse sensor)
    /// </summary>    
    public struct OpticalFlowData : IMAVLinkMessageData {

        /// <summary>
        /// Message Id Associated w/ this Struct
        /// </summary>    
        public int GetId() { return 100; }

        public ulong  TimeUsec;           //Timestamp (UNIX Epoch time or time since system boot). The receiving end can infer timestamp format (since 1.1.1970 or since system boot) by checking for the magnitude of the number.
        public float  FlowCompMX;         //Flow in x-sensor direction, angular-speed compensated
        public float  FlowCompMY;         //Flow in y-sensor direction, angular-speed compensated
        public float  GroundDistance;     //Ground distance. Positive value: distance known. Negative value: Unknown distance
        public short  FlowX;              //Flow in x-sensor direction
        public short  FlowY;              //Flow in y-sensor direction
        public byte   SensorId;           //Sensor ID
        public byte   Quality;            //Optical flow quality / confidence. 0: bad, 255: maximum quality
        public float  FlowRateX;          //Flow rate about X axis
        public float  FlowRateY;          //Flow rate about Y axis    

        #region CTOR
        /// <summary>
        /// Instantiates a new OpticalFlowData
        /// </summary>    
        public OpticalFlowData() {
            TimeUsec             = default(ulong);
            FlowCompMX           = default(float);
            FlowCompMY           = default(float);
            GroundDistance       = default(float);
            FlowX                = default(short);
            FlowY                = default(short);
            SensorId             = default(byte );
            Quality              = default(byte );
            FlowRateX            = default(float);
            FlowRateY            = default(float);
        }
        #endregion

        #region Read Buffer
        /// <summary>
        /// Reads the data from Buffer into this struct
        /// </summary>    
        public int Read(byte[] p_buffer,int p_offset=0) {
            int    l = 34;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals
            int[]  LS8  = MAVLinkCRC.U8_LSH8, LS16 = MAVLinkCRC.U8_LSH16, LS24 = MAVLinkCRC.U8_LSH24, LS32 = MAVLinkCRC.U8_LSH32, LS40 = MAVLinkCRC.U8_LSH40, LS48 = MAVLinkCRC.U8_LSH48, LS56 = MAVLinkCRC.U8_LSH56;
            Span<byte> b = p_buffer.AsSpan(p_offset);            
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            TimeUsec             = (ulong) ((ulong)b[p++] | (ulong)LS8[b[p++]] | (ulong)LS16[b[p++]] | (ulong)LS24[b[p++]] | (ulong)LS32[b[p++]] | (ulong)LS40[b[p++]] | (ulong)LS48[b[p++]] | (ulong)LS56[b[p++]]);
            FlowCompMX           = (float) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            FlowCompMY           = (float) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            GroundDistance       = (float) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            FlowX                = (short) (b[p++] | LS8[b[p++]]);
            FlowY                = (short) (b[p++] | LS8[b[p++]]);
            SensorId             = (byte ) (b[p++]);
            Quality              = (byte ) (b[p++]);
            FlowRateX            = (float) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            FlowRateY            = (float) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;            
            return p;
        }
        #endregion

        #region Write Buffer
        /// <summary>
        /// Writes the message data into a Buffer
        /// </summary>    
        public int Write(byte[] p_buffer,int p_offset=0) {
            int    l = 34;
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
            MemoryMarshal.Write(b.Slice(p, 4), ref FlowCompMX          ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref FlowCompMY          ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref GroundDistance      ); p+=4;
            b[p++] = (byte)(      FlowX);
            b[p++] = (byte)((int)FlowX>>8 );
            b[p++] = (byte)(      FlowY);
            b[p++] = (byte)((int)FlowY>>8 );
            b[p++] = (byte)(SensorId);
            b[p++] = (byte)(Quality);
            MemoryMarshal.Write(b.Slice(p, 4), ref FlowRateX           ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref FlowRateY           ); p+=4;
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
            int l = 34;
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
