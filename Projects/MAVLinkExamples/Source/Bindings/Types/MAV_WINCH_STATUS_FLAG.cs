        
namespace MAVLinkBindings {

    /// <summary>
    /// Winch status flags used in WINCH_STATUS
    /// </summary>    
    public enum MAVWinchStatusFlag {
        MavWinchStatusHealthy            = 1,           //Winch is healthy
        MavWinchStatusFullyRetracted     = 2,           //Winch line is fully retracted
        MavWinchStatusMoving             = 4,           //Winch motor is moving
        MavWinchStatusClutchEngaged      = 8,           //Winch clutch is engaged allowing motor to move freely.
        MavWinchStatusLocked             = 16,          //Winch is locked by locking mechanism.
        MavWinchStatusDropping           = 32,          //Winch is gravity dropping payload.
        MavWinchStatusArresting          = 64,          //Winch is arresting payload descent.
        MavWinchStatusGroundSense        = 128,         //Winch is using torque measurements to sense the ground.
        MavWinchStatusRetracting         = 256,         //Winch is returning to the fully retracted position.
        MavWinchStatusRedeliver          = 512,         //Winch is redelivering the payload. This is a failover state if the line tension goes above a threshold during RETRACTING.
        MavWinchStatusAbandonLine        = 1024         //Winch is abandoning the line and possibly payload. Winch unspools the entire calculated line length. This is a failover state from REDELIVER if the number of attempts exceeds a threshold.
    }

}
