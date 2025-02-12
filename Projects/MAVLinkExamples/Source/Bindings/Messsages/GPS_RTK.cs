        
using System.IO;
using System.Runtime.InteropServices;

#pragma warning disable CS0675

namespace MAVLinkBindings {

    /// <summary>
    /// RTK GPS data. Gives information on the relative baseline calculation the GPS is reporting
    /// </summary>    
    public struct GpsRtkData : IMAVLinkMessageData {

        /// <summary>
        /// Message Id Associated w/ this Struct
        /// </summary>    
        public int GetId() { return 127; }

        public uint                              TimeLastBaselineMs;       //Time since boot of last baseline message received.
        public uint                              Tow;                      //GPS Time of Week of last baseline
        public int                               BaselineAMm;              //Current baseline in ECEF x or NED north component.
        public int                               BaselineBMm;              //Current baseline in ECEF y or NED east component.
        public int                               BaselineCMm;              //Current baseline in ECEF z or NED down component.
        public uint                              Accuracy;                 //Current estimate of baseline accuracy.
        public int                               IarNumHypotheses;         //Current number of integer ambiguity hypotheses.
        public ushort                            Wn;                       //GPS Week Number of last baseline
        public byte                              RtkReceiverId;            //Identification of connected RTK receiver.
        public byte                              RtkHealth;                //GPS-specific health report for RTK data.
        public byte                              RtkRate;                  //Rate of baseline messages being received by GPS
        public byte                              Nsats;                    //Current number of sats used for RTK calculation.
        public RtkBaselineCoordinateSystemFlags  BaselineCoordsType;       //Coordinate system of baseline    

        #region CTOR
        /// <summary>
        /// Instantiates a new GpsRtkData
        /// </summary>    
        public GpsRtkData() {
            TimeLastBaselineMs         = default(uint                            );
            Tow                        = default(uint                            );
            BaselineAMm                = default(int                             );
            BaselineBMm                = default(int                             );
            BaselineCMm                = default(int                             );
            Accuracy                   = default(uint                            );
            IarNumHypotheses           = default(int                             );
            Wn                         = default(ushort                          );
            RtkReceiverId              = default(byte                            );
            RtkHealth                  = default(byte                            );
            RtkRate                    = default(byte                            );
            Nsats                      = default(byte                            );
            BaselineCoordsType         = default(RtkBaselineCoordinateSystemFlags);
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
            TimeLastBaselineMs         = (uint                            ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            Tow                        = (uint                            ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            BaselineAMm                = (int                             ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            BaselineBMm                = (int                             ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            BaselineCMm                = (int                             ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            Accuracy                   = (uint                            ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            IarNumHypotheses           = (int                             ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            Wn                         = (ushort                          ) (b[p++] | LS8[b[p++]]);
            RtkReceiverId              = (byte                            ) (b[p++]);
            RtkHealth                  = (byte                            ) (b[p++]);
            RtkRate                    = (byte                            ) (b[p++]);
            Nsats                      = (byte                            ) (b[p++]);
            BaselineCoordsType         = (RtkBaselineCoordinateSystemFlags) (b[p++]);            
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
            b[p++] = (byte)(      TimeLastBaselineMs);
            b[p++] = (byte)((int)TimeLastBaselineMs>>8 );
            b[p++] = (byte)((int)TimeLastBaselineMs>>16);
            b[p++] = (byte)((int)TimeLastBaselineMs>>24);
            b[p++] = (byte)(      Tow);
            b[p++] = (byte)((int)Tow>>8 );
            b[p++] = (byte)((int)Tow>>16);
            b[p++] = (byte)((int)Tow>>24);
            b[p++] = (byte)(      BaselineAMm);
            b[p++] = (byte)((int)BaselineAMm>>8 );
            b[p++] = (byte)((int)BaselineAMm>>16);
            b[p++] = (byte)((int)BaselineAMm>>24);
            b[p++] = (byte)(      BaselineBMm);
            b[p++] = (byte)((int)BaselineBMm>>8 );
            b[p++] = (byte)((int)BaselineBMm>>16);
            b[p++] = (byte)((int)BaselineBMm>>24);
            b[p++] = (byte)(      BaselineCMm);
            b[p++] = (byte)((int)BaselineCMm>>8 );
            b[p++] = (byte)((int)BaselineCMm>>16);
            b[p++] = (byte)((int)BaselineCMm>>24);
            b[p++] = (byte)(      Accuracy);
            b[p++] = (byte)((int)Accuracy>>8 );
            b[p++] = (byte)((int)Accuracy>>16);
            b[p++] = (byte)((int)Accuracy>>24);
            b[p++] = (byte)(      IarNumHypotheses);
            b[p++] = (byte)((int)IarNumHypotheses>>8 );
            b[p++] = (byte)((int)IarNumHypotheses>>16);
            b[p++] = (byte)((int)IarNumHypotheses>>24);
            b[p++] = (byte)(      Wn);
            b[p++] = (byte)((int)Wn>>8 );
            b[p++] = (byte)(RtkReceiverId);
            b[p++] = (byte)(RtkHealth);
            b[p++] = (byte)(RtkRate);
            b[p++] = (byte)(Nsats);
            b[p++] = (byte)(BaselineCoordsType);
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
