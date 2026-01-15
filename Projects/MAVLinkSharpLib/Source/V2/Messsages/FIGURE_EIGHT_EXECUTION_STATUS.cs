        
using System;
using System.IO;
using System.Runtime.InteropServices;
using MAVLinkSharp.Runtime;

#pragma warning disable CS0675

namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// 
    /// Vehicle status report that is sent out while figure eight execution is in progress (see MAV_CMD_DO_FIGURE_EIGHT).
    /// This may typically send at low rates: of the order of 2Hz.
    /// 
    /// </summary>    
    public struct FigureEightExecutionStatusData : IMAVLinkMessageData {

        /// <summary>
        /// Message Id Associated w/ this Struct
        /// </summary>    
        public int GetId() { return 361; }

        public ulong          TimeUsec;        //Timestamp (UNIX Epoch time or time since system boot). The receiving end can infer timestamp format (since 1.1.1970 or since system boot) by checking for the magnitude of the number.
        public float          MajorRadius;     //Major axis radius of the figure eight. Positive: orbit the north circle clockwise. Negative: orbit the north circle counter-clockwise.
        public float          MinorRadius;     //Minor axis radius of the figure eight. Defines the radius of two circles that make up the figure.
        public float          Orientation;     //Orientation of the figure eight major axis with respect to true north in [-pi,pi).
        public int            X;               //X coordinate of center point. Coordinate system depends on frame field.
        public int            Y;               //Y coordinate of center point. Coordinate system depends on frame field.
        public float          Z;               //Altitude of center point. Coordinate system depends on frame field.
        public MAVFrameFlags  Frame;           //The coordinate system of the fields: x, y, z.    

        #region CTOR
        /// <summary>
        /// Instantiates a new FigureEightExecutionStatusData
        /// </summary>    
        /*
        public FigureEightExecutionStatusData() {
            Init();
        }
        */
        public void Init() {
            TimeUsec          = default(ulong        );
            MajorRadius       = default(float        );
            MinorRadius       = default(float        );
            Orientation       = default(float        );
            X                 = default(int          );
            Y                 = default(int          );
            Z                 = default(float        );
            Frame             = default(MAVFrameFlags);
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
            TimeUsec          = (ulong        ) ((ulong)b[p++] | (ulong)LS8[b[p++]] | (ulong)LS16[b[p++]] | (ulong)LS24[b[p++]] | (ulong)LS32[b[p++]] | (ulong)LS40[b[p++]] | (ulong)LS48[b[p++]] | (ulong)LS56[b[p++]]);
            MajorRadius       = (float        ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            MinorRadius       = (float        ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Orientation       = (float        ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            X                 = (int          ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            Y                 = (int          ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            Z                 = (float        ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Frame             = (MAVFrameFlags) (b[p++]);            
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
            b[p++] = (byte)(      TimeUsec);
            b[p++] = (byte)((long)TimeUsec>>8 );
            b[p++] = (byte)((long)TimeUsec>>16);
            b[p++] = (byte)((long)TimeUsec>>24);
            b[p++] = (byte)((long)TimeUsec>>32);
            b[p++] = (byte)((long)TimeUsec>>40);
            b[p++] = (byte)((long)TimeUsec>>48);
            b[p++] = (byte)((long)TimeUsec>>56);
            MemoryMarshal.Write(b.Slice(p, 4), ref MajorRadius      ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref MinorRadius      ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref Orientation      ); p+=4;
            b[p++] = (byte)(      X);
            b[p++] = (byte)((int)X>>8 );
            b[p++] = (byte)((int)X>>16);
            b[p++] = (byte)((int)X>>24);
            b[p++] = (byte)(      Y);
            b[p++] = (byte)((int)Y>>8 );
            b[p++] = (byte)((int)Y>>16);
            b[p++] = (byte)((int)Y>>24);
            MemoryMarshal.Write(b.Slice(p, 4), ref Z                ); p+=4;
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
