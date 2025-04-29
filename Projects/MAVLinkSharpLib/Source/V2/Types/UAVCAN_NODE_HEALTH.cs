        
namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// Generalized UAVCAN node health
    /// </summary>    
    public enum UavcanNodeHealthFlags {
        Ok                          = 0,           //The node is functioning properly.
        Warning                     = 1,           //A critical parameter went out of range or the node has encountered a minor failure.
        Error                       = 2,           //The node has encountered a major failure.
        Critical                    = 3            //The node has suffered a fatal malfunction.
    }

}
