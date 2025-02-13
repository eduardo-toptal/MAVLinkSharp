
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

#pragma warning disable CS8618
#pragma warning disable CS8604

namespace MAVLinkBindings {

    /// <summary>
    /// Class that encodes MAVLink messages into buffers and streams
    /// </summary>    
    public class MAVLinkWriter {

        /// <summary>
        /// Reference to the stream.
        /// </summary>
        public Stream Stream { get; private set; }

        /// <summary>
        /// Internals
        /// </summary>        
        private byte[] m_buffer;

        /// <summary>
        /// Returns the next sequence for a given id
        /// </summary>
        /// <param name="p_id"></param>
        /// <returns></returns>
        internal byte NextSeq(int p_id) { return m_seq_lut.ContainsKey(p_id) ? m_seq_lut[p_id]++ : (m_seq_lut[p_id] = 0); }
        private Dictionary<int,byte> m_seq_lut;

        /// <summary>
        /// CTOR.
        /// </summary>
        /// <param name="p_stream"></param>
        public MAVLinkWriter(Stream p_stream) {            
            Stream    = p_stream;
            if(Stream==null) throw new NullReferenceException($"Stream is Null!");
            m_buffer  = new byte[MAVLinkConsts.MAX_PACKET_LEN];               
            m_seq_lut = new Dictionary<int,byte>();
        }

        /// <summary>
        /// Writes a MAVLinkV1 message into the stream
        /// </summary>
        /// <param name="p_id"></param>
        /// <param name="p_sequence"></param>
        /// <param name="p_sys_id"></param>
        /// <param name="p_comp_id"></param>
        /// <param name="p_payload"></param>
        /// <returns></returns>
        /// <exception cref="InvalidDataException"></exception>
        /// <exception cref="NullReferenceException"></exception>
        public int WriteV1(MAVLinkMsgId p_id,byte p_sequence,byte p_sys_id,byte p_comp_id,IMAVLinkMessageData p_payload) {
            //Assertions
            if((int)p_id > 255) throw new InvalidDataException($"MAVLink V1 Messages onlly supports 8 bits MSG_IDs!");
            if(p_payload==null) throw new NullReferenceException($"Payload is Null!");
            //Locals
            int[] U16RS8 = MAVLinkCRC.U16_RSH8;
            byte[] b = m_buffer;
            int    p = 0;
            byte   payload_len = MAVLinkMsg.GetMessagePayloadLength((int)p_id);
            byte   header_len  = MAVLinkConsts.MAVLINK_V1_HEADER_LEN;
            //Write
            b[p++] = MAVLinkConsts.STX_MAVLINK_V1; //V1 Flag            
            b[p++] = payload_len;                  //Payload Length            
            b[p++] = p_sequence;                   //Sequence Number            
            b[p++] = p_sys_id;                     //System Id            
            b[p++] = p_comp_id;                    //Component Id            
            b[p++] = (byte)p_id;                   //MessageId            
            p += p_payload.Write(b,p);             //Payload Data
            //CRC Calc                        
            ushort crc16 = MAVLinkCRC.GetCRC(b,1,header_len+payload_len);
            MAVLinkCRC.Accumulate(ref crc16,MAVLinkCRC.GetMessageCRC((int)p_id));            
            b[p++] = (byte)crc16;                  //CRC Low Byte            
            b[p++] = (byte)U16RS8[crc16];          //CRC Hight Byte
            //Writes into stream
            Stream.Write(b,0,p);
            //Return Size Written
            return p;
        }

        /// <summary>
        /// Writes a MAVLinkV1 message into the stream
        /// </summary>
        /// <param name="p_id"></param>
        /// <param name="p_sys_id"></param>
        /// <param name="p_comp_id"></param>
        /// <param name="p_payload"></param>
        /// <returns></returns>
        public int WriteV1(MAVLinkMsgId p_id,byte p_sys_id,byte p_comp_id,IMAVLinkMessageData p_payload) {
            byte seq = NextSeq((int)p_id);            
            int c = WriteV1(p_id,seq,p_sys_id,p_comp_id,p_payload);            
            return c;
        }

        /// <summary>
        /// Writes a MAVLinkV1 message into the stream
        /// </summary>
        /// <param name="p_sequence"></param>
        /// <param name="p_sys_id"></param>
        /// <param name="p_comp_id"></param>
        /// <param name="p_payload"></param>
        /// <returns></returns>
        public int WriteV1(byte p_sequence,byte p_sys_id,byte p_comp_id,IMAVLinkMessageData p_payload) {
            int msg_id = p_payload==null ? -1 : p_payload.GetId();
            return WriteV1((MAVLinkMsgId)msg_id,p_sequence,p_sys_id,p_comp_id,p_payload);
        }

