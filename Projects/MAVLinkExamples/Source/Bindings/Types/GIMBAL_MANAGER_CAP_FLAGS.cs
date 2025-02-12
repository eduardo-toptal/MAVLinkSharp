        
namespace MAVLinkBindings {

    /// <summary>
    /// Gimbal manager high level capability flags (bitmap). The first 16 bits are identical to the GIMBAL_DEVICE_CAP_FLAGS. However, the gimbal manager does not need to copy the flags from the gimbal but can also enhance the capabilities and thus add flags.
    /// </summary>    
    public enum GimbalManagerCapFlags {
        HasRetract                                         = 1,           //Based on GIMBAL_DEVICE_CAP_FLAGS_HAS_RETRACT.
        HasNeutral                                         = 2,           //Based on GIMBAL_DEVICE_CAP_FLAGS_HAS_NEUTRAL.
        HasRollAxis                                        = 4,           //Based on GIMBAL_DEVICE_CAP_FLAGS_HAS_ROLL_AXIS.
        HasRollFollow                                      = 8,           //Based on GIMBAL_DEVICE_CAP_FLAGS_HAS_ROLL_FOLLOW.
        HasRollLock                                        = 16,          //Based on GIMBAL_DEVICE_CAP_FLAGS_HAS_ROLL_LOCK.
        HasPitchAxis                                       = 32,          //Based on GIMBAL_DEVICE_CAP_FLAGS_HAS_PITCH_AXIS.
        HasPitchFollow                                     = 64,          //Based on GIMBAL_DEVICE_CAP_FLAGS_HAS_PITCH_FOLLOW.
        HasPitchLock                                       = 128,         //Based on GIMBAL_DEVICE_CAP_FLAGS_HAS_PITCH_LOCK.
        HasYawAxis                                         = 256,         //Based on GIMBAL_DEVICE_CAP_FLAGS_HAS_YAW_AXIS.
        HasYawFollow                                       = 512,         //Based on GIMBAL_DEVICE_CAP_FLAGS_HAS_YAW_FOLLOW.
        HasYawLock                                         = 1024,        //Based on GIMBAL_DEVICE_CAP_FLAGS_HAS_YAW_LOCK.
        SupportsInfiniteYaw                                = 2048,        //Based on GIMBAL_DEVICE_CAP_FLAGS_SUPPORTS_INFINITE_YAW.
        CanPointLocationLocal                              = 65536,       //Gimbal manager supports to point to a local position.
        CanPointLocationGlobal                             = 131072       //Gimbal manager supports to point to a global latitude, longitude, altitude position.
    }

}
