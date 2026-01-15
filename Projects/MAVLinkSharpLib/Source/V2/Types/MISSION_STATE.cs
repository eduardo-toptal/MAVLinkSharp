        
namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// 
    ///         States of the mission state machine.
    ///         Note that these states are independent of whether the mission is in a mode that can execute mission items or not (is suspended).
    ///         They may not all be relevant on all vehicles.
    ///       
    /// </summary>    
    public enum MissionStateFlags {
        Unknown                   = 0,           //The mission status reporting is not supported.
        NoMission                 = 1,           //No mission on the vehicle.
        NotStarted                = 2,           //Mission has not started. This is the case after a mission has uploaded but not yet started executing.
        Active                    = 3,           //Mission is active, and will execute mission items when in auto mode.
        Paused                    = 4,           //Mission is paused when in auto mode.
        Complete                  = 5            //Mission has executed all mission items.
    }

}
