        
namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// Supported component metadata types. These are used in the "general" metadata file returned by COMPONENT_METADATA to provide information about supported metadata types. The types are not used directly in MAVLink messages.
    /// </summary>    
    public enum CompMetadataTypeFlags {
        General                        = 0,           //General information about the component. General metadata includes information about other metadata types supported by the component. Files of this type must be supported, and must be downloadable from vehicle using a MAVLink FTP URI.
        Parameter                      = 1,           //Parameter meta data.
        Commands                       = 2,           //Meta data that specifies which commands and command parameters the vehicle supports. (WIP)
        Peripherals                    = 3,           //Meta data that specifies external non-MAVLink peripherals.
        Events                         = 4,           //Meta data for the events interface.
        Actuators                      = 5            //Meta data for actuator configuration (motors, servos and vehicle geometry) and testing.
    }

}
