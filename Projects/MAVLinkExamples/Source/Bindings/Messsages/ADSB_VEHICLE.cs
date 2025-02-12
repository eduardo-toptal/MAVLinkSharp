        
using System.IO;
using System.Runtime.InteropServices;

#pragma warning disable CS0675

namespace MAVLinkBindings {

    /// <summary>
    /// The location and information of an ADSB vehicle
    /// </summary>    
    public struct AdsbVehicleData : IMAVLinkMessageData {

        /// <summary>
        /// Message Id Associated w/ this Struct
        /// </summary>    
        public int GetId() { return 246; }

        public uint                   IcaoAddress;      //ICAO address
        public int                    Lat;              //Latitude
        public int                    Lon;              //Longitude
        public int                    Altitude;         //Altitude(ASL)
        public ushort                 Heading;          //Course over ground
        public ushort                 HorVelocity;      //The horizontal velocity
        public short                  VerVelocity;      //The vertical velocity. Positive is up
        public AdsbFlags              Flags;            //Bitmap to indicate various statuses including valid data fields
        public ushort                 Squawk;           //Squawk code
        public AdsbAltitudeTypeFlags  AltitudeType;     //ADSB altitude type.
        public char[]                 Callsign;         //The callsign, 8+null
        public AdsbEmitterTypeFlags   EmitterType;      //ADSB emitter type.
        public byte                   Tslc;             //Time since last communication in seconds    

        #region CTOR
        /// <summary>
        /// Instantiates a new AdsbVehicleData
        /// </summary>    
        public AdsbVehicleData() {
            IcaoAddress        = default(uint                 );
            Lat                = default(int                  );
            Lon                = default(int                  );
            Altitude           = default(int                  );
            Heading            = default(ushort               );
            HorVelocity        = default(ushort               );
            VerVelocity        = default(short                );
            Flags              = default(AdsbFlags            );
            Squawk             = default(ushort               );
            AltitudeType       = default(AdsbAltitudeTypeFlags);
            Callsign           = new char[  9];
            EmitterType        = default(AdsbEmitterTypeFlags );
            Tslc               = default(byte                 );
        }
        #endregion

        #region Read Buffer
        /// <summary>
        /// Reads the data from Buffer into this struct
        /// </summary>    
        public int Read(byte[] p_buffer,int p_offset=0) {
            int    l = 38;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals
            int[]  LS8  = MAVLinkCRC.U8_LSH8, LS16 = MAVLinkCRC.U8_LSH16, LS24 = MAVLinkCRC.U8_LSH24, LS32 = MAVLinkCRC.U8_LSH32, LS40 = MAVLinkCRC.U8_LSH40, LS48 = MAVLinkCRC.U8_LSH48, LS56 = MAVLinkCRC.U8_LSH56;
            Span<byte> b = p_buffer.AsSpan(p_offset);            
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            IcaoAddress        = (uint                 ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            Lat                = (int                  ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            Lon                = (int                  ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            Altitude           = (int                  ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            Heading            = (ushort               ) (b[p++] | LS8[b[p++]]);
            HorVelocity        = (ushort               ) (b[p++] | LS8[b[p++]]);
            VerVelocity        = (short                ) (b[p++] | LS8[b[p++]]);
            Flags              = (AdsbFlags            ) (b[p++] | LS8[b[p++]]);
            Squawk             = (ushort               ) (b[p++] | LS8[b[p++]]);
            AltitudeType       = (AdsbAltitudeTypeFlags) (b[p++]);
            for(int i=0;i<9  ;i++) { Callsign[i]        = (char                 ) (b[p++]); }
            EmitterType        = (AdsbEmitterTypeFlags ) (b[p++]);
            Tslc               = (byte                 ) (b[p++]);            
            return p;
        }
        #endregion

        #region Write Buffer
        /// <summary>
        /// Writes the message data into a Buffer
        /// </summary>    
        public int Write(byte[] p_buffer,int p_offset=0) {
            int    l = 38;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals            
            Span<byte> b = p_buffer.AsSpan(p_offset);
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            b[p++] = (byte)(      IcaoAddress);
            b[p++] = (byte)((int)IcaoAddress>>8 );
            b[p++] = (byte)((int)IcaoAddress>>16);
            b[p++] = (byte)((int)IcaoAddress>>24);
            b[p++] = (byte)(      Lat);
            b[p++] = (byte)((int)Lat>>8 );
            b[p++] = (byte)((int)Lat>>16);
            b[p++] = (byte)((int)Lat>>24);
            b[p++] = (byte)(      Lon);
            b[p++] = (byte)((int)Lon>>8 );
            b[p++] = (byte)((int)Lon>>16);
            b[p++] = (byte)((int)Lon>>24);
            b[p++] = (byte)(      Altitude);
            b[p++] = (byte)((int)Altitude>>8 );
            b[p++] = (byte)((int)Altitude>>16);
            b[p++] = (byte)((int)Altitude>>24);
            b[p++] = (byte)(      Heading);
            b[p++] = (byte)((int)Heading>>8 );
            b[p++] = (byte)(      HorVelocity);
            b[p++] = (byte)((int)HorVelocity>>8 );
            b[p++] = (byte)(      VerVelocity);
            b[p++] = (byte)((int)VerVelocity>>8 );
            b[p++] = (byte)(      Flags);
            b[p++] = (byte)((int)Flags>>8 );
            b[p++] = (byte)(      Squawk);
            b[p++] = (byte)((int)Squawk>>8 );
            b[p++] = (byte)(AltitudeType);
            for(int i=0;i<  9;i++) {
                b[p++] = (byte)(Callsign[i]);
            }
            b[p++] = (byte)(EmitterType);
            b[p++] = (byte)(Tslc);
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
            int l = 38;
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
