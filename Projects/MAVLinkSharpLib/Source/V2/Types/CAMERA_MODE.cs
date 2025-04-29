        
namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// Camera Modes.
    /// </summary>    
    public enum CameraModeFlags {
        Image                    = 0,           //Camera is in image/photo capture mode.
        Video                    = 1,           //Camera is in video capture mode.
        ImageSurvey              = 2            //Camera is in image survey capture mode. It allows for camera controller to do specific settings for surveys.
    }

}
