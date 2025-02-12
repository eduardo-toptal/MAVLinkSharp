        
namespace MAVLinkBindings {

    /// <summary>
    /// These flags encode the cellular network status
    /// </summary>    
    public enum CellularStatusFlag {
        Unknown                            = 0,           //State unknown or not reportable.
        Failed                             = 1,           //Modem is unusable
        Initializing                       = 2,           //Modem is being initialized
        Locked                             = 3,           //Modem is locked
        Disabled                           = 4,           //Modem is not enabled and is powered down
        Disabling                          = 5,           //Modem is currently transitioning to the CELLULAR_STATUS_FLAG_DISABLED state
        Enabling                           = 6,           //Modem is currently transitioning to the CELLULAR_STATUS_FLAG_ENABLED state
        Enabled                            = 7,           //Modem is enabled and powered on but not registered with a network provider and not available for data connections
        Searching                          = 8,           //Modem is searching for a network provider to register
        Registered                         = 9,           //Modem is registered with a network provider, and data connections and messaging may be available for use
        Disconnecting                      = 10,          //Modem is disconnecting and deactivating the last active packet data bearer. This state will not be entered if more than one packet data bearer is active and one of the active bearers is deactivated
        Connecting                         = 11,          //Modem is activating and connecting the first packet data bearer. Subsequent bearer activations when another bearer is already active do not cause this state to be entered
        Connected                          = 12           //One or more packet data bearers is active and connected
    }

}
