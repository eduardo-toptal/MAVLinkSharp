        
namespace MAVLinkBindings {

    /// <summary>
    /// Parachute actions. Trigger release and enable/disable auto-release.
    /// </summary>    
    public enum ParachuteActionFlags {
        ParachuteDisable  = 0,           //Disable auto-release of parachute (i.e. release triggered by crash detectors).
        ParachuteEnable   = 1,           //Enable auto-release of parachute.
        ParachuteRelease  = 2            //Release parachute and kill motors.
    }

}
