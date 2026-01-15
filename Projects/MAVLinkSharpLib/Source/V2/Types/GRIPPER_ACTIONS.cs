        
namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// Gripper actions.
    /// </summary>    
    public enum GripperActionsFlags {
        GripperActionRelease   = 0,           //Gripper release cargo.
        GripperActionGrab      = 1,           //Gripper grab onto cargo.
        GripperActionHold      = 2            //Gripper hold current grip state/position.
    }

}
