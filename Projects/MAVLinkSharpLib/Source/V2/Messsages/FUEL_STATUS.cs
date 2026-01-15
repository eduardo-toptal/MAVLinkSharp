        
using System;
using System.IO;
using System.Runtime.InteropServices;
using MAVLinkSharp.Runtime;

#pragma warning disable CS0675

namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// Fuel status.
    /// This message provides "generic" fuel level information for  in a GCS and for triggering failsafes in an autopilot.
    /// The fuel type and associated units for fields in this message are defined in the enum MAV_FUEL_TYPE.
    /// 
    /// The reported `consumed_fuel` and `remaining_fuel` must only be supplied if measured: they must not be inferred from the `maximum_fuel` and the other value.
    /// A recipient can assume that if these fields are supplied they are accurate.
    /// If not provided, the recipient can infer `remaining_fuel` from `maximum_fuel` and `consumed_fuel` on the assumption that the fuel was initially at its maximum (this is what battery monitors assume).
    /// Note however that this is an assumption, and the UI should prompt the user appropriately (i.e. notify user that they should fill the tank before boot).
    /// 
    /// This kind of information may also be sent in fuel-specific messages such as BATTERY_STATUS_V2.
    /// If both messages are sent for the same fuel system, the ids and corresponding information must match.
    /// 
    /// This should be streamed (nominally at 0.1 Hz).
    /// 
    /// </summary>    
    public struct FuelStatusData : IMAVLinkMessageData {

        /// <summary>
        /// Message Id Associated w/ this Struct
        /// </summary>    
        public int GetId() { return 371; }

        public float             MaximumFuel;          //Capacity when full. Must be provided.
        public float             ConsumedFuel;         //Consumed fuel (measured). This value should not be inferred: if not measured set to NaN. NaN: field not provided.
        public float             RemainingFuel;        //Remaining fuel until empty (measured). The value should not be inferred: if not measured set to NaN. NaN: field not provided.
        public float             FlowRate;             //Positive value when emptying/using, and negative if filling/replacing. NaN: field not provided.
        public float             Temperature;          //Fuel temperature. NaN: field not provided.
        public MAVFuelTypeFlags  FuelType;             //Fuel type. Defines units for fuel capacity and consumption fields above.
        public byte              Id;                   //Fuel ID. Must match ID of other messages for same fuel system, such as BATTERY_STATUS_V2.
        public byte              PercentRemaining;     //Percentage of remaining fuel, relative to full. Values: [0-100], UINT8_MAX: field not provided.    

        #region CTOR
        /// <summary>
        /// Instantiates a new FuelStatusData
        /// </summary>    
        /*
        public FuelStatusData() {
            Init();
        }
        */
        public void Init() {
            MaximumFuel            = default(float           );
            ConsumedFuel           = default(float           );
            RemainingFuel          = default(float           );
            FlowRate               = default(float           );
            Temperature            = default(float           );
            FuelType               = default(MAVFuelTypeFlags);
            Id                     = default(byte            );
            PercentRemaining       = default(byte            );
        }
        #endregion

        #region Read Buffer
        /// <summary>
        /// Reads the data from Buffer into this struct
        /// </summary>    
        public int Read(byte[] p_buffer,int p_offset=0) {
            int    l = 26;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals
            int[]  LS8  = MAVLinkCRC.U8_LSH8, LS16 = MAVLinkCRC.U8_LSH16, LS24 = MAVLinkCRC.U8_LSH24, LS32 = MAVLinkCRC.U8_LSH32, LS40 = MAVLinkCRC.U8_LSH40, LS48 = MAVLinkCRC.U8_LSH48, LS56 = MAVLinkCRC.U8_LSH56;
            Span<byte> b = p_buffer.AsSpan(p_offset);            
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            MaximumFuel            = (float           ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            ConsumedFuel           = (float           ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            RemainingFuel          = (float           ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            FlowRate               = (float           ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            Temperature            = (float           ) MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4;
            FuelType               = (MAVFuelTypeFlags) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            Id                     = (byte            ) (b[p++]);
            PercentRemaining       = (byte            ) (b[p++]);            
            return p;
        }
        #endregion

        #region Write Buffer
        /// <summary>
        /// Writes the message data into a Buffer
        /// </summary>    
        public int Write(byte[] p_buffer,int p_offset=0) {
            int    l = 26;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals            
            Span<byte> b = p_buffer.AsSpan(p_offset);
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            MemoryMarshal.Write(b.Slice(p, 4), ref MaximumFuel           ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref ConsumedFuel          ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref RemainingFuel         ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref FlowRate              ); p+=4;
            MemoryMarshal.Write(b.Slice(p, 4), ref Temperature           ); p+=4;
            b[p++] = (byte)(      FuelType);
            b[p++] = (byte)((int)FuelType>>8 );
            b[p++] = (byte)((int)FuelType>>16);
            b[p++] = (byte)((int)FuelType>>24);
            b[p++] = (byte)(Id);
            b[p++] = (byte)(PercentRemaining);
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
            int l = 26;
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
