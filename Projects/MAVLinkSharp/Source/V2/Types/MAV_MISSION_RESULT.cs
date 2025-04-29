        
namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// Result of mission operation (in a MISSION_ACK message).
    /// </summary>    
    public enum MAVMissionResultFlags {
        MavMissionAccepted              = 0,           //mission accepted OK
        MavMissionError                 = 1,           //Generic error / not accepting mission commands at all right now.
        MavMissionUnsupportedFrame      = 2,           //Coordinate frame is not supported.
        MavMissionUnsupported           = 3,           //Command is not supported.
        MavMissionNoSpace               = 4,           //Mission items exceed storage space.
        MavMissionInvalid               = 5,           //One of the parameters has an invalid value.
        MavMissionInvalidParam1         = 6,           //param1 has an invalid value.
        MavMissionInvalidParam2         = 7,           //param2 has an invalid value.
        MavMissionInvalidParam3         = 8,           //param3 has an invalid value.
        MavMissionInvalidParam4         = 9,           //param4 has an invalid value.
        MavMissionInvalidParam5X        = 10,          //x / param5 has an invalid value.
        MavMissionInvalidParam6Y        = 11,          //y / param6 has an invalid value.
        MavMissionInvalidParam7         = 12,          //z / param7 has an invalid value.
        MavMissionInvalidSequence       = 13,          //Mission item received out of sequence
        MavMissionDenied                = 14,          //Not accepting any mission commands from this communication partner.
        MavMissionOperationCancelled    = 15           //Current mission operation cancelled (e.g. mission upload, mission download).
    }

}
