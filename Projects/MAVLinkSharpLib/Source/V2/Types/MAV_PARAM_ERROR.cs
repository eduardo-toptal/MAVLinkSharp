        
namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// Parameter protocol error types (see PARAM_ERROR).
    /// </summary>    
    public enum MAVParamErrorFlags {
        NoError                             = 0,           //No error occurred (not expected in PARAM_ERROR but may be used in future implementations.
        DoesNotExist                        = 1,           //Parameter does not exist
        ValueOutOfRange                     = 2,           //Parameter value does not fit within accepted range
        PermissionDenied                    = 3,           //Caller is not permitted to set the value of this parameter
        ComponentNotFound                   = 4,           //Unknown component specified
        ReadOnly                            = 5,           //Parameter is read-only
        TypeUnsupported                     = 6,           //Parameter data type (MAV_PARAM_TYPE) is not supported by flight stack (at all)
        TypeMismatch                        = 7,           //Parameter type does not match expected type
        ReadFail                            = 8            //Parameter exists but reading failed
    }

}
