using System;
using System.IO;
using System.Threading;

#pragma warning disable CS8603
#pragma warning disable CS8632

namespace MAVLinkSharp.Runtime {

    /// <summary>
    /// Class that describes a generic node in a MAVLinkNetwork
    /// </summary>
    public class MAVLinkConnection : MAVLinkNode {

        /// <summary>
        /// Handler for message events.
        /// </summary>
        public Action<MAVLinkMsg>? OnMessageReceived;

        /// <summary>
        /// Reference to the data stream;
        /// </summary>
        public BaseDataStream Stream { get; protected set; }

        /// <summary>
        /// Handler for when a raw packet arrives
        /// </summary>
        public Action<byte[],int> OnPacketReceiveEvent;

        /// <summary>
        /// Handler for when a raw packet arrives
        /// </summary>
        public Action<MemoryStream> OnPacketSendEvent;

        protected MemoryStream m_snd_ms;
        protected MemoryStream m_rcv_ms;
        protected MAVLinkReader m_rcv;
        protected MAVLinkWriter m_snd;
        protected object m_send_seq_lock;
        protected byte m_snd_seq;

        /// <summary>
        /// CTOR
        /// </summary>
        public MAVLinkConnection(BaseDataStream p_stream,string p_name=null) : base(p_name ?? "") {

            Stream = p_stream;
            m_snd_ms = new MemoryStream();
            m_rcv_ms = new MemoryStream();
            m_rcv = new MAVLinkReader(m_rcv_ms);
            m_snd = new MAVLinkWriter(m_snd_ms);
            m_snd_seq = 0;
            m_send_seq_lock = new object();

            SetStream(p_stream);

        }

        /// <summary>
        /// Sets the data stream to be used by this connection
        /// </summary>
        /// <param name="p_stream"></param>
        protected void SetStream(BaseDataStream p_stream) {
            if(Stream!=null) { Stream.Dispose(); }
            Stream = null;
            if(p_stream == null) return;
            Stream = p_stream;
            Stream.OnDataReceiveEvent = OnDataReceive;
            Stream.OnDataSendEvent    = OnDataSend;
            Stream.Start();

        }

        /// <summary>
        /// Sends a message over this connection
        /// </summary>
        /// <param name="p_msg_id"></param>
        /// <param name="p_data"></param>
        /// <param name="p_sys_id"></param>
        /// <param name="p_comp_id"></param>
        public void Send(MAVLinkMsgId p_msg_id,IMAVLinkMessageData p_data,byte p_sys_id=1,byte p_comp_id=0) {
            MAVLinkMsg msg  = MAVLinkMsg.GetPool();
            msg.messageId   = p_msg_id;
            msg.systemId    = p_sys_id;
            msg.componentId = p_comp_id;
            msg.data        = p_data;                      
            Send(msg);
            MAVLinkMsg.SetPool(msg);
        }

        /// <summary>
        /// Sends a message over this connection
        /// </summary>
        /// <param name="p_data"></param>
        /// <param name="p_sys_id"></param>
        /// <param name="p_comp_id"></param>
        public void Send(IMAVLinkMessageData p_data,byte p_sys_id=1,byte p_comp_id=0) {
            if(p_data==null) return;
            Send((MAVLinkMsgId)p_data.GetId(),p_data,p_sys_id,p_comp_id);
        }

        /// <summary>
        /// Sends a MAVLink Message
        /// </summary>
        /// <param name="p_msg"></param>
        public void Send(MAVLinkMsg p_msg, bool p_sequence = true) {            
            if(p_sequence) lock(m_send_seq_lock) p_msg.sequence = m_snd_seq++;
            InternalSend(p_msg);
        }

        /// <summary>
        /// Handles message actual delivery
        /// </summary>
        /// <param name="p_msg"></param>
        internal void InternalSend(MAVLinkMsg p_msg) {
            if(m_snd_ms == null) return;
            lock(m_snd_ms) {
                m_snd_ms.SetLength(0);
                m_snd_ms.Position = 0;
                m_snd.WriteV2(p_msg);                
                m_snd_ms.Position = 0;
                if(Stream != null) Stream.Send(m_snd_ms);
                m_snd_ms.Position = 0;
            }            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p_packet"></param>
        /// <param name="p_length"></param>
        public void SendPacket(byte[] p_packet,int p_length = -1) {
            if(Stream != null)
                Stream.Send(p_packet,p_length < 0 ? p_packet.Length : p_length);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p_data"></param>
        /// <param name="p_length"></param>
        protected void OnDataSend(byte[] p_data, int p_length) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p_data"></param>
        /// <param name="p_length"></param>
        protected void OnDataReceive(byte[] p_data,int p_length) {
            if(m_rcv_ms == null) return;
            MemoryStream ms = m_rcv_ms;
            byte[] d   = p_data;
            int    len = p_length;
            if(ms.CanWrite) {
                ms.SetLength(0);
                if(len>0) if(d!=null) ms.Write(d,0,len);                
                ms.Position = 0;
                if(len>0) if (OnPacketReceiveEvent != null) OnPacketReceiveEvent(d, len);
            }                
            bool will_read  = len>0;
            //bool is_success = false;
            while(will_read) {   
                if(ms.Position>=ms.Length) break;
                MAVLinkMsg msg = MAVLinkMsg.GetPool();
                MAVLinkParseResult res = MAVLinkParseResult.Unknown;
                res = m_rcv.Read(ref msg);                    
                switch(res) {
                    case MAVLinkParseResult.Success:    
                        Dispatch(msg); 
                        if(OnMessageReceived!=null) OnMessageReceived(msg); 
                        //is_success=true;
                    break;
                    case MAVLinkParseResult.NotFound: 
                    case MAVLinkParseResult.Incomplete: will_read=false; break;
                    case MAVLinkParseResult.BadCRC:     break;
                }                    
                MAVLinkMsg.SetPool(msg);
            }                  
        }

        /// <summary>
        /// Sends the message thru the connection link
        /// </summary>
        /// <param name="p_sender"></param>
        /// <param name="p_msg"></param>
        protected override void OnMessage(MAVLinkNode p_sender, MAVLinkMsg p_msg) {
            //Externally originated messages are just relayed
            InternalSend(p_msg);
        }

        /// <summary>
        /// DTOR
        /// </summary>
        protected override void OnDispose() {            
            if(Stream!=null) Stream.Dispose();
            m_snd_seq = 0;
            m_rcv_ms.Dispose();
            m_snd_ms.Dispose();
            m_snd_ms = null;
            m_rcv_ms = null;
        }

    }
}
