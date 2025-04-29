        
using System.IO;
using System.Runtime.InteropServices;
using MAVLinkSharp.Runtime;

#pragma warning disable CS0675

namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// Reports results of completed compass calibration. Sent until MAG_CAL_ACK received.
    /// </summary>    
    public struct MagCalReportData : IMAVLinkMessageData {

        /// <summary>
        /// Message Id Associated w/ this Struct
        /// </summary>    
        public int GetId() { return 192; }

        public float                      Fitness;                   //RMS milligauss residuals.
        public float                      OfsX;                      //X offset.
        public float                      OfsY;                      //Y offset.
        public float                      OfsZ;                      //Z offset.
        public float                      DiagX;                     //X diagonal (matrix 11).
        public float                      DiagY;                     //Y diagonal (matrix 22).
        public float                      DiagZ;                     //Z diagonal (matrix 33).
        public float                      OffdiagX;                  //X off-diagonal (matrix 12 and 21).
        public float                      OffdiagY;                  //Y off-diagonal (matrix 13 and 31).
        public float                      OffdiagZ;                  //Z off-diagonal (matrix 32 and 23).
        public byte                       CompassId;                 //Compass being calibrated.
        public byte                       CalMask;                   //Bitmask of compasses being calibrated.
        public MagCalStatusFlags          CalStatus;                 //Calibration Status.
        public byte                       Autosaved;                 //0=requires a MAV_CMD_DO_ACCEPT_MAG_CAL, 1=saved to parameters.
        public float                      OrientationConfidence;     //Confidence in orientation (higher is better).
        public MAVSensorOrientationFlags  OldOrientation;            //orientation before calibration.
        public MAVSensorOrientationFlags  NewOrientation;            //orientation after calibration.
        public float                      ScaleFactor;               //field radius correction factor    

        #region CTOR
        /// <summary>
        /// Instantiates a new MagCalReportData
        /// </summary>    
        public MagCalReportData() {
            Fitness                     = default(float                    );
            OfsX                        = default(float                    );
            OfsY                        = default(float                    );
            OfsZ                        = default(float                    );
            DiagX                       = default(float                    );
            DiagY                       = default(float                    );
            DiagZ                       = default(float                    );
            OffdiagX                    = default(float                    );
            OffdiagY                    = default(float                    );
            OffdiagZ                    = default(float                    );
            CompassId                   = default(byte                     );
            CalMask                     = default(byte                     );
            CalStatus                   = default(MagCalStatusFlags        );
            Autosaved                   = default(byte                     );
            OrientationConfidence       = default(float                    );
            OldOrientation              = default(MAVSensorOrientationFlags);
            NewOrientation              = default(MAVSensorOrientationFlags);
            ScaleFactor                 = default(float                    );
        }
        #endregion

        #region Read Buffer
        /// <summary>
        /// Reads the data from Buffer into this struct
        /// </summary>    
        public int Read(byte[] p_buffer,int p_offset=0) {
            int    l = 54;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals
            int[]  LS8  = MAVLinkCRC.U8_LSH8, LS16 = MAVLinkCRC.U8_LSH16, LS24 = MAVLinkCRC.U8_LSH24, LS32 = MAVLinkCRC.U8_LSH32, LS40 = MAVLinkCRC.U8_LSH40, LS48 = MAVLinkCRC.U8_LSH48, LS56 = MAVLinkCRC.U8_LSH56;
            Span<byte> b = p_buffer.AsSpan(p_offset);            
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            Fitness                     = (float                    ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            OfsX                        = (float                    ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            OfsY                        = (float                    ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            OfsZ                        = (float                    ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            DiagX                       = (float                    ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            DiagY                       = (float                    ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            DiagZ                       = (float                    ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            OffdiagX                    = (float                    ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            OffdiagY                    = (float                    ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            OffdiagZ                    = (float                    ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            CompassId                   = (byte                     ) (b[p++]);
            CalMask                     = (byte                     ) (b[p++]);
            CalStatus                   = (MagCalStatusFlags        ) (b[p++]);
            Autosaved                   = (byte                     ) (b[p++]);
            OrientationConfidence       = (float                    ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            OldOrientation              = (MAVSensorOrientationFlags) (b[p++]);
            NewOrientation              = (MAVSensorOrientationFlags) (b[p++]);
            ScaleFactor                 = (float                    ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;            
            return p;
        }
        #endregion

        #region Write Buffer
        /// <summary>
        /// Writes the message data into a Buffer
        /// </summary>    
        public int Write(byte[] p_buffer,int p_offset=0) {
            int    l = 54;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals            
            Span<byte> b = p_buffer.AsSpan(p_offset);
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            MemoryMarshal.Write(b.Slice(p, 4), ref Fitness                    ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref OfsX                       ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref OfsY                       ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref OfsZ                       ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref DiagX                      ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref DiagY                      ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref DiagZ                      ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref OffdiagX                   ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref OffdiagY                   ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref OffdiagZ                   ); p+=4;
            b[p++] = (byte)(CompassId);
            b[p++] = (byte)(CalMask);
            b[p++] = (byte)(CalStatus);
            b[p++] = (byte)(Autosaved);
            MemoryMarshal.Write(b.Slice(p, 4), ref OrientationConfidence      ); p+=4;
            b[p++] = (byte)(OldOrientation);
            b[p++] = (byte)(NewOrientation);
            MemoryMarshal.Write(b.Slice(p, 4), ref ScaleFactor                ); p+=4;
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
            int l = 54;
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
