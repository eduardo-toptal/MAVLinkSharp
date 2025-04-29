        
namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// Flags to report ESC failures.
    /// </summary>    
    public enum EscFailureFlags {
        EscFailureNone               = 0,           //No ESC failure.
        EscFailureOverCurrent        = 1,           //Over current failure.
        EscFailureOverVoltage        = 2,           //Over voltage failure.
        EscFailureOverTemperature    = 4,           //Over temperature failure.
        EscFailureOverRpm            = 8,           //Over RPM failure.
        EscFailureInconsistentCmd    = 16,          //Inconsistent command failure i.e. out of bounds.
        EscFailureMotorStuck         = 32,          //Motor stuck failure.
        EscFailureGeneric            = 64           //Generic ESC failure.
    }

}
