        
namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// WiFi Mode.
    /// </summary>    
    public enum WifiConfigApModeFlags {
        Undefined                     = 0,           //WiFi mode is undefined.
        Ap                            = 1,           //WiFi configured as an access point.
        Station                       = 2,           //WiFi configured as a station connected to an existing local WiFi network.
        Disabled                      = 3            //WiFi disabled.
    }

}
