        
using System.IO;
using System.Runtime.InteropServices;

#pragma warning disable CS0675

namespace MAVLinkBindings {

    /// <summary>
    /// Data for filling the OpenDroneID System message. The System Message contains general system information including the operator location/altitude and possible aircraft group and/or category/class information.
    /// </summary>    
    public struct OpenDroneIdSystemData : IMAVLinkMessageData {

        /// <summary>
        /// Message Id Associated w/ this Struct
        /// </summary>    
        public int GetId() { return 12904; }

        public int                               OperatorLatitude;          //Latitude of the operator. If unknown: 0 (both Lat/Lon).
        public int                               OperatorLongitude;         //Longitude of the operator. If unknown: 0 (both Lat/Lon).
        public float                             AreaCeiling;               //Area Operations Ceiling relative to WGS84. If unknown: -1000 m.
        public float                             AreaFloor;                 //Area Operations Floor relative to WGS84. If unknown: -1000 m.
        public float                             OperatorAltitudeGeo;       //Geodetic altitude of the operator relative to WGS84. If unknown: -1000 m.
        public uint                              Timestamp;                 //32 bit Unix Timestamp in seconds since 00:00:00 01/01/2019.
        public ushort                            AreaCount;                 //Number of aircraft in the area, group or formation (default 1).
        public ushort                            AreaRadius;                //Radius of the cylindrical area of the group or formation (default 0).
        public byte                              TargetSystem;              //System ID (0 for broadcast).
        public byte                              TargetComponent;           //Component ID (0 for broadcast).
        public byte[]                            IdOrMac;                   //Only used for drone ID data received from other UAs. See detailed description at https://mavlink.io/en/services/opendroneid.html.
        public MAVOdidOperatorLocationTypeFlags  OperatorLocationType;      //Specifies the operator location type.
        public MAVOdidClassificationTypeFlags    ClassificationType;        //Specifies the classification type of the UA.
        public MAVOdidCategoryEuFlags            CategoryEu;                //When classification_type is MAV_ODID_CLASSIFICATION_TYPE_EU, specifies the category of the UA.
        public MAVOdidClassEuFlags               ClassEu;                   //When classification_type is MAV_ODID_CLASSIFICATION_TYPE_EU, specifies the class of the UA.    

        #region CTOR
        /// <summary>
        /// Instantiates a new OpenDroneIdSystemData
        /// </summary>    
        public OpenDroneIdSystemData() {
            OperatorLatitude            = default(int                             );
            OperatorLongitude           = default(int                             );
            AreaCeiling                 = default(float                           );
            AreaFloor                   = default(float                           );
            OperatorAltitudeGeo         = default(float                           );
            Timestamp                   = default(uint                            );
            AreaCount                   = default(ushort                          );
            AreaRadius                  = default(ushort                          );
            TargetSystem                = default(byte                            );
            TargetComponent             = default(byte                            );
            IdOrMac                     = new byte[ 20];
            OperatorLocationType        = default(MAVOdidOperatorLocationTypeFlags);
            ClassificationType          = default(MAVOdidClassificationTypeFlags  );
            CategoryEu                  = default(MAVOdidCategoryEuFlags          );
            ClassEu                     = default(MAVOdidClassEuFlags             );
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
            OperatorLatitude            = (int                             ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            OperatorLongitude           = (int                             ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            AreaCeiling                 = (float                           ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            AreaFloor                   = (float                           ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            OperatorAltitudeGeo         = (float                           ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Timestamp                   = (uint                            ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            AreaCount                   = (ushort                          ) (b[p++] | LS8[b[p++]]);
            AreaRadius                  = (ushort                          ) (b[p++] | LS8[b[p++]]);
            TargetSystem                = (byte                            ) (b[p++]);
            TargetComponent             = (byte                            ) (b[p++]);
            for(int i=0;i<20 ;i++) { IdOrMac[i]                  = (byte                            ) (b[p++]); }
            OperatorLocationType        = (MAVOdidOperatorLocationTypeFlags) (b[p++]);
            ClassificationType          = (MAVOdidClassificationTypeFlags  ) (b[p++]);
            CategoryEu                  = (MAVOdidCategoryEuFlags          ) (b[p++]);
            ClassEu                     = (MAVOdidClassEuFlags             ) (b[p++]);            
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
            b[p++] = (byte)(      OperatorLatitude);
            b[p++] = (byte)((int)OperatorLatitude>>8 );
            b[p++] = (byte)((int)OperatorLatitude>>16);
            b[p++] = (byte)((int)OperatorLatitude>>24);
            b[p++] = (byte)(      OperatorLongitude);
            b[p++] = (byte)((int)OperatorLongitude>>8 );
            b[p++] = (byte)((int)OperatorLongitude>>16);
            b[p++] = (byte)((int)OperatorLongitude>>24);
            MemoryMarshal.Write(b.Slice(p, 4), in AreaCeiling                ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), in AreaFloor                  ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), in OperatorAltitudeGeo        ); p+=4;
            b[p++] = (byte)(      Timestamp);
            b[p++] = (byte)((int)Timestamp>>8 );
            b[p++] = (byte)((int)Timestamp>>16);
            b[p++] = (byte)((int)Timestamp>>24);
            b[p++] = (byte)(      AreaCount);
            b[p++] = (byte)((int)AreaCount>>8 );
            b[p++] = (byte)(      AreaRadius);
            b[p++] = (byte)((int)AreaRadius>>8 );
            b[p++] = (byte)(TargetSystem);
            b[p++] = (byte)(TargetComponent);
            for(int i=0;i< 20;i++) {
                b[p++] = (byte)(IdOrMac[i]);
            }
            b[p++] = (byte)(OperatorLocationType);
            b[p++] = (byte)(ClassificationType);
            b[p++] = (byte)(CategoryEu);
            b[p++] = (byte)(ClassEu);
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
