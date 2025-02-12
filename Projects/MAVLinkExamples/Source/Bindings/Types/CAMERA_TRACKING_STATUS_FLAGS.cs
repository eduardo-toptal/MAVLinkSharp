        
namespace MAVLinkBindings {

    /// <summary>
    /// Camera tracking status flags
    /// </summary>    
    public enum CameraTrackingStatusFlags {
        Idle                                = 0,           //Camera is not tracking
        Active                              = 1,           //Camera is tracking
        Error                               = 2            //Camera tracking in error state
    }

}
