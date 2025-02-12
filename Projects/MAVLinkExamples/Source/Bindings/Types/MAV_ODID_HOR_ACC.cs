        
namespace MAVLinkBindings {

    /// <summary>
    /// 
    /// </summary>    
    public enum MAVOdidHorAccFlags {
        Unknown                   = 0,           //The horizontal accuracy is unknown.
        _10nm                     = 1,           //The horizontal accuracy is smaller than 10 Nautical Miles. 18.52 km.
        _4nm                      = 2,           //The horizontal accuracy is smaller than 4 Nautical Miles. 7.408 km.
        _2nm                      = 3,           //The horizontal accuracy is smaller than 2 Nautical Miles. 3.704 km.
        _1nm                      = 4,           //The horizontal accuracy is smaller than 1 Nautical Miles. 1.852 km.
        _05nm                     = 5,           //The horizontal accuracy is smaller than 0.5 Nautical Miles. 926 m.
        _03nm                     = 6,           //The horizontal accuracy is smaller than 0.3 Nautical Miles. 555.6 m.
        _01nm                     = 7,           //The horizontal accuracy is smaller than 0.1 Nautical Miles. 185.2 m.
        _005nm                    = 8,           //The horizontal accuracy is smaller than 0.05 Nautical Miles. 92.6 m.
        _30Meter                  = 9,           //The horizontal accuracy is smaller than 30 meter.
        _10Meter                  = 10,          //The horizontal accuracy is smaller than 10 meter.
        _3Meter                   = 11,          //The horizontal accuracy is smaller than 3 meter.
        _1Meter                   = 12           //The horizontal accuracy is smaller than 1 meter.
    }

}
