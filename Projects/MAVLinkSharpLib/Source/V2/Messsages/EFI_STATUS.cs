        
using System;
using System.IO;
using System.Runtime.InteropServices;
using MAVLinkSharp.Runtime;

#pragma warning disable CS0675

namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// EFI status output
    /// </summary>    
    public struct EfiStatusData : IMAVLinkMessageData {

        /// <summary>
        /// Message Id Associated w/ this Struct
        /// </summary>    
        public int GetId() { return 225; }

        public float  EcuIndex;                       //ECU index
        public float  Rpm;                            //RPM
        public float  FuelConsumed;                   //Fuel consumed
        public float  FuelFlow;                       //Fuel flow rate
        public float  EngineLoad;                     //Engine load
        public float  ThrottlePosition;               //Throttle position
        public float  SparkDwellTime;                 //Spark dwell time
        public float  BarometricPressure;             //Barometric pressure
        public float  IntakeManifoldPressure;         //Intake manifold pressure(
        public float  IntakeManifoldTemperature;      //Intake manifold temperature
        public float  CylinderHeadTemperature;        //Cylinder head temperature
        public float  IgnitionTiming;                 //Ignition timing (Crank angle degrees)
        public float  InjectionTime;                  //Injection time
        public float  ExhaustGasTemperature;          //Exhaust gas temperature
        public float  ThrottleOut;                    //Output throttle
        public float  PtCompensation;                 //Pressure/temperature compensation
        public byte   Health;                         //EFI health status
        public float  IgnitionVoltage;                //Supply voltage to EFI sparking system.  Zero in this value means "unknown", so if the supply voltage really is zero volts use 0.0001 instead.    

        #region CTOR
        /// <summary>
        /// Instantiates a new EfiStatusData
        /// </summary>    
        /*
        public EfiStatusData() {
            Init();
        }
        */
        public void Init() {
            EcuIndex                         = default(float);
            Rpm                              = default(float);
            FuelConsumed                     = default(float);
            FuelFlow                         = default(float);
            EngineLoad                       = default(float);
            ThrottlePosition                 = default(float);
            SparkDwellTime                   = default(float);
            BarometricPressure               = default(float);
            IntakeManifoldPressure           = default(float);
            IntakeManifoldTemperature        = default(float);
            CylinderHeadTemperature          = default(float);
            IgnitionTiming                   = default(float);
            InjectionTime                    = default(float);
            ExhaustGasTemperature            = default(float);
            ThrottleOut                      = default(float);
            PtCompensation                   = default(float);
            Health                           = default(byte );
            IgnitionVoltage                  = default(float);
        }
        #endregion

        #region Read Buffer
        /// <summary>
        /// Reads the data from Buffer into this struct
        /// </summary>    
        public int Read(byte[] p_buffer,int p_offset=0) {
            int    l = 69;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals
            int[]  LS8  = MAVLinkCRC.U8_LSH8, LS16 = MAVLinkCRC.U8_LSH16, LS24 = MAVLinkCRC.U8_LSH24, LS32 = MAVLinkCRC.U8_LSH32, LS40 = MAVLinkCRC.U8_LSH40, LS48 = MAVLinkCRC.U8_LSH48, LS56 = MAVLinkCRC.U8_LSH56;
            Span<byte> b = p_buffer.AsSpan(p_offset);            
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            EcuIndex                         = (float) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Rpm                              = (float) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            FuelConsumed                     = (float) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            FuelFlow                         = (float) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            EngineLoad                       = (float) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            ThrottlePosition                 = (float) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            SparkDwellTime                   = (float) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            BarometricPressure               = (float) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            IntakeManifoldPressure           = (float) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            IntakeManifoldTemperature        = (float) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            CylinderHeadTemperature          = (float) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            IgnitionTiming                   = (float) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            InjectionTime                    = (float) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            ExhaustGasTemperature            = (float) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            ThrottleOut                      = (float) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            PtCompensation                   = (float) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Health                           = (byte ) (b[p++]);
            IgnitionVoltage                  = (float) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;            
            return p;
        }
        #endregion

        #region Write Buffer
        /// <summary>
        /// Writes the message data into a Buffer
        /// </summary>    
        public int Write(byte[] p_buffer,int p_offset=0) {
            int    l = 69;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals            
            Span<byte> b = p_buffer.AsSpan(p_offset);
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            MemoryMarshal.Write(b.Slice(p, 4), ref EcuIndex                        ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref Rpm                             ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref FuelConsumed                    ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref FuelFlow                        ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref EngineLoad                      ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref ThrottlePosition                ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref SparkDwellTime                  ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref BarometricPressure              ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref IntakeManifoldPressure          ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref IntakeManifoldTemperature       ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref CylinderHeadTemperature         ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref IgnitionTiming                  ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref InjectionTime                   ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref ExhaustGasTemperature           ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref ThrottleOut                     ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref PtCompensation                  ); p+=4;
            b[p++] = (byte)(Health);
            MemoryMarshal.Write(b.Slice(p, 4), ref IgnitionVoltage                 ); p+=4;
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
            int l = 69;
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
