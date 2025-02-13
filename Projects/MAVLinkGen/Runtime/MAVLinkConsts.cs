        
using System.IO;

namespace MAVLinkBindings {

    /// <summary>
    /// Bit Enumeration for Incompatiblity Flags
    /// </summary>
    public enum MAVLinkIncFlags : int {
        Empty   = 0,
        Signed  = (1 << 0),
    }

    /// <summary>
    /// Class that describes a MAVLink Message Container
    /// </summary>    
    public class MAVLinkConsts {
        public const string BUILD_DATE                      = "Thu Jan 30 2025";
        public const string WIRE_PROTOCOL_VERSION           = "2.0";
        public const int    MAX_PAYLOAD_LEN                 = 255;
        public const byte   MAVLINK_V2_HEADER_LEN           = 9;                     // Length of core header (of the comm. layer)
        public const byte   MAVLINK_V1_HEADER_LEN           = 5;                     // Length of MAVLink1 core header (of the comm. layer)
        public const byte   NUM_HEADER_BYTES                = (MAVLINK_V2_HEADER_LEN + 1); // Length of all header bytes, including core and stx
        public const byte   NUM_CHECKSUM_BYTES              = 2;
        public const byte   NUM_NON_PAYLOAD_BYTES           = (NUM_HEADER_BYTES + NUM_CHECKSUM_BYTES);
        public const int    MAX_PACKET_LEN                  = (MAX_PAYLOAD_LEN  + NUM_NON_PAYLOAD_BYTES + SIGNATURE_BLOCK_LEN); //< Maximum packet length
        public const int    MIN_PACKET_LEN                  = (1 + MAVLINK_V2_HEADER_LEN + 0 + NUM_CHECKSUM_BYTES);
        public const byte   SIGNATURE_BLOCK_LEN             = 13;
        public const int    LITTLE_ENDIAN                   = 1;
        public const int    BIG_ENDIAN                      = 0;
        public const byte   STX_MAVLINK_V2                  = 0xFD;
        public const byte   STX_MAVLINK_V1                  = 0xFE;
        public const byte   ENDIAN                          = LITTLE_ENDIAN;
        public const bool   ALIGNED_FIELDS                  = (1 == 1);
        public const byte   CRC_EXTRA                       = 1;    
        public const byte   COMMAND_24BIT                   = 1;        
        public const bool   NEED_BYTE_SWAP                  = (ENDIAN == LITTLE_ENDIAN);        
    }

}
