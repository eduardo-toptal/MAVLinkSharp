        
namespace MAVLinkBindings {

    /// <summary>
    /// Winch actions.
    /// </summary>    
    public enum WinchActionsFlags {
        WinchRelaxed                  = 0,           //Allow motor to freewheel.
        WinchRelativeLengthControl    = 1,           //Wind or unwind specified length of line, optionally using specified rate.
        WinchRateControl              = 2,           //Wind or unwind line at specified rate.
        WinchLock                     = 3,           //Perform the locking sequence to relieve motor while in the fully retracted position. Only action and instance command parameters are used, others are ignored.
        WinchDeliver                  = 4,           //Sequence of drop, slow down, touch down, reel up, lock. Only action and instance command parameters are used, others are ignored.
        WinchHold                     = 5,           //Engage motor and hold current position. Only action and instance command parameters are used, others are ignored.
        WinchRetract                  = 6,           //Return the reel to the fully retracted position. Only action and instance command parameters are used, others are ignored.
        WinchLoadLine                 = 7,           //Load the reel with line. The winch will calculate the total loaded length and stop when the tension exceeds a threshold. Only action and instance command parameters are used, others are ignored.
        WinchAbandonLine              = 8            //Spool out the entire length of the line. Only action and instance command parameters are used, others are ignored.
    }

}
