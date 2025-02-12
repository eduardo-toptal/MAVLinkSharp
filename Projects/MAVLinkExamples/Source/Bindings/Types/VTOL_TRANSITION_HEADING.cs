        
namespace MAVLinkBindings {

    /// <summary>
    /// Direction of VTOL transition
    /// </summary>    
    public enum VtolTransitionHeadingFlags {
        VehicleDefault                          = 0,           //Respect the heading configuration of the vehicle.
        NextWaypoint                            = 1,           //Use the heading pointing towards the next waypoint.
        Takeoff                                 = 2,           //Use the heading on takeoff (while sitting on the ground).
        Specified                               = 3,           //Use the specified heading in parameter 4.
        Any                                     = 4            //Use the current heading when reaching takeoff altitude (potentially facing the wind when weather-vaning is active).
    }

}
