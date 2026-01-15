        
namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// Axes that will be autotuned by MAV_CMD_DO_AUTOTUNE_ENABLE.
    ///         Note that at least one flag must be set in MAV_CMD_DO_AUTOTUNE_ENABLE.param2: if none are set, the flight stack will tune its default set of axes.
    /// </summary>    
    public enum AutotuneAxisFlags {
        Roll                = 1,           //Autotune roll axis.
        Pitch               = 2,           //Autotune pitch axis.
        Yaw                 = 4            //Autotune yaw axis.
    }

}
