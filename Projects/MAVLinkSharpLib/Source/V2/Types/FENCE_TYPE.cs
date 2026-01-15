        
namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// Fence types to enable or disable when using MAV_CMD_DO_FENCE_ENABLE.
    ///         Note that at least one of these flags must be set in MAV_CMD_DO_FENCE_ENABLE.param2.
    ///         If none are set, the flight stack will ignore the field and enable/disable its default set of fences (usually all of them).
    ///       
    /// </summary>    
    public enum FenceTypeFlags {
        AltMax             = 1,           //Maximum altitude fence
        Circle             = 2,           //Circle fence
        Polygon            = 4,           //Polygon fence
        AltMin             = 8            //Minimum altitude fence
    }

}
