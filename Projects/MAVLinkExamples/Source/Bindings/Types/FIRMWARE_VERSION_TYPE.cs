        
namespace MAVLinkBindings {

    /// <summary>
    /// These values define the type of firmware release.  These values indicate the first version or release of this type.  For example the first alpha release would be 64, the second would be 65.
    /// </summary>    
    public enum FirmwareVersionTypeFlags {
        Dev                            = 0,           //development release
        Alpha                          = 64,          //alpha release
        Beta                           = 128,         //beta release
        Rc                             = 192,         //release candidate
        Official                       = 255          //official stable release
    }

}
