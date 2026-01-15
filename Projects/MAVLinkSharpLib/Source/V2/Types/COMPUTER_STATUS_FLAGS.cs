        
namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// Flags used to report computer status.
    /// </summary>    
    public enum ComputerStatusFlags {
        UnderVoltage                           = 1,           //Indicates if the system is experiencing voltage outside of acceptable range.
        CpuThrottle                            = 2,           //Indicates if CPU throttling is active.
        ThermalThrottle                        = 4,           //Indicates if thermal throttling is active.
        DiskFull                               = 8            //Indicates if main disk is full.
    }

}
