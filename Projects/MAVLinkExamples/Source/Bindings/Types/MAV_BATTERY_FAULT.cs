        
namespace MAVLinkBindings {

    /// <summary>
    /// Smart battery supply status/fault flags (bitmask) for health indication. The battery must also report either MAV_BATTERY_CHARGE_STATE_FAILED or MAV_BATTERY_CHARGE_STATE_UNHEALTHY if any of these are set.
    /// </summary>    
    public enum MAVBatteryFaultFlags {
        DeepDischarge                                  = 1,           //Battery has deep discharged.
        Spikes                                         = 2,           //Voltage spikes.
        CellFail                                       = 4,           //One or more cells have failed. Battery should also report MAV_BATTERY_CHARGE_STATE_FAILE (and should not be used).
        OverCurrent                                    = 8,           //Over-current fault.
        OverTemperature                                = 16,          //Over-temperature fault.
        UnderTemperature                               = 32,          //Under-temperature fault.
        IncompatibleVoltage                            = 64,          //Vehicle voltage is not compatible with this battery (batteries on same power rail should have similar voltage).
        IncompatibleFirmware                           = 128,         //Battery firmware is not compatible with current autopilot firmware.
        BatteryFaultIncompatibleCellsConfiguration     = 256          //Battery is not compatible due to cell configuration (e.g. 5s1p when vehicle requires 6s).
    }

}
