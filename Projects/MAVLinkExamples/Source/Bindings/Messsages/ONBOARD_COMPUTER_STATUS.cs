        
using System.IO;
using System.Runtime.InteropServices;

#pragma warning disable CS0675

namespace MAVLinkBindings {

    /// <summary>
    /// Hardware status sent by an onboard computer.
    /// </summary>    
    public struct OnboardComputerStatusData : IMAVLinkMessageData {

        /// <summary>
        /// Message Id Associated w/ this Struct
        /// </summary>    
        public int GetId() { return 390; }

        public ulong    TimeUsec;             //Timestamp (UNIX Epoch time or time since system boot). The receiving end can infer timestamp format (since 1.1.1970 or since system boot) by checking for the magnitude of the number.
        public uint     Uptime;               //Time since system boot.
        public uint     RamUsage;             //Amount of used RAM on the component system. A value of UINT32_MAX implies the field is unused.
        public uint     RamTotal;             //Total amount of RAM on the component system. A value of UINT32_MAX implies the field is unused.
        public uint[]   StorageType;          //Storage type: 0: HDD, 1: SSD, 2: EMMC, 3: SD card (non-removable), 4: SD card (removable). A value of UINT32_MAX implies the field is unused.
        public uint[]   StorageUsage;         //Amount of used storage space on the component system. A value of UINT32_MAX implies the field is unused.
        public uint[]   StorageTotal;         //Total amount of storage space on the component system. A value of UINT32_MAX implies the field is unused.
        public uint[]   LinkType;             //Link type: 0-9: UART, 10-19: Wired network, 20-29: Wifi, 30-39: Point-to-point proprietary, 40-49: Mesh proprietary
        public uint[]   LinkTxRate;           //Network traffic from the component system. A value of UINT32_MAX implies the field is unused.
        public uint[]   LinkRxRate;           //Network traffic to the component system. A value of UINT32_MAX implies the field is unused.
        public uint[]   LinkTxMax;            //Network capacity from the component system. A value of UINT32_MAX implies the field is unused.
        public uint[]   LinkRxMax;            //Network capacity to the component system. A value of UINT32_MAX implies the field is unused.
        public short[]  FanSpeed;             //Fan speeds. A value of INT16_MAX implies the field is unused.
        public byte     Type;                 //Type of the onboard computer: 0: Mission computer primary, 1: Mission computer backup 1, 2: Mission computer backup 2, 3: Compute node, 4-5: Compute spares, 6-9: Payload computers.
        public byte[]   CpuCores;             //CPU usage on the component in percent (100 - idle). A value of UINT8_MAX implies the field is unused.
        public byte[]   CpuCombined;          //Combined CPU usage as the last 10 slices of 100 MS (a histogram). This allows to identify spikes in load that max out the system, but only for a short amount of time. A value of UINT8_MAX implies the field is unused.
        public byte[]   GpuCores;             //GPU usage on the component in percent (100 - idle). A value of UINT8_MAX implies the field is unused.
        public byte[]   GpuCombined;          //Combined GPU usage as the last 10 slices of 100 MS (a histogram). This allows to identify spikes in load that max out the system, but only for a short amount of time. A value of UINT8_MAX implies the field is unused.
        public sbyte    TemperatureBoard;     //Temperature of the board. A value of INT8_MAX implies the field is unused.
        public sbyte[]  TemperatureCore;      //Temperature of the CPU core. A value of INT8_MAX implies the field is unused.    

        #region CTOR
        /// <summary>
        /// Instantiates a new OnboardComputerStatusData
        /// </summary>    
        public OnboardComputerStatusData() {
            TimeUsec               = default(ulong);
            Uptime                 = default(uint );
            RamUsage               = default(uint );
            RamTotal               = default(uint );
            StorageType            = new uint[  4];
            StorageUsage           = new uint[  4];
            StorageTotal           = new uint[  4];
            LinkType               = new uint[  6];
            LinkTxRate             = new uint[  6];
            LinkRxRate             = new uint[  6];
            LinkTxMax              = new uint[  6];
            LinkRxMax              = new uint[  6];
            FanSpeed               = new short[  4];
            Type                   = default(byte );
            CpuCores               = new byte[  8];
            CpuCombined            = new byte[ 10];
            GpuCores               = new byte[  4];
            GpuCombined            = new byte[ 10];
            TemperatureBoard       = default(sbyte);
            TemperatureCore        = new sbyte[  8];
        }
        #endregion

