        
namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// Focus types for MAV_CMD_SET_CAMERA_FOCUS
    /// </summary>    
    public enum SetFocusTypeFlags {
        FocusTypeStep              = 0,           //Focus one step increment (-1 for focusing in, 1 for focusing out towards infinity).
        FocusTypeContinuous        = 1,           //Continuous normalized focus in/out rate until stopped. Range -1..1, negative: in, positive: out towards infinity, 0 to stop focusing. Other values should be clipped to the range.
        FocusTypeRange             = 2,           //Focus value as proportion of full camera focus range (a value between 0.0 and 100.0)
        FocusTypeMeters            = 3,           //Focus value in metres. Note that there is no message to get the valid focus range of the camera, so this can type can only be used for cameras where the range is known (implying that this cannot reliably be used in a GCS for an arbitrary camera).
        FocusTypeAuto              = 4,           //Focus automatically.
        FocusTypeAutoSingle        = 5,           //Single auto focus. Mainly used for still pictures. Usually abbreviated as AF-S.
        FocusTypeAutoContinuous    = 6            //Continuous auto focus. Mainly used for dynamic scenes. Abbreviated as AF-C.
    }

}
