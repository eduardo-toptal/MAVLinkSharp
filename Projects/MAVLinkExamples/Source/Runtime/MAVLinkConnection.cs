using MAVLinkBindings;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable CS8603

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
            ThreadPool.QueueUserWorkItem(InternalReadLoop);
            ThreadPool.QueueUserWorkItem(InternalWriteLoop);
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
            lock(m_snd_ms) {
                m_snd.WriteV2(p_msg);
            }
        }

        /// <summary>
        /// Sends the message thru the connection link
        /// </summary>
        /// <param name="p_sender"></param>
        /// <param name="p_msg"></param>
        protected override void OnMessage(MAVLinkNode p_sender, MAVLinkMsg p_msg) {
            Send(p_msg);
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
                //if(len>0) Console.WriteLine($"[{name}] RCV {len} bytes");
                MemoryStream ms = m_rcv_ms;
                ms.SetLength(0);
                if(len>0) if(d!=null) m_rcv_ms.Write(d,0,len);                
                ms.Position = 0;
                bool will_read  = len>0;
                bool is_success = false;
                while(will_read) {                    
                    MAVLinkMsg msg = MAVLinkMsg.GetPool();
                    MAVLinkParseResult res = MAVLinkParseResult.Unknown;
                    res = m_rcv.Read(ref msg);                    
                    switch(res) {
                        case MAVLinkParseResult.Success:    Dispatch(msg); if(OnMessageReceived!=null) OnMessageReceived(msg); is_success=true; break;
                        case MAVLinkParseResult.NotFound: 
                        case MAVLinkParseResult.Incomplete: will_read=false; break;
                        case MAVLinkParseResult.BadCRC:     break;
                    }                                                    
                    MAVLinkMsg.SetPool(msg);
                }                    
                if(!is_success) Thread.Sleep(10);
                
            }
        }

        /// <summary>
        /// Wait for buffered sent messages and process submissions
        /// </summary>
        /// <param name="so"></param>
        protected void InternalWriteLoop(object? so) {            
            while(m_snd_active) {                 
                MemoryStream ms = m_snd_ms;
                byte[] b     = ms.GetBuffer();
                int    b_len = 0;
                lock(ms) {                    
                    b_len = (int)ms.Position;
                    if(b_len>0) {
                        //if(b_len>0) Console.WriteLine($"[{name}] SND {b_len} bytes");
                        OnPacketSend(b,b_len);
                        ms.Position=0;
                        //if(name=="hil") { Console.WriteLine($">>> {(int)pfl.Elapsed.TotalMilliseconds}ms | pos: {b_len}"); pfl.Restart(); }                        
                    }                    
                }
                if(b_len<=0) Thread.Sleep(10);                
            }
        }        
        
        /// <summary>
        /// DTOR
        /// </summary>
        protected override void OnDispose() {
            m_rcv_ms.Dispose();
            m_snd_ms.Dispose();
            m_rcv_active = false;
            m_snd_active = false;
        }

    }
}
