        
using System;
using System.IO;
using System.Runtime.InteropServices;
using MAVLinkSharp.Runtime;

#pragma warning disable CS0675

namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// Status of simulation environment, if used
    /// </summary>    
    public struct SimStateData : IMAVLinkMessageData {

        /// <summary>
        /// Message Id Associated w/ this Struct
        /// </summary>    
        public int GetId() { return 108; }

        public float  Q1;              //True attitude quaternion component 1, w (1 in null-rotation)
        public float  Q2;              //True attitude quaternion component 2, x (0 in null-rotation)
        public float  Q3;              //True attitude quaternion component 3, y (0 in null-rotation)
        public float  Q4;              //True attitude quaternion component 4, z (0 in null-rotation)
        public float  Roll;            //Attitude roll expressed as Euler angles, not recommended except for human-readable outputs
        public float  Pitch;           //Attitude pitch expressed as Euler angles, not recommended except for human-readable outputs
        public float  Yaw;             //Attitude yaw expressed as Euler angles, not recommended except for human-readable outputs
        public float  Xacc;            //X acceleration
        public float  Yacc;            //Y acceleration
        public float  Zacc;            //Z acceleration
        public float  Xgyro;           //Angular speed around X axis
        public float  Ygyro;           //Angular speed around Y axis
        public float  Zgyro;           //Angular speed around Z axis
        public float  Lat;             //Latitude (lower precision). Both this and the lat_int field should be set.
        public float  Lon;             //Longitude (lower precision). Both this and the lon_int field should be set.
        public float  Alt;             //Altitude
        public float  StdDevHorz;      //Horizontal position standard deviation
        public float  StdDevVert;      //Vertical position standard deviation
        public float  Vn;              //True velocity in north direction in earth-fixed NED frame
        public float  Ve;              //True velocity in east direction in earth-fixed NED frame
        public float  Vd;              //True velocity in down direction in earth-fixed NED frame
        public int    LatInt;          //Latitude (higher precision). If 0, recipients should use the lat field value (otherwise this field is preferred).
        public int    LonInt;          //Longitude (higher precision). If 0, recipients should use the lon field value (otherwise this field is preferred).    

        #region CTOR
        /// <summary>
        /// Instantiates a new SimStateData
        /// </summary>    
        /*
        public SimStateData() {
            Init();
        }
        */
        public void Init() {
            Q1                = default(float);
            Q2                = default(float);
            Q3                = default(float);
            Q4                = default(float);
            Roll              = default(float);
            Pitch             = default(float);
            Yaw               = default(float);
            Xacc              = default(float);
            Yacc              = default(float);
            Zacc              = default(float);
            Xgyro             = default(float);
            Ygyro             = default(float);
            Zgyro             = default(float);
            Lat               = default(float);
            Lon               = default(float);
            Alt               = default(float);
            StdDevHorz        = default(float);
            StdDevVert        = default(float);
            Vn                = default(float);
            Ve                = default(float);
            Vd                = default(float);
            LatInt            = default(int  );
            LonInt            = default(int  );
        }
        #endregion

        #region Read Buffer
        /// <summary>
        /// Reads the data from Buffer into this struct
        /// </summary>    
        public int Read(byte[] p_buffer,int p_offset=0) {
            int    l = 92;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals
            int[]  LS8  = MAVLinkCRC.U8_LSH8, LS16 = MAVLinkCRC.U8_LSH16, LS24 = MAVLinkCRC.U8_LSH24, LS32 = MAVLinkCRC.U8_LSH32, LS40 = MAVLinkCRC.U8_LSH40, LS48 = MAVLinkCRC.U8_LSH48, LS56 = MAVLinkCRC.U8_LSH56;
            Span<byte> b = p_buffer.AsSpan(p_offset);            
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            Q1                = (float) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Q2                = (float) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Q3                = (float) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Q4                = (float) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Roll              = (float) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Pitch             = (float) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Yaw               = (float) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Xacc              = (float) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Yacc              = (float) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Zacc              = (float) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Xgyro             = (float) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Ygyro             = (float) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Zgyro             = (float) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Lat               = (float) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Lon               = (float) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Alt               = (float) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            StdDevHorz        = (float) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            StdDevVert        = (float) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Vn                = (float) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Ve                = (float) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Vd                = (float) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            LatInt            = (int  ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            LonInt            = (int  ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);            
            return p;
        }
        #endregion

        #region Write Buffer
        /// <summary>
        /// Writes the message data into a Buffer
        /// </summary>    
        public int Write(byte[] p_buffer,int p_offset=0) {
            int    l = 92;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals            
            Span<byte> b = p_buffer.AsSpan(p_offset);
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            MemoryMarshal.Write(b.Slice(p, 4), ref Q1               ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref Q2               ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref Q3               ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref Q4               ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref Roll             ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref Pitch            ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref Yaw              ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref Xacc             ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref Yacc             ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref Zacc             ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref Xgyro            ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref Ygyro            ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref Zgyro            ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref Lat              ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref Lon              ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref Alt              ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref StdDevHorz       ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref StdDevVert       ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref Vn               ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref Ve               ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref Vd               ); p+=4;
            b[p++] = (byte)(      LatInt);
            b[p++] = (byte)((int)LatInt>>8 );
            b[p++] = (byte)((int)LatInt>>16);
            b[p++] = (byte)((int)LatInt>>24);
            b[p++] = (byte)(      LonInt);
            b[p++] = (byte)((int)LonInt>>8 );
            b[p++] = (byte)((int)LonInt>>16);
            b[p++] = (byte)((int)LonInt>>24);
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
            int l = 92;
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
