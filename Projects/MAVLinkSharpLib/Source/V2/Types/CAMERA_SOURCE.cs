        
namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// Camera sources for MAV_CMD_SET_CAMERA_SOURCE
    /// </summary>    
    public enum CameraSourceFlags {
        Default               = 0,           //Default camera source.
        Rgb                   = 1,           //RGB camera source.
        Ir                    = 2,           //IR camera source.
        Ndvi                  = 3            //NDVI camera source.
    }

}
