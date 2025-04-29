        
namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// Power supply status flags (bitmask)
    /// </summary>    
    public enum MAVPowerStatusFlags {
        BrickValid                                  = 1,           //main brick power supply valid
        ServoValid                                  = 2,           //main servo power supply valid for FMU
        UsbConnected                                = 4,           //USB power is connected
        PeriphOvercurrent                           = 8,           //peripheral supply is in over-current state
        PeriphHipowerOvercurrent                    = 16,          //hi-power peripheral supply is in over-current state
        Changed                                     = 32           //Power status has changed since boot
    }

}
