        
namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// MAV FTP opcodes: https://mavlink.io/en/services/ftp.html
    /// </summary>    
    public enum MAVFtpOpcodeFlags {
        None                            = 0,           //None. Ignored, always ACKed
        Terminatesession                = 1,           //TerminateSession: Terminates open Read session
        Resetsession                    = 2,           //ResetSessions: Terminates all open read sessions
        Listdirectory                   = 3,           //ListDirectory. List files and directories in path from offset
        Openfilero                      = 4,           //OpenFileRO: Opens file at path for reading, returns session
        Readfile                        = 5,           //ReadFile: Reads size bytes from offset in session
        Createfile                      = 6,           //CreateFile: Creates file at path for writing, returns session
        Writefile                       = 7,           //WriteFile: Writes size bytes to offset in session
        Removefile                      = 8,           //RemoveFile: Remove file at path
        Createdirectory                 = 9,           //CreateDirectory: Creates directory at path
        Removedirectory                 = 10,          //RemoveDirectory: Removes directory at path. The directory must be empty.
        Openfilewo                      = 11,          //OpenFileWO: Opens file at path for writing, returns session
        Truncatefile                    = 12,          //TruncateFile: Truncate file at path to offset length
        Rename                          = 13,          //Rename: Rename path1 to path2
        Calcfilecrc                     = 14,          //CalcFileCRC32: Calculate CRC32 for file at path
        Burstreadfile                   = 15,          //BurstReadFile: Burst download session file
        Ack                             = 128,         //ACK: ACK response
        Nak                             = 129          //NAK: NAK response
    }

}
