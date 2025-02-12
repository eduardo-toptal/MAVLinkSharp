        
namespace MAVLinkBindings {

    /// <summary>
    /// Bitmap to indicate which dimensions should be ignored by the vehicle: a value of 0b0000000000000000 or 0b0000001000000000 indicates that none of the setpoint dimensions should be ignored. If bit 9 is set the floats afx afy afz should be interpreted as force instead of acceleration.
    /// </summary>    
    public enum PositionTargetTypemaskFlags {
        XIgnore                                  = 1,           //Ignore position x
        YIgnore                                  = 2,           //Ignore position y
        ZIgnore                                  = 4,           //Ignore position z
        VxIgnore                                 = 8,           //Ignore velocity x
        VyIgnore                                 = 16,          //Ignore velocity y
        VzIgnore                                 = 32,          //Ignore velocity z
        AxIgnore                                 = 64,          //Ignore acceleration x
        AyIgnore                                 = 128,         //Ignore acceleration y
        AzIgnore                                 = 256,         //Ignore acceleration z
        ForceSet                                 = 512,         //Use force instead of acceleration
        YawIgnore                                = 1024,        //Ignore yaw
        YawRateIgnore                            = 2048         //Ignore yaw rate
    }

}
