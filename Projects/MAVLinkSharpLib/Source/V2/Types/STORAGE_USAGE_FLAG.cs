        
namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// Flags to indicate usage for a particular storage (see STORAGE_INFORMATION.storage_usage and MAV_CMD_SET_STORAGE_USAGE).
    /// </summary>    
    public enum StorageUsageFlag {
        Set                      = 1,           //Always set to 1 (indicates STORAGE_INFORMATION.storage_usage is supported).
        Photo                    = 2,           //Storage for saving photos.
        Video                    = 4,           //Storage for saving videos.
        Logs                     = 8            //Storage for saving logs.
    }

}
