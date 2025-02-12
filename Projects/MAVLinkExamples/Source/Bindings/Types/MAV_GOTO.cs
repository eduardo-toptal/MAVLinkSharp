        
namespace MAVLinkBindings {

    /// <summary>
    /// Actions that may be specified in MAV_CMD_OVERRIDE_GOTO to override mission execution.
    /// </summary>    
    public enum MAVGotoFlags {
        DoHold                              = 0,           //Hold at the current position.
        DoContinue                          = 1,           //Continue with the next item in mission execution.
        HoldAtCurrentPosition               = 2,           //Hold at the current position of the system
        HoldAtSpecifiedPosition             = 3            //Hold at the position specified in the parameters of the DO_HOLD action
    }

}
