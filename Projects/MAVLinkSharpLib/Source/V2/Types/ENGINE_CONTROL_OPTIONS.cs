        
namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// Engine control options
    /// </summary>    
    public enum EngineControlOptionsFlags {
        AllowStartWhileDisarmed                           = 1            //Allow starting the engine while disarmed (without changing the vehicle's armed state). This effectively arms just the ICE, without arming the vehicle to start other motors or propellers.
    }

}
