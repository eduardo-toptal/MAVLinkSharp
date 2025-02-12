        
namespace MAVLinkBindings {

    /// <summary>
    /// 
    /// </summary>    
    public enum MAVStateFlags {
        Uninit                       = 0,           //Uninitialized system, state is unknown.
        Boot                         = 1,           //System is booting up.
        Calibrating                  = 2,           //System is calibrating and not flight-ready.
        Standby                      = 3,           //System is grounded and on standby. It can be launched any time.
        Active                       = 4,           //System is active and might be already airborne. Motors are engaged.
        Critical                     = 5,           //System is in a non-normal flight mode. It can however still navigate.
        Emergency                    = 6,           //System is in a non-normal flight mode. It lost control over parts or over the whole airframe. It is in mayday and going down.
        Poweroff                     = 7,           //System just initialized its power-down sequence, will shut down now.
        FlightTermination            = 8            //System is terminating itself.
    }

}
