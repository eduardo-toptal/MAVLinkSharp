        
namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// Flags for high level gimbal manager operation The first 16 bits are identical to the GIMBAL_DEVICE_FLAGS.
    /// </summary>    
    public enum GimbalManagerFlags {
        Retract                                         = 1,           //Based on GIMBAL_DEVICE_FLAGS_RETRACT.
        Neutral                                         = 2,           //Based on GIMBAL_DEVICE_FLAGS_NEUTRAL.
        RollLock                                        = 4,           //Based on GIMBAL_DEVICE_FLAGS_ROLL_LOCK.
        PitchLock                                       = 8,           //Based on GIMBAL_DEVICE_FLAGS_PITCH_LOCK.
        YawLock                                         = 16,          //Based on GIMBAL_DEVICE_FLAGS_YAW_LOCK.
        YawInVehicleFrame                               = 32,          //Based on GIMBAL_DEVICE_FLAGS_YAW_IN_VEHICLE_FRAME.
        YawInEarthFrame                                 = 64,          //Based on GIMBAL_DEVICE_FLAGS_YAW_IN_EARTH_FRAME.
        AcceptsYawInEarthFrame                          = 128,         //Based on GIMBAL_DEVICE_FLAGS_ACCEPTS_YAW_IN_EARTH_FRAME.
        RcExclusive                                     = 256,         //Based on GIMBAL_DEVICE_FLAGS_RC_EXCLUSIVE.
        RcMixed                                         = 512          //Based on GIMBAL_DEVICE_FLAGS_RC_MIXED.
    }

}
