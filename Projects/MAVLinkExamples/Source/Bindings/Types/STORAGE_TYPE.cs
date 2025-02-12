        
namespace MAVLinkBindings {

    /// <summary>
    /// Flags to indicate the type of storage.
    /// </summary>    
    public enum StorageTypeFlags {
        Unknown                = 0,           //Storage type is not known.
        UsbStick               = 1,           //Storage type is USB device.
        Sd                     = 2,           //Storage type is SD card.
        Microsd                = 3,           //Storage type is microSD card.
        Cf                     = 4,           //Storage type is CFast.
        Cfe                    = 5,           //Storage type is CFexpress.
        Xqd                    = 6,           //Storage type is XQD.
        Hd                     = 7,           //Storage type is HD mass storage type.
        Other                  = 254          //Storage type is other, not listed type.
    }

}
