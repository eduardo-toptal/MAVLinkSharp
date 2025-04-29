        
namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// 
    /// </summary>    
    public enum MAVOdidOperatorLocationTypeFlags {
        Takeoff                                   = 0,           //The location/altitude of the operator is the same as the take-off location.
        LiveGnss                                  = 1,           //The location/altitude of the operator is dynamic. E.g. based on live GNSS data.
        Fixed                                     = 2            //The location/altitude of the operator are fixed values.
    }

}
