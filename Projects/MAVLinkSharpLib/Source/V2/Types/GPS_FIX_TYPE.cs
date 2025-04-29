        
namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// Type of GPS fix
    /// </summary>    
    public enum GpsFixTypeFlags {
        NoGps                  = 0,           //No GPS connected
        NoFix                  = 1,           //No position information, GPS is connected
        _2dFix                 = 2,           //2D position
        _3dFix                 = 3,           //3D position
        Dgps                   = 4,           //DGPS/SBAS aided 3D position
        RtkFloat               = 5,           //RTK float, 3D position
        RtkFixed               = 6,           //RTK Fixed, 3D position
        Static                 = 7,           //Static fixed, typically used for base stations
        Ppp                    = 8            //PPP, 3D position.
    }

}
