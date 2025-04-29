        
namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// 
    /// </summary>    
    public enum GpsInputIgnoreFlags {
        GpsInputIgnoreFlagAlt                     = 1,           //ignore altitude field
        GpsInputIgnoreFlagHdop                    = 2,           //ignore hdop field
        GpsInputIgnoreFlagVdop                    = 4,           //ignore vdop field
        GpsInputIgnoreFlagVelHoriz                = 8,           //ignore horizontal velocity field (vn and ve)
        GpsInputIgnoreFlagVelVert                 = 16,          //ignore vertical velocity field (vd)
        GpsInputIgnoreFlagSpeedAccuracy           = 32,          //ignore speed accuracy field
        GpsInputIgnoreFlagHorizontalAccuracy      = 64,          //ignore horizontal accuracy field
        GpsInputIgnoreFlagVerticalAccuracy        = 128          //ignore vertical accuracy field
    }

}
