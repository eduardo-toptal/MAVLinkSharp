        
namespace MAVLinkBindings {

    /// <summary>
    /// Flags to indicate the status of camera storage.
    /// </summary>    
    public enum StorageStatusFlags {
        Empty                        = 0,           //Storage is missing (no microSD card loaded for example.)
        Unformatted                  = 1,           //Storage present but unformatted.
        Ready                        = 2,           //Storage present and ready.
        NotSupported                 = 3            //Camera does not supply storage status information. Capacity information in STORAGE_INFORMATION fields will be ignored.
    }

}
