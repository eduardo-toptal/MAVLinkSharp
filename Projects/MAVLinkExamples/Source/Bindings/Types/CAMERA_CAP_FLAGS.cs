        
namespace MAVLinkBindings {

    /// <summary>
    /// Camera capability flags (Bitmap)
    /// </summary>    
    public enum CameraCapFlags {
        CaptureVideo                                     = 1,           //Camera is able to record video
        CaptureImage                                     = 2,           //Camera is able to capture images
        HasModes                                         = 4,           //Camera has separate Video and Image/Photo modes (MAV_CMD_SET_CAMERA_MODE)
        CanCaptureImageInVideoMode                       = 8,           //Camera can capture images while in video mode
        CanCaptureVideoInImageMode                       = 16,          //Camera can capture videos while in Photo/Image mode
        HasImageSurveyMode                               = 32,          //Camera has image survey mode (MAV_CMD_SET_CAMERA_MODE)
        HasBasicZoom                                     = 64,          //Camera has basic zoom control (MAV_CMD_SET_CAMERA_ZOOM)
        HasBasicFocus                                    = 128,         //Camera has basic focus control (MAV_CMD_SET_CAMERA_FOCUS)
        HasVideoStream                                   = 256,         //Camera has video streaming capabilities (request VIDEO_STREAM_INFORMATION with MAV_CMD_REQUEST_MESSAGE for video streaming info)
        HasTrackingPoint                                 = 512,         //Camera supports tracking of a point on the camera view.
        HasTrackingRectangle                             = 1024,        //Camera supports tracking of a selection rectangle on the camera view.
        HasTrackingGeoStatus                             = 2048         //Camera supports tracking geo status (CAMERA_TRACKING_GEO_STATUS).
    }

}
