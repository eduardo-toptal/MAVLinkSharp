        
namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// 
    /// </summary>    
    public enum MAVOdidUaTypeFlags {
        None                                       = 0,           //No UA (Unmanned Aircraft) type defined.
        Aeroplane                                  = 1,           //Aeroplane/Airplane. Fixed wing.
        HelicopterOrMultirotor                     = 2,           //Helicopter or multirotor.
        Gyroplane                                  = 3,           //Gyroplane.
        HybridLift                                 = 4,           //VTOL (Vertical Take-Off and Landing). Fixed wing aircraft that can take off vertically.
        Ornithopter                                = 5,           //Ornithopter.
        Glider                                     = 6,           //Glider.
        Kite                                       = 7,           //Kite.
        FreeBalloon                                = 8,           //Free Balloon.
        CaptiveBalloon                             = 9,           //Captive Balloon.
        Airship                                    = 10,          //Airship. E.g. a blimp.
        FreeFallParachute                          = 11,          //Free Fall/Parachute (unpowered).
        Rocket                                     = 12,          //Rocket.
        TetheredPoweredAircraft                    = 13,          //Tethered powered aircraft.
        GroundObstacle                             = 14,          //Ground Obstacle.
        Other                                      = 15           //Other type of aircraft not listed earlier.
    }

}
