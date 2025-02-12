        
namespace MAVLinkBindings {

    /// <summary>
    /// 
    /// </summary>    
    public enum MAVOdidSpeedAccFlags {
        Unknown                                  = 0,           //The speed accuracy is unknown.
        _10MetersPerSecond                       = 1,           //The speed accuracy is smaller than 10 meters per second.
        _3MetersPerSecond                        = 2,           //The speed accuracy is smaller than 3 meters per second.
        _1MetersPerSecond                        = 3,           //The speed accuracy is smaller than 1 meters per second.
        _03MetersPerSecond                       = 4            //The speed accuracy is smaller than 0.3 meters per second.
    }

}
