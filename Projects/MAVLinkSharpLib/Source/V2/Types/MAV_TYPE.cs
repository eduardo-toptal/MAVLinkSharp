        
namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// MAVLINK component type reported in HEARTBEAT message. Flight controllers must report the type of the vehicle on which they are mounted (e.g. MAV_TYPE_OCTOROTOR). All other components must report a value appropriate for their type (e.g. a camera must use MAV_TYPE_CAMERA).
    /// </summary>    
    public enum MAVTypeFlags {
        Generic                            = 0,           //Generic micro air vehicle
        FixedWing                          = 1,           //Fixed wing aircraft.
        Quadrotor                          = 2,           //Quadrotor
        Coaxial                            = 3,           //Coaxial helicopter
        Helicopter                         = 4,           //Normal helicopter with tail rotor.
        AntennaTracker                     = 5,           //Ground installation
        Gcs                                = 6,           //Operator control unit / ground control station
        Airship                            = 7,           //Airship, controlled
        FreeBalloon                        = 8,           //Free balloon, uncontrolled
        Rocket                             = 9,           //Rocket
        GroundRover                        = 10,          //Ground rover
        SurfaceBoat                        = 11,          //Surface vessel, boat, ship
        Submarine                          = 12,          //Submarine
        Hexarotor                          = 13,          //Hexarotor
        Octorotor                          = 14,          //Octorotor
        Tricopter                          = 15,          //Tricopter
        FlappingWing                       = 16,          //Flapping wing
        Kite                               = 17,          //Kite
        OnboardController                  = 18,          //Onboard companion controller
        VtolTailsitterDuorotor             = 19,          //Two-rotor Tailsitter VTOL that additionally uses control surfaces in vertical operation. Note, value previously named MAV_TYPE_VTOL_DUOROTOR.
        VtolTailsitterQuadrotor            = 20,          //Quad-rotor Tailsitter VTOL using a V-shaped quad config in vertical operation. Note: value previously named MAV_TYPE_VTOL_QUADROTOR.
        VtolTiltrotor                      = 21,          //Tiltrotor VTOL. Fuselage and wings stay (nominally) horizontal in all flight phases. It able to tilt (some) rotors to provide thrust in cruise flight.
        VtolFixedrotor                     = 22,          //VTOL with separate fixed rotors for hover and cruise flight. Fuselage and wings stay (nominally) horizontal in all flight phases.
        VtolTailsitter                     = 23,          //Tailsitter VTOL. Fuselage and wings orientation changes depending on flight phase: vertical for hover, horizontal for cruise. Use more specific VTOL MAV_TYPE_VTOL_TAILSITTER_DUOROTOR or MAV_TYPE_VTOL_TAILSITTER_QUADROTOR if appropriate.
        VtolTiltwing                       = 24,          //Tiltwing VTOL. Fuselage stays horizontal in all flight phases. The whole wing, along with any attached engine, can tilt between vertical and horizontal mode.
        VtolReserved5                      = 25,          //VTOL reserved 5
        Gimbal                             = 26,          //Gimbal
        Adsb                               = 27,          //ADSB system
        Parafoil                           = 28,          //Steerable, nonrigid airfoil
        Dodecarotor                        = 29,          //Dodecarotor
        Camera                             = 30,          //Camera
        ChargingStation                    = 31,          //Charging station
        Flarm                              = 32,          //FLARM collision avoidance system
        Servo                              = 33,          //Servo
        Odid                               = 34,          //Open Drone ID. See https://mavlink.io/en/services/opendroneid.html.
        Decarotor                          = 35,          //Decarotor
        Battery                            = 36,          //Battery
        Parachute                          = 37,          //Parachute
        Log                                = 38,          //Log
        Osd                                = 39,          //OSD
        Imu                                = 40,          //IMU
        Gps                                = 41,          //GPS
        Winch                              = 42,          //Winch
        GenericMultirotor                  = 43,          //Generic multirotor that does not fit into a specific type or whose type is unknown
        Illuminator                        = 44,          //Illuminator. An illuminator is a light source that is used for lighting up dark areas external to the system: e.g. a torch or searchlight (as opposed to a light source for illuminating the system itself, e.g. an indicator light).
        SpacecraftOrbiter                  = 45,          //Orbiter spacecraft. Includes satellites orbiting terrestrial and extra-terrestrial bodies. Follows NASA Spacecraft Classification.
        GroundQuadruped                    = 46,          //A generic four-legged ground vehicle (e.g., a robot dog).
        VtolGyrodyne                       = 47,          //VTOL hybrid of helicopter and autogyro. It has a main rotor for lift and separate propellers for forward flight. The rotor must be powered for hover but can autorotate in cruise flight. See: https://en.wikipedia.org/wiki/Gyrodyne
        Gripper                            = 48,          //Gripper
        Radio                              = 49           //Radio
    }

}
