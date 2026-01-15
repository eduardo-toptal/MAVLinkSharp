        
namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// Speed setpoint types used in MAV_CMD_DO_CHANGE_SPEED
    /// </summary>    
    public enum SpeedTypeFlags {
        Airspeed                 = 0,           //Airspeed
        Groundspeed              = 1,           //Groundspeed
        ClimbSpeed               = 2,           //Climb speed
        DescentSpeed             = 3            //Descent speed
    }

}
