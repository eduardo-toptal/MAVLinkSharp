        
namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// 
    /// </summary>    
    public enum MAVTunnelPayloadTypeFlags {
        Unknown                                           = 0,           //Encoding of payload unknown.
        Storm32Reserved0                                  = 200,         //Registered for STorM32 gimbal controller.
        Storm32Reserved1                                  = 201,         //Registered for STorM32 gimbal controller.
        Storm32Reserved2                                  = 202,         //Registered for STorM32 gimbal controller.
        Storm32Reserved3                                  = 203,         //Registered for STorM32 gimbal controller.
        Storm32Reserved4                                  = 204,         //Registered for STorM32 gimbal controller.
        Storm32Reserved5                                  = 205,         //Registered for STorM32 gimbal controller.
        Storm32Reserved6                                  = 206,         //Registered for STorM32 gimbal controller.
        Storm32Reserved7                                  = 207,         //Registered for STorM32 gimbal controller.
        Storm32Reserved8                                  = 208,         //Registered for STorM32 gimbal controller.
        Storm32Reserved9                                  = 209,         //Registered for STorM32 gimbal controller.
        ModalaiRemoteOsd                                  = 210,         //Registered for ModalAI remote OSD protocol.
        ModalaiEscUartPassthru                            = 211,         //Registered for ModalAI ESC UART passthru protocol.
        ModalaiIoUartPassthru                             = 212          //Registered for ModalAI vendor use.
    }

}
