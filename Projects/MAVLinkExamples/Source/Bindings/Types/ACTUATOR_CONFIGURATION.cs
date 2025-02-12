        
namespace MAVLinkBindings {

    /// <summary>
    /// Actuator configuration, used to change a setting on an actuator. Component information metadata can be used to know which outputs support which commands.
    /// </summary>    
    public enum ActuatorConfigurationFlags {
        None                                   = 0,           //Do nothing.
        Beep                                   = 1,           //Command the actuator to beep now.
        _3dModeOn                              = 2,           //Permanently set the actuator (ESC) to 3D mode (reversible thrust).
        _3dModeOff                             = 3,           //Permanently set the actuator (ESC) to non 3D mode (non-reversible thrust).
        SpinDirection1                         = 4,           //Permanently set the actuator (ESC) to spin direction 1 (which can be clockwise or counter-clockwise).
        SpinDirection2                         = 5            //Permanently set the actuator (ESC) to spin direction 2 (opposite of direction 1).
    }

}
