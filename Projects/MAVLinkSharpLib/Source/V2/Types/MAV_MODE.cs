        
namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// Predefined OR-combined MAV_MODE_FLAG values. These can simplify using the flags when setting modes. Note that manual input is enabled in all modes as a safety override.
    /// </summary>    
    public enum MAVModeFlags {
        Preflight                   = 0,           //System is not ready to fly, booting, calibrating, etc. No flag is set.
        ManualDisarmed              = 64,          //System is allowed to be active, under manual (RC) control, no stabilization (MAV_MODE_FLAG_MANUAL_INPUT_ENABLED)
        TestDisarmed                = 66,          //UNDEFINED mode. This solely depends on the autopilot - use with caution, intended for developers only. (MAV_MODE_FLAG_MANUAL_INPUT_ENABLED, MAV_MODE_FLAG_TEST_ENABLED).
        StabilizeDisarmed           = 80,          //System is allowed to be active, under assisted RC control (MAV_MODE_FLAG_SAFETY_ARMED, MAV_MODE_FLAG_STABILIZE_ENABLED)
        GuidedDisarmed              = 88,          //System is allowed to be active, under autonomous control, manual setpoint (MAV_MODE_FLAG_SAFETY_ARMED, MAV_MODE_FLAG_STABILIZE_ENABLED, MAV_MODE_FLAG_GUIDED_ENABLED)
        AutoDisarmed                = 92,          //System is allowed to be active, under autonomous control and navigation (the trajectory is decided onboard and not pre-programmed by waypoints). (MAV_MODE_FLAG_SAFETY_ARMED, MAV_MODE_FLAG_STABILIZE_ENABLED, MAV_MODE_FLAG_GUIDED_ENABLED, MAV_MODE_FLAG_AUTO_ENABLED).
        ManualArmed                 = 192,         //System is allowed to be active, under manual (RC) control, no stabilization (MAV_MODE_FLAG_SAFETY_ARMED, MAV_MODE_FLAG_MANUAL_INPUT_ENABLED)
        TestArmed                   = 194,         //UNDEFINED mode. This solely depends on the autopilot - use with caution, intended for developers only (MAV_MODE_FLAG_SAFETY_ARMED, MAV_MODE_FLAG_MANUAL_INPUT_ENABLED, MAV_MODE_FLAG_TEST_ENABLED)
        StabilizeArmed              = 208,         //System is allowed to be active, under assisted RC control (MAV_MODE_FLAG_SAFETY_ARMED, MAV_MODE_FLAG_MANUAL_INPUT_ENABLED, MAV_MODE_FLAG_STABILIZE_ENABLED)
        GuidedArmed                 = 216,         //System is allowed to be active, under autonomous control, manual setpoint (MAV_MODE_FLAG_SAFETY_ARMED, MAV_MODE_FLAG_MANUAL_INPUT_ENABLED, MAV_MODE_FLAG_STABILIZE_ENABLED, MAV_MODE_FLAG_GUIDED_ENABLED)
        AutoArmed                   = 220          //System is allowed to be active, under autonomous control and navigation (the trajectory is decided onboard and not pre-programmed by waypoints). (MAV_MODE_FLAG_SAFETY_ARMED, MAV_MODE_FLAG_MANUAL_INPUT_ENABLED, MAV_MODE_FLAG_STABILIZE_ENABLED, MAV_MODE_FLAG_GUIDED_ENABLED,MAV_MODE_FLAG_AUTO_ENABLED).
    }

}
