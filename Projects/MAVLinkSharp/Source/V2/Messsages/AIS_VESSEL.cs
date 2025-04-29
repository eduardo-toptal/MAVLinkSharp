        
using System.IO;
using System.Runtime.InteropServices;
using MAVLinkSharp.Runtime;

#pragma warning disable CS0675

namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// The location and information of an AIS vessel
    /// </summary>    
    public struct AisVesselData : IMAVLinkMessageData {

        /// <summary>
        /// Message Id Associated w/ this Struct
        /// </summary>    
        public int GetId() { return 301; }

        public uint               Mmsi;                   //Mobile Marine Service Identifier, 9 decimal digits
        public int                Lat;                    //Latitude
        public int                Lon;                    //Longitude
        public ushort             Cog;                    //Course over ground
        public ushort             Heading;                //True heading
        public ushort             Velocity;               //Speed over ground
        public ushort             DimensionBow;           //Distance from lat/lon location to bow
        public ushort             DimensionStern;         //Distance from lat/lon location to stern
        public ushort             Tslc;                   //Time since last communication in seconds
        public AisFlags           Flags;                  //Bitmask to indicate various statuses including valid data fields
        public sbyte              TurnRate;               //Turn rate
        public AisNavStatusFlags  NavigationalStatus;     //Navigational status
        public AisTypeFlags       Type;                   //Type of vessels
        public byte               DimensionPort;          //Distance from lat/lon location to port side
        public byte               DimensionStarboard;     //Distance from lat/lon location to starboard side
        public char[]             Callsign;               //The vessel callsign
        public char[]             Name;                   //The vessel name    

        #region CTOR
        /// <summary>
        /// Instantiates a new AisVesselData
        /// </summary>    
        public AisVesselData() {
            Mmsi                     = default(uint             );
            Lat                      = default(int              );
            Lon                      = default(int              );
            Cog                      = default(ushort           );
            Heading                  = default(ushort           );
            Velocity                 = default(ushort           );
            DimensionBow             = default(ushort           );
            DimensionStern           = default(ushort           );
            Tslc                     = default(ushort           );
            Flags                    = default(AisFlags         );
            TurnRate                 = default(sbyte            );
            NavigationalStatus       = default(AisNavStatusFlags);
            Type                     = default(AisTypeFlags     );
            DimensionPort            = default(byte             );
            DimensionStarboard       = default(byte             );
            Callsign                 = new char[  7];
            Name                     = new char[ 20];
        }
        #endregion

        #region Read Buffer
        /// <summary>
        /// Reads the data from Buffer into this struct
        /// </summary>    
        public int Read(byte[] p_buffer,int p_offset=0) {
            int    l = 58;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals
            int[]  LS8  = MAVLinkCRC.U8_LSH8, LS16 = MAVLinkCRC.U8_LSH16, LS24 = MAVLinkCRC.U8_LSH24, LS32 = MAVLinkCRC.U8_LSH32, LS40 = MAVLinkCRC.U8_LSH40, LS48 = MAVLinkCRC.U8_LSH48, LS56 = MAVLinkCRC.U8_LSH56;
            Span<byte> b = p_buffer.AsSpan(p_offset);            
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            Mmsi                     = (uint             ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            Lat                      = (int              ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            Lon                      = (int              ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            Cog                      = (ushort           ) (b[p++] | LS8[b[p++]]);
            Heading                  = (ushort           ) (b[p++] | LS8[b[p++]]);
            Velocity                 = (ushort           ) (b[p++] | LS8[b[p++]]);
            DimensionBow             = (ushort           ) (b[p++] | LS8[b[p++]]);
            DimensionStern           = (ushort           ) (b[p++] | LS8[b[p++]]);
            Tslc                     = (ushort           ) (b[p++] | LS8[b[p++]]);
            Flags                    = (AisFlags         ) (b[p++] | LS8[b[p++]]);
            TurnRate                 = (sbyte            ) (b[p++]);
            NavigationalStatus       = (AisNavStatusFlags) (b[p++]);
            Type                     = (AisTypeFlags     ) (b[p++]);
            DimensionPort            = (byte             ) (b[p++]);
            DimensionStarboard       = (byte             ) (b[p++]);
            for(int i=0;i<7  ;i++) { Callsign[i]              = (char             ) (b[p++]); }
            for(int i=0;i<20 ;i++) { Name[i]                  = (char             ) (b[p++]); }            
            return p;
        }
        #endregion

        #region Write Buffer
        /// <summary>
        /// Writes the message data into a Buffer
        /// </summary>    
        public int Write(byte[] p_buffer,int p_offset=0) {
            int    l = 58;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals            
            Span<byte> b = p_buffer.AsSpan(p_offset);
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            b[p++] = (byte)(      Mmsi);
            b[p++] = (byte)((int)Mmsi>>8 );
            b[p++] = (byte)((int)Mmsi>>16);
            b[p++] = (byte)((int)Mmsi>>24);
            b[p++] = (byte)(      Lat);
            b[p++] = (byte)((int)Lat>>8 );
            b[p++] = (byte)((int)Lat>>16);
            b[p++] = (byte)((int)Lat>>24);
            b[p++] = (byte)(      Lon);
            b[p++] = (byte)((int)Lon>>8 );
            b[p++] = (byte)((int)Lon>>16);
            b[p++] = (byte)((int)Lon>>24);
            b[p++] = (byte)(      Cog);
            b[p++] = (byte)((int)Cog>>8 );
            b[p++] = (byte)(      Heading);
            b[p++] = (byte)((int)Heading>>8 );
            b[p++] = (byte)(      Velocity);
            b[p++] = (byte)((int)Velocity>>8 );
            b[p++] = (byte)(      DimensionBow);
            b[p++] = (byte)((int)DimensionBow>>8 );
            b[p++] = (byte)(      DimensionStern);
            b[p++] = (byte)((int)DimensionStern>>8 );
            b[p++] = (byte)(      Tslc);
            b[p++] = (byte)((int)Tslc>>8 );
            b[p++] = (byte)(      Flags);
            b[p++] = (byte)((int)Flags>>8 );
            b[p++] = (byte)(TurnRate);
            b[p++] = (byte)(NavigationalStatus);
            b[p++] = (byte)(Type);
            b[p++] = (byte)(DimensionPort);
            b[p++] = (byte)(DimensionStarboard);
            for(int i=0;i<  7;i++) {
                b[p++] = (byte)(Callsign[i]);
            }
            for(int i=0;i< 20;i++) {
                b[p++] = (byte)(Name[i]);
            }
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
            int l = 58;
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
