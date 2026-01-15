        
namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// Illuminator module error flags (bitmap, 0 means no error)
    /// </summary>    
    public enum IlluminatorErrorFlags {
        ThermalThrottling                                 = 1,           //Illuminator thermal throttling error.
        OverTemperatureShutdown                           = 2,           //Illuminator over temperature shutdown error.
        ThermistorFailure                                 = 4            //Illuminator thermistor failure.
    }

}
