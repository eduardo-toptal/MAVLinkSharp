        
namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// Coordinate frames used by MAVLink. Not all frames are supported by all commands, messages, or vehicles.
    /// 
    ///       Global frames use the following naming conventions:
    ///       - "GLOBAL": Global coordinate frame with WGS84 latitude/longitude and altitude positive over mean sea level (MSL) by default.
    ///         The following modifiers may be used with "GLOBAL":
    ///         - "RELATIVE_ALT": Altitude is relative to the vehicle home position rather than MSL.
    ///         - "TERRAIN_ALT": Altitude is relative to ground level rather than MSL.
    ///         - "INT": Latitude/longitude (in degrees) are scaled by multiplying by 1E7.
    /// 
    ///       Local frames use the following naming conventions:
    ///       - "LOCAL": Origin of local frame is fixed relative to earth. Unless otherwise specified this origin is the origin of the vehicle position-estimator ("EKF").
    ///       - "BODY": Origin of local frame travels with the vehicle. NOTE, "BODY" does NOT indicate alignment of frame axis with vehicle attitude.
    ///       - "OFFSET": Deprecated synonym for "BODY" (origin travels with the vehicle). Not to be used for new frames.
    /// 
    ///       Some deprecated frames do not follow these conventions (e.g. MAV_FRAME_BODY_NED and MAV_FRAME_BODY_OFFSET_NED).
    ///  
    /// </summary>    
    public enum MAVFrameFlags {
        Global                            = 0,           //Global (WGS84) coordinate frame + altitude relative to mean sea level (MSL).
        LocalNed                          = 1,           //NED local tangent frame (x: North, y: East, z: Down) with origin fixed relative to earth.
        Mission                           = 2,           //NOT a coordinate frame, indicates a mission command.
        GlobalRelativeAlt                 = 3,           //Global (WGS84) coordinate frame + altitude relative to the home position.
        LocalEnu                          = 4,           //ENU local tangent frame (x: East, y: North, z: Up) with origin fixed relative to earth.
        GlobalInt                         = 5,           //Global (WGS84) coordinate frame (scaled) + altitude relative to mean sea level (MSL).
        GlobalRelativeAltInt              = 6,           //Global (WGS84) coordinate frame (scaled) + altitude relative to the home position.
        LocalOffsetNed                    = 7,           //NED local tangent frame (x: North, y: East, z: Down) with origin that travels with the vehicle.
        BodyNed                           = 8,           //Same as MAV_FRAME_LOCAL_NED when used to represent position values. Same as MAV_FRAME_BODY_FRD when used with velocity/acceleration values.
        BodyOffsetNed                     = 9,           //This is the same as MAV_FRAME_BODY_FRD.
        GlobalTerrainAlt                  = 10,          //Global (WGS84) coordinate frame with AGL altitude (altitude at ground level).
        GlobalTerrainAltInt               = 11,          //Global (WGS84) coordinate frame (scaled) with AGL altitude (altitude at ground level).
        BodyFrd                           = 12,          //FRD local frame aligned to the vehicle's attitude (x: Forward, y: Right, z: Down) with an origin that travels with vehicle.
        Reserved13                        = 13,          //MAV_FRAME_BODY_FLU - Body fixed frame of reference, Z-up (x: Forward, y: Left, z: Up).
        Reserved14                        = 14,          //MAV_FRAME_MOCAP_NED - Odometry local coordinate frame of data given by a motion capture system, Z-down (x: North, y: East, z: Down).
        Reserved15                        = 15,          //MAV_FRAME_MOCAP_ENU - Odometry local coordinate frame of data given by a motion capture system, Z-up (x: East, y: North, z: Up).
        Reserved16                        = 16,          //MAV_FRAME_VISION_NED - Odometry local coordinate frame of data given by a vision estimation system, Z-down (x: North, y: East, z: Down).
        Reserved17                        = 17,          //MAV_FRAME_VISION_ENU - Odometry local coordinate frame of data given by a vision estimation system, Z-up (x: East, y: North, z: Up).
        Reserved18                        = 18,          //MAV_FRAME_ESTIM_NED - Odometry local coordinate frame of data given by an estimator running onboard the vehicle, Z-down (x: North, y: East, z: Down).
        Reserved19                        = 19,          //MAV_FRAME_ESTIM_ENU - Odometry local coordinate frame of data given by an estimator running onboard the vehicle, Z-up (x: East, y: North, z: Up).
        LocalFrd                          = 20,          //FRD local tangent frame (x: Forward, y: Right, z: Down) with origin fixed relative to earth. The forward axis is aligned to the front of the vehicle in the horizontal plane.
        LocalFlu                          = 21           //FLU local tangent frame (x: Forward, y: Left, z: Up) with origin fixed relative to earth. The forward axis is aligned to the front of the vehicle in the horizontal plane.
    }

}
