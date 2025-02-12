        
namespace MAVLinkBindings {

    /// <summary>
    /// Aircraft-rated danger from this threat.
    /// </summary>    
    public enum MAVCollisionThreatLevelFlags {
        None                            = 0,           //Not a threat
        Low                             = 1,           //Craft is mildly concerned about this threat
        High                            = 2            //Craft is panicking, and may take actions to avoid threat
    }

}
