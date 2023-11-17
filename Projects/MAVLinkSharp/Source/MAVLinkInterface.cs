using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MAVLink;

namespace MAVLinkSharp {

    /// <summary>
    /// Class that describes a MAVLink interface which will stream in and out data over any kind of medium like Serial or Network.
    /// Every message processed will be sent over the entities graph
    /// </summary>
    public class MAVLinkInterface : MAVLinkEntity {

        /// <summary>
        /// Reference to the sender stream
        /// </summary>
        public MAVLinkStream sender { get; set; }

        /// <summary>
        /// Reference to the receiver stream
        /// </summary>
        public MAVLinkStream receiver { get; set; }

        /// <summary>
        /// CTOR.
        /// </summary>
        /// <param name="p_name"></param>
        public MAVLinkInterface(string p_name="") : base(p_name) {
            sender   = new MAVLinkStream();
            receiver = new MAVLinkStream();
            syncRate = 5;
        }

        /// <summary>
        /// Handler for flushing buffered message data
        /// </summary>
        virtual protected void OnDataSend(byte[] p_data) { }

        /// <summary>
        /// Handler for incoming data
        /// </summary>
        /// <param name="p_data"></param>
        protected void OnDataReceive(byte[] p_data,int p_offset,int p_length) {
            if(enabled)
            if (receiver != null) {
                receiver.Write(p_data,p_offset,p_length);
            }
        }  

        /// <summary>
        /// Handler for data updating loop
        /// </summary>
        virtual protected void OnDataUpdate() { }

        /// <summary>
        /// Handles incoming messages and forward them
        /// </summary>
        /// <param name="p_msg"></param>
        override protected void OnMessage(MAVLinkEntity p_caller,MAVLinkMessage p_msg) {            
            if (sender != null) {
                MSG_ID msg_id = (MSG_ID)p_msg.msgid;
                switch(msg_id) {
                    case MSG_ID.PARAM_VALUE:
                    case MSG_ID.PARAM_EXT_VALUE:
                    case MSG_ID.PARAM_REQUEST_READ:
                    case MSG_ID.PARAM_EXT_REQUEST_READ:
                    case MSG_ID.PARAM_EXT_REQUEST_LIST:
                    case MSG_ID.PARAM_REQUEST_LIST: {
                        //Console.WriteLine($">>> {msg_id}");
                    }
                    break;
                }                
                sender.Write(p_msg);                
            }
        }

        /// <summary>
        /// Handles messages coming from stream and relay to the entity graph.
        /// Also handles data buffering
        /// </summary>
        override protected void OnUpdate() {
            //Updates data handling loops
            OnDataUpdate();
            //Process messages
            if(receiver != null) {                
                MAVLinkMessage msg = receiver.ReadMessage();                
                if (msg != null) Send(msg);
            }
            //Check if sender has any pending data and sends it emptying the stream
            if (sender != null) {                                     
                byte[] d = sender.Read();                
                if (d.Length>0) OnDataSend(d);                 
            }
        }

    }
}
