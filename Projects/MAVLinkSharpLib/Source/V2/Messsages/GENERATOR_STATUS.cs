        
using System;
using System.IO;
using System.Runtime.InteropServices;
using MAVLinkSharp.Runtime;

#pragma warning disable CS0675

namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// Telemetry of power generation system. Alternator or mechanical generator.
    /// </summary>    
    public struct GeneratorStatusData : IMAVLinkMessageData {

        /// <summary>
        /// Message Id Associated w/ this Struct
        /// </summary>    
        public int GetId() { return 373; }

        public MAVGeneratorStatusFlag  Status;                    //Status flags.
        public float                   BatteryCurrent;            //Current into/out of battery. Positive for out. Negative for in. NaN: field not provided.
        public float                   LoadCurrent;               //Current going to the UAV. If battery current not available this is the DC current from the generator. Positive for out. Negative for in. NaN: field not provided
        public float                   PowerGenerated;            //The power being generated. NaN: field not provided
        public float                   BusVoltage;                //Voltage of the bus seen at the generator, or battery bus if battery bus is controlled by generator and at a different voltage to main bus.
        public float                   BatCurrentSetpoint;        //The target battery current. Positive for out. Negative for in. NaN: field not provided
        public uint                    Runtime;                   //Seconds this generator has run since it was rebooted. UINT32_MAX: field not provided.
        public int                     TimeUntilMaintenance;      //Seconds until this generator requires maintenance.  A negative value indicates maintenance is past-due. INT32_MAX: field not provided.
        public ushort                  GeneratorSpeed;            //Speed of electrical generator or alternator. UINT16_MAX: field not provided.
        public short                   RectifierTemperature;      //The temperature of the rectifier or power converter. INT16_MAX: field not provided.
        public short                   GeneratorTemperature;      //The temperature of the mechanical motor, fuel cell core or generator. INT16_MAX: field not provided.    

        #region CTOR
        /// <summary>
        /// Instantiates a new GeneratorStatusData
        /// </summary>    
        /*
        public GeneratorStatusData() {
            Init();
        }
        */
        public void Init() {
            Status                      = default(MAVGeneratorStatusFlag);
            BatteryCurrent              = default(float                 );
            LoadCurrent                 = default(float                 );
            PowerGenerated              = default(float                 );
            BusVoltage                  = default(float                 );
            BatCurrentSetpoint          = default(float                 );
            Runtime                     = default(uint                  );
            TimeUntilMaintenance        = default(int                   );
            GeneratorSpeed              = default(ushort                );
            RectifierTemperature        = default(short                 );
            GeneratorTemperature        = default(short                 );
        }
        #endregion

        #region Read Buffer
        /// <summary>
        /// Reads the data from Buffer into this struct
        /// </summary>    
        public int Read(byte[] p_buffer,int p_offset=0) {
            int    l = 42;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals
            int[]  LS8  = MAVLinkCRC.U8_LSH8, LS16 = MAVLinkCRC.U8_LSH16, LS24 = MAVLinkCRC.U8_LSH24, LS32 = MAVLinkCRC.U8_LSH32, LS40 = MAVLinkCRC.U8_LSH40, LS48 = MAVLinkCRC.U8_LSH48, LS56 = MAVLinkCRC.U8_LSH56;
            Span<byte> b = p_buffer.AsSpan(p_offset);            
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            Status                      = (MAVGeneratorStatusFlag) ((ulong)b[p++] | (ulong)LS8[b[p++]] | (ulong)LS16[b[p++]] | (ulong)LS24[b[p++]] | (ulong)LS32[b[p++]] | (ulong)LS40[b[p++]] | (ulong)LS48[b[p++]] | (ulong)LS56[b[p++]]);
            BatteryCurrent              = (float                 ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            LoadCurrent                 = (float                 ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            PowerGenerated              = (float                 ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            BusVoltage                  = (float                 ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            BatCurrentSetpoint          = (float                 ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Runtime                     = (uint                  ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            TimeUntilMaintenance        = (int                   ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            GeneratorSpeed              = (ushort                ) (b[p++] | LS8[b[p++]]);
            RectifierTemperature        = (short                 ) (b[p++] | LS8[b[p++]]);
            GeneratorTemperature        = (short                 ) (b[p++] | LS8[b[p++]]);            
            return p;
        }
        #endregion

        #region Write Buffer
        /// <summary>
        /// Writes the message data into a Buffer
        /// </summary>    
        public int Write(byte[] p_buffer,int p_offset=0) {
            int    l = 42;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals            
            Span<byte> b = p_buffer.AsSpan(p_offset);
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            b[p++] = (byte)(      Status);
            b[p++] = (byte)((long)Status>>8 );
            b[p++] = (byte)((long)Status>>16);
            b[p++] = (byte)((long)Status>>24);
            b[p++] = (byte)((long)Status>>32);
            b[p++] = (byte)((long)Status>>40);
            b[p++] = (byte)((long)Status>>48);
            b[p++] = (byte)((long)Status>>56);
            MemoryMarshal.Write(b.Slice(p, 4), ref BatteryCurrent             ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref LoadCurrent                ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref PowerGenerated             ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref BusVoltage                 ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref BatCurrentSetpoint         ); p+=4;
            b[p++] = (byte)(      Runtime);
            b[p++] = (byte)((int)Runtime>>8 );
            b[p++] = (byte)((int)Runtime>>16);
            b[p++] = (byte)((int)Runtime>>24);
            b[p++] = (byte)(      TimeUntilMaintenance);
            b[p++] = (byte)((int)TimeUntilMaintenance>>8 );
            b[p++] = (byte)((int)TimeUntilMaintenance>>16);
            b[p++] = (byte)((int)TimeUntilMaintenance>>24);
            b[p++] = (byte)(      GeneratorSpeed);
            b[p++] = (byte)((int)GeneratorSpeed>>8 );
            b[p++] = (byte)(      RectifierTemperature);
            b[p++] = (byte)((int)RectifierTemperature>>8 );
            b[p++] = (byte)(      GeneratorTemperature);
            b[p++] = (byte)((int)GeneratorTemperature>>8 );
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
            int l = 42;
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
