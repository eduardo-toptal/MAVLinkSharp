        
namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// Navigational status of AIS vessel, enum duplicated from AIS standard, https://gpsd.gitlab.io/gpsd/AIVDM.html
    /// </summary>    
    public enum AisNavStatusFlags {
        UnderWay                                   = 0,           //Under way using engine.
        Anchored                                   = 1,           //
        UnCommanded                                = 2,           //
        RestrictedManoeuverability                 = 3,           //
        DraughtConstrained                         = 4,           //
        Moored                                     = 5,           //
        Aground                                    = 6,           //
        Fishing                                    = 7,           //
        Sailing                                    = 8,           //
        ReservedHsc                                = 9,           //
        ReservedWig                                = 10,          //
        Reserved1                                  = 11,          //
        Reserved2                                  = 12,          //
        Reserved3                                  = 13,          //
        AisSart                                    = 14,          //Search And Rescue Transponder.
        Unknown                                    = 15           //Not available (default).
    }

}
