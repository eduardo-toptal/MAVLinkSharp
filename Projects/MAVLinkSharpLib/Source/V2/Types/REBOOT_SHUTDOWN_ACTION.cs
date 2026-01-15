        
namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// Reboot/shutdown action for selected component in MAV_CMD_PREFLIGHT_REBOOT_SHUTDOWN.
    /// </summary>    
    public enum RebootShutdownActionFlags {
        None                                        = 0,           //Do nothing.
        Reboot                                      = 1,           //Reboot component.
        Shutdown                                    = 2,           //Shutdown component.
        RebootToBootloader                          = 3,           //Reboot component and keep it in the bootloader until upgraded.
        PowerOn                                     = 4            //Power on component. Do nothing if component is already powered (ACK command with MAV_RESULT_ACCEPTED).
    }

}
