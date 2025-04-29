        
using System.IO;
using System.Runtime.InteropServices;
using MAVLinkSharp.Runtime;

#pragma warning disable CS0675

namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// Streamed from drone to report progress of terrain map download (initiated by TERRAIN_REQUEST), or sent as a response to a TERRAIN_CHECK request. See terrain protocol docs: https://mavlink.io/en/services/terrain.html
    /// </summary>    
    public struct TerrainReportData : IMAVLinkMessageData {

        /// <summary>
        /// Message Id Associated w/ this Struct
        /// </summary>    
        public int GetId() { return 136; }

        public int     Lat;               //Latitude
        public int     Lon;               //Longitude
        public float   TerrainHeight;     //Terrain height MSL
        public float   CurrentHeight;     //Current vehicle height above lat/lon terrain height
        public ushort  Spacing;           //grid spacing (zero if terrain at this location unavailable)
        public ushort  Pending;           //Number of 4x4 terrain blocks waiting to be received or read from disk
        public ushort  Loaded;            //Number of 4x4 terrain blocks in memory    

        #region CTOR
        /// <summary>
        /// Instantiates a new TerrainReportData
        /// </summary>    
        /*
        public TerrainReportData() {
            Init();
        }
        */
        public void Init() {
            Lat                 = default(int   );
            Lon                 = default(int   );
            TerrainHeight       = default(float );
            CurrentHeight       = default(float );
            Spacing             = default(ushort);
            Pending             = default(ushort);
            Loaded              = default(ushort);
        }
        #endregion

        #region Read Buffer
        /// <summary>
        /// Reads the data from Buffer into this struct
        /// </summary>    
        public int Read(byte[] p_buffer,int p_offset=0) {
            int    l = 22;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals
            int[]  LS8  = MAVLinkCRC.U8_LSH8, LS16 = MAVLinkCRC.U8_LSH16, LS24 = MAVLinkCRC.U8_LSH24, LS32 = MAVLinkCRC.U8_LSH32, LS40 = MAVLinkCRC.U8_LSH40, LS48 = MAVLinkCRC.U8_LSH48, LS56 = MAVLinkCRC.U8_LSH56;
            Span<byte> b = p_buffer.AsSpan(p_offset);            
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            Lat                 = (int   ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            Lon                 = (int   ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            TerrainHeight       = (float ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            CurrentHeight       = (float ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Spacing             = (ushort) (b[p++] | LS8[b[p++]]);
            Pending             = (ushort) (b[p++] | LS8[b[p++]]);
            Loaded              = (ushort) (b[p++] | LS8[b[p++]]);            
            return p;
        }
        #endregion

        #region Write Buffer
        /// <summary>
        /// Writes the message data into a Buffer
        /// </summary>    
        public int Write(byte[] p_buffer,int p_offset=0) {
            int    l = 22;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals            
            Span<byte> b = p_buffer.AsSpan(p_offset);
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            b[p++] = (byte)(      Lat);
            b[p++] = (byte)((int)Lat>>8 );
            b[p++] = (byte)((int)Lat>>16);
            b[p++] = (byte)((int)Lat>>24);
            b[p++] = (byte)(      Lon);
            b[p++] = (byte)((int)Lon>>8 );
            b[p++] = (byte)((int)Lon>>16);
            b[p++] = (byte)((int)Lon>>24);
            MemoryMarshal.Write(b.Slice(p, 4), ref TerrainHeight      ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref CurrentHeight      ); p+=4;
            b[p++] = (byte)(      Spacing);
            b[p++] = (byte)((int)Spacing>>8 );
            b[p++] = (byte)(      Pending);
            b[p++] = (byte)((int)Pending>>8 );
            b[p++] = (byte)(      Loaded);
            b[p++] = (byte)((int)Loaded>>8 );
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
            int l = 22;
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
