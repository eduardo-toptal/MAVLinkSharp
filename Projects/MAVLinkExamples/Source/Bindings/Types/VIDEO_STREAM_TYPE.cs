        
namespace MAVLinkBindings {

    /// <summary>
    /// Video stream types
    /// </summary>    
    public enum VideoStreamTypeFlags {
        Rtsp                           = 0,           //Stream is RTSP
        Rtpudp                         = 1,           //Stream is RTP UDP (URI gives the port number)
        TcpMpeg                        = 2,           //Stream is MPEG on TCP
        MpegTsH264                     = 3            //Stream is h.264 on MPEG TS (URI gives the port number)
    }

}
