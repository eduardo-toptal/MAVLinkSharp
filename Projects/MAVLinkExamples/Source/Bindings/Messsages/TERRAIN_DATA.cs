        
using System.IO;
using System.Runtime.InteropServices;

#pragma warning disable CS0675

namespace MAVLinkBindings {

    /// <summary>
    /// Terrain data sent from GCS. The lat/lon and grid_spacing must be the same as a lat/lon from a TERRAIN_REQUEST. See terrain protocol docs: https://mavlink.io/en/services/terrain.html
    /// </summary>    
    public struct TerrainDataData : IMAVLinkMessageData {

        /// <summary>
        /// Message Id Associated w/ this Struct
        /// </summary>    
        public int GetId() { return 134; }

        public int      Lat;             //Latitude of SW corner of first grid
        public int      Lon;             //Longitude of SW corner of first grid
        public ushort   GridSpacing;     //Grid spacing
        public short[]  Data;            //Terrain data MSL
        public byte     Gridbit;         //bit within the terrain request mask    

        #region CTOR
        /// <summary>
        /// Instantiates a new TerrainDataData
        /// </summary>    
        public TerrainDataData() {
            Lat               = default(int   );
            Lon               = default(int   );
            GridSpacing       = default(ushort);
            Data              = new short[ 16];
            Gridbit           = default(byte  );
        }
        #endregion

        #region Read Buffer
        /// <summary>
        /// Reads the data from Buffer into this struct
        /// </summary>    
        public int Read(byte[] p_buffer,int p_offset=0) {
            int    l = 43;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals
            int[]  LS8  = MAVLinkCRC.U8_LSH8, LS16 = MAVLinkCRC.U8_LSH16, LS24 = MAVLinkCRC.U8_LSH24, LS32 = MAVLinkCRC.U8_LSH32, LS40 = MAVLinkCRC.U8_LSH40, LS48 = MAVLinkCRC.U8_LSH48, LS56 = MAVLinkCRC.U8_LSH56;
            Span<byte> b = p_buffer.AsSpan(p_offset);            
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            Lat               = (int   ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            Lon               = (int   ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            GridSpacing       = (ushort) (b[p++] | LS8[b[p++]]);
            for(int i=0;i<16 ;i++) { Data[i]           = (short ) (b[p++] | LS8[b[p++]]); }
            Gridbit           = (byte  ) (b[p++]);            
            return p;
        }
        #endregion

        #region Write Buffer
        /// <summary>
        /// Writes the message data into a Buffer
        /// </summary>    
        public int Write(byte[] p_buffer,int p_offset=0) {
            int    l = 43;
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
            b[p++] = (byte)(      GridSpacing);
            b[p++] = (byte)((int)GridSpacing>>8 );
            for(int i=0;i< 16;i++) {
                b[p++] = (byte)(      Data[i]);
                b[p++] = (byte)((int)Data[i]>>8 );
            }
            b[p++] = (byte)(Gridbit);
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
            int l = 43;
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
