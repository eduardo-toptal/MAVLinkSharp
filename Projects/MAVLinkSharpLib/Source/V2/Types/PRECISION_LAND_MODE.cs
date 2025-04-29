        
namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// Precision land modes (used in MAV_CMD_NAV_LAND).
    /// </summary>    
    public enum PrecisionLandModeFlags {
        Disabled                          = 0,           //Normal (non-precision) landing.
        Opportunistic                     = 1,           //Use precision landing if beacon detected when land command accepted, otherwise land normally.
        Required                          = 2            //Use precision landing, searching for beacon if not found when land command accepted (land normally if beacon cannot be found).
    }

}
