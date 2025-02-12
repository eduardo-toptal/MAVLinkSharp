        
namespace MAVLinkBindings {

    /// <summary>
    /// Bitmap to indicate which dimensions should be ignored by the vehicle: a value of 0b00000000 indicates that none of the setpoint dimensions should be ignored.
    /// </summary>    
    public enum AttitudeTargetTypemaskFlags {
        BodyRollRateIgnore                              = 1,           //Ignore body roll rate
        BodyPitchRateIgnore                             = 2,           //Ignore body pitch rate
        BodyYawRateIgnore                               = 4,           //Ignore body yaw rate
        ThrustBodySet                                   = 32,          //Use 3D body thrust setpoint instead of throttle
        ThrottleIgnore                                  = 64,          //Ignore throttle
        AttitudeIgnore                                  = 128          //Ignore attitude
    }

}
