        
namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// ACK / NACK / ERROR values as a result of MAV_CMDs and for mission item transmission.
    /// </summary>    
    public enum MAVCmdAckFlags {
        Ok                                             = 0,           //Command / mission item is ok.
        ErrFail                                        = 1,           //Generic error message if none of the other reasons fails or if no detailed error reporting is implemented.
        ErrAccessDenied                                = 2,           //The system is refusing to accept this command from this source / communication partner.
        ErrNotSupported                                = 3,           //Command or mission item is not supported, other commands would be accepted.
        ErrCoordinateFrameNotSupported                 = 4,           //The coordinate frame of this command / mission item is not supported.
        ErrCoordinatesOutOfRange                       = 5,           //The coordinate frame of this command is ok, but he coordinate values exceed the safety limits of this system. This is a generic error, please use the more specific error messages below if possible.
        ErrXLatOutOfRange                              = 6,           //The X or latitude value is out of range.
        ErrYLonOutOfRange                              = 7,           //The Y or longitude value is out of range.
        ErrZAltOutOfRange                              = 8            //The Z or altitude value is out of range.
    }

}
