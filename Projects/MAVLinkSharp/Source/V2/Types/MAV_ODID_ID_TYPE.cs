        
namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// 
    /// </summary>    
    public enum MAVOdidIdTypeFlags {
        None                                 = 0,           //No type defined.
        SerialNumber                         = 1,           //Manufacturer Serial Number (ANSI/CTA-2063 format).
        CaaRegistrationId                    = 2,           //CAA (Civil Aviation Authority) registered ID. Format: [ICAO Country Code].[CAA Assigned ID].
        UtmAssignedUuid                      = 3,           //UTM (Unmanned Traffic Management) assigned UUID (RFC4122).
        SpecificSessionId                    = 4            //A 20 byte ID for a specific flight/session. The exact ID type is indicated by the first byte of uas_id and these type values are managed by ICAO.
    }

}
