        
namespace MAVLinkBindings {

    /// <summary>
    /// These flags are used to diagnose the failure state of CELLULAR_STATUS
    /// </summary>    
    public enum CellularNetworkFailedReasonFlags {
        None                                       = 0,           //No error
        Unknown                                    = 1,           //Error state is unknown
        SimMissing                                 = 2,           //SIM is required for the modem but missing
        SimError                                   = 3            //SIM is available, but not usable for connection
    }

}
