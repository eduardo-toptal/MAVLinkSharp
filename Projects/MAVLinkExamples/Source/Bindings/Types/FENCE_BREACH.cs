        
namespace MAVLinkBindings {

    /// <summary>
    /// 
    /// </summary>    
    public enum FenceBreachFlags {
        None                  = 0,           //No last fence breach
        Minalt                = 1,           //Breached minimum altitude
        Maxalt                = 2,           //Breached maximum altitude
        Boundary              = 3            //Breached fence boundary
    }

}
