        
namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// Enumeration of distance sensor types
    /// </summary>    
    public enum MAVDistanceSensorFlags {
        Laser                          = 0,           //Laser rangefinder, e.g. LightWare SF02/F or PulsedLight units
        Ultrasound                     = 1,           //Ultrasound rangefinder, e.g. MaxBotix units
        Infrared                       = 2,           //Infrared rangefinder, e.g. Sharp units
        Radar                          = 3,           //Radar type, e.g. uLanding units
        Unknown                        = 4            //Broken or unknown type, e.g. analog units
    }

}
