        
namespace MAVLinkBindings {

    /// <summary>
    /// Flags in the HIGHRES_IMU message indicate which fields have updated since the last message
    /// </summary>    
    public enum HighresImuUpdatedFlags {
        HighresImuUpdatedNone             = 0,           //None of the fields in HIGHRES_IMU have been updated
        HighresImuUpdatedXacc             = 1,           //The value in the xacc field has been updated
        HighresImuUpdatedYacc             = 2,           //The value in the yacc field has been updated
        HighresImuUpdatedZacc             = 4,           //The value in the zacc field has been updated since
        HighresImuUpdatedXgyro            = 8,           //The value in the xgyro field has been updated
        HighresImuUpdatedYgyro            = 16,          //The value in the ygyro field has been updated
        HighresImuUpdatedZgyro            = 32,          //The value in the zgyro field has been updated
        HighresImuUpdatedXmag             = 64,          //The value in the xmag field has been updated
        HighresImuUpdatedYmag             = 128,         //The value in the ymag field has been updated
        HighresImuUpdatedZmag             = 256,         //The value in the zmag field has been updated
        HighresImuUpdatedAbsPressure      = 512,         //The value in the abs_pressure field has been updated
        HighresImuUpdatedDiffPressure     = 1024,        //The value in the diff_pressure field has been updated
        HighresImuUpdatedPressureAlt      = 2048,        //The value in the pressure_alt field has been updated
        HighresImuUpdatedTemperature      = 4096,        //The value in the temperature field has been updated
        HighresImuUpdatedAll              = 65535        //All fields in HIGHRES_IMU have been updated.
    }

}
