        
namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// Enumeration of sensor orientation, according to its rotations
    /// </summary>    
    public enum MAVSensorOrientationFlags {
        MavSensorRotationNone                        = 0,           //Roll: 0, Pitch: 0, Yaw: 0
        MavSensorRotationYaw45                       = 1,           //Roll: 0, Pitch: 0, Yaw: 45
        MavSensorRotationYaw90                       = 2,           //Roll: 0, Pitch: 0, Yaw: 90
        MavSensorRotationYaw135                      = 3,           //Roll: 0, Pitch: 0, Yaw: 135
        MavSensorRotationYaw180                      = 4,           //Roll: 0, Pitch: 0, Yaw: 180
        MavSensorRotationYaw225                      = 5,           //Roll: 0, Pitch: 0, Yaw: 225
        MavSensorRotationYaw270                      = 6,           //Roll: 0, Pitch: 0, Yaw: 270
        MavSensorRotationYaw315                      = 7,           //Roll: 0, Pitch: 0, Yaw: 315
        MavSensorRotationRoll180                     = 8,           //Roll: 180, Pitch: 0, Yaw: 0
        MavSensorRotationRoll180Yaw45                = 9,           //Roll: 180, Pitch: 0, Yaw: 45
        MavSensorRotationRoll180Yaw90                = 10,          //Roll: 180, Pitch: 0, Yaw: 90
        MavSensorRotationRoll180Yaw135               = 11,          //Roll: 180, Pitch: 0, Yaw: 135
        MavSensorRotationPitch180                    = 12,          //Roll: 0, Pitch: 180, Yaw: 0
        MavSensorRotationRoll180Yaw225               = 13,          //Roll: 180, Pitch: 0, Yaw: 225
        MavSensorRotationRoll180Yaw270               = 14,          //Roll: 180, Pitch: 0, Yaw: 270
        MavSensorRotationRoll180Yaw315               = 15,          //Roll: 180, Pitch: 0, Yaw: 315
        MavSensorRotationRoll90                      = 16,          //Roll: 90, Pitch: 0, Yaw: 0
        MavSensorRotationRoll90Yaw45                 = 17,          //Roll: 90, Pitch: 0, Yaw: 45
        MavSensorRotationRoll90Yaw90                 = 18,          //Roll: 90, Pitch: 0, Yaw: 90
        MavSensorRotationRoll90Yaw135                = 19,          //Roll: 90, Pitch: 0, Yaw: 135
        MavSensorRotationRoll270                     = 20,          //Roll: 270, Pitch: 0, Yaw: 0
        MavSensorRotationRoll270Yaw45                = 21,          //Roll: 270, Pitch: 0, Yaw: 45
        MavSensorRotationRoll270Yaw90                = 22,          //Roll: 270, Pitch: 0, Yaw: 90
        MavSensorRotationRoll270Yaw135               = 23,          //Roll: 270, Pitch: 0, Yaw: 135
        MavSensorRotationPitch90                     = 24,          //Roll: 0, Pitch: 90, Yaw: 0
        MavSensorRotationPitch270                    = 25,          //Roll: 0, Pitch: 270, Yaw: 0
        MavSensorRotationPitch180Yaw90               = 26,          //Roll: 0, Pitch: 180, Yaw: 90
        MavSensorRotationPitch180Yaw270              = 27,          //Roll: 0, Pitch: 180, Yaw: 270
        MavSensorRotationRoll90Pitch90               = 28,          //Roll: 90, Pitch: 90, Yaw: 0
        MavSensorRotationRoll180Pitch90              = 29,          //Roll: 180, Pitch: 90, Yaw: 0
        MavSensorRotationRoll270Pitch90              = 30,          //Roll: 270, Pitch: 90, Yaw: 0
        MavSensorRotationRoll90Pitch180              = 31,          //Roll: 90, Pitch: 180, Yaw: 0
        MavSensorRotationRoll270Pitch180             = 32,          //Roll: 270, Pitch: 180, Yaw: 0
        MavSensorRotationRoll90Pitch270              = 33,          //Roll: 90, Pitch: 270, Yaw: 0
        MavSensorRotationRoll180Pitch270             = 34,          //Roll: 180, Pitch: 270, Yaw: 0
        MavSensorRotationRoll270Pitch270             = 35,          //Roll: 270, Pitch: 270, Yaw: 0
        MavSensorRotationRoll90Pitch180Yaw90         = 36,          //Roll: 90, Pitch: 180, Yaw: 90
        MavSensorRotationRoll90Yaw270                = 37,          //Roll: 90, Pitch: 0, Yaw: 270
        MavSensorRotationRoll90Pitch68Yaw293         = 38,          //Roll: 90, Pitch: 68, Yaw: 293
        MavSensorRotationPitch315                    = 39,          //Pitch: 315
        MavSensorRotationRoll90Pitch315              = 40,          //Roll: 90, Pitch: 315
        MavSensorRotationCustom                      = 100          //Custom orientation
    }

}
