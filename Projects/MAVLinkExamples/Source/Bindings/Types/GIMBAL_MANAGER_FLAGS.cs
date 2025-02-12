        
namespace MAVLinkBindings {

    /// <summary>
    /// Flags for high level gimbal manager operation The first 16 bits are identical to the GIMBAL_DEVICE_FLAGS.
    /// </summary>    
    public enum GimbalManagerFlags {
        Retract                         = 1,           //Based on GIMBAL_DEVICE_FLAGS_RETRACT
        Neutral                         = 2,           //Based on GIMBAL_DEVICE_FLAGS_NEUTRAL
        RollLock                        = 4,           //Based on GIMBAL_DEVICE_FLAGS_ROLL_LOCK
        PitchLock                       = 8,           //Based on GIMBAL_DEVICE_FLAGS_PITCH_LOCK
        YawLock                         = 16           //Based on GIMBAL_DEVICE_FLAGS_YAW_LOCK
    }

}
