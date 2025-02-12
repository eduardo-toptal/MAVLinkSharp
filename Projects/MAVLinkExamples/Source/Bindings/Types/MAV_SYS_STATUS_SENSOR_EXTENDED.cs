        
namespace MAVLinkBindings {

    /// <summary>
    /// These encode the sensors whose status is sent as part of the SYS_STATUS message in the extended fields.
    /// </summary>    
    public enum MAVSysStatusSensorExtendedFlags {
        MavSysStatusRecoverySystem     = 1            //0x01 Recovery system (parachute, balloon, retracts etc)
    }

}
