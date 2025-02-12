        
namespace MAVLinkBindings {

    /// <summary>
    /// These values encode the bit positions of the decode position. These values can be used to read the value of a flag bit by combining the base_mode variable with AND with the flag position value. The result will be either 0 or 1, depending on if the flag is set or not.
    /// </summary>    
    public enum MAVModeFlagDecodePositionFlags {
        CustomMode                                = 1,           //Eighth bit: 00000001
        Test                                      = 2,           //Seventh bit: 00000010
        Auto                                      = 4,           //Sixth bit:   00000100
        Guided                                    = 8,           //Fifth bit:  00001000
        Stabilize                                 = 16,          //Fourth bit: 00010000
        Hil                                       = 32,          //Third bit:  00100000
        Manual                                    = 64,          //Second bit: 01000000
        Safety                                    = 128          //First bit:  10000000
    }

}
