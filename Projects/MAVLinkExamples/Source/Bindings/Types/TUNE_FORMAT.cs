        
namespace MAVLinkBindings {

    /// <summary>
    /// Tune formats (used for vehicle buzzer/tone generation).
    /// </summary>    
    public enum TuneFormatFlags {
        Qbasic11               = 1,           //Format is QBasic 1.1 Play: https://www.qbasic.net/en/reference/qb11/Statement/PLAY-006.htm.
        MmlModern              = 2            //Format is Modern Music Markup Language (MML): https://en.wikipedia.org/wiki/Music_Macro_Language#Modern_MML.
    }

}
