        
namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// Enumeration of battery functions
    /// </summary>    
    public enum MAVBatteryFunctionFlags {
        Unknown                         = 0,           //Battery function is unknown
        All                             = 1,           //Battery supports all flight systems
        Propulsion                      = 2,           //Battery for the propulsion system
        Avionics                        = 3,           //Avionics battery
        Payload                         = 4            //Payload battery
    }

}
