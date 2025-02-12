        
namespace MAVLinkBindings {

    /// <summary>
    /// 
    /// </summary>    
    public enum MAVOdidVerAccFlags {
        Unknown                    = 0,           //The vertical accuracy is unknown.
        _150Meter                  = 1,           //The vertical accuracy is smaller than 150 meter.
        _45Meter                   = 2,           //The vertical accuracy is smaller than 45 meter.
        _25Meter                   = 3,           //The vertical accuracy is smaller than 25 meter.
        _10Meter                   = 4,           //The vertical accuracy is smaller than 10 meter.
        _3Meter                    = 5,           //The vertical accuracy is smaller than 3 meter.
        _1Meter                    = 6            //The vertical accuracy is smaller than 1 meter.
    }

}
