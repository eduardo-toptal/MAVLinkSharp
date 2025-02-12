        
namespace MAVLinkBindings {

    /// <summary>
    /// Enumeration of the ADSB altimeter types
    /// </summary>    
    public enum AdsbAltitudeTypeFlags {
        PressureQnh                     = 0,           //Altitude reported from a Baro source using QNH reference
        Geometric                       = 1            //Altitude reported from a GNSS source
    }

}
