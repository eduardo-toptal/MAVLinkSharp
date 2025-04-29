        
namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// Indicates the severity level, generally used for status messages to indicate their relative urgency. Based on RFC-5424 using expanded definitions at: http://www.kiwisyslog.com/kb/info:-syslog-message-levels/.
    /// </summary>    
    public enum MAVSeverityFlags {
        Emergency              = 0,           //System is unusable. This is a "panic" condition.
        Alert                  = 1,           //Action should be taken immediately. Indicates error in non-critical systems.
        Critical               = 2,           //Action must be taken immediately. Indicates failure in a primary system.
        Error                  = 3,           //Indicates an error in secondary/redundant systems.
        Warning                = 4,           //Indicates about a possible future error if this is not resolved within a given timeframe. Example would be a low battery warning.
        Notice                 = 5,           //An unusual event has occurred, though not an error condition. This should be investigated for the root cause.
        Info                   = 6,           //Normal operational messages. Useful for logging. No action is required for these messages.
        Debug                  = 7            //Useful non-operational messages that can assist in debugging. These should not occur during normal operation.
    }

}
