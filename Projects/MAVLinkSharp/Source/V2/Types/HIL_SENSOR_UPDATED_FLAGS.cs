        
namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// Flags in the HIL_SENSOR message indicate which fields have updated since the last message
    /// </summary>    
    public enum HilSensorUpdatedFlags {
        HilSensorUpdatedNone             = 0,           //None of the fields in HIL_SENSOR have been updated
        HilSensorUpdatedReset            = 0,           //Full reset of attitude/position/velocities/etc was performed in sim (Bit 31).
        HilSensorUpdatedXacc             = 1,           //The value in the xacc field has been updated
        HilSensorUpdatedYacc             = 2,           //The value in the yacc field has been updated
        HilSensorUpdatedZacc             = 4,           //The value in the zacc field has been updated
        HilSensorUpdatedXgyro            = 8,           //The value in the xgyro field has been updated
        HilSensorUpdatedYgyro            = 16,          //The value in the ygyro field has been updated
        HilSensorUpdatedZgyro            = 32,          //The value in the zgyro field has been updated
        HilSensorUpdatedXmag             = 64,          //The value in the xmag field has been updated
        HilSensorUpdatedYmag             = 128,         //The value in the ymag field has been updated
        HilSensorUpdatedZmag             = 256,         //The value in the zmag field has been updated
        HilSensorUpdatedAbsPressure      = 512,         //The value in the abs_pressure field has been updated
        HilSensorUpdatedDiffPressure     = 1024,        //The value in the diff_pressure field has been updated
        HilSensorUpdatedPressureAlt      = 2048,        //The value in the pressure_alt field has been updated
        HilSensorUpdatedTemperature      = 4096         //The value in the temperature field has been updated
    }

}
