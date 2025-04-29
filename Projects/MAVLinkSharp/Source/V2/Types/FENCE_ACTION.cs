        
namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// Actions following geofence breach.
    /// </summary>    
    public enum FenceActionFlags {
        None                         = 0,           //Disable fenced mode. If used in a plan this would mean the next fence is disabled.
        Guided                       = 1,           //Fly to geofence MAV_CMD_NAV_FENCE_RETURN_POINT in GUIDED mode. Note: This action is only supported by ArduPlane, and may not be supported in all versions.
        Report                       = 2,           //Report fence breach, but don't take action
        GuidedThrPass                = 3,           //Fly to geofence MAV_CMD_NAV_FENCE_RETURN_POINT with manual throttle control in GUIDED mode. Note: This action is only supported by ArduPlane, and may not be supported in all versions.
        Rtl                          = 4,           //Return/RTL mode.
        Hold                         = 5,           //Hold at current location.
        Terminate                    = 6,           //Termination failsafe. Motors are shut down (some flight stacks may trigger other failsafe actions).
        Land                         = 7            //Land at current location.
    }

}
