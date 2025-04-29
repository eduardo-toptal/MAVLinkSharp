        
using System;
using System.IO;
using System.Runtime.InteropServices;
using MAVLinkSharp.Runtime;

#pragma warning disable CS0675

namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// Request for terrain data and terrain status. See terrain protocol docs: https://mavlink.io/en/services/terrain.html
    /// </summary>    
    public struct TerrainRequestData : IMAVLinkMessageData {

        /// <summary>
        /// Message Id Associated w/ this Struct
        /// </summary>    
        public int GetId() { return 133; }

        public ulong   Mask;            //Bitmask of requested 4x4 grids (row major 8x7 array of grids, 56 bits)
        public int     Lat;             //Latitude of SW corner of first grid
        public int     Lon;             //Longitude of SW corner of first grid
        public ushort  GridSpacing;     //Grid spacing    

        #region CTOR
        /// <summary>
        /// Instantiates a new TerrainRequestData
        /// </summary>    
        /*
        public TerrainRequestData() {
            Init();
        }
        */
        public void Init() {
            Mask              = default(ulong );
            Lat               = default(int   );
            Lon               = default(int   );
            GridSpacing       = default(ushort);
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
            Mask              = (ulong ) ((ulong)b[p++] | (ulong)LS8[b[p++]] | (ulong)LS16[b[p++]] | (ulong)LS24[b[p++]] | (ulong)LS32[b[p++]] | (ulong)LS40[b[p++]] | (ulong)LS48[b[p++]] | (ulong)LS56[b[p++]]);
            Lat               = (int   ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            Lon               = (int   ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            GridSpacing       = (ushort) (b[p++] | LS8[b[p++]]);            
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
            b[p++] = (byte)(      Mask);
            b[p++] = (byte)((long)Mask>>8 );
            b[p++] = (byte)((long)Mask>>16);
            b[p++] = (byte)((long)Mask>>24);
            b[p++] = (byte)((long)Mask>>32);
            b[p++] = (byte)((long)Mask>>40);
            b[p++] = (byte)((long)Mask>>48);
            b[p++] = (byte)((long)Mask>>56);
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
