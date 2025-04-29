        
namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// These defines are predefined OR-combined mode flags. There is no need to use values from this enum, but it
    ///                simplifies the use of the mode flags. Note that manual input is enabled in all modes as a safety override.
    /// </summary>    
    public enum MAVModeFlags {
        Preflight                   = 0,           //System is not ready to fly, booting, calibrating, etc. No flag is set.
        ManualDisarmed              = 64,          //System is allowed to be active, under manual (RC) control, no stabilization
        TestDisarmed                = 66,          //UNDEFINED mode. This solely depends on the autopilot - use with caution, intended for developers only.
        StabilizeDisarmed           = 80,          //System is allowed to be active, under assisted RC control.
        GuidedDisarmed              = 88,          //System is allowed to be active, under autonomous control, manual setpoint
        AutoDisarmed                = 92,          //System is allowed to be active, under autonomous control and navigation (the trajectory is decided onboard and not pre-programmed by waypoints)
        ManualArmed                 = 192,         //System is allowed to be active, under manual (RC) control, no stabilization
        TestArmed                   = 194,         //UNDEFINED mode. This solely depends on the autopilot - use with caution, intended for developers only.
        StabilizeArmed              = 208,         //System is allowed to be active, under assisted RC control.
        GuidedArmed                 = 216,         //System is allowed to be active, under autonomous control, manual setpoint
        AutoArmed                   = 220          //System is allowed to be active, under autonomous control and navigation (the trajectory is decided onboard and not pre-programmed by waypoints)
    }

}
