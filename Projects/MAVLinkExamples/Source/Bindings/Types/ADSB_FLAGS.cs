        
namespace MAVLinkBindings {

    /// <summary>
    /// These flags indicate status such as data validity of each data source. Set = data valid
    /// </summary>    
    public enum AdsbFlags {
        ValidCoords                        = 1,           //
        ValidAltitude                      = 2,           //
        ValidHeading                       = 4,           //
        ValidVelocity                      = 8,           //
        ValidCallsign                      = 16,          //
        ValidSquawk                        = 32,          //
        Simulated                          = 64,          //
        VerticalVelocityValid              = 128,         //
        BaroValid                          = 256,         //
        SourceUat                          = 32768        //
    }

}
