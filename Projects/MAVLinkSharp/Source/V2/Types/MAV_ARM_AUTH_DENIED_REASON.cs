        
namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// 
    /// </summary>    
    public enum MAVArmAuthDeniedReasonFlags {
        Generic                                     = 0,           //Not a specific reason
        None                                        = 1,           //Authorizer will send the error as string to GCS
        InvalidWaypoint                             = 2,           //At least one waypoint have a invalid value
        Timeout                                     = 3,           //Timeout in the authorizer process(in case it depends on network)
        AirspaceInUse                               = 4,           //Airspace of the mission in use by another vehicle, second result parameter can have the waypoint id that caused it to be denied.
        BadWeather                                  = 5            //Weather is not good to fly
    }

}
