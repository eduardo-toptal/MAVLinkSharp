        
namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// Actions being taken to mitigate/prevent fence breach
    /// </summary>    
    public enum FenceMitigateFlags {
        Unknown                  = 0,           //Unknown
        None                     = 1,           //No actions being taken
        VelLimit                 = 2            //Velocity limiting active to prevent breach
    }

}
