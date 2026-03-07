using System;
using System.Collections.Generic;

#pragma warning disable CS8618
#pragma warning disable CS1522

namespace MAVLinkSharp.Runtime {
    
    /// <summary>
    /// Utility for Samplig CRC
    /// </summary>
    public class MAVLinkCRC {

        #region MessageID to CRC
        /// <summary>
        /// Returns the message CRC generated from the definition XML
        /// </summary>
        static public byte GetMessageCRC(int p_msg_id) {
            switch(p_msg_id) {
                //%message-crc%
            }
            return 0;
        }
        #endregion
        
        /// <summary>
        /// Consts
        /// </summary>
        internal const ushort    X25_INIT_CRC     = 0xffff;
        internal const ushort    X25_VALIDATE_CRC = 0xf0b8;

        /// <summary>
        /// LUT
        /// </summary>        
        static public int[] U8_LSH8;
        static public int[] U8_LSH16;
        static public int[] U8_LSH24;
        static public int[] U8_LSH32;
        static public int[] U8_LSH40;
        static public int[] U8_LSH48;
        static public int[] U8_LSH56;

        static public int[] U8_RSH8;
        static public int[] U8_RSH16;
        static public int[] U8_RSH24;
        static public int[] U8_RSH32;
        static public int[] U8_RSH40;
        static public int[] U8_RSH48;
        static public int[] U8_RSH56;

        static public int[] U16_RSH8;

        /*
        static public int[] U16_RSH16;
        static public int[] U16_RSH24;
        static public int[] U16_RSH32;
        static public int[] U16_RSH40;
        static public int[] U16_RSH48;
        static public int[] U16_RSH56;
        //*/

        static public int[] U8_LSH3;
        static public int[] U8_RSH4;        
        static public int[] U8_LSH4;
        static public int[][] U8_XOR;
        static public ushort[][] U16_CRC;

        /// <summary>
        /// CTOR
        /// </summary>
        static MAVLinkCRC() {
            Init();
        }

        #region void Init
        /// <summary>
        /// Initializes LUT and cached info.
        /// </summary>
        static public void Init() {
            if(m_has_init) return;
            m_has_init = true;

            U16_RSH8  = new int[65536]; for(int i=0;i<65536;i++) U16_RSH8 [i] = i>> 8;

            /*
            U16_RSH16 = new int[65536]; for(int i=0;i<65536;i++) U16_RSH16[i] = i>>16;
            U16_RSH24 = new int[65536]; for(int i=0;i<65536;i++) U16_RSH24[i] = i>>24;
            U16_RSH32 = new int[65536]; for(int i=0;i<65536;i++) U16_RSH32[i] = i>>32;
            U16_RSH40 = new int[65536]; for(int i=0;i<65536;i++) U16_RSH40[i] = i>>40;
            U16_RSH48 = new int[65536]; for(int i=0;i<65536;i++) U16_RSH48[i] = i>>48;
            U16_RSH56 = new int[65536]; for(int i=0;i<65536;i++) U16_RSH56[i] = i>>56;
            //*/

            U8_LSH3  = new int[256]; for(int i=0;i<256;i++) U8_LSH3 [i] = i<< 3;
            U8_LSH4  = new int[256]; for(int i=0;i<256;i++) U8_LSH4 [i] = i<< 4;
            U8_LSH8  = new int[256]; for(int i=0;i<256;i++) U8_LSH8 [i] = i<< 8;            
            U8_LSH16 = new int[256]; for(int i=0;i<256;i++) U8_LSH16[i] = i<<16;            
            U8_LSH24 = new int[256]; for(int i=0;i<256;i++) U8_LSH24[i] = i<<24;
            U8_LSH32 = new int[256]; for(int i=0;i<256;i++) U8_LSH32[i] = i<<32;
            U8_LSH40 = new int[256]; for(int i=0;i<256;i++) U8_LSH40[i] = i<<40;
            U8_LSH48 = new int[256]; for(int i=0;i<256;i++) U8_LSH48[i] = i<<48;
            U8_LSH56 = new int[256]; for(int i=0;i<256;i++) U8_LSH56[i] = i<<56;

            U8_RSH4  = new int[256]; for(int i=0;i<256;i++) U8_RSH4 [i] = i>>4;
            U8_RSH8  = new int[256]; for(int i=0;i<256;i++) U8_RSH8 [i] = i>>8;            
            U8_RSH16 = new int[256]; for(int i=0;i<256;i++) U8_RSH16[i] = i>>16;            
            U8_RSH24 = new int[256]; for(int i=0;i<256;i++) U8_RSH24[i] = i>>24;
            U8_RSH32 = new int[256]; for(int i=0;i<256;i++) U8_RSH32[i] = i>>32;
            U8_RSH40 = new int[256]; for(int i=0;i<256;i++) U8_RSH40[i] = i>>40;
            U8_RSH48 = new int[256]; for(int i=0;i<256;i++) U8_RSH48[i] = i>>48;
            U8_RSH56 = new int[256]; for(int i=0;i<256;i++) U8_RSH56[i] = i>>56;

            U8_XOR = new int[256][];
            for(int i=0;i<U8_XOR.Length;i++) U8_XOR[i] = new int[256];
            for(int i=0;i<256;i++) for(int j=0;j<256;j++) U8_XOR[i][j] = i^j;

            U16_CRC = new ushort[65536][];
            for(int i=0;i<U16_CRC.Length;i++) U16_CRC[i] = new ushort[256];
            for(int i=0;i<65536;i++) for(int j=0;j<256  ;j++) { U16_CRC[i][j] = (ushort)i; AccumulateBase(ref U16_CRC[i][j],(byte)j); }

        }
        static bool m_has_init;
        #endregion

