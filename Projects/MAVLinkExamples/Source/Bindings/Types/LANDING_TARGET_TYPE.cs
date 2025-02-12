        
namespace MAVLinkBindings {

    /// <summary>
    /// Type of landing target
    /// </summary>    
    public enum LandingTargetTypeFlags {
        LightBeacon                         = 0,           //Landing target signaled by light beacon (ex: IR-LOCK)
        RadioBeacon                         = 1,           //Landing target signaled by radio beacon (ex: ILS, NDB)
        VisionFiducial                      = 2,           //Landing target represented by a fiducial marker (ex: ARTag)
        VisionOther                         = 3            //Landing target represented by a pre-defined visual shape/feature (ex: X-marker, H-marker, square)
    }

}
