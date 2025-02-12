        
namespace MAVLinkBindings {

    /// <summary>
    /// Yaw behaviour during orbit flight.
    /// </summary>    
    public enum OrbitYawBehaviourFlags {
        HoldFrontToCircleCenter                          = 0,           //Vehicle front points to the center (default).
        HoldInitialHeading                               = 1,           //Vehicle front holds heading when message received.
        Uncontrolled                                     = 2,           //Yaw uncontrolled.
        HoldFrontTangentToCircle                         = 3,           //Vehicle front follows flight path (tangential to circle).
        RcControlled                                     = 4            //Yaw controlled by RC input.
    }

}
