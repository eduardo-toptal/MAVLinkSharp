        
namespace MAVLinkBindings {

    /// <summary>
    /// The ROI (region of interest) for the vehicle. This can be
    ///                 be used by the vehicle for camera/vehicle attitude alignment (see
    ///                 MAV_CMD_NAV_ROI).
    /// </summary>    
    public enum MAVRoiFlags {
        None             = 0,           //No region of interest.
        Wpnext           = 1,           //Point toward next waypoint, with optional pitch/roll/yaw offset.
        Wpindex          = 2,           //Point toward given waypoint.
        Location         = 3,           //Point toward fixed location.
        Target           = 4            //Point toward of given id.
    }

}
