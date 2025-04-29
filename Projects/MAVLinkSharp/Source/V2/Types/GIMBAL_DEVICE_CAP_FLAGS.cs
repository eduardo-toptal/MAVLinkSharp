        
namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// Gimbal device (low level) capability flags (bitmap)
    /// </summary>    
    public enum GimbalDeviceCapFlags {
        HasRetract                                    = 1,           //Gimbal device supports a retracted position
        HasNeutral                                    = 2,           //Gimbal device supports a horizontal, forward looking position, stabilized
        HasRollAxis                                   = 4,           //Gimbal device supports rotating around roll axis.
        HasRollFollow                                 = 8,           //Gimbal device supports to follow a roll angle relative to the vehicle
        HasRollLock                                   = 16,          //Gimbal device supports locking to an roll angle (generally that's the default with roll stabilized)
        HasPitchAxis                                  = 32,          //Gimbal device supports rotating around pitch axis.
        HasPitchFollow                                = 64,          //Gimbal device supports to follow a pitch angle relative to the vehicle
        HasPitchLock                                  = 128,         //Gimbal device supports locking to an pitch angle (generally that's the default with pitch stabilized)
        HasYawAxis                                    = 256,         //Gimbal device supports rotating around yaw axis.
        HasYawFollow                                  = 512,         //Gimbal device supports to follow a yaw angle relative to the vehicle (generally that's the default)
        HasYawLock                                    = 1024,        //Gimbal device supports locking to an absolute heading (often this is an option available)
        SupportsInfiniteYaw                           = 2048         //Gimbal device supports yawing/panning infinetely (e.g. using slip disk).
    }

}
