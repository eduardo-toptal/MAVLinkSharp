        
namespace MAVLinkBindings {

    /// <summary>
    /// Camera tracking target data (shows where tracked target is within image)
    /// </summary>    
    public enum CameraTrackingTargetDataFlags {
        None                                  = 0,           //No target data
        Embedded                              = 1,           //Target data embedded in image data (proprietary)
        Rendered                              = 2,           //Target data rendered in image
        InStatus                              = 4            //Target data within status message (Point or Rectangle)
    }

}
