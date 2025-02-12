        
namespace MAVLinkBindings {

    /// <summary>
    /// Sequence that motors are tested when using MAV_CMD_DO_MOTOR_TEST.
    /// </summary>    
    public enum MotorTestOrderFlags {
        Default                   = 0,           //Default autopilot motor test method.
        Sequence                  = 1,           //Motor numbers are specified as their index in a predefined vehicle-specific sequence.
        Board                     = 2            //Motor numbers are specified as the output as labeled on the board.
    }

}
