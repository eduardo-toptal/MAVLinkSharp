        
namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// Possible responses from a CELLULAR_CONFIG message.
    /// </summary>    
    public enum CellularConfigResponseFlags {
        Accepted                             = 0,           //Changes accepted.
        ApnError                             = 1,           //Invalid APN.
        PinError                             = 2,           //Invalid PIN.
        Rejected                             = 3,           //Changes rejected.
        CellularConfigBlockedPukRequired     = 4            //PUK is required to unblock SIM card.
    }

}
