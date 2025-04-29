        
namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// RTK GPS baseline coordinate system, used for RTK corrections
    /// </summary>    
    public enum RtkBaselineCoordinateSystemFlags {
        Ecef                                = 0,           //Earth-centered, Earth-fixed
        Ned                                 = 1            //RTK basestation centered, north, east, down
    }

}
