        
namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// Bitmask of (optional) autopilot capabilities (64 bit). If a bit is set, the autopilot supports this capability.
    /// </summary>    
    public enum MAVProtocolCapabilityFlags {
        MissionFloat                                                = 1,           //Autopilot supports the MISSION_ITEM float message type. | Note that MISSION_ITEM is deprecated, and autopilots should use MISSION_ITEM_INT instead.
        ParamFloat                                                  = 2,           //Autopilot supports the new param float message type.
        MissionInt                                                  = 4,           //Autopilot supports MISSION_ITEM_INT scaled integer message type. | Note that this flag must always be set if missions are supported, because missions must always use MISSION_ITEM_INT (rather than MISSION_ITEM, which is deprecated).
        CommandInt                                                  = 8,           //Autopilot supports COMMAND_INT scaled integer message type.
        ParamEncodeBytewise                                         = 16,          //Parameter protocol uses byte-wise encoding of parameter values into param_value (float) fields: https://mavlink.io/en/services/parameter.html#parameter-encoding. | Note that either this flag or MAV_PROTOCOL_CAPABILITY_PARAM_ENCODE_C_CAST should be set if the parameter protocol is supported.
        Ftp                                                         = 32,          //Autopilot supports the File Transfer Protocol v1: https://mavlink.io/en/services/ftp.html.
        SetAttitudeTarget                                           = 64,          //Autopilot supports commanding attitude offboard.
        SetPositionTargetLocalNed                                   = 128,         //Autopilot supports commanding position and velocity targets in local NED frame.
        SetPositionTargetGlobalInt                                  = 256,         //Autopilot supports commanding position and velocity targets in global scaled integers.
        Terrain                                                     = 512,         //Autopilot supports terrain protocol / data handling.
        Reserved3                                                   = 1024,        //Reserved for future use.
        FlightTermination                                           = 2048,        //Autopilot supports the MAV_CMD_DO_FLIGHTTERMINATION command (flight termination).
        CompassCalibration                                          = 4096,        //Autopilot supports onboard compass calibration.
        Mavlink2                                                    = 8192,        //Autopilot supports MAVLink version 2.
        MissionFence                                                = 16384,       //Autopilot supports mission fence protocol.
        MissionRally                                                = 32768,       //Autopilot supports mission rally point protocol.
        Reserved2                                                   = 65536,       //Reserved for future use.
        ParamEncodeCCast                                            = 131072,      //Parameter protocol uses C-cast of parameter values to set the param_value (float) fields: https://mavlink.io/en/services/parameter.html#parameter-encoding. | Note that either this flag or MAV_PROTOCOL_CAPABILITY_PARAM_ENCODE_BYTEWISE should be set if the parameter protocol is supported.
        ComponentImplementsGimbalManager                            = 262144,      //This component implements/is a gimbal manager. This means the GIMBAL_MANAGER_INFORMATION, and other messages can be requested.
        ComponentAcceptsGcsControl                                  = 524288,      //Component supports locking control to a particular GCS independent of its system (via MAV_CMD_REQUEST_OPERATOR_CONTROL).
        Gripper                                                     = 1048576      //Autopilot has a connected gripper. MAVLink Grippers would set MAV_TYPE_GRIPPER instead.
    }

}
