        
namespace MAVLinkBindings {

    /// <summary>
    /// Flags for the global position report.
    /// </summary>    
    public enum UtmDataAvailFlags {
        TimeValid                                        = 1,           //The field time contains valid data.
        UasIdAvailable                                   = 2,           //The field uas_id contains valid data.
        PositionAvailable                                = 4,           //The fields lat, lon and h_acc contain valid data.
        AltitudeAvailable                                = 8,           //The fields alt and v_acc contain valid data.
        RelativeAltitudeAvailable                        = 16,          //The field relative_alt contains valid data.
        HorizontalVeloAvailable                          = 32,          //The fields vx and vy contain valid data.
        VerticalVeloAvailable                            = 64,          //The field vz contains valid data.
        NextWaypointAvailable                            = 128          //The fields next_lat, next_lon and next_alt contain valid data.
    }

}
