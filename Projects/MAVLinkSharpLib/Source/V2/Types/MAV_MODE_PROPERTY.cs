        
namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// Mode properties.
    ///       
    /// </summary>    
    public enum MAVModePropertyFlags {
        Advanced                              = 1,           //If set, this mode is an advanced mode. | For example a rate-controlled manual mode might be advanced, whereas a position-controlled manual mode is not. | A GCS can optionally use this flag to configure the UI for its intended users.
        NotUserSelectable                     = 2,           //If set, this mode should not be added to the list of selectable modes. | The mode might still be selected by the FC directly (for example as part of a failsafe).
        AutoMode                              = 4            //If set, this mode is automatically controlled (it may use but does not require a manual controller). | If unset the mode is a assumed to require user input (be a manual mode).
    }

}
