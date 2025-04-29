        
namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// Enumeration for battery charge states.
    /// </summary>    
    public enum MAVBatteryChargeStateFlags {
        Undefined                          = 0,           //Low battery state is not provided
        Ok                                 = 1,           //Battery is not in low state. Normal operation.
        Low                                = 2,           //Battery state is low, warn and monitor close.
        Critical                           = 3,           //Battery state is critical, return or abort immediately.
        Emergency                          = 4,           //Battery state is too low for ordinary abort sequence. Perform fastest possible emergency stop to prevent damage.
        Failed                             = 5,           //Battery failed, damage unavoidable. Possible causes (faults) are listed in MAV_BATTERY_FAULT.
        Unhealthy                          = 6,           //Battery is diagnosed to be defective or an error occurred, usage is discouraged / prohibited. Possible causes (faults) are listed in MAV_BATTERY_FAULT.
        Charging                           = 7            //Battery is charging.
    }

}
