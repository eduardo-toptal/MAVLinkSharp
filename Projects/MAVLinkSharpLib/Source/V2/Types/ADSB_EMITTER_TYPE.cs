        
namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// ADSB classification for the type of vehicle emitting the transponder signal
    /// </summary>    
    public enum AdsbEmitterTypeFlags {
        NoInfo                              = 0,           //
        Light                               = 1,           //
        Small                               = 2,           //
        Large                               = 3,           //
        HighVortexLarge                     = 4,           //
        Heavy                               = 5,           //
        HighlyManuv                         = 6,           //
        Rotocraft                           = 7,           //
        Unassigned                          = 8,           //
        Glider                              = 9,           //
        LighterAir                          = 10,          //
        Parachute                           = 11,          //
        UltraLight                          = 12,          //
        Unassigned2                         = 13,          //
        Uav                                 = 14,          //
        Space                               = 15,          //
        Unassgined3                         = 16,          //
        EmergencySurface                    = 17,          //
        ServiceSurface                      = 18,          //
        PointObstacle                       = 19           //
    }

}
