        
namespace MAVLinkBindings {

    /// <summary>
    /// Flags to report failure cases over the high latency telemtry.
    /// </summary>    
    public enum HlFailureFlag {
        Gps                                   = 1,           //GPS failure.
        DifferentialPressure                  = 2,           //Differential pressure sensor failure.
        AbsolutePressure                      = 4,           //Absolute pressure sensor failure.
        _3dAccel                              = 8,           //Accelerometer sensor failure.
        _3dGyro                               = 16,          //Gyroscope sensor failure.
        _3dMag                                = 32,          //Magnetometer sensor failure.
        Terrain                               = 64,          //Terrain subsystem failure.
        Battery                               = 128,         //Battery failure/critical low battery.
        RcReceiver                            = 256,         //RC receiver failure/no rc connection.
        OffboardLink                          = 512,         //Offboard link failure.
        Engine                                = 1024,        //Engine failure.
        Geofence                              = 2048,        //Geofence violation.
        Estimator                             = 4096,        //Estimator failure, for example measurement rejection or large variances.
        Mission                               = 8192         //Mission failure.
    }

}
