        
namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// Generalized UAVCAN node mode
    /// </summary>    
    public enum UavcanNodeModeFlags {
        Operational                      = 0,           //The node is performing its primary functions.
        Initialization                   = 1,           //The node is initializing; this mode is entered immediately after startup.
        Maintenance                      = 2,           //The node is under maintenance.
        SoftwareUpdate                   = 3,           //The node is in the process of updating its software.
        Offline                          = 7            //The node is no longer available online.
    }

}
