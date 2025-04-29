        
namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// 
    /// </summary>    
    public enum MAVOdidDescTypeFlags {
        Text                               = 0,           //Optional free-form text description of the purpose of the flight.
        Emergency                          = 1,           //Optional additional clarification when status == MAV_ODID_STATUS_EMERGENCY.
        ExtendedStatus                     = 2            //Optional additional clarification when status != MAV_ODID_STATUS_EMERGENCY.
    }

}
