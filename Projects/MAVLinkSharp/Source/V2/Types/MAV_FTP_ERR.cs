        
namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// MAV FTP error codes (https://mavlink.io/en/services/ftp.html)
    /// </summary>    
    public enum MAVFtpErrFlags {
        None                            = 0,           //None: No error
        Fail                            = 1,           //Fail: Unknown failure
        Failerrno                       = 2,           //FailErrno: Command failed, Err number sent back in PayloadHeader.data[1]. | This is a file-system error number understood by the server operating system.
        Invaliddatasize                 = 3,           //InvalidDataSize: Payload size is invalid
        Invalidsession                  = 4,           //InvalidSession: Session is not currently open
        Nosessionsavailable             = 5,           //NoSessionsAvailable: All available sessions are already in use
        Eof                             = 6,           //EOF: Offset past end of file for ListDirectory and ReadFile commands
        Unknowncommand                  = 7,           //UnknownCommand: Unknown command / opcode
        Fileexists                      = 8,           //FileExists: File/directory already exists
        Fileprotected                   = 9,           //FileProtected: File/directory is write protected
        Filenotfound                    = 10           //FileNotFound: File/directory not found
    }

}
