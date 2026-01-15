        
namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// Gimbal device (low level) capability flags (bitmap).
    /// </summary>    
    public enum GimbalDeviceCapFlags {
        HasRetract                                          = 1,           //Gimbal device supports a retracted position.
        HasNeutral                                          = 2,           //Gimbal device supports a horizontal, forward looking position, stabilized.
        HasRollAxis                                         = 4,           //Gimbal device supports rotating around roll axis.
        HasRollFollow                                       = 8,           //Gimbal device supports to follow a roll angle relative to the vehicle.
        HasRollLock                                         = 16,          //Gimbal device supports locking to a roll angle (generally that's the default with roll stabilized).
        HasPitchAxis                                        = 32,          //Gimbal device supports rotating around pitch axis.
        HasPitchFollow                                      = 64,          //Gimbal device supports to follow a pitch angle relative to the vehicle.
        HasPitchLock                                        = 128,         //Gimbal device supports locking to a pitch angle (generally that's the default with pitch stabilized).
        HasYawAxis                                          = 256,         //Gimbal device supports rotating around yaw axis.
        HasYawFollow                                        = 512,         //Gimbal device supports to follow a yaw angle relative to the vehicle (generally that's the default).
        HasYawLock                                          = 1024,        //Gimbal device supports locking to an absolute heading, i.e., yaw angle relative to North (earth frame, often this is an option available).
        SupportsInfiniteYaw                                 = 2048,        //Gimbal device supports yawing/panning infinitely (e.g. using slip disk).
        SupportsYawInEarthFrame                             = 4096,        //Gimbal device supports yaw angles and angular velocities relative to North (earth frame). This usually requires support by an autopilot via AUTOPILOT_STATE_FOR_GIMBAL_DEVICE. Support can go on and off during runtime, which is reported by the flag GIMBAL_DEVICE_FLAGS_CAN_ACCEPT_YAW_IN_EARTH_FRAME.
        HasRcInputs                                         = 8192         //Gimbal device supports radio control inputs as an alternative input for controlling the gimbal orientation.
    }

}
