        
namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// Gimbal device (low level) error flags (bitmap, 0 means no error)
    /// </summary>    
    public enum GimbalDeviceErrorFlags {
        AtRollLimit                                   = 1,           //Gimbal device is limited by hardware roll limit.
        AtPitchLimit                                  = 2,           //Gimbal device is limited by hardware pitch limit.
        AtYawLimit                                    = 4,           //Gimbal device is limited by hardware yaw limit.
        EncoderError                                  = 8,           //There is an error with the gimbal encoders.
        PowerError                                    = 16,          //There is an error with the gimbal power source.
        MotorError                                    = 32,          //There is an error with the gimbal motors.
        SoftwareError                                 = 64,          //There is an error with the gimbal's software.
        CommsError                                    = 128,         //There is an error with the gimbal's communication.
        CalibrationRunning                            = 256,         //Gimbal device is currently calibrating.
        NoManager                                     = 512          //Gimbal device is not assigned to a gimbal manager.
    }

}
