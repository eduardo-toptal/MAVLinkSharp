        
namespace MAVLinkBindings {

    /// <summary>
    /// Source of information about this collision.
    /// </summary>    
    public enum MAVCollisionSrcFlags {
        Adsb                                     = 0,           //ID field references ADSB_VEHICLE packets
        MavlinkGpsGlobalInt                      = 1            //ID field references MAVLink SRC ID
    }

}
