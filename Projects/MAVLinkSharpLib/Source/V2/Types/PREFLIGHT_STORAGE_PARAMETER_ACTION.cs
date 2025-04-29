        
namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// 
    ///         Actions for reading/writing parameters between persistent and volatile storage when using MAV_CMD_PREFLIGHT_STORAGE.
    ///         (Commonly parameters are loaded from persistent storage (flash/EEPROM) into volatile storage (RAM) on startup and written back when they are changed.)
    ///       
    /// </summary>    
    public enum PreflightStorageParameterActionFlags {
        ParamReadPersistent        = 0,           //Read all parameters from persistent storage. Replaces values in volatile storage.
        ParamWritePersistent       = 1,           //Write all parameter values to persistent storage (flash/EEPROM)
        ParamResetConfigDefault    = 2,           //Reset all user configurable parameters to their default value (including airframe selection, sensor calibration data, safety settings, and so on). Does not reset values that contain operation counters and vehicle computed statistics.
        ParamResetSensorDefault    = 3,           //Reset only sensor calibration parameters to factory defaults (or firmware default if not available)
        ParamResetAllDefault       = 4            //Reset all parameters, including operation counters, to default values
    }

}
