        
namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// Camera tracking status flags
    /// </summary>    
    public enum CameraTrackingStatusFlags {
        Idle                                  = 0,           //Camera is not tracking
        Active                                = 1,           //Camera is tracking
        Error                                 = 2,           //Camera tracking in error state
        Mti                                   = 4,           //Camera Moving Target Indicators (MTI) are active
        Coasting                              = 8            //Camera tracking target is obscured and is being predicted
    }

}
