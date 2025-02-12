        
namespace MAVLinkBindings {

    /// <summary>
    /// List of possible failure type to inject.
    /// </summary>    
    public enum FailureTypeFlags {
        Ok                        = 0,           //No failure injected, used to reset a previous failure.
        Off                       = 1,           //Sets unit off, so completely non-responsive.
        Stuck                     = 2,           //Unit is stuck e.g. keeps reporting the same value.
        Garbage                   = 3,           //Unit is reporting complete garbage.
        Wrong                     = 4,           //Unit is consistently wrong.
        Slow                      = 5,           //Unit is slow, so e.g. reporting at slower than expected rate.
        Delayed                   = 6,           //Data of unit is delayed in time.
        Intermittent              = 7            //Unit is sometimes working, sometimes not.
    }

}
