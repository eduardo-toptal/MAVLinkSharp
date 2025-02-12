        
namespace MAVLinkBindings {

    /// <summary>
    /// Type of mission items being requested/sent in mission protocol.
    /// </summary>    
    public enum MAVMissionTypeFlags {
        Mission                  = 0,           //Items are mission commands for main mission.
        Fence                    = 1,           //Specifies GeoFence area(s). Items are MAV_CMD_NAV_FENCE_ GeoFence items.
        Rally                    = 2,           //Specifies the rally points for the vehicle. Rally points are alternative RTL points. Items are MAV_CMD_NAV_RALLY_POINT rally point items.
        All                      = 255          //Only used in MISSION_CLEAR_ALL to clear all mission types.
    }

}
