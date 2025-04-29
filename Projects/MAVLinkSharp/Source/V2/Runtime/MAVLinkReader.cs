
using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

#pragma warning disable CS8618

namespace MAVLinkSharp.Runtime {

    #region enum MAVLinkParseResult
    /// <summary>
    /// Parsing Result Flag
    /// </summary>
    public enum MAVLinkParseResult {
        Unknown       = 0,
        Success       = 1,
        NullStream    = 2,
        Incomplete    = 3,
        NotFound      = 4,        
        BadCRC        = 6,
    }
    #endregion

    /// <summary>
    /// Class that decodes MAVLink messages from buffers and streams
    /// </summary>    
    public class MAVLinkReader {

        /// <summary>
        /// Reference to the stream.
        /// </summary>
        public Stream Stream { get; private set; }
        private MemoryStream m_mem_stream;
        private bool m_is_mem_stream;
        
        /// <summary>
        /// Flag that tells there are enough bytes to try reading messages.
        /// </summary>
        public bool Available { get { return Stream==null ? false : Stream.Length>MAVLinkConsts.MIN_PACKET_LEN;} }

        /// <summary>
        /// Internals
        /// </summary>
        //private BinaryReader m_reader;
        private byte[]       m_buffer;
        private byte[]       m_payload;
        //private byte[]       m_sign;
        
        /// <summary>
        /// CTOR.
        /// </summary>
        /// <param name="p_stream"></param>
        public MAVLinkReader(Stream p_stream) {            
            Stream    = p_stream;
            if(Stream==null) throw new NullReferenceException($"Stream is Null!");
            if(Stream is MemoryStream) { 
                m_mem_stream      = (MemoryStream)Stream; 
                m_is_mem_stream   = true;                
            }
            m_buffer  = new byte[MAVLinkConsts.MAX_PACKET_LEN];   
            m_payload = new byte[MAVLinkConsts.MAX_PAYLOAD_LEN];  
            //m_sign    = new byte[13];            
        }

