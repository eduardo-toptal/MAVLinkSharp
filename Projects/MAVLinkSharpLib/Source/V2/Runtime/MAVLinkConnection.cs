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
        /// Internals
        /// </summary>
        protected MemoryStream m_snd_ms;
        protected MemoryStream m_rcv_ms;
        protected MAVLinkReader m_rcv;
        protected MAVLinkWriter m_snd;
        protected bool m_rcv_active;
        protected bool m_snd_active;
        protected byte m_snd_seq;
        protected object m_send_seq_lock;
        private Thread m_rcv_thd;
        private Thread m_snd_thd;        
        private ManualResetEvent m_snd_signal;

        /// <summary>
        /// CTOR
        /// </summary>
        public MAVLinkConnection(string p_name="") : base(p_name) {
            m_snd_ms = new MemoryStream();
            m_rcv_ms = new MemoryStream();
            m_rcv    = new MAVLinkReader(m_rcv_ms);
            m_snd    = new MAVLinkWriter(m_snd_ms);
            m_rcv_active = true;
            m_snd_active = true;
            m_snd_seq    = 0;
            m_send_seq_lock = new object();

            m_snd_signal = new ManualResetEvent(false);

            m_rcv_thd = new Thread(InternalReadLoop);
            m_snd_thd = new Thread(InternalWriteLoop);
            m_rcv_thd.Name = $"MAVLINK_{name.ToUpper()}.RCV";
            m_snd_thd.Name = $"MAVLINK_{name.ToUpper()}.SND";
            m_rcv_thd.Start();
            m_snd_thd.Start();

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
        public void Send(MAVLinkMsg p_msg) {            
            lock(m_send_seq_lock) p_msg.sequence = m_snd_seq++;
            InternalSend(p_msg);
        }

        /// <summary>
        /// Handles message actual delivery
        /// </summary>
        /// <param name="p_msg"></param>
        internal void InternalSend(MAVLinkMsg p_msg) {                        
            lock(m_snd_ms) {
                m_snd.WriteV2(p_msg);                
            }
            try { m_snd_signal.Set(); } catch(System.Exception){ }
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
        /// Handler for when a data packet arrived at the link
        /// </summary>
        /// <param name="p_packet"></param>
        virtual protected void OnPacketReceive(out byte[]? p_buffer,out int p_length) {  p_buffer = null; p_length = 0; }

        /// <summary>
        /// Handler for sending data packets thru the link.
        /// </summary>
        /// <param name="p_packet"></param>
        /// <param name="p_length"></param>
        virtual protected void OnPacketSend(byte[] p_packet,int p_length) { }

        /// <summary>
        /// Waits for packets and process incoming messages
        /// </summary>
        /// <param name="so"></param>
        protected void InternalReadLoop(object? so) {
            
            while(m_rcv_active) {
                byte[]? d = null; 
                int len = 0;
                OnPacketReceive(out d,out len);                
                MemoryStream ms = m_rcv_ms;
                if(ms.CanWrite) {
                    ms.SetLength(0);
                    if(len>0) if(d!=null) ms.Write(d,0,len);                
                    ms.Position = 0;
                }                
                bool will_read  = len>0;
                //bool is_success = false;
                while(will_read) {   
                    if(ms.Position>=ms.Length) break;
                    MAVLinkMsg msg = MAVLinkMsg.GetPool();
                    MAVLinkParseResult res = MAVLinkParseResult.Unknown;
                    res = m_rcv.Read(ref msg);                    
                    switch(res) {
                        case MAVLinkParseResult.Success:    Dispatch(msg); if(OnMessageReceived!=null) OnMessageReceived(msg); /*is_success=true;*/ break;
                        case MAVLinkParseResult.NotFound: 
                        case MAVLinkParseResult.Incomplete: will_read=false; break;
                        case MAVLinkParseResult.BadCRC:     break;
                    }                    
                    MAVLinkMsg.SetPool(msg);
                }                    
                Thread.Yield();                
            }
        }

        /// <summary>
        /// Wait for buffered sent messages and process submissions
        /// </summary>
        /// <param name="so"></param>
        protected void InternalWriteLoop(object? so) {  
            byte[] b;
            int    b_len;
            MemoryStream ms = m_snd_ms;
            while(m_snd_active) {                   
                m_snd_signal.WaitOne(10);
                lock(ms) {                 
                    b = ms.GetBuffer();
                    b_len = (int)ms.Position;
                    if(b_len>0) { 
                        OnPacketSend(b,b_len);                        
                        ms.Position=0;
                    }
                }                      
                m_snd_signal.Reset();                
            }            
        }        
        
        /// <summary>
        /// DTOR
        /// </summary>
        protected override void OnDispose() {            
            m_rcv_active = false;
            m_snd_active = false;
            if(m_rcv_thd!=null) if(!m_rcv_thd.Join(48)) m_rcv_thd.Abort();
            if(m_snd_thd!=null) if(!m_snd_thd.Join(48)) m_snd_thd.Abort();
            m_rcv_thd = null;
            m_snd_thd = null;
            m_snd_seq = 0;
            m_rcv_ms.Dispose();
            m_snd_ms.Dispose();
            m_snd_signal.Dispose();
        }

    }
}
