        
namespace MAVLinkBindings {

    /// <summary>
    /// 
    ///         Actions for reading and writing plan information (mission, rally points, geofence) between persistent and volatile storage when using MAV_CMD_PREFLIGHT_STORAGE.
    ///         (Commonly missions are loaded from persistent storage (flash/EEPROM) into volatile storage (RAM) on startup and written back when they are changed.)
    ///       
    /// </summary>    
    public enum PreflightStorageMissionActionFlags {
        MissionReadPersistent    = 0,           //Read current mission data from persistent storage
        MissionWritePersistent   = 1,           //Write current mission data to persistent storage
        MissionResetDefault      = 2            //Erase all mission data stored on the vehicle (both persistent and volatile storage)
    }

}
