using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using static MAVLink;

#pragma warning disable CS8600
#pragma warning disable CS8602
#pragma warning disable CS8603

namespace MAVLinkSharp {

    /// <summary>
    /// Class that implements MAVLink messaging provided by Streams in general
    /// </summary>    
    public class MAVLinkStream {

        /// <summary>
        /// Flag that tells if the stream is configured correctly.
        /// </summary>
        public bool valid {
            get {
                Stream ss = m_stream;
                if(ss == null) return false;                
                if (!ss.CanSeek)  return false;
                if (!ss.CanRead)  return false;
                if (!ss.CanWrite) return false;
                return true;
            }
        }

        /// <summary>
        /// Returns the base stream running within this MAVLinkStream
        /// </summary>
        public Stream BaseStream {
            get { return m_stream; }
        }

        /// <summary>
        /// Internals
        /// </summary>
        private bool m_is_file;
        private Stream m_stream;
        internal Stream m_buffer;
        private byte[] m_buffer_arr;
        private Stream m_copy;
        private MavlinkParse m_parser;
        private object m_buffer_lock;

        /// <summary>
        /// CTOR.
        /// </summary>
        /// <param name="p_capacity"></param>
        public MAVLinkStream(int p_capacity = 65*1024) {
            m_stream  = new MemoryStream(p_capacity);
            m_copy    = new MemoryStream(p_capacity);
            m_buffer_arr = new byte[p_capacity];
            m_buffer  = new MemoryStream(p_capacity);
            m_is_file = false;
            m_parser  = new MavlinkParse(false);
            m_buffer_lock = new object();
        }

        /// <summary>
        /// CTOR.
        /// </summary>
        /// <param name="p_stream"></param>
        public MAVLinkStream(Stream p_stream) {
            m_stream = p_stream;
            if(m_stream == null) { throw new NullReferenceException("Stream can't be null"); }
            bool is_valid = true;
            if(!m_stream.CanSeek)  is_valid = false;
            if(!m_stream.CanRead)  is_valid = false;
            if(!m_stream.CanWrite) is_valid = false;
            if(is_valid) { throw new InvalidDataException("Stream must be able to Seek/Read/Write"); }
            m_copy    = new MemoryStream(65*1024);
            m_buffer_arr = new byte[65*1024];
            m_buffer = new MemoryStream(65 * 1024);
            m_is_file = m_stream is FileStream; 
            m_parser  = new MavlinkParse(false);
            m_buffer_lock = new object();
        }

        /// <summary>
        /// Flushes any buffered data to make it ready for reading
        /// </summary>
        public void Flush() {
            //Local Streams
            Stream sb = m_buffer;
            Stream ss = m_stream;
            bool is_valid = (sb.CanRead && sb.CanWrite && sb.CanSeek) && (ss.CanSeek && ss.CanRead && ss.CanWrite);
            if (!is_valid) return;
            //Skip if empty
            if (sb.Length <= 0) return;
            //Transfer buffered data into 'stream'
            lock (m_buffer_lock) {
                //Write stream
                sb.Position = 0;
                sb.CopyTo(ss);
                //Reset length
                sb.SetLength(0);
            }
            //If file force flushing into disk
            FileStream fs = m_is_file ? ss as FileStream : null;
            if (m_is_file) fs.Flush(true);
        }

        /// <summary>
        /// Writes byte chunks for message decoding.
        /// </summary>
        /// <param name="p_data"></param>
        /// <param name="p_offset"></param>
        /// <param name="p_length"></param>
        public void Write(byte[] p_data, int p_offset, int p_length) {
            if (!valid) return;
            if (p_data == null)  return;
            if(p_data.Length<=0) return;
            if(p_length<=0)      return;             
            //Thread-safe buffering data
            lock(m_buffer_lock) {
                m_buffer.Write(p_data,p_offset,p_length);
            }            
        } 

        /// <summary>
        /// Writes byte chunks for message decoding.
        /// </summary>
        /// <param name="p_data"></param>
        public void Write(byte[] p_data) {
            if(p_data == null) return;
            Write(p_data,0,p_data.Length);
        }

