        
using System;
using System.IO;
using System.Runtime.InteropServices;
using MAVLinkSharp.Runtime;

#pragma warning disable CS0675

namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// Data for filling the OpenDroneID Location message. The float data types are 32-bit IEEE 754. The Location message provides the location, altitude, direction and speed of the aircraft.
    /// </summary>    
    public struct OpenDroneIdLocationData : IMAVLinkMessageData {

        /// <summary>
        /// Message Id Associated w/ this Struct
        /// </summary>    
        public int GetId() { return 12901; }

        public int                    Latitude;               //Current latitude of the unmanned aircraft. If unknown: 0 (both Lat/Lon).
        public int                    Longitude;              //Current longitude of the unmanned aircraft. If unknown: 0 (both Lat/Lon).
        public float                  AltitudeBarometric;     //The altitude calculated from the barometric pressure. Reference is against 29.92inHg or 1013.2mb. If unknown: -1000 m.
        public float                  AltitudeGeodetic;       //The geodetic altitude as defined by WGS84. If unknown: -1000 m.
        public float                  Height;                 //The current height of the unmanned aircraft above the take-off location or the ground as indicated by height_reference. If unknown: -1000 m.
        public float                  Timestamp;              //Seconds after the full hour with reference to UTC time. Typically the GPS outputs a time-of-week value in milliseconds. First convert that to UTC and then convert for this field using ((float) (time_week_ms % (60*60*1000))) / 1000. If unknown: 0xFFFF.
        public ushort                 Direction;              //Direction over ground (not heading, but direction of movement) measured clockwise from true North: 0 - 35999 centi-degrees. If unknown: 36100 centi-degrees.
        public ushort                 SpeedHorizontal;        //Ground speed. Positive only. If unknown: 25500 cm/s. If speed is larger than 25425 cm/s, use 25425 cm/s.
        public short                  SpeedVertical;          //The vertical speed. Up is positive. If unknown: 6300 cm/s. If speed is larger than 6200 cm/s, use 6200 cm/s. If lower than -6200 cm/s, use -6200 cm/s.
        public byte                   TargetSystem;           //System ID (0 for broadcast).
        public byte                   TargetComponent;        //Component ID (0 for broadcast).
        public byte[]                 IdOrMac;                //Only used for drone ID data received from other UAs. See detailed description at https://mavlink.io/en/services/opendroneid.html.
        public MAVOdidStatusFlags     Status;                 //Indicates whether the unmanned aircraft is on the ground or in the air.
        public MAVOdidHeightRefFlags  HeightReference;        //Indicates the reference point for the height field.
        public MAVOdidHorAccFlags     HorizontalAccuracy;     //The accuracy of the horizontal position.
        public MAVOdidVerAccFlags     VerticalAccuracy;       //The accuracy of the vertical position.
        public MAVOdidVerAccFlags     BarometerAccuracy;      //The accuracy of the barometric altitude.
        public MAVOdidSpeedAccFlags   SpeedAccuracy;          //The accuracy of the horizontal and vertical speed.
        public MAVOdidTimeAccFlags    TimestampAccuracy;      //The accuracy of the timestamps.    

        #region CTOR
        /// <summary>
        /// Instantiates a new OpenDroneIdLocationData
        /// </summary>    
        /*
        public OpenDroneIdLocationData() {
            Init();
        }
        */
        public void Init() {
            Latitude                 = default(int                  );
            Longitude                = default(int                  );
            AltitudeBarometric       = default(float                );
            AltitudeGeodetic         = default(float                );
            Height                   = default(float                );
            Timestamp                = default(float                );
            Direction                = default(ushort               );
            SpeedHorizontal          = default(ushort               );
            SpeedVertical            = default(short                );
            TargetSystem             = default(byte                 );
            TargetComponent          = default(byte                 );
            IdOrMac                  = new byte[ 20];
            Status                   = default(MAVOdidStatusFlags   );
            HeightReference          = default(MAVOdidHeightRefFlags);
            HorizontalAccuracy       = default(MAVOdidHorAccFlags   );
            VerticalAccuracy         = default(MAVOdidVerAccFlags   );
            BarometerAccuracy        = default(MAVOdidVerAccFlags   );
            SpeedAccuracy            = default(MAVOdidSpeedAccFlags );
            TimestampAccuracy        = default(MAVOdidTimeAccFlags  );
        }
        #endregion

        #region Read Buffer
        /// <summary>
        /// Reads the data from Buffer into this struct
        /// </summary>    
        public int Read(byte[] p_buffer,int p_offset=0) {
            int    l = 59;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals
            int[]  LS8  = MAVLinkCRC.U8_LSH8, LS16 = MAVLinkCRC.U8_LSH16, LS24 = MAVLinkCRC.U8_LSH24, LS32 = MAVLinkCRC.U8_LSH32, LS40 = MAVLinkCRC.U8_LSH40, LS48 = MAVLinkCRC.U8_LSH48, LS56 = MAVLinkCRC.U8_LSH56;
            Span<byte> b = p_buffer.AsSpan(p_offset);            
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            Latitude                 = (int                  ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            Longitude                = (int                  ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            AltitudeBarometric       = (float                ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            AltitudeGeodetic         = (float                ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Height                   = (float                ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Timestamp                = (float                ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Direction                = (ushort               ) (b[p++] | LS8[b[p++]]);
            SpeedHorizontal          = (ushort               ) (b[p++] | LS8[b[p++]]);
            SpeedVertical            = (short                ) (b[p++] | LS8[b[p++]]);
            TargetSystem             = (byte                 ) (b[p++]);
            TargetComponent          = (byte                 ) (b[p++]);
            for(int i=0;i<20 ;i++) { IdOrMac[i]               = (byte                 ) (b[p++]); }
            Status                   = (MAVOdidStatusFlags   ) (b[p++]);
            HeightReference          = (MAVOdidHeightRefFlags) (b[p++]);
            HorizontalAccuracy       = (MAVOdidHorAccFlags   ) (b[p++]);
            VerticalAccuracy         = (MAVOdidVerAccFlags   ) (b[p++]);
            BarometerAccuracy        = (MAVOdidVerAccFlags   ) (b[p++]);
            SpeedAccuracy            = (MAVOdidSpeedAccFlags ) (b[p++]);
            TimestampAccuracy        = (MAVOdidTimeAccFlags  ) (b[p++]);            
            return p;
        }
        #endregion

        #region Write Buffer
        /// <summary>
        /// Writes the message data into a Buffer
        /// </summary>    
        public int Write(byte[] p_buffer,int p_offset=0) {
            int    l = 59;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals            
            Span<byte> b = p_buffer.AsSpan(p_offset);
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            b[p++] = (byte)(      Latitude);
            b[p++] = (byte)((int)Latitude>>8 );
            b[p++] = (byte)((int)Latitude>>16);
            b[p++] = (byte)((int)Latitude>>24);
            b[p++] = (byte)(      Longitude);
            b[p++] = (byte)((int)Longitude>>8 );
            b[p++] = (byte)((int)Longitude>>16);
            b[p++] = (byte)((int)Longitude>>24);
            MemoryMarshal.Write(b.Slice(p, 4), ref AltitudeBarometric      ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref AltitudeGeodetic        ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref Height                  ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref Timestamp               ); p+=4;
            b[p++] = (byte)(      Direction);
            b[p++] = (byte)((int)Direction>>8 );
            b[p++] = (byte)(      SpeedHorizontal);
            b[p++] = (byte)((int)SpeedHorizontal>>8 );
            b[p++] = (byte)(      SpeedVertical);
            b[p++] = (byte)((int)SpeedVertical>>8 );
            b[p++] = (byte)(TargetSystem);
            b[p++] = (byte)(TargetComponent);
            for(int i=0;i< 20;i++) {
                b[p++] = (byte)(IdOrMac[i]);
            }
            b[p++] = (byte)(Status);
            b[p++] = (byte)(HeightReference);
            b[p++] = (byte)(HorizontalAccuracy);
            b[p++] = (byte)(VerticalAccuracy);
            b[p++] = (byte)(BarometerAccuracy);
            b[p++] = (byte)(SpeedAccuracy);
            b[p++] = (byte)(TimestampAccuracy);
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
            int l = 59;
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
