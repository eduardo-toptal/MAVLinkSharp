        
namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// Enumeration of landed detector states
    /// </summary>    
    public enum MAVLandedStateFlags {
        Undefined                  = 0,           //MAV landed state is unknown
        OnGround                   = 1,           //MAV is landed (on ground)
        InAir                      = 2,           //MAV is in air
        Takeoff                    = 3,           //MAV currently taking off
        Landing                    = 4            //MAV currently landing
    }

}
