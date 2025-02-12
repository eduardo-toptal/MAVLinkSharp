        
namespace MAVLinkBindings {

    /// <summary>
    /// A data stream is not a fixed set of messages, but rather a
    ///      recommendation to the autopilot software. Individual autopilots may or may not obey
    ///      the recommended messages.
    /// </summary>    
    public enum MAVDataStreamFlags {
        All                             = 0,           //Enable all data streams
        RawSensors                      = 1,           //Enable IMU_RAW, GPS_RAW, GPS_STATUS packets.
        ExtendedStatus                  = 2,           //Enable GPS_STATUS, CONTROL_STATUS, AUX_STATUS
        RcChannels                      = 3,           //Enable RC_CHANNELS_SCALED, RC_CHANNELS_RAW, SERVO_OUTPUT_RAW
        RawController                   = 4,           //Enable ATTITUDE_CONTROLLER_OUTPUT, POSITION_CONTROLLER_OUTPUT, NAV_CONTROLLER_OUTPUT.
        Position                        = 6,           //Enable LOCAL_POSITION, GLOBAL_POSITION_INT messages.
        Extra1                          = 10,          //Dependent on the autopilot
        Extra2                          = 11,          //Dependent on the autopilot
        Extra3                          = 12           //Dependent on the autopilot
    }

}
