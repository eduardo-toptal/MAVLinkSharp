        
namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// Enumeration of battery types
    /// </summary>    
    public enum MAVBatteryTypeFlags {
        Unknown                  = 0,           //Not specified.
        Lipo                     = 1,           //Lithium polymer battery
        Life                     = 2,           //Lithium-iron-phosphate battery
        Lion                     = 3,           //Lithium-ION battery
        Nimh                     = 4            //Nickel metal hydride battery
    }

}
