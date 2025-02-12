        
namespace MAVLinkBindings {

    /// <summary>
    /// 
    /// </summary>    
    public enum NavVtolLandOptionsFlags {
        Default                             = 0,           //Default autopilot landing behaviour.
        FwDescent                           = 1,           //Descend in fixed wing mode, transitioning to multicopter mode for vertical landing when close to the ground. | The fixed wing descent pattern is at the discretion of the vehicle (e.g. transition altitude, loiter direction, radius, and speed, etc.).
        HoverDescent                        = 2            //Land in multicopter mode on reaching the landing coordinates (the whole landing is by "hover descent").
    }

}
