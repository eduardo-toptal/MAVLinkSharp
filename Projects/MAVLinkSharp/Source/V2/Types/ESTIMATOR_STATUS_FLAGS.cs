        
namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// Flags in ESTIMATOR_STATUS message
    /// </summary>    
    public enum EstimatorStatusFlags {
        EstimatorAttitude            = 1,           //True if the attitude estimate is good
        EstimatorVelocityHoriz       = 2,           //True if the horizontal velocity estimate is good
        EstimatorVelocityVert        = 4,           //True if the  vertical velocity estimate is good
        EstimatorPosHorizRel         = 8,           //True if the horizontal position (relative) estimate is good
        EstimatorPosHorizAbs         = 16,          //True if the horizontal position (absolute) estimate is good
        EstimatorPosVertAbs          = 32,          //True if the vertical position (absolute) estimate is good
        EstimatorPosVertAgl          = 64,          //True if the vertical position (above ground) estimate is good
        EstimatorConstPosMode        = 128,         //True if the EKF is in a constant position mode and is not using external measurements (eg GPS or optical flow)
        EstimatorPredPosHorizRel     = 256,         //True if the EKF has sufficient data to enter a mode that will provide a (relative) position estimate
        EstimatorPredPosHorizAbs     = 512,         //True if the EKF has sufficient data to enter a mode that will provide a (absolute) position estimate
        EstimatorGpsGlitch           = 1024,        //True if the EKF has detected a GPS glitch
        EstimatorAccelError          = 2048         //True if the EKF has detected bad accelerometer data
    }

}
