        
namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// Enumeration of estimator types
    /// </summary>    
    public enum MAVEstimatorTypeFlags {
        Unknown                      = 0,           //Unknown type of the estimator.
        Naive                        = 1,           //This is a naive estimator without any real covariance feedback.
        Vision                       = 2,           //Computer vision based estimate. Might be up to scale.
        Vio                          = 3,           //Visual-inertial estimate.
        Gps                          = 4,           //Plain GPS estimate.
        GpsIns                       = 5,           //Estimator integrating GPS and inertial sensing.
        Mocap                        = 6,           //Estimate from external motion capturing system.
        Lidar                        = 7,           //Estimator based on lidar sensor input.
        Autopilot                    = 8            //Estimator on autopilot.
    }

}