        /// <summary>
        /// Writes a MAVLinkV1 message into the stream
        /// </summary>
        /// <param name="p_id"></param>
        /// <param name="p_sys_id"></param>
        /// <param name="p_comp_id"></param>
        /// <param name="p_payload"></param>
        /// <returns></returns>
        public int WriteV1(byte p_sys_id,byte p_comp_id,IMAVLinkMessageData p_payload) {            
            int msg_id = p_payload==null ? -1 : p_payload.GetId();            
            int c = WriteV1((MAVLinkMsgId)msg_id,p_sys_id,p_comp_id,p_payload);
            return c;
        }

        /// <summary>
        /// Writes a MAVLinkV1 Message into the stream
        /// </summary>
        /// <param name="p_message"></param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        public int WriteV1(MAVLinkMsg p_message) {
            if(p_message==null) throw new NullReferenceException($"Message is Null!");
            MAVLinkMsgId id     = p_message.messageId;
            byte         seq    = p_message.sequence;
            byte         sys_id = p_message.systemId;
            byte         cmp_id = p_message.componentId;
            IMAVLinkMessageData data = p_message.data;
            int c = WriteV1(id,seq, sys_id, cmp_id, data);
            p_message.sequence = NextSeq((int)id);
            return c;
        }

        /// <summary>
        /// Writes a MAVLinkV2 message into the stream
        /// </summary>
        /// <param name="p_id"></param>
        /// <param name="p_iflags"></param>
        /// <param name="p_cflags"></param>
        /// <param name="p_sequence"></param>
        /// <param name="p_sys_id"></param>
        /// <param name="p_comp_id"></param>
        /// <param name="p_payload"></param>
        /// <param name="p_secret_key"></param>
        /// <returns></returns>
        /// <exception cref="InvalidDataException"></exception>
        /// <exception cref="NullReferenceException"></exception>
        public int WriteV2(MAVLinkMsgId p_id,byte p_iflags,byte p_cflags,byte p_sequence,byte p_sys_id,byte p_comp_id,IMAVLinkMessageData p_payload,string p_secret_key="") {
            //Assertions            
            if(p_payload==null) throw new NullReferenceException($"Payload is Null!");
            //Locals
            int[] U8RS8 = MAVLinkCRC.U8_RSH8,U8RS16 = MAVLinkCRC.U8_RSH16,U16RS8 = MAVLinkCRC.U16_RSH8;
            byte[] b = m_buffer;
            int    p = 0;
            byte   payload_len = MAVLinkMsg.GetMessagePayloadLength((int)p_id);
            byte   header_len  = MAVLinkConsts.MAVLINK_V2_HEADER_LEN;
            bool   is_sign     = !string.IsNullOrEmpty(p_secret_key);
            if(is_sign) p_iflags |= 0x1;
            int payload_len_pos = 0;
            //Write
            b[p++] = MAVLinkConsts.STX_MAVLINK_V2; //V2 Flag            
            payload_len_pos = p;
            b[p++] = payload_len;                  //Payload Length
            b[p++] = p_iflags;                     //Incompatibility Flags            
            b[p++] = p_cflags;                     //Compatibility Flags            
            b[p++] = p_sequence;                   //Sequence Number            
            b[p++] = p_sys_id;                     //System Id            
            b[p++] = p_comp_id;                    //Component Id            
            b[p++] = (byte)p_id;                   //MessageId Low Byte            
            b[p++] = (byte)((int)p_id >>  8);      //MessageId Mid Byte            
            b[p++] = (byte)((int)p_id >> 16);      //MessageId High Byte            
            p += p_payload.Write(b,p);             //Payload Data
            //Zero Value Truncation
            byte zt=0;
            for(int i=0;i<255;i++) { 
                if(b[p-(zt+1)]>0) break;
                zt++;
            }
            //Only adjust indexing if truncation happens 
            if(zt>0) {
                p-= zt;
                b[payload_len_pos] -= zt;
                if(b[payload_len_pos]<=0) b[payload_len_pos] = 1;
                payload_len = b[payload_len_pos];
            }            
            //CRC Calc
            ushort crc16 = MAVLinkCRC.GetCRC(b,1,header_len+payload_len);
            MAVLinkCRC.Accumulate(ref crc16,MAVLinkCRC.GetMessageCRC((int)p_id));
            b[p++] = (byte)crc16;                  //CRC Low Byte            
            b[p++] = (byte)U16RS8[crc16];          //CRC Hight Byte
            //Signature if any
            if(is_sign) { /*Skip SIGN for now*/ p+=13; }
            //Writes into stream
            Stream.Write(b,0,p);
            //Return Size Written
            return p;
        }

        /// <summary>
        /// Writes a MAVLinkV2 message into the stream
        /// </summary>
        /// <param name="p_id"></param>
        /// <param name="p_iflags"></param>
        /// <param name="p_cflags"></param>
        /// <param name="p_sys_id"></param>
        /// <param name="p_comp_id"></param>
        /// <param name="p_payload"></param>
        /// <param name="p_secret_key"></param>
        /// <returns></returns>
        public int WriteV2(MAVLinkMsgId p_id,byte p_iflags,byte p_cflags,byte p_sys_id,byte p_comp_id,IMAVLinkMessageData p_payload,string p_secret_key="") {
            byte seq = NextSeq((int)p_id);            
            int c = WriteV2(p_id,p_iflags,p_cflags,seq,p_sys_id,p_comp_id,p_payload,p_secret_key);            
            return c;
        }

