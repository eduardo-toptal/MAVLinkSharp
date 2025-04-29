        
namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// Indicates the ESC connection type.
    /// </summary>    
    public enum EscConnectionTypeFlags {
        Ppm                         = 0,           //Traditional PPM ESC.
        Serial                      = 1,           //Serial Bus connected ESC.
        Oneshot                     = 2,           //One Shot PPM ESC.
        I2c                         = 3,           //I2C ESC.
        Can                         = 4,           //CAN-Bus ESC.
        Dshot                       = 5            //DShot ESC.
    }

}
