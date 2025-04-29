        
namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// Enable axes that will be tuned via autotuning. Used in MAV_CMD_DO_AUTOTUNE_ENABLE.
    /// </summary>    
    public enum AutotuneAxisFlags {
        Default               = 0,           //Flight stack tunes axis according to its default settings.
        Roll                  = 1,           //Autotune roll axis.
        Pitch                 = 2,           //Autotune pitch axis.
        Yaw                   = 4            //Autotune yaw axis.
    }

}
