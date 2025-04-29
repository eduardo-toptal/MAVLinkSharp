        
namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// Navigational status of AIS vessel, enum duplicated from AIS standard, https://gpsd.gitlab.io/gpsd/AIVDM.html
    /// </summary>    
    public enum AisNavStatusFlags {
        UnderWay                            = 0,           //Under way using engine.
        AisNavAnchored                      = 1,           //
        AisNavUnCommanded                   = 2,           //
        AisNavRestrictedManoeuverability    = 3,           //
        AisNavDraughtConstrained            = 4,           //
        AisNavMoored                        = 5,           //
        AisNavAground                       = 6,           //
        AisNavFishing                       = 7,           //
        AisNavSailing                       = 8,           //
        AisNavReservedHsc                   = 9,           //
        AisNavReservedWig                   = 10,          //
        AisNavReserved1                     = 11,          //
        AisNavReserved2                     = 12,          //
        AisNavReserved3                     = 13,          //
        AisNavAisSart                       = 14,          //Search And Rescue Transponder.
        AisNavUnknown                       = 15           //Not available (default).
    }

}
