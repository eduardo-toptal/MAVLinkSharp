        
namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// Stream status flags (Bitmap)
    /// </summary>    
    public enum VideoStreamStatusFlags {
        Running                                         = 1,           //Stream is active (running)
        Thermal                                         = 2,           //Stream is thermal imaging
        ThermalRangeEnabled                             = 4            //Stream can report absolute thermal range (see CAMERA_THERMAL_RANGE).
    }

}
