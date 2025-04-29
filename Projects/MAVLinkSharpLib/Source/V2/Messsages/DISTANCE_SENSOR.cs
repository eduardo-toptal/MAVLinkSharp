        
using System;
using System.IO;
using System.Runtime.InteropServices;
using MAVLinkSharp.Runtime;

#pragma warning disable CS0675

namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// Distance sensor information for an onboard rangefinder.
    /// </summary>    
    public struct DistanceSensorData : IMAVLinkMessageData {

        /// <summary>
        /// Message Id Associated w/ this Struct
        /// </summary>    
        public int GetId() { return 132; }

        public uint                       TimeBootMs;          //Timestamp (time since system boot).
        public ushort                     MinDistance;         //Minimum distance the sensor can measure
        public ushort                     MaxDistance;         //Maximum distance the sensor can measure
        public ushort                     CurrentDistance;     //Current distance reading
        public MAVDistanceSensorFlags     Type;                //Type of distance sensor.
        public byte                       Id;                  //Onboard ID of the sensor
        public MAVSensorOrientationFlags  Orientation;         //Direction the sensor faces. downward-facing: ROTATION_PITCH_270, upward-facing: ROTATION_PITCH_90, backward-facing: ROTATION_PITCH_180, forward-facing: ROTATION_NONE, left-facing: ROTATION_YAW_90, right-facing: ROTATION_YAW_270
        public byte                       Covariance;          //Measurement variance. Max standard deviation is 6cm. UINT8_MAX if unknown.
        public float                      HorizontalFov;       //Horizontal Field of View (angle) where the distance measurement is valid and the field of view is known. Otherwise this is set to 0.
        public float                      VerticalFov;         //Vertical Field of View (angle) where the distance measurement is valid and the field of view is known. Otherwise this is set to 0.
        public float[]                    Quaternion;          //Quaternion of the sensor orientation in vehicle body frame (w, x, y, z order, zero-rotation is 1, 0, 0, 0). Zero-rotation is along the vehicle body x-axis. This field is required if the orientation is set to MAV_SENSOR_ROTATION_CUSTOM. Set it to 0 if invalid."
        public byte                       SignalQuality;       //Signal quality of the sensor. Specific to each sensor type, representing the relation of the signal strength with the target reflectivity, distance, size or aspect, but normalised as a percentage. 0 = unknown/unset signal quality, 1 = invalid signal, 100 = perfect signal.    

        #region CTOR
        /// <summary>
        /// Instantiates a new DistanceSensorData
        /// </summary>    
        /*
        public DistanceSensorData() {
            Init();
        }
        */
        public void Init() {
            TimeBootMs            = default(uint                     );
            MinDistance           = default(ushort                   );
            MaxDistance           = default(ushort                   );
            CurrentDistance       = default(ushort                   );
            Type                  = default(MAVDistanceSensorFlags   );
            Id                    = default(byte                     );
            Orientation           = default(MAVSensorOrientationFlags);
            Covariance            = default(byte                     );
            HorizontalFov         = default(float                    );
            VerticalFov           = default(float                    );
            Quaternion            = new float[  4];
            SignalQuality         = default(byte                     );
        }
        #endregion

        #region Read Buffer
        /// <summary>
        /// Reads the data from Buffer into this struct
        /// </summary>    
        public int Read(byte[] p_buffer,int p_offset=0) {
            int    l = 39;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals
            int[]  LS8  = MAVLinkCRC.U8_LSH8, LS16 = MAVLinkCRC.U8_LSH16, LS24 = MAVLinkCRC.U8_LSH24, LS32 = MAVLinkCRC.U8_LSH32, LS40 = MAVLinkCRC.U8_LSH40, LS48 = MAVLinkCRC.U8_LSH48, LS56 = MAVLinkCRC.U8_LSH56;
            Span<byte> b = p_buffer.AsSpan(p_offset);            
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            TimeBootMs            = (uint                     ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            MinDistance           = (ushort                   ) (b[p++] | LS8[b[p++]]);
            MaxDistance           = (ushort                   ) (b[p++] | LS8[b[p++]]);
            CurrentDistance       = (ushort                   ) (b[p++] | LS8[b[p++]]);
            Type                  = (MAVDistanceSensorFlags   ) (b[p++]);
            Id                    = (byte                     ) (b[p++]);
            Orientation           = (MAVSensorOrientationFlags) (b[p++]);
            Covariance            = (byte                     ) (b[p++]);
            HorizontalFov         = (float                    ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            VerticalFov           = (float                    ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            for(int i=0;i<4  ;i++) { Quaternion[i]         = (float                    ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4; }
            SignalQuality         = (byte                     ) (b[p++]);            
            return p;
        }
        #endregion

        #region Write Buffer
        /// <summary>
        /// Writes the message data into a Buffer
        /// </summary>    
        public int Write(byte[] p_buffer,int p_offset=0) {
            int    l = 39;
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
            b[p++] = (byte)(      MinDistance);
            b[p++] = (byte)((int)MinDistance>>8 );
            b[p++] = (byte)(      MaxDistance);
            b[p++] = (byte)((int)MaxDistance>>8 );
            b[p++] = (byte)(      CurrentDistance);
            b[p++] = (byte)((int)CurrentDistance>>8 );
            b[p++] = (byte)(Type);
            b[p++] = (byte)(Id);
            b[p++] = (byte)(Orientation);
            b[p++] = (byte)(Covariance);
            MemoryMarshal.Write(b.Slice(p, 4), ref HorizontalFov        ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref VerticalFov          ); p+=4;
            for(int i=0;i<  4;i++) {
                MemoryMarshal.Write(b.Slice(p, 4), ref Quaternion[i]        ); p+=4;
            }
            b[p++] = (byte)(SignalQuality);
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
            int l = 39;
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
