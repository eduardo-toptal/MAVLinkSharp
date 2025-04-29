        
namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// Specifies the datatype of a MAVLink extended parameter.
    /// </summary>    
    public enum MAVParamExtTypeFlags {
        Uint8                     = 1,           //8-bit unsigned integer
        Int8                      = 2,           //8-bit signed integer
        Uint16                    = 3,           //16-bit unsigned integer
        Int16                     = 4,           //16-bit signed integer
        Uint32                    = 5,           //32-bit unsigned integer
        Int32                     = 6,           //32-bit signed integer
        Uint64                    = 7,           //64-bit unsigned integer
        Int64                     = 8,           //64-bit signed integer
        Real32                    = 9,           //32-bit floating-point
        Real64                    = 10,          //64-bit floating-point
        Custom                    = 11           //Custom Type
    }

}
