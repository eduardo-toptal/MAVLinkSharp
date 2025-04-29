        
namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// Micro air vehicle / autopilot classes. This identifies the individual model.
    /// </summary>    
    public enum MAVAutopilotFlags {
        Generic                                                    = 0,           //Generic autopilot, full support for everything
        Reserved                                                   = 1,           //Reserved for future use.
        Slugs                                                      = 2,           //SLUGS autopilot, http://slugsuav.soe.ucsc.edu
        Ardupilotmega                                              = 3,           //ArduPilot - Plane/Copter/Rover/Sub/Tracker, https://ardupilot.org
        Openpilot                                                  = 4,           //OpenPilot, http://openpilot.org
        GenericWaypointsOnly                                       = 5,           //Generic autopilot only supporting simple waypoints
        GenericWaypointsAndSimpleNavigationOnly                    = 6,           //Generic autopilot supporting waypoints and other simple navigation commands
        GenericMissionFull                                         = 7,           //Generic autopilot supporting the full mission command set
        Invalid                                                    = 8,           //No valid autopilot, e.g. a GCS or other MAVLink component
        Ppz                                                        = 9,           //PPZ UAV - http://nongnu.org/paparazzi
        Udb                                                        = 10,          //UAV Dev Board
        Fp                                                         = 11,          //FlexiPilot
        Px4                                                        = 12,          //PX4 Autopilot - http://px4.io/
        Smaccmpilot                                                = 13,          //SMACCMPilot - http://smaccmpilot.org
        Autoquad                                                   = 14,          //AutoQuad -- http://autoquad.org
        Armazila                                                   = 15,          //Armazila -- http://armazila.com
        Aerob                                                      = 16,          //Aerob -- http://aerob.ru
        Asluav                                                     = 17,          //ASLUAV autopilot -- http://www.asl.ethz.ch
        Smartap                                                    = 18,          //SmartAP Autopilot - http://sky-drones.com
        Airrails                                                   = 19,          //AirRails - http://uaventure.com
        Reflex                                                     = 20           //Fusion Reflex - https://fusion.engineering
    }

}
