        
namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// 
    /// </summary>    
    public enum MagCalStatusFlags {
        MagCalNotStarted         = 0,           //
        MagCalWaitingToStart     = 1,           //
        MagCalRunningStepOne     = 2,           //
        MagCalRunningStepTwo     = 3,           //
        MagCalSuccess            = 4,           //
        MagCalFailed             = 5,           //
        MagCalBadOrientation     = 6,           //
        MagCalBadRadius          = 7            //
    }

}
