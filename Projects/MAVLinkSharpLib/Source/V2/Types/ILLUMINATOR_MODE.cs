        
namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// Modes of illuminator
    /// </summary>    
    public enum IlluminatorModeFlags {
        Unknown                           = 0,           //Illuminator mode is not specified/unknown
        InternalControl                   = 1,           //Illuminator behavior is controlled by MAV_CMD_DO_ILLUMINATOR_CONFIGURE settings
        ExternalSync                      = 2            //Illuminator behavior is controlled by external factors: e.g. an external hardware signal
    }

}
