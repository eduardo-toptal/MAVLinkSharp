        
namespace MAVLinkBindings {

    /// <summary>
    /// Possible responses from a WIFI_CONFIG_AP message.
    /// </summary>    
    public enum WifiConfigApResponseFlags {
        Undefined                              = 0,           //Undefined response. Likely an indicative of a system that doesn't support this request.
        Accepted                               = 1,           //Changes accepted.
        Rejected                               = 2,           //Changes rejected.
        ModeError                              = 3,           //Invalid Mode.
        SsidError                              = 4,           //Invalid SSID.
        PasswordError                          = 5            //Invalid Password.
    }

}
