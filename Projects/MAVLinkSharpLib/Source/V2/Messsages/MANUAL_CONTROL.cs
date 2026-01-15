        
using System;
using System.IO;
using System.Runtime.InteropServices;
using MAVLinkSharp.Runtime;

#pragma warning disable CS0675

namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// Manual (joystick) control message.
    /// This message represents movement axes and button using standard joystick axes nomenclature. Unused axes can be disabled and buttons states are transmitted as individual on/off bits of a bitmask. For more information see https://mavlink.io/en/manual_control.html
    /// </summary>    
    public struct ManualControlData : IMAVLinkMessageData {

        /// <summary>
        /// Message Id Associated w/ this Struct
        /// </summary>    
        public int GetId() { return 69; }

        public short   X;                     //X-axis, normalized to the range [-1000,1000]. A value of INT16_MAX indicates that this axis is invalid. Generally corresponds to forward(1000)-backward(-1000) movement on a joystick and the pitch of a vehicle.
        public short   Y;                     //Y-axis, normalized to the range [-1000,1000]. A value of INT16_MAX indicates that this axis is invalid. Generally corresponds to left(-1000)-right(1000) movement on a joystick and the roll of a vehicle.
        public short   Z;                     //Z-axis, normalized to the range [-1000,1000]. A value of INT16_MAX indicates that this axis is invalid. Generally corresponds to a separate slider movement with maximum being 1000 and minimum being -1000 on a joystick and the thrust of a vehicle. Positive values are positive thrust, negative values are negative thrust.
        public short   R;                     //R-axis, normalized to the range [-1000,1000]. A value of INT16_MAX indicates that this axis is invalid. Generally corresponds to a twisting of the joystick, with clockwise being 1000 and counter-clockwise being -1000, and the yaw of a vehicle.
        public ushort  Buttons;               //A bitfield corresponding to the joystick buttons' 0-15 current state, 1 for pressed, 0 for released. The lowest bit corresponds to Button 1.
        public byte    Target;                //The system to be controlled.
        public ushort  Buttons2;              //A bitfield corresponding to the joystick buttons' 16-31 current state, 1 for pressed, 0 for released. The lowest bit corresponds to Button 16.
        public byte    EnabledExtensions;     //Set bits to 1 to indicate which of the following extension fields contain valid data: bit 0: pitch, bit 1: roll, bit 2: aux1, bit 3: aux2, bit 4: aux3, bit 5: aux4, bit 6: aux5, bit 7: aux6
        public short   S;                     //Pitch-only-axis, normalized to the range [-1000,1000]. Generally corresponds to pitch on vehicles with additional degrees of freedom. Valid if bit 0 of enabled_extensions field is set. Set to 0 if invalid.
        public short   T;                     //Roll-only-axis, normalized to the range [-1000,1000]. Generally corresponds to roll on vehicles with additional degrees of freedom. Valid if bit 1 of enabled_extensions field is set. Set to 0 if invalid.
        public short   Aux1;                  //Aux continuous input field 1. Normalized in the range [-1000,1000]. Purpose defined by recipient. Valid data if bit 2 of enabled_extensions field is set. 0 if bit 2 is unset.
        public short   Aux2;                  //Aux continuous input field 2. Normalized in the range [-1000,1000]. Purpose defined by recipient. Valid data if bit 3 of enabled_extensions field is set. 0 if bit 3 is unset.
        public short   Aux3;                  //Aux continuous input field 3. Normalized in the range [-1000,1000]. Purpose defined by recipient. Valid data if bit 4 of enabled_extensions field is set. 0 if bit 4 is unset.
        public short   Aux4;                  //Aux continuous input field 4. Normalized in the range [-1000,1000]. Purpose defined by recipient. Valid data if bit 5 of enabled_extensions field is set. 0 if bit 5 is unset.
        public short   Aux5;                  //Aux continuous input field 5. Normalized in the range [-1000,1000]. Purpose defined by recipient. Valid data if bit 6 of enabled_extensions field is set. 0 if bit 6 is unset.
        public short   Aux6;                  //Aux continuous input field 6. Normalized in the range [-1000,1000]. Purpose defined by recipient. Valid data if bit 7 of enabled_extensions field is set. 0 if bit 7 is unset.    

        #region CTOR
        /// <summary>
        /// Instantiates a new ManualControlData
        /// </summary>    
        /*
        public ManualControlData() {
            Init();
        }
        */
        public void Init() {
            X                       = default(short );
            Y                       = default(short );
            Z                       = default(short );
            R                       = default(short );
            Buttons                 = default(ushort);
            Target                  = default(byte  );
            Buttons2                = default(ushort);
            EnabledExtensions       = default(byte  );
            S                       = default(short );
            T                       = default(short );
            Aux1                    = default(short );
            Aux2                    = default(short );
            Aux3                    = default(short );
            Aux4                    = default(short );
            Aux5                    = default(short );
            Aux6                    = default(short );
        }
        #endregion

        #region Read Buffer
        /// <summary>
        /// Reads the data from Buffer into this struct
        /// </summary>    
        public int Read(byte[] p_buffer,int p_offset=0) {
            int    l = 30;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals
            int[]  LS8  = MAVLinkCRC.U8_LSH8, LS16 = MAVLinkCRC.U8_LSH16, LS24 = MAVLinkCRC.U8_LSH24, LS32 = MAVLinkCRC.U8_LSH32, LS40 = MAVLinkCRC.U8_LSH40, LS48 = MAVLinkCRC.U8_LSH48, LS56 = MAVLinkCRC.U8_LSH56;
            Span<byte> b = p_buffer.AsSpan(p_offset);            
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            X                       = (short ) (b[p++] | LS8[b[p++]]);
            Y                       = (short ) (b[p++] | LS8[b[p++]]);
            Z                       = (short ) (b[p++] | LS8[b[p++]]);
            R                       = (short ) (b[p++] | LS8[b[p++]]);
            Buttons                 = (ushort) (b[p++] | LS8[b[p++]]);
            Target                  = (byte  ) (b[p++]);
            Buttons2                = (ushort) (b[p++] | LS8[b[p++]]);
            EnabledExtensions       = (byte  ) (b[p++]);
            S                       = (short ) (b[p++] | LS8[b[p++]]);
            T                       = (short ) (b[p++] | LS8[b[p++]]);
            Aux1                    = (short ) (b[p++] | LS8[b[p++]]);
            Aux2                    = (short ) (b[p++] | LS8[b[p++]]);
            Aux3                    = (short ) (b[p++] | LS8[b[p++]]);
            Aux4                    = (short ) (b[p++] | LS8[b[p++]]);
            Aux5                    = (short ) (b[p++] | LS8[b[p++]]);
            Aux6                    = (short ) (b[p++] | LS8[b[p++]]);            
            return p;
        }
        #endregion

        #region Write Buffer
        /// <summary>
        /// Writes the message data into a Buffer
        /// </summary>    
        public int Write(byte[] p_buffer,int p_offset=0) {
            int    l = 30;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals            
            Span<byte> b = p_buffer.AsSpan(p_offset);
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            b[p++] = (byte)(      X);
            b[p++] = (byte)((int)X>>8 );
            b[p++] = (byte)(      Y);
            b[p++] = (byte)((int)Y>>8 );
            b[p++] = (byte)(      Z);
            b[p++] = (byte)((int)Z>>8 );
            b[p++] = (byte)(      R);
            b[p++] = (byte)((int)R>>8 );
            b[p++] = (byte)(      Buttons);
            b[p++] = (byte)((int)Buttons>>8 );
            b[p++] = (byte)(Target);
            b[p++] = (byte)(      Buttons2);
            b[p++] = (byte)((int)Buttons2>>8 );
            b[p++] = (byte)(EnabledExtensions);
            b[p++] = (byte)(      S);
            b[p++] = (byte)((int)S>>8 );
            b[p++] = (byte)(      T);
            b[p++] = (byte)((int)T>>8 );
            b[p++] = (byte)(      Aux1);
            b[p++] = (byte)((int)Aux1>>8 );
            b[p++] = (byte)(      Aux2);
            b[p++] = (byte)((int)Aux2>>8 );
            b[p++] = (byte)(      Aux3);
            b[p++] = (byte)((int)Aux3>>8 );
            b[p++] = (byte)(      Aux4);
            b[p++] = (byte)((int)Aux4>>8 );
            b[p++] = (byte)(      Aux5);
            b[p++] = (byte)((int)Aux5>>8 );
            b[p++] = (byte)(      Aux6);
            b[p++] = (byte)((int)Aux6>>8 );
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
            int l = 30;
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
