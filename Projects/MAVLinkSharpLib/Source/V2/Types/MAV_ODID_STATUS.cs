        
namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// 
    /// </summary>    
    public enum MAVOdidStatusFlags {
        Undeclared                               = 0,           //The status of the (UA) Unmanned Aircraft is undefined.
        Ground                                   = 1,           //The UA is on the ground.
        Airborne                                 = 2,           //The UA is in the air.
        Emergency                                = 3,           //The UA is having an emergency.
        RemoteIdSystemFailure                    = 4            //The remote ID system is failing or unreliable in some way.
    }

}
