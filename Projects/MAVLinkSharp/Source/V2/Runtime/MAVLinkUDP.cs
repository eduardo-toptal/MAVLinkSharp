using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable CS8603
#pragma warning disable CS0168
#pragma warning disable CS8632

namespace MAVLinkSharp.Runtime {

    /// <summary>
    /// Class that describes an UDP MAVLinkConnection
    /// </summary>
    public class MAVLinkUDP : MAVLinkConnection {

        /// <summary>
        /// Internals
        /// </summary>
        private UdpClient? m_conn;
        private List<IPEndPoint> m_targets;
        private IPEndPoint m_rcv_ep;

        /// <summary>
        /// CTOR
        /// </summary>
        public MAVLinkUDP(string p_name="") : base(p_name) {            
            m_targets = new List<IPEndPoint>();
            m_rcv_ep  = new IPEndPoint(IPAddress.Parse("0.0.0.0"),0);
        }

        /// <summary>
        /// Starts listening into the local port.
        /// </summary>
        /// <param name="p_port"></param>
        public void Start(int p_port=0) {
            if(m_conn!=null) { 
                try { m_conn.Close(); } catch(System.Exception){ }
                m_conn = null;
            }
            try { m_conn = p_port<=0 ? new UdpClient() : new UdpClient(p_port); } catch(System.Exception) { }
        }

        /// <summary>
        /// Sets the target endpoints to send
        /// </summary>
        /// <param name="p_targets"></param>
        public void SetTargets(params IPEndPoint[] p_targets) {
            m_targets = new List<IPEndPoint>(p_targets);
        }

        /// <summary>
        /// Handler for sending data packets thru the link.
        /// </summary>
        /// <param name="p_packet"></param>
        /// <param name="p_length"></param>
        override protected void OnPacketSend(byte[] p_packet,int p_length) {
            if(m_conn==null) return;
            for(int i=0;i<m_targets.Count;i++) {
                IPEndPoint ep = m_targets[i];
                try { m_conn.Send(p_packet,p_length, ep); } catch(System.Exception) { }
            }            
        }

        /// <summary>
        /// Handler for byte data received.
        /// </summary>
        /// <param name="p_buffer"></param>
        /// <param name="p_length"></param>
        protected override void OnPacketReceive(out byte[]? p_buffer,out int p_length) {
            byte[]? d = null;
            p_buffer = d;
            p_length = 0;
            if(m_conn==null) return;
            try { d = m_conn.Receive(ref m_rcv_ep); } catch(System.Exception p_err) { /*Console.WriteLine($"[{name}] RCV Err {p_err.Message}");*/ }   
            p_buffer = d;
            p_length = d==null ? 0 : d.Length;
        }

        /// <summary>
        /// DTOR
        /// </summary>
        protected override void OnDispose() {
            base.OnDispose();
            if (m_conn!=null) m_conn.Dispose();
        }

    }
}
