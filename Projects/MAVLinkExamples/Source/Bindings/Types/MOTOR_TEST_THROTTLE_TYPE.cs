        
namespace MAVLinkBindings {

    /// <summary>
    /// Defines how throttle value is represented in MAV_CMD_DO_MOTOR_TEST.
    /// </summary>    
    public enum MotorTestThrottleTypeFlags {
        MotorTestThrottlePercent    = 0,           //Throttle as a percentage (0 ~ 100)
        MotorTestThrottlePwm        = 1,           //Throttle as an absolute PWM value (normally in range of 1000~2000).
        MotorTestThrottlePilot      = 2,           //Throttle pass-through from pilot's transmitter.
        MotorTestCompassCal         = 3            //Per-motor compass calibration test.
    }

}
