        
using System.IO;
using System.Runtime.InteropServices;
using MAVLinkSharp.Runtime;

#pragma warning disable CS0675

namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// Event message. Each new event from a particular component gets a new sequence number. The same message might be sent multiple times if (re-)requested. Most events are broadcast, some can be specific to a target component (as receivers keep track of the sequence for missed events, all events need to be broadcast. Thus we use destination_component instead of target_component).
    /// </summary>    
    public struct EventData : IMAVLinkMessageData {

        /// <summary>
        /// Message Id Associated w/ this Struct
        /// </summary>    
        public int GetId() { return 410; }

        public uint    Id;                       //Event ID (as defined in the component metadata)
        public uint    EventTimeBootMs;          //Timestamp (time since system boot when the event happened).
        public ushort  Sequence;                 //Sequence number.
        public byte    DestinationComponent;     //Component ID
        public byte    DestinationSystem;        //System ID
        public byte    LogLevels;                //Log levels: 4 bits MSB: internal (for logging purposes), 4 bits LSB: external. Levels: Emergency = 0, Alert = 1, Critical = 2, Error = 3, Warning = 4, Notice = 5, Info = 6, Debug = 7, Protocol = 8, Disabled = 9
        public byte[]  Arguments;                //Arguments (depend on event ID).    

        #region CTOR
        /// <summary>
        /// Instantiates a new EventData
        /// </summary>    
        public EventData() {
            Id                         = default(uint  );
            EventTimeBootMs            = default(uint  );
            Sequence                   = default(ushort);
            DestinationComponent       = default(byte  );
            DestinationSystem          = default(byte  );
            LogLevels                  = default(byte  );
            Arguments                  = new byte[ 40];
        }
        #endregion

        #region Read Buffer
        /// <summary>
        /// Reads the data from Buffer into this struct
        /// </summary>    
        public int Read(byte[] p_buffer,int p_offset=0) {
            int    l = 53;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals
            int[]  LS8  = MAVLinkCRC.U8_LSH8, LS16 = MAVLinkCRC.U8_LSH16, LS24 = MAVLinkCRC.U8_LSH24, LS32 = MAVLinkCRC.U8_LSH32, LS40 = MAVLinkCRC.U8_LSH40, LS48 = MAVLinkCRC.U8_LSH48, LS56 = MAVLinkCRC.U8_LSH56;
            Span<byte> b = p_buffer.AsSpan(p_offset);            
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            Id                         = (uint  ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            EventTimeBootMs            = (uint  ) (b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]]);
            Sequence                   = (ushort) (b[p++] | LS8[b[p++]]);
            DestinationComponent       = (byte  ) (b[p++]);
            DestinationSystem          = (byte  ) (b[p++]);
            LogLevels                  = (byte  ) (b[p++]);
            for(int i=0;i<40 ;i++) { Arguments[i]               = (byte  ) (b[p++]); }            
            return p;
        }
        #endregion

        #region Write Buffer
        /// <summary>
        /// Writes the message data into a Buffer
        /// </summary>    
        public int Write(byte[] p_buffer,int p_offset=0) {
            int    l = 53;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals            
            Span<byte> b = p_buffer.AsSpan(p_offset);
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            b[p++] = (byte)(      Id);
            b[p++] = (byte)((int)Id>>8 );
            b[p++] = (byte)((int)Id>>16);
            b[p++] = (byte)((int)Id>>24);
            b[p++] = (byte)(      EventTimeBootMs);
            b[p++] = (byte)((int)EventTimeBootMs>>8 );
            b[p++] = (byte)((int)EventTimeBootMs>>16);
            b[p++] = (byte)((int)EventTimeBootMs>>24);
            b[p++] = (byte)(      Sequence);
            b[p++] = (byte)((int)Sequence>>8 );
            b[p++] = (byte)(DestinationComponent);
            b[p++] = (byte)(DestinationSystem);
            b[p++] = (byte)(LogLevels);
            for(int i=0;i< 40;i++) {
                b[p++] = (byte)(Arguments[i]);
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
            int l = 53;
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
