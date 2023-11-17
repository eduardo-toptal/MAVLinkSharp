using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Ports;

#pragma warning disable CS8600
#pragma warning disable CS8602
#pragma warning disable CS8603
#pragma warning disable CS8618
#pragma warning disable CS8625

namespace MAVConsole {

    /// <summary>
    /// Class that implements a SerialPort based interface to sync messages over wire.
    /// </summary>
    public class MAVLinkSerial : MAVLinkInterface {

        /// <summary>
        /// Reference to the client
        /// </summary>
        public SerialPort serial { get; set; }

        /// <summary>
        /// Internals
        /// </summary>
        private byte[]    m_rcv_buff;
        private Task<int> m_rcv_tsk;
        
        /// <summary>
        /// CTOR.
        /// </summary>
        /// <param name="p_name"></param>
        public MAVLinkSerial(SerialPort p_serial,string p_name = "") : this(p_name) {
            if (p_serial == null) throw new ArgumentNullException();
            serial = p_serial;
        }

        /// <summary>
        /// CTOR.
        /// </summary>
        /// <param name="p_name"></param>
        public MAVLinkSerial(string p_name = "") : base(p_name) {
            m_rcv_buff = new byte[65 * 1024];
        }

        /// <summary>
        /// Flushes the data into the clien't buffer
        /// </summary>
        /// <param name="p_data"></param>
        protected override void OnDataSend(byte[] p_data) {
            if(serial == null) return;
            if(!serial.IsOpen) return;
            Stream ss = serial.BaseStream;
            try {
                if(ss!=null)ss.Write(p_data);
            } catch (Exception) { }            
        }

        /// <summary>
        /// Handles the UDP client network logixc
        /// </summary>
        override protected void OnUpdate() {
            //Check if ther is a valid serial port
            bool has_serial = serial != null;
            //Client is available then we can start syncing data
            if(has_serial) {
                //Skip if not open yet
                if (!serial.IsOpen) return;
                //Check if there is any receiving task ongoing
                Task<int> tsk = m_rcv_tsk;
                bool is_read = tsk != null;
                if(is_read) {
                    switch(tsk.Status) {
                        case TaskStatus.Canceled:
                        case TaskStatus.Faulted: {
                            //Invalidate if error and try again
                            m_rcv_tsk = null;
                        }
                        break;

                        case TaskStatus.RanToCompletion: {
                            //Fetch the data and pipe it thru the stream
                            int    c = tsk.Result;
                            byte[] b = m_rcv_buff;
                            OnDataReceive(b,0,c);
                            m_rcv_tsk=null;
                        }
                        break;
                    }
                }
                else {
                    //Fetch the active stream
                    Stream ss = serial.BaseStream;
                    try {
                        //Start the receive task
                        m_rcv_tsk = ss.ReadAsync(m_rcv_buff,0,m_rcv_buff.Length);
                    } catch (Exception) { }
                }                
            }
           
        }
            
        }

    
}
