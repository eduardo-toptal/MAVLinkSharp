        
using System.IO;
using System.Runtime.InteropServices;
using MAVLinkSharp.Runtime;

#pragma warning disable CS0675

namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// Smart Battery information (static/infrequent update). Use for updates from: smart battery to flight stack, flight stack to GCS. Use BATTERY_STATUS for smart battery frequent updates.
    /// </summary>    
    public struct SmartBatteryInfoData : IMAVLinkMessageData {

        /// <summary>
        /// Message Id Associated w/ this Struct
        /// </summary>    
        public int GetId() { return 370; }

        public int                      CapacityFullSpecification;          //Capacity when full according to manufacturer, -1: field not provided.
        public int                      CapacityFull;                       //Capacity when full (accounting for battery degradation), -1: field not provided.
        public ushort                   CycleCount;                         //Charge/discharge cycle count. UINT16_MAX: field not provided.
        public ushort                   Weight;                             //Battery weight. 0: field not provided.
        public ushort                   DischargeMinimumVoltage;            //Minimum per-cell voltage when discharging. If not supplied set to UINT16_MAX value.
        public ushort                   ChargingMinimumVoltage;             //Minimum per-cell voltage when charging. If not supplied set to UINT16_MAX value.
        public ushort                   RestingMinimumVoltage;              //Minimum per-cell voltage when resting. If not supplied set to UINT16_MAX value.
        public byte                     Id;                                 //Battery ID
        public MAVBatteryFunctionFlags  BatteryFunction;                    //Function of the battery
        public MAVBatteryTypeFlags      Type;                               //Type (chemistry) of the battery
        public char[]                   SerialNumber;                       //Serial number in ASCII characters, 0 terminated. All 0: field not provided.
        public char[]                   DeviceName;                         //Static device name in ASCII characters, 0 terminated. All 0: field not provided. Encode as manufacturer name then product name separated using an underscore.
        public ushort                   ChargingMaximumVoltage;             //Maximum per-cell voltage when charged. 0: field not provided.
        public byte                     CellsInSeries;                      //Number of battery cells in series. 0: field not provided.
        public uint                     DischargeMaximumCurrent;            //Maximum pack discharge current. 0: field not provided.
        public uint                     DischargeMaximumBurstCurrent;       //Maximum pack discharge burst current. 0: field not provided.
        public char[]                   ManufactureDate;                    //Manufacture date (DD/MM/YYYY) in ASCII characters, 0 terminated. All 0: field not provided.    

        #region CTOR
        /// <summary>
        /// Instantiates a new SmartBatteryInfoData
        /// </summary>    
        public SmartBatteryInfoData() {
            CapacityFullSpecification            = default(int                    );
            CapacityFull                         = default(int                    );
            CycleCount                           = default(ushort                 );
            Weight                               = default(ushort                 );
            DischargeMinimumVoltage              = default(ushort                 );
            ChargingMinimumVoltage               = default(ushort                 );
            RestingMinimumVoltage                = default(ushort                 );
            Id                                   = default(byte                   );
            BatteryFunction                      = default(MAVBatteryFunctionFlags);
            Type                                 = default(MAVBatteryTypeFlags    );
            SerialNumber                         = new char[ 16];
            DeviceName                           = new char[ 50];
            ChargingMaximumVoltage               = default(ushort                 );
            CellsInSeries                        = default(byte                   );
            DischargeMaximumCurrent              = default(uint                   );
            DischargeMaximumBurstCurrent         = default(uint                   );
            ManufactureDate                      = new char[ 11];
        }
        #endregion

        #region Read Buffer
        /// <summary>
        /// Reads the data from Buffer into this struct
        /// </summary>    
        public int Read(byte[] p_buffer,int p_offset=0) {
            int    l = 109;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals
            int[]  LS8  = MAVLinkCRC.U8_LSH8, LS16 = MAVLinkCRC.U8_LSH16, LS24 = MAVLinkCRC.U8_LSH24, LS32 = MAVLinkCRC.U8_LSH32, LS40 = MAVLinkCRC.U8_LSH40, LS48 = MAVLinkCRC.U8_LSH48, LS56 = MAVLinkCRC.U8_LSH56;
            Span<byte> b = p_buffer.AsSpan(p_offset);            
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            CapacityFullSpecification            = (int                    ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            CapacityFull                         = (int                    ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            CycleCount                           = (ushort                 ) (b[p++] | LS8[b[p++]]);
            Weight                               = (ushort                 ) (b[p++] | LS8[b[p++]]);
            DischargeMinimumVoltage              = (ushort                 ) (b[p++] | LS8[b[p++]]);
            ChargingMinimumVoltage               = (ushort                 ) (b[p++] | LS8[b[p++]]);
            RestingMinimumVoltage                = (ushort                 ) (b[p++] | LS8[b[p++]]);
            Id                                   = (byte                   ) (b[p++]);
            BatteryFunction                      = (MAVBatteryFunctionFlags) (b[p++]);
            Type                                 = (MAVBatteryTypeFlags    ) (b[p++]);
            for(int i=0;i<16 ;i++) { SerialNumber[i]                      = (char                   ) (b[p++]); }
            for(int i=0;i<50 ;i++) { DeviceName[i]                        = (char                   ) (b[p++]); }
            ChargingMaximumVoltage               = (ushort                 ) (b[p++] | LS8[b[p++]]);
            CellsInSeries                        = (byte                   ) (b[p++]);
            DischargeMaximumCurrent              = (uint                   ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            DischargeMaximumBurstCurrent         = (uint                   ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            for(int i=0;i<11 ;i++) { ManufactureDate[i]                   = (char                   ) (b[p++]); }            
            return p;
        }
        #endregion

        #region Write Buffer
        /// <summary>
        /// Writes the message data into a Buffer
        /// </summary>    
        public int Write(byte[] p_buffer,int p_offset=0) {
            int    l = 109;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals            
            Span<byte> b = p_buffer.AsSpan(p_offset);
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            b[p++] = (byte)(      CapacityFullSpecification);
            b[p++] = (byte)((int)CapacityFullSpecification>>8 );
            b[p++] = (byte)((int)CapacityFullSpecification>>16);
            b[p++] = (byte)((int)CapacityFullSpecification>>24);
            b[p++] = (byte)(      CapacityFull);
            b[p++] = (byte)((int)CapacityFull>>8 );
            b[p++] = (byte)((int)CapacityFull>>16);
            b[p++] = (byte)((int)CapacityFull>>24);
            b[p++] = (byte)(      CycleCount);
            b[p++] = (byte)((int)CycleCount>>8 );
            b[p++] = (byte)(      Weight);
            b[p++] = (byte)((int)Weight>>8 );
            b[p++] = (byte)(      DischargeMinimumVoltage);
            b[p++] = (byte)((int)DischargeMinimumVoltage>>8 );
            b[p++] = (byte)(      ChargingMinimumVoltage);
            b[p++] = (byte)((int)ChargingMinimumVoltage>>8 );
            b[p++] = (byte)(      RestingMinimumVoltage);
            b[p++] = (byte)((int)RestingMinimumVoltage>>8 );
            b[p++] = (byte)(Id);
            b[p++] = (byte)(BatteryFunction);
            b[p++] = (byte)(Type);
            for(int i=0;i< 16;i++) {
                b[p++] = (byte)(SerialNumber[i]);
            }
            for(int i=0;i< 50;i++) {
                b[p++] = (byte)(DeviceName[i]);
            }
            b[p++] = (byte)(      ChargingMaximumVoltage);
            b[p++] = (byte)((int)ChargingMaximumVoltage>>8 );
            b[p++] = (byte)(CellsInSeries);
            b[p++] = (byte)(      DischargeMaximumCurrent);
            b[p++] = (byte)((int)DischargeMaximumCurrent>>8 );
            b[p++] = (byte)((int)DischargeMaximumCurrent>>16);
            b[p++] = (byte)((int)DischargeMaximumCurrent>>24);
            b[p++] = (byte)(      DischargeMaximumBurstCurrent);
            b[p++] = (byte)((int)DischargeMaximumBurstCurrent>>8 );
            b[p++] = (byte)((int)DischargeMaximumBurstCurrent>>16);
            b[p++] = (byte)((int)DischargeMaximumBurstCurrent>>24);
            for(int i=0;i< 11;i++) {
                b[p++] = (byte)(ManufactureDate[i]);
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
            int l = 109;
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
