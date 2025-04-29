        
namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// Bitmap of options for the MAV_CMD_DO_REPOSITION
    /// </summary>    
    public enum MAVDoRepositionFlags {
        ChangeMode                          = 1            //The aircraft should immediately transition into guided. This should not be set for follow me applications
    }

}
