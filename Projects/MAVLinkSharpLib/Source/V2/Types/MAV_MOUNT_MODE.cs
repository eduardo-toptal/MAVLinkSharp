        
namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// Enumeration of possible mount operation modes. This message is used by obsolete/deprecated gimbal messages.
    /// </summary>    
    public enum MAVMountModeFlags {
        Retract                          = 0,           //Load and keep safe position (Roll,Pitch,Yaw) from permant memory and stop stabilization
        Neutral                          = 1,           //Load and keep neutral position (Roll,Pitch,Yaw) from permanent memory.
        MavlinkTargeting                 = 2,           //Load neutral position and start MAVLink Roll,Pitch,Yaw control with stabilization
        RcTargeting                      = 3,           //Load neutral position and start RC Roll,Pitch,Yaw control with stabilization
        GpsPoint                         = 4,           //Load neutral position and start to point to Lat,Lon,Alt
        SysidTarget                      = 5,           //Gimbal tracks system with specified system ID
        HomeLocation                     = 6            //Gimbal tracks home position
    }

}
