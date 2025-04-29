        
using System;
using System.IO;
using System.Runtime.InteropServices;
using MAVLinkSharp.Runtime;

#pragma warning disable CS0675

namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// Update the data in the OPEN_DRONE_ID_SYSTEM message with new location information. This can be sent to update the location information for the operator when no other information in the SYSTEM message has changed. This message allows for efficient operation on radio links which have limited uplink bandwidth while meeting requirements for update frequency of the operator location.
    /// </summary>    
    public struct OpenDroneIdSystemUpdateData : IMAVLinkMessageData {

        /// <summary>
        /// Message Id Associated w/ this Struct
        /// </summary>    
        public int GetId() { return 12919; }

        public int    OperatorLatitude;         //Latitude of the operator. If unknown: 0 (both Lat/Lon).
        public int    OperatorLongitude;        //Longitude of the operator. If unknown: 0 (both Lat/Lon).
        public float  OperatorAltitudeGeo;      //Geodetic altitude of the operator relative to WGS84. If unknown: -1000 m.
        public uint   Timestamp;                //32 bit Unix Timestamp in seconds since 00:00:00 01/01/2019.
        public byte   TargetSystem;             //System ID (0 for broadcast).
        public byte   TargetComponent;          //Component ID (0 for broadcast).    

        #region CTOR
        /// <summary>
        /// Instantiates a new OpenDroneIdSystemUpdateData
        /// </summary>    
        /*
        public OpenDroneIdSystemUpdateData() {
            Init();
        }
        */
        public void Init() {
            OperatorLatitude           = default(int  );
            OperatorLongitude          = default(int  );
            OperatorAltitudeGeo        = default(float);
            Timestamp                  = default(uint );
            TargetSystem               = default(byte );
            TargetComponent            = default(byte );
        }
        #endregion

        #region Read Buffer
        /// <summary>
        /// Reads the data from Buffer into this struct
        /// </summary>    
        public int Read(byte[] p_buffer,int p_offset=0) {
            int    l = 18;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals
            int[]  LS8  = MAVLinkCRC.U8_LSH8, LS16 = MAVLinkCRC.U8_LSH16, LS24 = MAVLinkCRC.U8_LSH24, LS32 = MAVLinkCRC.U8_LSH32, LS40 = MAVLinkCRC.U8_LSH40, LS48 = MAVLinkCRC.U8_LSH48, LS56 = MAVLinkCRC.U8_LSH56;
            Span<byte> b = p_buffer.AsSpan(p_offset);            
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            OperatorLatitude           = (int  ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            OperatorLongitude          = (int  ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            OperatorAltitudeGeo        = (float) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Timestamp                  = (uint ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            TargetSystem               = (byte ) (b[p++]);
            TargetComponent            = (byte ) (b[p++]);            
            return p;
        }
        #endregion

        #region Write Buffer
        /// <summary>
        /// Writes the message data into a Buffer
        /// </summary>    
        public int Write(byte[] p_buffer,int p_offset=0) {
            int    l = 18;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals            
            Span<byte> b = p_buffer.AsSpan(p_offset);
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            b[p++] = (byte)(      OperatorLatitude);
            b[p++] = (byte)((int)OperatorLatitude>>8 );
            b[p++] = (byte)((int)OperatorLatitude>>16);
            b[p++] = (byte)((int)OperatorLatitude>>24);
            b[p++] = (byte)(      OperatorLongitude);
            b[p++] = (byte)((int)OperatorLongitude>>8 );
            b[p++] = (byte)((int)OperatorLongitude>>16);
            b[p++] = (byte)((int)OperatorLongitude>>24);
            MemoryMarshal.Write(b.Slice(p, 4), ref OperatorAltitudeGeo       ); p+=4;
            b[p++] = (byte)(      Timestamp);
            b[p++] = (byte)((int)Timestamp>>8 );
            b[p++] = (byte)((int)Timestamp>>16);
            b[p++] = (byte)((int)Timestamp>>24);
            b[p++] = (byte)(TargetSystem);
            b[p++] = (byte)(TargetComponent);
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
            int l = 18;
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
