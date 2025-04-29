        
namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// These flags are used in the AIS_VESSEL.fields bitmask to indicate validity of data in the other message fields. When set, the data is valid.
    /// </summary>    
    public enum AisFlags {
        PositionAccuracy                    = 1,           //1 = Position accuracy less than 10m, 0 = position accuracy greater than 10m.
        ValidCog                            = 2,           //
        ValidVelocity                       = 4,           //
        HighVelocity                        = 8,           //1 = Velocity over 52.5765m/s (102.2 knots)
        ValidTurnRate                       = 16,          //
        TurnRateSignOnly                    = 32,          //Only the sign of the returned turn rate value is valid, either greater than 5deg/30s or less than -5deg/30s
        ValidDimensions                     = 64,          //
        LargeBowDimension                   = 128,         //Distance to bow is larger than 511m
        LargeSternDimension                 = 256,         //Distance to stern is larger than 511m
        LargePortDimension                  = 512,         //Distance to port side is larger than 63m
        LargeStarboardDimension             = 1024,        //Distance to starboard side is larger than 63m
        ValidCallsign                       = 2048,        //
        ValidName                           = 4096         //
    }

}
