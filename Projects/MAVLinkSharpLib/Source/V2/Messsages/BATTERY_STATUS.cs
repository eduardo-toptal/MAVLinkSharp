        
using System;
using System.IO;
using System.Runtime.InteropServices;
using MAVLinkSharp.Runtime;

#pragma warning disable CS0675

namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// Battery information. Updates GCS with flight controller battery status. Smart batteries also use this message, but may additionally send SMART_BATTERY_INFO.
    /// </summary>    
    public struct BatteryStatusData : IMAVLinkMessageData {

        /// <summary>
        /// Message Id Associated w/ this Struct
        /// </summary>    
        public int GetId() { return 147; }

        public int                         CurrentConsumed;      //Consumed charge, -1: autopilot does not provide consumption estimate
        public int                         EnergyConsumed;       //Consumed energy, -1: autopilot does not provide energy consumption estimate
        public short                       Temperature;          //Temperature of the battery. INT16_MAX for unknown temperature.
        public ushort[]                    Voltages;             //Battery voltage of cells 1 to 10 (see voltages_ext for cells 11-14). Cells in this field above the valid cell count for this battery should have the UINT16_MAX value. If individual cell voltages are unknown or not measured for this battery, then the overall battery voltage should be filled in cell 0, with all others set to UINT16_MAX. If the voltage of the battery is greater than (UINT16_MAX - 1), then cell 0 should be set to (UINT16_MAX - 1), and cell 1 to the remaining voltage. This can be extended to multiple cells if the total voltage is greater than 2 * (UINT16_MAX - 1).
        public short                       CurrentBattery;       //Battery current, -1: autopilot does not measure the current
        public byte                        Id;                   //Battery ID
        public MAVBatteryFunctionFlags     BatteryFunction;      //Function of the battery
        public MAVBatteryTypeFlags         Type;                 //Type (chemistry) of the battery
        public sbyte                       BatteryRemaining;     //Remaining battery energy. Values: [0-100], -1: autopilot does not estimate the remaining battery.
        public int                         TimeRemaining;        //Remaining battery time, 0: autopilot does not provide remaining battery time estimate
        public MAVBatteryChargeStateFlags  ChargeState;          //State for extent of discharge, provided by autopilot for warning or external reactions
        public ushort[]                    VoltagesExt;          //Battery voltages for cells 11 to 14. Cells above the valid cell count for this battery should have a value of 0, where zero indicates not supported (note, this is different than for the voltages field and allows empty byte truncation). If the measured value is 0 then 1 should be sent instead.
        public MAVBatteryModeFlags         Mode;                 //Battery mode. Default (0) is that battery mode reporting is not supported or battery is in normal-use mode.
        public MAVBatteryFaultFlags        FaultBitmask;         //Fault/health indications. These should be set when charge_state is MAV_BATTERY_CHARGE_STATE_FAILED or MAV_BATTERY_CHARGE_STATE_UNHEALTHY (if not, fault reporting is not supported).    

        #region CTOR
        /// <summary>
        /// Instantiates a new BatteryStatusData
        /// </summary>    
        /*
        public BatteryStatusData() {
            Init();
        }
        */
        public void Init() {
            CurrentConsumed        = default(int                       );
            EnergyConsumed         = default(int                       );
            Temperature            = default(short                     );
            Voltages               = new ushort[ 10];
            CurrentBattery         = default(short                     );
            Id                     = default(byte                      );
            BatteryFunction        = default(MAVBatteryFunctionFlags   );
            Type                   = default(MAVBatteryTypeFlags       );
            BatteryRemaining       = default(sbyte                     );
            TimeRemaining          = default(int                       );
            ChargeState            = default(MAVBatteryChargeStateFlags);
            VoltagesExt            = new ushort[  4];
            Mode                   = default(MAVBatteryModeFlags       );
            FaultBitmask           = default(MAVBatteryFaultFlags      );
        }
        #endregion

        #region Read Buffer
        /// <summary>
        /// Reads the data from Buffer into this struct
        /// </summary>    
        public int Read(byte[] p_buffer,int p_offset=0) {
            int    l = 54;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals
            int[]  LS8  = MAVLinkCRC.U8_LSH8, LS16 = MAVLinkCRC.U8_LSH16, LS24 = MAVLinkCRC.U8_LSH24, LS32 = MAVLinkCRC.U8_LSH32, LS40 = MAVLinkCRC.U8_LSH40, LS48 = MAVLinkCRC.U8_LSH48, LS56 = MAVLinkCRC.U8_LSH56;
            Span<byte> b = p_buffer.AsSpan(p_offset);            
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            CurrentConsumed        = (int                       ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            EnergyConsumed         = (int                       ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            Temperature            = (short                     ) (b[p++] | LS8[b[p++]]);
            for(int i=0;i<10 ;i++) { Voltages[i]            = (ushort                    ) (b[p++] | LS8[b[p++]]); }
            CurrentBattery         = (short                     ) (b[p++] | LS8[b[p++]]);
            Id                     = (byte                      ) (b[p++]);
            BatteryFunction        = (MAVBatteryFunctionFlags   ) (b[p++]);
            Type                   = (MAVBatteryTypeFlags       ) (b[p++]);
            BatteryRemaining       = (sbyte                     ) (b[p++]);
            TimeRemaining          = (int                       ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            ChargeState            = (MAVBatteryChargeStateFlags) (b[p++]);
            for(int i=0;i<4  ;i++) { VoltagesExt[i]         = (ushort                    ) (b[p++] | LS8[b[p++]]); }
            Mode                   = (MAVBatteryModeFlags       ) (b[p++]);
            FaultBitmask           = (MAVBatteryFaultFlags      ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);            
            return p;
        }
        #endregion

        #region Write Buffer
        /// <summary>
        /// Writes the message data into a Buffer
        /// </summary>    
        public int Write(byte[] p_buffer,int p_offset=0) {
            int    l = 54;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals            
            Span<byte> b = p_buffer.AsSpan(p_offset);
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            b[p++] = (byte)(      CurrentConsumed);
            b[p++] = (byte)((int)CurrentConsumed>>8 );
            b[p++] = (byte)((int)CurrentConsumed>>16);
            b[p++] = (byte)((int)CurrentConsumed>>24);
            b[p++] = (byte)(      EnergyConsumed);
            b[p++] = (byte)((int)EnergyConsumed>>8 );
            b[p++] = (byte)((int)EnergyConsumed>>16);
            b[p++] = (byte)((int)EnergyConsumed>>24);
            b[p++] = (byte)(      Temperature);
            b[p++] = (byte)((int)Temperature>>8 );
            for(int i=0;i< 10;i++) {
                b[p++] = (byte)(      Voltages[i]);
                b[p++] = (byte)((int)Voltages[i]>>8 );
            }
            b[p++] = (byte)(      CurrentBattery);
            b[p++] = (byte)((int)CurrentBattery>>8 );
            b[p++] = (byte)(Id);
            b[p++] = (byte)(BatteryFunction);
            b[p++] = (byte)(Type);
            b[p++] = (byte)(BatteryRemaining);
            b[p++] = (byte)(      TimeRemaining);
            b[p++] = (byte)((int)TimeRemaining>>8 );
            b[p++] = (byte)((int)TimeRemaining>>16);
            b[p++] = (byte)((int)TimeRemaining>>24);
            b[p++] = (byte)(ChargeState);
            for(int i=0;i<  4;i++) {
                b[p++] = (byte)(      VoltagesExt[i]);
                b[p++] = (byte)((int)VoltagesExt[i]>>8 );
            }
            b[p++] = (byte)(Mode);
            b[p++] = (byte)(      FaultBitmask);
            b[p++] = (byte)((int)FaultBitmask>>8 );
            b[p++] = (byte)((int)FaultBitmask>>16);
            b[p++] = (byte)((int)FaultBitmask>>24);
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
            int l = 54;
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
