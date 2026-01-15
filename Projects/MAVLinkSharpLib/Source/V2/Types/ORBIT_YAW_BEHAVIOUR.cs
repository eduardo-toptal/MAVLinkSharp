        
namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// Yaw behaviour during orbit flight.
    /// </summary>    
    public enum OrbitYawBehaviourFlags {
        HoldFrontToCircleCenter                          = 0,           //Vehicle front points to the center (default).
        HoldInitialHeading                               = 1,           //Vehicle front holds heading when message received.
        Uncontrolled                                     = 2,           //Yaw uncontrolled.
        HoldFrontTangentToCircle                         = 3,           //Vehicle front follows flight path (tangential to circle).
        RcControlled                                     = 4,           //Yaw controlled by RC input.
        Unchanged                                        = 5            //Vehicle uses current yaw behaviour (unchanged). The vehicle-default yaw behaviour is used if this value is specified when orbit is first commanded.
    }

}
