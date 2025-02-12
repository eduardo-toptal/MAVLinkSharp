        
namespace MAVLinkBindings {

    /// <summary>
    /// Battery mode. Note, the normal operation mode (i.e. when flying) should be reported as MAV_BATTERY_MODE_UNKNOWN to allow message trimming in normal flight.
    /// </summary>    
    public enum MAVBatteryModeFlags {
        Unknown                           = 0,           //Battery mode not supported/unknown battery mode/normal operation.
        AutoDischarging                   = 1,           //Battery is auto discharging (towards storage level).
        HotSwap                           = 2            //Battery in hot-swap mode (current limited to prevent spikes that might damage sensitive electrical circuits).
    }

}
