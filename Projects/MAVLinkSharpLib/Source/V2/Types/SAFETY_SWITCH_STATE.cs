        
namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// 
    /// 	Possible safety switch states.
    ///       
    /// </summary>    
    public enum SafetySwitchStateFlags {
        Safe                          = 0,           //Safety switch is engaged and vehicle should be safe to approach.
        Dangerous                     = 1            //Safety switch is NOT engaged and motors, propellers and other actuators should be considered active.
    }

}
