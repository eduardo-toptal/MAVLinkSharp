        
namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// SERIAL_CONTROL flags (bitmask)
    /// </summary>    
    public enum SerialControlFlag {
        Reply                         = 1,           //Set if this is a reply
        Respond                       = 2,           //Set if the sender wants the receiver to send a response as another SERIAL_CONTROL message
        Exclusive                     = 4,           //Set if access to the serial port should be removed from whatever driver is currently using it, giving exclusive access to the SERIAL_CONTROL protocol. The port can be handed back by sending a request without this flag set
        Blocking                      = 8,           //Block on writes to the serial port
        Multi                         = 16           //Send multiple replies until port is drained
    }

}
