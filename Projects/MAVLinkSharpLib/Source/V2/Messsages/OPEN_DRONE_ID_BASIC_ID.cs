        
using System.IO;
using System.Runtime.InteropServices;
using MAVLinkSharp.Runtime;

#pragma warning disable CS0675

namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// Data for filling the OpenDroneID Basic ID message. This and the below messages are primarily meant for feeding data to/from an OpenDroneID implementation. E.g. https://github.com/opendroneid/opendroneid-core-c. These messages are compatible with the ASTM F3411 Remote ID standard and the ASD-STAN prEN 4709-002 Direct Remote ID standard. Additional information and usage of these messages is documented at https://mavlink.io/en/services/opendroneid.html.
    /// </summary>    
    public struct OpenDroneIdBasicIdData : IMAVLinkMessageData {

        /// <summary>
        /// Message Id Associated w/ this Struct
        /// </summary>    
        public int GetId() { return 12900; }

        public byte                TargetSystem;        //System ID (0 for broadcast).
        public byte                TargetComponent;     //Component ID (0 for broadcast).
        public byte[]              IdOrMac;             //Only used for drone ID data received from other UAs. See detailed description at https://mavlink.io/en/services/opendroneid.html.
        public MAVOdidIdTypeFlags  IdType;              //Indicates the format for the uas_id field of this message.
        public MAVOdidUaTypeFlags  UaType;              //Indicates the type of UA (Unmanned Aircraft).
        public byte[]              UasId;               //UAS (Unmanned Aircraft System) ID following the format specified by id_type. Shall be filled with nulls in the unused portion of the field.    

        #region CTOR
        /// <summary>
        /// Instantiates a new OpenDroneIdBasicIdData
        /// </summary>    
        /*
        public OpenDroneIdBasicIdData() {
            Init();
        }
        */
        public void Init() {
            TargetSystem          = default(byte              );
            TargetComponent       = default(byte              );
            IdOrMac               = new byte[ 20];
            IdType                = default(MAVOdidIdTypeFlags);
            UaType                = default(MAVOdidUaTypeFlags);
            UasId                 = new byte[ 20];
        }
        #endregion

        #region Read Buffer
        /// <summary>
        /// Reads the data from Buffer into this struct
        /// </summary>    
        public int Read(byte[] p_buffer,int p_offset=0) {
            int    l = 44;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals
            int[]  LS8  = MAVLinkCRC.U8_LSH8, LS16 = MAVLinkCRC.U8_LSH16, LS24 = MAVLinkCRC.U8_LSH24, LS32 = MAVLinkCRC.U8_LSH32, LS40 = MAVLinkCRC.U8_LSH40, LS48 = MAVLinkCRC.U8_LSH48, LS56 = MAVLinkCRC.U8_LSH56;
            Span<byte> b = p_buffer.AsSpan(p_offset);            
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            TargetSystem          = (byte              ) (b[p++]);
            TargetComponent       = (byte              ) (b[p++]);
            for(int i=0;i<20 ;i++) { IdOrMac[i]            = (byte              ) (b[p++]); }
            IdType                = (MAVOdidIdTypeFlags) (b[p++]);
            UaType                = (MAVOdidUaTypeFlags) (b[p++]);
            for(int i=0;i<20 ;i++) { UasId[i]              = (byte              ) (b[p++]); }            
            return p;
        }
        #endregion

        #region Write Buffer
        /// <summary>
        /// Writes the message data into a Buffer
        /// </summary>    
        public int Write(byte[] p_buffer,int p_offset=0) {
            int    l = 44;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals            
            Span<byte> b = p_buffer.AsSpan(p_offset);
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            b[p++] = (byte)(TargetSystem);
            b[p++] = (byte)(TargetComponent);
            for(int i=0;i< 20;i++) {
                b[p++] = (byte)(IdOrMac[i]);
            }
            b[p++] = (byte)(IdType);
            b[p++] = (byte)(UaType);
            for(int i=0;i< 20;i++) {
                b[p++] = (byte)(UasId[i]);
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
            int l = 44;
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
