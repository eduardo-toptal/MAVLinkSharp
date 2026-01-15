        
namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// RC sub-type of types defined in RC_TYPE. Used in MAV_CMD_START_RX_PAIR. Ignored if value does not correspond to the set RC_TYPE.
    /// </summary>    
    public enum RcSubTypeFlags {
        SpektrumDsm2               = 0,           //Spektrum DSM2
        SpektrumDsmx               = 1,           //Spektrum DSMX
        SpektrumDsmx8              = 2            //Spektrum DSMX8
    }

}