        /// <summary>
        /// Writes the content of the stream for later decoding.
        /// </summary>
        /// <param name="p_stream"></param>
        public void Write(Stream p_stream) {
            if(p_stream == null) return;
            Stream s   = p_stream;
            int    len = 0;
            bool is_ns = s is NetworkStream;
            //If network stream safeguard exceptions when connections fail
            if(is_ns) {
                try {
                    len = s.Read(m_buffer_arr,0,m_buffer_arr.Length);
                }
                catch(Exception) {
                    len = 0;
                }
            }
            //Regular stream usage
            else {
                len = s.CanRead ? s.Read(m_buffer_arr,0,m_buffer_arr.Length) : 0;
            }            
            if (len <= 0) return;
            Write(m_buffer_arr,0,len);
        }



        /// <summary>
        /// Writes a msg in the stream
        /// </summary>
        /// <param name="p_msg_type"></param>
        /// <param name="p_msg_data"></param>
        /// <param name="p_sign"></param>
        /// <param name="p_system_id"></param>
        /// <param name="p_component_id"></param>
        /// <param name="p_sequence"></param>
        public void Write(MSG_ID p_msg_type,object p_msg_data,bool p_sign = false,byte p_system_id = 255,byte p_component_id = (byte)MAV_COMPONENT.MAV_COMP_ID_AUTOPILOT1,int p_sequence = -1) {
            byte[] b = m_parser.GenerateMAVLinkPacket20(p_msg_type,p_msg_data,p_sign,p_system_id,p_component_id,p_sequence);
            Write(b);
        }

        /// <summary>
        /// Writes a mavlink msg instance in the stream
        /// </summary>
        /// <param name="p_msg"></param>
        public void Write(MAVLinkMessage p_msg) {
            MAVLinkMessage m = p_msg;
            if (m == null) return;
            byte[] b = m.buffer;
            Write(b);            
        }

        /// <summary>
        /// Consumes the data available in the stream and returns the next buffered message.
        /// </summary>
        /// <returns></returns>
        public MAVLinkMessage ReadMessage() {
            //Message Result
            MAVLinkMessage msg = null;
            //Local Streams            
            Stream   ss  = m_stream;
            Stream   cp  = m_copy;
            //Flushes any buffered data
            Flush(); 
            //Loop the 'stream' and extract the next message
            while (true) {
                bool is_valid = ss.CanSeek && ss.CanRead && ss.CanWrite;
                if (!is_valid) break;
                //Skip if stream empty
                if (ss.Length <= 0) break;
                //Store the current position for backtracking
                long p = ss.Position;                
                //Position offset for entrypoint sliding and try to find working message parsing
                long off = 0;
                //Loop read the stream
                while(true) {
                    //Start stream at the current 'offset'
                    ss.Position = off;
                    //Use try-catch to asses valid or invalid messages (ugly)
                    try {
                        msg = m_parser.ReadPacket(ss);
                    }
                    catch(System.Exception) {
                        //Ignore exceptions and 'null' msg
                        //int i = 0;
                    }
                    //If msg is somewhat valid exit and continue
                    if (msg != null) break;
                    //Increment offset to try reading at different location
                    off++;
                    //If offset exceeds the 'length' then nothing to read and 'null' msg it is
                    if (off >= ss.Length) {                                                
                        //Exit the loop
                        break;
                    }
                    //Try reading again
                    continue;
                }
                //If still 'null' needs more data
                if(msg==null) {
                    //Reset position and exit to try again after buffering more
                    ss.Position = p;
                    break;
                }                                
                //If msg is valid we need to "consume" the data we read
                //Reset the 'copy' stream
                cp.Position = 0;
                cp.SetLength(0);
                //Copy the remaining data into 'copy'
                ss.CopyTo(cp);
                //Reset 'copy' to start
                cp.Position = 0;
                //Reset the main stream
                ss.SetLength(0);
                //Copy the original remaining data into main stream
                cp.CopyTo(ss);
                //ss.Position == Length so ok to keep writing
                //Exit and return the msg
                break;
            }
            //Returns the resulting msg
            return msg;
        }

        /// <summary>
        /// Reads the whole stream as byte array
        /// </summary>
        /// <returns></returns>
        public byte[] Read() {
            Flush();
            long len = valid ? m_stream.Length : 0;
            byte[] b = len<=0 ? m_empty_buff : new byte[len];
            if (len <= 0) return b;            
            m_stream.Position = 0;
            m_stream.Read(b,0,(int)len);
            m_stream.SetLength(0);
            return b;
        }
        static byte[] m_empty_buff = new byte[0];

    }
}
