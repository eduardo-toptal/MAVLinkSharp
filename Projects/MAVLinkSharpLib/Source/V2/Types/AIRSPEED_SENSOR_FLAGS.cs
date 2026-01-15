        
namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// Airspeed sensor flags
    /// </summary>    
    public enum AirspeedSensorFlags {
        AirspeedSensorUnhealthy   = 1,           //Airspeed sensor is unhealthy
        AirspeedSensorUsing       = 2            //True if the data from this sensor is being actively used by the flight controller for guidance, navigation or control.
    }

}
