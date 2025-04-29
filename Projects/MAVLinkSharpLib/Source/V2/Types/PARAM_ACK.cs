        
namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// Result from PARAM_EXT_SET message (or a PARAM_SET within a transaction).
    /// </summary>    
    public enum ParamAckFlags {
        Accepted                    = 0,           //Parameter value ACCEPTED and SET
        ValueUnsupported            = 1,           //Parameter value UNKNOWN/UNSUPPORTED
        Failed                      = 2,           //Parameter failed to set
        InProgress                  = 3            //Parameter value received but not yet set/accepted. A subsequent PARAM_ACK_TRANSACTION or PARAM_EXT_ACK with the final result will follow once operation is completed. This is returned immediately for parameters that take longer to set, indicating that the the parameter was received and does not need to be resent.
    }

}
