        
namespace MAVLinkBindings {

    /// <summary>
    /// Enumeration of VTOL states
    /// </summary>    
    public enum MAVVtolStateFlags {
        Undefined                       = 0,           //MAV is not configured as VTOL
        TransitionToFw                  = 1,           //VTOL is in transition from multicopter to fixed-wing
        TransitionToMc                  = 2,           //VTOL is in transition from fixed-wing to multicopter
        Mc                              = 3,           //VTOL is in multicopter state
        Fw                              = 4            //VTOL is in fixed-wing state
    }

}