        #region Read Buffer
        /// <summary>
        /// Reads the data from Buffer into this struct
        /// </summary>    
        public int Read(byte[] p_buffer,int p_offset=0) {
            int    l = 238;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals
            int[]  LS8  = MAVLinkCRC.U8_LSH8, LS16 = MAVLinkCRC.U8_LSH16, LS24 = MAVLinkCRC.U8_LSH24, LS32 = MAVLinkCRC.U8_LSH32, LS40 = MAVLinkCRC.U8_LSH40, LS48 = MAVLinkCRC.U8_LSH48, LS56 = MAVLinkCRC.U8_LSH56;
            Span<byte> b = p_buffer.AsSpan(p_offset);            
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            TimeUsec               = (ulong) ((ulong)b[p++] | (ulong)LS8[b[p++]] | (ulong)LS16[b[p++]] | (ulong)LS24[b[p++]] | (ulong)LS32[b[p++]] | (ulong)LS40[b[p++]] | (ulong)LS48[b[p++]] | (ulong)LS56[b[p++]]);
            Uptime                 = (uint ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            RamUsage               = (uint ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            RamTotal               = (uint ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            for(int i=0;i<4  ;i++) { StorageType[i]         = (uint ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]); }
            for(int i=0;i<4  ;i++) { StorageUsage[i]        = (uint ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]); }
            for(int i=0;i<4  ;i++) { StorageTotal[i]        = (uint ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]); }
            for(int i=0;i<6  ;i++) { LinkType[i]            = (uint ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]); }
            for(int i=0;i<6  ;i++) { LinkTxRate[i]          = (uint ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]); }
            for(int i=0;i<6  ;i++) { LinkRxRate[i]          = (uint ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]); }
            for(int i=0;i<6  ;i++) { LinkTxMax[i]           = (uint ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]); }
            for(int i=0;i<6  ;i++) { LinkRxMax[i]           = (uint ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]); }
            for(int i=0;i<4  ;i++) { FanSpeed[i]            = (short) (b[p++] | LS8[b[p++]]); }
            Type                   = (byte ) (b[p++]);
            for(int i=0;i<8  ;i++) { CpuCores[i]            = (byte ) (b[p++]); }
            for(int i=0;i<10 ;i++) { CpuCombined[i]         = (byte ) (b[p++]); }
            for(int i=0;i<4  ;i++) { GpuCores[i]            = (byte ) (b[p++]); }
            for(int i=0;i<10 ;i++) { GpuCombined[i]         = (byte ) (b[p++]); }
            TemperatureBoard       = (sbyte) (b[p++]);
            for(int i=0;i<8  ;i++) { TemperatureCore[i]     = (sbyte) (b[p++]); }            
            return p;
        }
        #endregion

        #region Write Buffer
        /// <summary>
        /// Writes the message data into a Buffer
        /// </summary>    
        public int Write(byte[] p_buffer,int p_offset=0) {
            int    l = 238;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals            
            Span<byte> b = p_buffer.AsSpan(p_offset);
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            b[p++] = (byte)(      TimeUsec);
            b[p++] = (byte)((long)TimeUsec>>8 );
            b[p++] = (byte)((long)TimeUsec>>16);
            b[p++] = (byte)((long)TimeUsec>>24);
            b[p++] = (byte)((long)TimeUsec>>32);
            b[p++] = (byte)((long)TimeUsec>>40);
            b[p++] = (byte)((long)TimeUsec>>48);
            b[p++] = (byte)((long)TimeUsec>>56);
            b[p++] = (byte)(      Uptime);
            b[p++] = (byte)((int)Uptime>>8 );
            b[p++] = (byte)((int)Uptime>>16);
            b[p++] = (byte)((int)Uptime>>24);
            b[p++] = (byte)(      RamUsage);
            b[p++] = (byte)((int)RamUsage>>8 );
            b[p++] = (byte)((int)RamUsage>>16);
            b[p++] = (byte)((int)RamUsage>>24);
            b[p++] = (byte)(      RamTotal);
            b[p++] = (byte)((int)RamTotal>>8 );
            b[p++] = (byte)((int)RamTotal>>16);
            b[p++] = (byte)((int)RamTotal>>24);
            for(int i=0;i<  4;i++) {
                b[p++] = (byte)(      StorageType[i]);
                b[p++] = (byte)((int)StorageType[i]>>8 );
                b[p++] = (byte)((int)StorageType[i]>>16);
                b[p++] = (byte)((int)StorageType[i]>>24);
            }
            for(int i=0;i<  4;i++) {
                b[p++] = (byte)(      StorageUsage[i]);
                b[p++] = (byte)((int)StorageUsage[i]>>8 );
                b[p++] = (byte)((int)StorageUsage[i]>>16);
                b[p++] = (byte)((int)StorageUsage[i]>>24);
            }
            for(int i=0;i<  4;i++) {
                b[p++] = (byte)(      StorageTotal[i]);
                b[p++] = (byte)((int)StorageTotal[i]>>8 );
                b[p++] = (byte)((int)StorageTotal[i]>>16);
                b[p++] = (byte)((int)StorageTotal[i]>>24);
            }
            for(int i=0;i<  6;i++) {
                b[p++] = (byte)(      LinkType[i]);
                b[p++] = (byte)((int)LinkType[i]>>8 );
                b[p++] = (byte)((int)LinkType[i]>>16);
                b[p++] = (byte)((int)LinkType[i]>>24);
            }
            for(int i=0;i<  6;i++) {
                b[p++] = (byte)(      LinkTxRate[i]);
                b[p++] = (byte)((int)LinkTxRate[i]>>8 );
                b[p++] = (byte)((int)LinkTxRate[i]>>16);
                b[p++] = (byte)((int)LinkTxRate[i]>>24);
            }
            for(int i=0;i<  6;i++) {
                b[p++] = (byte)(      LinkRxRate[i]);
                b[p++] = (byte)((int)LinkRxRate[i]>>8 );
                b[p++] = (byte)((int)LinkRxRate[i]>>16);
                b[p++] = (byte)((int)LinkRxRate[i]>>24);
            }
            for(int i=0;i<  6;i++) {
                b[p++] = (byte)(      LinkTxMax[i]);
                b[p++] = (byte)((int)LinkTxMax[i]>>8 );
                b[p++] = (byte)((int)LinkTxMax[i]>>16);
                b[p++] = (byte)((int)LinkTxMax[i]>>24);
            }
            for(int i=0;i<  6;i++) {
                b[p++] = (byte)(      LinkRxMax[i]);
                b[p++] = (byte)((int)LinkRxMax[i]>>8 );
                b[p++] = (byte)((int)LinkRxMax[i]>>16);
                b[p++] = (byte)((int)LinkRxMax[i]>>24);
            }
            for(int i=0;i<  4;i++) {
                b[p++] = (byte)(      FanSpeed[i]);
                b[p++] = (byte)((int)FanSpeed[i]>>8 );
            }
            b[p++] = (byte)(Type);
            for(int i=0;i<  8;i++) {
                b[p++] = (byte)(CpuCores[i]);
            }
            for(int i=0;i< 10;i++) {
                b[p++] = (byte)(CpuCombined[i]);
            }
            for(int i=0;i<  4;i++) {
                b[p++] = (byte)(GpuCores[i]);
            }
            for(int i=0;i< 10;i++) {
                b[p++] = (byte)(GpuCombined[i]);
            }
            b[p++] = (byte)(TemperatureBoard);
            for(int i=0;i<  8;i++) {
                b[p++] = (byte)(TemperatureCore[i]);
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
            int l = 238;
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