        /// <summary>
        /// Writes a MAVLinkV2 message into the stream
        /// </summary>
        /// <param name="p_iflags"></param>
        /// <param name="p_cflags"></param>
        /// <param name="p_sys_id"></param>
        /// <param name="p_comp_id"></param>
        /// <param name="p_payload"></param>
        /// <param name="p_secret_key"></param>
        /// <returns></returns>
        public int WriteV2(byte p_iflags,byte p_cflags,byte p_sys_id,byte p_comp_id,IMAVLinkMessageData p_payload,string p_secret_key="") {
            int msg_id = p_payload==null ? -1 : p_payload.GetId();
            int c = WriteV2((MAVLinkMsgId)msg_id,p_iflags,p_cflags,p_sys_id,p_comp_id,p_payload,p_secret_key);            
            return c;
        }

        /// <summary>
        /// Writes a MAVLinkV2 message into the stream
        /// </summary>
        /// <param name="p_sys_id"></param>
        /// <param name="p_comp_id"></param>
        /// <param name="p_payload"></param>
        /// <param name="p_secret_key"></param>
        /// <returns></returns>
        public int WriteV2(byte p_sys_id,byte p_comp_id,IMAVLinkMessageData p_payload,string p_secret_key="") {  
            int msg_id = p_payload==null ? -1 : p_payload.GetId();
            int c = WriteV2((MAVLinkMsgId)msg_id,0,0,p_sys_id,p_comp_id,p_payload,p_secret_key);
            return c;
        }

        /// <summary>
        /// Writes a MAVLinkV2 message into the stream
        /// </summary>
        /// <param name="p_iflags"></param>
        /// <param name="p_cflags"></param>
        /// <param name="p_sequence"></param>
        /// <param name="p_sys_id"></param>
        /// <param name="p_comp_id"></param>
        /// <param name="p_payload"></param>
        /// <param name="p_secret_key"></param>
        /// <returns></returns>
        public int WriteV2(byte p_iflags,byte p_cflags,byte p_sequence,byte p_sys_id,byte p_comp_id,IMAVLinkMessageData p_payload,string p_secret_key="") {
            int msg_id = p_payload==null ? -1 : p_payload.GetId();
            return WriteV2((MAVLinkMsgId)msg_id,p_iflags,p_cflags,p_sequence,p_sys_id,p_comp_id,p_payload,p_secret_key);
        }

        /// <summary>
        /// Writes a MAVLinkV2 message into the stream
        /// </summary>
        /// <param name="p_sequence"></param>
        /// <param name="p_sys_id"></param>
        /// <param name="p_comp_id"></param>
        /// <param name="p_payload"></param>
        /// <param name="p_secret_key"></param>
        /// <returns></returns>
        public int WriteV2(byte p_sequence,byte p_sys_id,byte p_comp_id,IMAVLinkMessageData p_payload,string p_secret_key="") {
            int msg_id = p_payload==null ? -1 : p_payload.GetId();
            return WriteV2((MAVLinkMsgId)msg_id,p_sequence,p_sys_id,p_comp_id,p_payload,p_secret_key);
        }

        /// <summary>
        /// Writes a MAVLinkV2 message into the stream
        /// </summary>
        /// <param name="p_id"></param>
        /// <param name="p_sequence"></param>
        /// <param name="p_sys_id"></param>
        /// <param name="p_comp_id"></param>
        /// <param name="p_payload"></param>
        /// <param name="p_secret_key"></param>
        /// <returns></returns>
        public int WriteV2(MAVLinkMsgId p_id,byte p_sequence,byte p_sys_id,byte p_comp_id,IMAVLinkMessageData p_payload,string p_secret_key="") {
            return WriteV2(p_id,0,0,p_sequence,p_sys_id,p_comp_id,p_payload,p_secret_key);
        }

        /// <summary>
        /// Writes a MAVLinkV2 Message into the stream
        /// </summary>
        /// <param name="p_message"></param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        public int WriteV2(MAVLinkMsg p_message,string p_secret_key="") {
            if(p_message==null) throw new NullReferenceException($"Message is Null!");
            MAVLinkMsgId id     = p_message.messageId;
            byte         iflags = p_message.incompatibilityFlags;
            byte         cflags = p_message.compatibilityFlags;
            byte         seq    = p_message.sequence;
            byte         sys_id = p_message.systemId;
            byte         cmp_id = p_message.componentId;
            IMAVLinkMessageData data = p_message.data;            
            int c = WriteV2(id,iflags,cflags,seq, sys_id, cmp_id, data,p_secret_key);            
            p_message.sequence = NextSeq((int)id);
            return c;
        }

    }
}