        /// <summary>
        /// Given a list of data returns the accumulated CRC.
        /// </summary>
        /// <param name="p_data"></param>
        /// <returns></returns>
        static public ushort GetCRC(List<object> p_data) {
            ushort res = X25_INIT_CRC;
            for(int i=0;i<p_data.Count;i++) {
                object it = p_data[i];
                if(it is byte  ) Accumulate(ref res, (byte  )it); else
                if(it is byte[]) Accumulate(ref res, (byte[])it); else
                if(it is string) Accumulate(ref res, (string)it);
            }
            return res;
        }

        /// <summary>
        /// Returns the CRC16 for the specified data
        /// </summary>
        /// <param name="p_data"></param>
        /// <returns></returns>
        static public ushort GetCRC(byte[] p_data,int p_offset,int p_length) {
            ushort res = X25_INIT_CRC;
            Accumulate(ref res,p_data,p_offset,p_length);
            return res;            
        }

        /// <summary>
        /// Returns the CRC16 for the specified data
        /// </summary>
        /// <param name="p_data"></param>
        /// <returns></returns>
        static public ushort GetCRC(Span<byte> p_data) {
            ushort res = X25_INIT_CRC;
            Accumulate(ref res,p_data);
            return res;            
        }

        /// <summary>
        /// Retruns the byte section for MAVLink
        /// </summary>
        /// <param name="p_crc"></param>
        /// <returns></returns>
        static public byte GetCRCExtra(ushort p_crc) {
            //return (byte)((p_crc & 0xFF) ^ (p_crc >> 8));
            return (byte)((p_crc & 0xFF) ^ (U16_RSH8[p_crc]));
        }
 
        /// <summary>
        /// Accumulate a single byte
        /// </summary>
        /// <param name="p_crc"></param>
        /// <param name="p_data"></param>
        static internal void AccumulateBase(ref ushort p_crc,byte p_data) {
            byte tmp;
            byte crc_low = (byte)(p_crc & 0xff);
            //tmp = (byte)U8_XOR[p_data , crc_low];
            tmp = (byte)U8_XOR[p_data][crc_low];
            //tmp   = (byte)(p_data ^ crc_low);            
            tmp   = (byte)(tmp    ^ tmp<<4 );
            //p_crc = (ushort)((p_crc>>8) ^ (tmp<<8) ^ (tmp <<3) ^ (tmp>>4));
            int xor_a = U16_RSH8[p_crc];
            int xor_b = U8_LSH8[tmp];
            int xor_c = U8_LSH3[tmp];
            int xor_d = U8_RSH4[tmp];
            p_crc = (ushort)(xor_a ^ xor_b ^ xor_c ^ xor_d);
            //p_crc = (ushort)(xor_ab ^ xor_cd);
        }

        /// <summary>
        /// Accumulates a bytem cached version
        /// </summary>
        /// <param name="p_crc"></param>
        /// <param name="p_data"></param>
        static public void Accumulate(ref ushort p_crc,byte p_data) {
           //p_crc = U16_CRC[p_crc,p_data];
           p_crc = U16_CRC[p_crc][p_data];
        }

        /// <summary>
        /// Accumulate RawBytes
        /// </summary>
        /// <param name="data"></param>
        static public void Accumulate(ref ushort p_crc,byte[] p_data,int p_offset,int p_length) {
            for(int i=0;i<p_length; i++) Accumulate(ref p_crc,p_data[p_offset+i]);            
        }

        static public void Accumulate(ref ushort p_crc,byte[] p_data) {
            for(int i=0;i<p_data.Length; i++) Accumulate(ref p_crc,p_data[i]);            
        }

        /// <summary>
        /// Accumulate RawBytes
        /// </summary>
        /// <param name="data"></param>
        static public void Accumulate(ref ushort p_crc,Span<byte> p_data) {
            for(int i=0;i<p_data.Length; i++) Accumulate(ref p_crc,p_data[i]);            
        }

        /// <summary>
        /// Acumulate the string bytes
        /// </summary>
        /// <param name="p_data"></param>
        static public void Accumulate(ref ushort p_crc,string p_data) {            
            for(int i=0;i<p_data.Length; i++) Accumulate(ref p_crc,(byte)p_data[i]);
        }


    }
}
