        
namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// Video stream types
    /// </summary>    
    public enum VideoStreamTypeFlags {
        Rtsp                       = 0,           //Stream is RTSP
        Rtpudp                     = 1,           //Stream is RTP UDP (URI gives the port number)
        TcpMpeg                    = 2,           //Stream is MPEG on TCP
        MpegTs                     = 3            //Stream is MPEG TS (URI gives the port number)
    }

}
