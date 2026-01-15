        
namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// These encode the sensors whose status is sent as part of the SYS_STATUS message.
    /// </summary>    
    public enum MAVSysStatusSensorFlags {
        MavSysStatusExtensionUsed                    = 0,           //0x80000000 Extended bit-field are used for further sensor status bits (needs to be set in onboard_control_sensors_present only)
        _3dGyro                                      = 1,           //0x01 3D gyro
        _3dAccel                                     = 2,           //0x02 3D accelerometer
        _3dMag                                       = 4,           //0x04 3D magnetometer
        AbsolutePressure                             = 8,           //0x08 absolute pressure
        DifferentialPressure                         = 16,          //0x10 differential pressure
        Gps                                          = 32,          //0x20 GPS
        OpticalFlow                                  = 64,          //0x40 optical flow
        VisionPosition                               = 128,         //0x80 computer vision position
        LaserPosition                                = 256,         //0x100 laser based position
        ExternalGroundTruth                          = 512,         //0x200 external ground truth (Vicon or Leica)
        AngularRateControl                           = 1024,        //0x400 3D angular rate control
        AttitudeStabilization                        = 2048,        //0x800 attitude stabilization
        YawPosition                                  = 4096,        //0x1000 yaw position
        ZAltitudeControl                             = 8192,        //0x2000 z/altitude control
        XyPositionControl                            = 16384,       //0x4000 x/y position control
        MotorOutputs                                 = 32768,       //0x8000 motor outputs / control
        RcReceiver                                   = 65536,       //0x10000 RC receiver
        _3dGyro2                                     = 131072,      //0x20000 2nd 3D gyro
        _3dAccel2                                    = 262144,      //0x40000 2nd 3D accelerometer
        _3dMag2                                      = 524288,      //0x80000 2nd 3D magnetometer
        MavSysStatusGeofence                         = 1048576,     //0x100000 geofence
        MavSysStatusAhrs                             = 2097152,     //0x200000 AHRS subsystem health
        MavSysStatusTerrain                          = 4194304,     //0x400000 Terrain subsystem health
        MavSysStatusReverseMotor                     = 8388608,     //0x800000 Motors are reversed
        MavSysStatusLogging                          = 16777216,    //0x1000000 Logging
        Battery                                      = 33554432,    //0x2000000 Battery
        Proximity                                    = 67108864,    //0x4000000 Proximity
        Satcom                                       = 134217728,   //0x8000000 Satellite Communication
        MavSysStatusPrearmCheck                      = 268435456,   //0x10000000 pre-arm check status. Always healthy when armed
        MavSysStatusObstacleAvoidance                = 536870912,   //0x20000000 Avoidance/collision prevention
        Propulsion                                   = 1073741824   //0x40000000 propulsion (actuator, esc, motor or propellor)
    }

}
