        
using System;
using System.IO;
using System.Runtime.InteropServices;
using MAVLinkSharp.Runtime;

#pragma warning disable CS0675

namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// 
    /// Battery information that is static, or requires infrequent update.
    /// This message should requested using MAV_CMD_REQUEST_MESSAGE and/or streamed at very low rate.
    /// BATTERY_STATUS_V2 is used for higher-rate battery status information.
    /// 
    /// </summary>    
    public struct BatteryInfoData : IMAVLinkMessageData {

        /// <summary>
        /// Message Id Associated w/ this Struct
        /// </summary>    
        public int GetId() { return 372; }

        public float                    DischargeMinimumVoltage;            //Minimum per-cell voltage when discharging. 0: field not provided.
        public float                    ChargingMinimumVoltage;             //Minimum per-cell voltage when charging. 0: field not provided.
        public float                    RestingMinimumVoltage;              //Minimum per-cell voltage when resting. 0: field not provided.
        public float                    ChargingMaximumVoltage;             //Maximum per-cell voltage when charged. 0: field not provided.
        public float                    ChargingMaximumCurrent;             //Maximum pack continuous charge current. 0: field not provided.
        public float                    NominalVoltage;                     //Battery nominal voltage. Used for conversion between Wh and Ah. 0: field not provided.
        public float                    DischargeMaximumCurrent;            //Maximum pack discharge current. 0: field not provided.
        public float                    DischargeMaximumBurstCurrent;       //Maximum pack discharge burst current. 0: field not provided.
        public float                    DesignCapacity;                     //Fully charged design capacity. 0: field not provided.
        public float                    FullChargeCapacity;                 //Predicted battery capacity when fully charged (accounting for battery degradation). NAN: field not provided.
        public ushort                   CycleCount;                         //Lifetime count of the number of charge/discharge cycles (https://en.wikipedia.org/wiki/Charge_cycle). UINT16_MAX: field not provided.
        public ushort                   Weight;                             //Battery weight. 0: field not provided.
        public byte                     Id;                                 //Battery ID
        public MAVBatteryFunctionFlags  BatteryFunction;                    //Function of the battery.
        public MAVBatteryTypeFlags      Type;                               //Type (chemistry) of the battery.
        public byte                     StateOfHealth;                      //State of Health (SOH) estimate. Typically 100% at the time of manufacture and will decrease over time and use. -1: field not provided.
        public byte                     CellsInSeries;                      //Number of battery cells in series. 0: field not provided.
        public char[]                   ManufactureDate;                    //Manufacture date (DDMMYYYY) in ASCII characters, 0 terminated. All 0: field not provided.
        public char[]                   SerialNumber;                       //Serial number in ASCII characters, 0 terminated. All 0: field not provided.
        public char[]                   Name;                               //Battery device name. Formatted as manufacturer name then product name, separated with an underscore (in ASCII characters), 0 terminated. All 0: field not provided.    

        #region CTOR
        /// <summary>
        /// Instantiates a new BatteryInfoData
        /// </summary>    
        /*
        public BatteryInfoData() {
            Init();
        }
        */
        public void Init() {
            DischargeMinimumVoltage              = default(float                  );
            ChargingMinimumVoltage               = default(float                  );
            RestingMinimumVoltage                = default(float                  );
            ChargingMaximumVoltage               = default(float                  );
            ChargingMaximumCurrent               = default(float                  );
            NominalVoltage                       = default(float                  );
            DischargeMaximumCurrent              = default(float                  );
            DischargeMaximumBurstCurrent         = default(float                  );
            DesignCapacity                       = default(float                  );
            FullChargeCapacity                   = default(float                  );
            CycleCount                           = default(ushort                 );
            Weight                               = default(ushort                 );
            Id                                   = default(byte                   );
            BatteryFunction                      = default(MAVBatteryFunctionFlags);
            Type                                 = default(MAVBatteryTypeFlags    );
            StateOfHealth                        = default(byte                   );
            CellsInSeries                        = default(byte                   );
            ManufactureDate                      = new char[  9];
            SerialNumber                         = new char[ 32];
            Name                                 = new char[ 50];
        }
        #endregion

        #region Read Buffer
        /// <summary>
        /// Reads the data from Buffer into this struct
        /// </summary>    
        public int Read(byte[] p_buffer,int p_offset=0) {
            int    l = 140;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals
            int[]  LS8  = MAVLinkCRC.U8_LSH8, LS16 = MAVLinkCRC.U8_LSH16, LS24 = MAVLinkCRC.U8_LSH24, LS32 = MAVLinkCRC.U8_LSH32, LS40 = MAVLinkCRC.U8_LSH40, LS48 = MAVLinkCRC.U8_LSH48, LS56 = MAVLinkCRC.U8_LSH56;
            Span<byte> b = p_buffer.AsSpan(p_offset);            
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            DischargeMinimumVoltage              = (float                  ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            ChargingMinimumVoltage               = (float                  ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            RestingMinimumVoltage                = (float                  ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            ChargingMaximumVoltage               = (float                  ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            ChargingMaximumCurrent               = (float                  ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            NominalVoltage                       = (float                  ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            DischargeMaximumCurrent              = (float                  ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            DischargeMaximumBurstCurrent         = (float                  ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            DesignCapacity                       = (float                  ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            FullChargeCapacity                   = (float                  ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            CycleCount                           = (ushort                 ) (b[p++] | LS8[b[p++]]);
            Weight                               = (ushort                 ) (b[p++] | LS8[b[p++]]);
            Id                                   = (byte                   ) (b[p++]);
            BatteryFunction                      = (MAVBatteryFunctionFlags) (b[p++]);
            Type                                 = (MAVBatteryTypeFlags    ) (b[p++]);
            StateOfHealth                        = (byte                   ) (b[p++]);
            CellsInSeries                        = (byte                   ) (b[p++]);
            for(int i=0;i<9  ;i++) { ManufactureDate[i]                   = (char                   ) (b[p++]); }
            for(int i=0;i<32 ;i++) { SerialNumber[i]                      = (char                   ) (b[p++]); }
            for(int i=0;i<50 ;i++) { Name[i]                              = (char                   ) (b[p++]); }            
            return p;
        }
        #endregion

        #region Write Buffer
        /// <summary>
        /// Writes the message data into a Buffer
        /// </summary>    
        public int Write(byte[] p_buffer,int p_offset=0) {
            int    l = 140;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals            
            Span<byte> b = p_buffer.AsSpan(p_offset);
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            MemoryMarshal.Write(b.Slice(p, 4), ref DischargeMinimumVoltage             ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref ChargingMinimumVoltage              ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref RestingMinimumVoltage               ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref ChargingMaximumVoltage              ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref ChargingMaximumCurrent              ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref NominalVoltage                      ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref DischargeMaximumCurrent             ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref DischargeMaximumBurstCurrent        ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref DesignCapacity                      ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref FullChargeCapacity                  ); p+=4;
            b[p++] = (byte)(      CycleCount);
            b[p++] = (byte)((int)CycleCount>>8 );
            b[p++] = (byte)(      Weight);
            b[p++] = (byte)((int)Weight>>8 );
            b[p++] = (byte)(Id);
            b[p++] = (byte)(BatteryFunction);
            b[p++] = (byte)(Type);
            b[p++] = (byte)(StateOfHealth);
            b[p++] = (byte)(CellsInSeries);
            for(int i=0;i<  9;i++) {
                b[p++] = (byte)(ManufactureDate[i]);
            }
            for(int i=0;i< 32;i++) {
                b[p++] = (byte)(SerialNumber[i]);
            }
            for(int i=0;i< 50;i++) {
                b[p++] = (byte)(Name[i]);
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
            int l = 140;
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
