        
namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// Flags for gimbal device (lower level) operation.
    /// </summary>    
    public enum GimbalDeviceFlags {
        Retract                        = 1,           //Set to retracted safe position (no stabilization), takes presedence over all other flags.
        Neutral                        = 2,           //Set to neutral/default position, taking precedence over all other flags except RETRACT. Neutral is commonly forward-facing and horizontal (pitch=yaw=0) but may be any orientation.
        RollLock                       = 4,           //Lock roll angle to absolute angle relative to horizon (not relative to drone). This is generally the default with a stabilizing gimbal.
        PitchLock                      = 8,           //Lock pitch angle to absolute angle relative to horizon (not relative to drone). This is generally the default.
        YawLock                        = 16           //Lock yaw angle to absolute angle relative to North (not relative to drone). If this flag is set, the quaternion is in the Earth frame with the x-axis pointing North (yaw absolute). If this flag is not set, the quaternion frame is in the Earth frame rotated so that the x-axis is pointing forward (yaw relative to vehicle).
    }

}