        /// <summary>
        /// Readsa MAVLink message from the provided stream
        /// </summary>
        /// <param name="p_msg"></param>
        /// <returns></returns>
        public MAVLinkParseResult Read(ref MAVLinkMsg p_msg) {            
            //Assert MAVLinkMsg instance
            MAVLinkMsg         msg = p_msg;
            if(msg == null) { throw new NullReferenceException("MAVLink Message is Null!"); }
            //Locals
            Stream ss     = Stream;
            long   ss_pos = ss.Position;
            long   ss_len = ss.Length;            
            //Buffer needed Length
            byte[] b     = m_buffer;
            byte[] bpl   = m_payload;
            int    b_len = (int)(ss_len - ss_pos);
            if(b_len > MAVLinkConsts.MAX_PACKET_LEN) b_len=MAVLinkConsts.MAX_PACKET_LEN;
            long   b_pos = 0;
            //For MemoryStreams operate on faster setup
            if(m_is_mem_stream) {
                MemoryStream ms        = m_mem_stream;
                byte[]       ms_buffer = m_mem_stream.GetBuffer();
                long         ms_pos    = ms.Position;                
                //for(int i=0;i<b_len;i++) b[i] = ms_buffer[ms_pos+i];
                b     = ms_buffer;
                b_pos = ms_pos;
            }
            else {
                ss.Read(b,0,b_len);
            }
            //Message Specs
            long msg_start      = 0;
            long msg_len        = 0;
            int  msg_stx_len    = 1;            
            int  msg_crc_len    = 2;            
            int  msg_version    = 0;
            int  msg_min_len    = 0;            
            int  msg_header_len = 0; // v1 = payload-len | seq | sys-id | cmp-id | msg-id[1]  / v2 = payload-len | i-flag | c-flag | seq | sys-id | cmp-id | msg-id[3]
            //Search for STX Header
            int read_count = 0;            
            for(int i=0;i<b_len;i++) {
                int v = b[b_pos++];
                switch(v) {
                    case MAVLinkConsts.STX_MAVLINK_V1: msg_start = b_pos-1; msg_version = 1; msg_min_len =  8; msg_header_len = 5; break;
                    case MAVLinkConsts.STX_MAVLINK_V2: msg_start = b_pos-1; msg_version = 2; msg_min_len = 12; msg_header_len = 9; break;
                }                    
                if(msg_version!=0) break;
                read_count++;
            }
            //If no STX Header return not found and bytes read
            if(msg_version <= 0) { ss.Position += read_count;   return MAVLinkParseResult.NotFound; }
            //If STX found but not at first, return bytes read and "not found"
            if(read_count  >  0) { ss.Position += read_count;   return MAVLinkParseResult.NotFound; }
            //Fetch Message remaining length
            //msg_len   = b_len - msg_start;
            msg_len   = ss_len - msg_start;
            if(msg_len>MAVLinkConsts.MAX_PACKET_LEN) msg_len = MAVLinkConsts.MAX_PACKET_LEN;
            //If message is smaller than minimum return incomplete and no bytes read
            if(msg_len<msg_min_len)  { /*Don't increment position*/ return MAVLinkParseResult.Incomplete; }
            //Get Payload Size
            int  payload_len = b[b_pos++]; //0 - 255 bytes
            //Header Fields
            bool is_v2       = msg_version==2;
            byte inc_flags   = 0;   //[v2]    Incompatibility Flags
            byte cmp_flags   = 0;   //[v2]    Compatibility Flags
            byte seq         = 0;   //[v1 v2] Sequence
            byte sys_id      = 0;   //[v1 v2] System Id
            byte comp_id     = 0;   //[v1 v2] Component Id
            byte id_low      = 0;   //[v1 v2] Message Id / Byte Low
            byte id_mid      = 0;   //[v2]    Message Id / Byte Mid
            byte id_high     = 0;   //[v2]    Message Id / Byte High
            int  id          = 0;   //[v1 v2] Message Id Value / v1 = 1 byte | v2 = 3 bytes               
            //Span<byte> payload;   //[v1 v2] Payload Data
            byte crc8_low    = 0;   //[v1 v2] CRC Byte Low
            byte crc8_high   = 0;   //[v1 v2] CRC Byte High
            //Span<byte> sign ;     //[v2]    Signature / 13 bytes
            //Read Header fields
            if(is_v2) inc_flags = b[b_pos++];
            if(is_v2) cmp_flags = b[b_pos++];
            seq         = b[b_pos++];
            sys_id      = b[b_pos++];
            comp_id     = b[b_pos++];
            id_low      = b[b_pos++];
            if(is_v2) id_mid   = b[b_pos++];
            if(is_v2) id_high  = b[b_pos++];            
            if(is_v2) id       = MAVLinkCRC.U8_LSH16[id_high] | MAVLinkCRC.U8_LSH8[id_mid] | id_low; else id = id_low;
            //v2 signature info
            bool is_signed = is_v2 ? ((inc_flags & (int)MAVLinkIncFlags.Signed) != 0) : false;
            long sign_len  = is_signed ? MAVLinkConsts.SIGNATURE_BLOCK_LEN : 0;
            //Remaining Slice of Data
            //long slice_len = b_len - b_pos;
            long slice_len = ss_len - b_pos;
            //Expected Remaining Data            
            long total_remaining = payload_len + msg_crc_len + sign_len;
            //If missing data return incomplete
            if(slice_len < total_remaining) { /*Don't increment position*/ return MAVLinkParseResult.Incomplete; }
            //Total Message Size
            long msg_total_len = msg_stx_len + msg_header_len + payload_len + msg_crc_len + sign_len;
            //Store payload position
            long payload_pos = b_pos;            
            //Read Payload that might be zero truncated
            int max_payload_size = MAVLinkMsg.GetMessagePayloadLength(id);
            for(int i=0;i<max_payload_size;i++) bpl[i] = i<payload_len ? b[b_pos++] : (byte)0;
            //b_pos += payload_len;            
            //Read CRC           
            crc8_low  = (b[b_pos++]);
            crc8_high = (b[b_pos++]);
            ushort crc16 = (ushort)((MAVLinkCRC.U8_LSH8[crc8_high]) | crc8_low);
            //Sample CRC16
            ushort crc16_check = MAVLinkCRC.GetCRC(b,(int)(msg_start+1),msg_header_len+payload_len);
            MAVLinkCRC.Accumulate(ref crc16_check,MAVLinkCRC.GetMessageCRC((int)id));
            //Compare CRCs and check if valid
            if(crc16 != crc16_check) {              
                //In case of BadCRC increment 1 byte read and keep parsing
                ss.Position += 1;
                return MAVLinkParseResult.BadCRC;
            }            
            //Store known fields
            msg.version              = msg_version;
            msg.payloadLength        = (byte)payload_len;
            msg.incompatibilityFlags =       inc_flags;
            msg.compatibilityFlags   =       cmp_flags;
            msg.sequence             =       seq;
            msg.systemId             =       sys_id;
            msg.componentId          =       comp_id;
            msg.messageId            =       (MAVLinkMsgId)id;
            //Fetch Signature if any                        
            if(is_signed) {
                byte  sign_link_id   = 0;
                ulong sign_timestamp = 0;
                ulong sign_hash      = 0;
                //for(int i=0;i<MAVLinkConsts.SIGNATURE_BLOCK_LEN;i++) m_sign[i] = b[b_pos++];
                //https://mavlink.io/en/guide/message_signing.html
                //0  1  2  3  4  5  6  7  8  9  10  11  12
                //Id T0 T1 T2 T3 T4 T5 S0 S1 S2 S3  S4  S5 
                //sign_link_id   = m_sign[0];
                //sign_timestamp = (ulong) ((m_sign[1] << 5) | (m_sign[2] << 4) | (m_sign[3] << 3) | (m_sign[4]  << 2) | (m_sign[5]  << 1) | (m_sign[6] ));
                //sign_hash      = (ulong) ((m_sign[7] << 5) | (m_sign[8] << 4) | (m_sign[9] << 3) | (m_sign[10] << 2) | (m_sign[11] << 1) | (m_sign[12]));
                sign_link_id   = b[b_pos++];
                sign_timestamp = (ulong) ((b[b_pos++] << 5) | (b[b_pos++] << 4) | (b[b_pos++] << 3) | (b[b_pos++] << 2) | (b[b_pos++] << 1) | (b[b_pos++] ));
                sign_hash      = (ulong) ((b[b_pos++] << 5) | (b[b_pos++] << 4) | (b[b_pos++] << 3) | (b[b_pos++] << 2) | (b[b_pos++] << 1) | (b[b_pos++] ));

                //signature = sha256_48(secret_key + header + payload + CRC + link-ID + timestamp)
                if(msg.signature==null) msg.signature = new MAVLinkMsg.Signature();
                msg.signature.linkId           = sign_link_id;
                msg.signature.timestamp        = sign_timestamp;
                msg.signature.hash             = sign_hash;
            } 
            //Parse payload data into desired structure
            IMAVLinkMessageData d = MAVLinkMsg.GetMessageInstance(id);
            if(d==null) throw new InvalidDataException($"Message Id {(MAVLinkMsgId)id} does not have a valid data structure!");
            //d.Read(b,(int)payload_pos);
            d.Read(bpl,0);
            msg.data = d;
            //Increment Stream position
            ss.Position += msg_total_len;            
            //SUCCESS!
            return MAVLinkParseResult.Success;
        }

    }
}
