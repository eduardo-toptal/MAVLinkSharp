namespace MAVLinkSharp.Runtime {

    /// <summary>
    /// Interface that implements IO for MAVLink Message Data Structures
    /// </summary>    
    public interface IMAVLinkMessageData {

        /// <summary>
        /// Returns this Data MSG_ID
        /// </summary>
        /// <returns></returns>
        int GetId();

        /// <summary>
        /// Copy the messages content into the buffer
        /// </summary>
        /// <param name="ss"></param>
        int Write(byte[] p_buffer,int p_offset=0);

        /// <summary>
        /// Reads the message data into the struct.
        /// </summary>
        /// <param name="ss"></param>
        int Read(byte[] p_buffer,int p_offset=0);

    }
}
