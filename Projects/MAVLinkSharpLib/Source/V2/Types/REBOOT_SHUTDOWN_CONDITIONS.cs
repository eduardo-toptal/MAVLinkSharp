        
namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// Specifies the conditions under which the MAV_CMD_PREFLIGHT_REBOOT_SHUTDOWN command should be accepted.
    /// </summary>    
    public enum RebootShutdownConditionsFlags {
        SafetyInterlocked                             = 0,           //Reboot/Shutdown only if allowed by safety checks, such as being landed.
        Force                                         = 20190226     //Force reboot/shutdown of the autopilot/component regardless of system state.
    }

}
