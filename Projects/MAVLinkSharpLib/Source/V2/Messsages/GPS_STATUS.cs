        
using System.IO;
using System.Runtime.InteropServices;
using MAVLinkSharp.Runtime;

#pragma warning disable CS0675

namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// The positioning status, as reported by GPS. This message is intended to display status information about each satellite visible to the receiver. See message GLOBAL_POSITION_INT for the global position estimate. This message can contain information for up to 20 satellites.
    /// </summary>    
    public struct GpsStatusData : IMAVLinkMessageData {

        /// <summary>
        /// Message Id Associated w/ this Struct
        /// </summary>    
        public int GetId() { return 25; }

        public byte    SatellitesVisible;      //Number of satellites visible
        public byte[]  SatellitePrn;           //Global satellite ID
        public byte[]  SatelliteUsed;          //0: Satellite not used, 1: used for localization
        public byte[]  SatelliteElevation;     //Elevation (0: right on top of receiver, 90: on the horizon) of satellite
        public byte[]  SatelliteAzimuth;       //Direction of satellite, 0: 0 deg, 255: 360 deg.
        public byte[]  SatelliteSnr;           //Signal to noise ratio of satellite    

        #region CTOR
        /// <summary>
        /// Instantiates a new GpsStatusData
        /// </summary>    
        /*
        public GpsStatusData() {
            Init();
        }
        */
        public void Init() {
            SatellitesVisible        = default(byte);
            SatellitePrn             = new byte[ 20];
            SatelliteUsed            = new byte[ 20];
            SatelliteElevation       = new byte[ 20];
            SatelliteAzimuth         = new byte[ 20];
            SatelliteSnr             = new byte[ 20];
        }
        #endregion

        #region Read Buffer
        /// <summary>
        /// Reads the data from Buffer into this struct
        /// </summary>    
        public int Read(byte[] p_buffer,int p_offset=0) {
            int    l = 101;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals
            int[]  LS8  = MAVLinkCRC.U8_LSH8, LS16 = MAVLinkCRC.U8_LSH16, LS24 = MAVLinkCRC.U8_LSH24, LS32 = MAVLinkCRC.U8_LSH32, LS40 = MAVLinkCRC.U8_LSH40, LS48 = MAVLinkCRC.U8_LSH48, LS56 = MAVLinkCRC.U8_LSH56;
            Span<byte> b = p_buffer.AsSpan(p_offset);            
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            SatellitesVisible        = (byte) (b[p++]);
            for(int i=0;i<20 ;i++) { SatellitePrn[i]          = (byte) (b[p++]); }
            for(int i=0;i<20 ;i++) { SatelliteUsed[i]         = (byte) (b[p++]); }
            for(int i=0;i<20 ;i++) { SatelliteElevation[i]    = (byte) (b[p++]); }
            for(int i=0;i<20 ;i++) { SatelliteAzimuth[i]      = (byte) (b[p++]); }
            for(int i=0;i<20 ;i++) { SatelliteSnr[i]          = (byte) (b[p++]); }            
            return p;
        }
        #endregion

        #region Write Buffer
        /// <summary>
        /// Writes the message data into a Buffer
        /// </summary>    
        public int Write(byte[] p_buffer,int p_offset=0) {
            int    l = 101;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals            
            Span<byte> b = p_buffer.AsSpan(p_offset);
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            b[p++] = (byte)(SatellitesVisible);
            for(int i=0;i< 20;i++) {
                b[p++] = (byte)(SatellitePrn[i]);
            }
            for(int i=0;i< 20;i++) {
                b[p++] = (byte)(SatelliteUsed[i]);
            }
            for(int i=0;i< 20;i++) {
                b[p++] = (byte)(SatelliteElevation[i]);
            }
            for(int i=0;i< 20;i++) {
                b[p++] = (byte)(SatelliteAzimuth[i]);
            }
            for(int i=0;i< 20;i++) {
                b[p++] = (byte)(SatelliteSnr[i]);
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
            int l = 101;
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
