        
namespace MAVLinkBindings {

    /// <summary>
    /// Airborne status of UAS.
    /// </summary>    
    public enum UtmFlightStateFlags {
        Unknown                    = 1,           //The flight state can't be determined.
        Ground                     = 2,           //UAS on ground.
        Airborne                   = 3,           //UAS airborne.
        Emergency                  = 16,          //UAS is in an emergency flight state.
        Noctrl                     = 32           //UAS has no active controls.
    }

}
