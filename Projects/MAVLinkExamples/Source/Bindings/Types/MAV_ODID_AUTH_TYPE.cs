        
namespace MAVLinkBindings {

    /// <summary>
    /// 
    /// </summary>    
    public enum MAVOdidAuthTypeFlags {
        None                                       = 0,           //No authentication type is specified.
        UasIdSignature                             = 1,           //Signature for the UAS (Unmanned Aircraft System) ID.
        OperatorIdSignature                        = 2,           //Signature for the Operator ID.
        MessageSetSignature                        = 3,           //Signature for the entire message set.
        NetworkRemoteId                            = 4,           //Authentication is provided by Network Remote ID.
        SpecificAuthentication                     = 5            //The exact authentication type is indicated by the first byte of authentication_data and these type values are managed by ICAO.
    }

}
