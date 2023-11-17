using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static MAVLink;

#pragma warning disable CS8625

namespace MAVConsole {

    /// <summary>
    /// MAVLink Entity extension to support broadcasting heartbeat messages across the MAVLink network.
    /// </summary>
    public abstract class MAVLinkLiveEntity : MAVLinkEntity {

        /// <summary>
        /// Defines the heart beat rate in ms
        /// </summary>
        public int heartbeatRate;

        /// <summary>
        /// Flag that tells this entity is alive and will send heartbeats
        /// </summary>
        public bool alive;

        /// <summary>
        /// Live Entity system id (might owner or owned)
        /// </summary>
        internal byte systemId;

        /// <summary>
        /// Live Entity component id (might owner or owned)
        /// </summary>
        internal byte componentId;

        /// <summary>
        /// Current State of this live entity
        /// </summary>
        public MAV_STATE state;

        /// <summary>
        /// Entity Type
        /// </summary>
        public MAV_TYPE type { get; private set; }

        /// <summary>
        /// Elapsed timer for heartbeat
        /// </summary>
        private double m_hb_elapsed;

        /// <summary>
        /// CTOR.
        /// </summary>
        /// <param name="p_name"></param>
        public MAVLinkLiveEntity(MAV_TYPE p_type,string p_name="") : base(p_name) {
            heartbeatRate = 900;
            state = MAV_STATE.UNINIT;            
            type  = p_type;
            m_hb_elapsed = 9999;
        }

        #region Params
        /// <summary>
        /// Sends a PARAM_VALUE message
        /// </summary>
        /// <param name="p_id"></param>
        /// <param name="p_value"></param>
        public void SendParam(MAVLinkParam p_param,ushort p_index=0,ushort p_count=1) {
            PARAM_VALUE_MSG d = p_param.GetMessage(p_index,p_count);
            MAVLinkMessage msg = CreateMessage(MSG_ID.PARAM_VALUE,d,false,systemId,componentId);
            Send(msg);
        }
        public void SendParam(string p_id,byte   v,ushort p_index=0,ushort p_count=1) { MAVLinkParam p = new MAVLinkParam() { id = p_id }; p.SetByte   (v); SendParam(p,p_index,p_count); }
        public void SendParam(string p_id,sbyte  v,ushort p_index=0,ushort p_count=1) { MAVLinkParam p = new MAVLinkParam() { id = p_id }; p.SetSByte  (v); SendParam(p,p_index,p_count); }
        public void SendParam(string p_id,ushort v,ushort p_index=0,ushort p_count=1) { MAVLinkParam p = new MAVLinkParam() { id = p_id }; p.SetUShort (v); SendParam(p,p_index,p_count); }
        public void SendParam(string p_id,short  v,ushort p_index=0,ushort p_count=1) { MAVLinkParam p = new MAVLinkParam() { id = p_id }; p.SetShort  (v); SendParam(p,p_index,p_count); }
        public void SendParam(string p_id,uint   v,ushort p_index=0,ushort p_count=1) { MAVLinkParam p = new MAVLinkParam() { id = p_id }; p.SetUInt   (v); SendParam(p,p_index,p_count); }
        public void SendParam(string p_id,int    v,ushort p_index=0,ushort p_count=1) { MAVLinkParam p = new MAVLinkParam() { id = p_id }; p.SetInt    (v); SendParam(p,p_index,p_count); }
        public void SendParam(string p_id,ulong  v,ushort p_index=0,ushort p_count=1) { MAVLinkParam p = new MAVLinkParam() { id = p_id }; p.SetULong  (v); SendParam(p,p_index,p_count); }
        public void SendParam(string p_id,long   v,ushort p_index=0,ushort p_count=1) { MAVLinkParam p = new MAVLinkParam() { id = p_id }; p.SetLong   (v); SendParam(p,p_index,p_count); }
        public void SendParam(string p_id,float  v,ushort p_index=0,ushort p_count=1) { MAVLinkParam p = new MAVLinkParam() { id = p_id }; p.SetFloat  (v); SendParam(p,p_index,p_count); }
        public void SendParam(string p_id,double v,ushort p_index=0,ushort p_count=1) { MAVLinkParam p = new MAVLinkParam() { id = p_id }; p.SetDouble (v); SendParam(p,p_index,p_count); }

        /// <summary>
        /// Sends a set of params
        /// </summary>
        /// <param name="p_params"></param>
        public void SendParams(IList<MAVLinkParam> p_params) {            
            //Console.WriteLine($"MAVLinkSystem> [{name}] starting params upload: {p_params.Count} parameters");
            lock(m_snd_params_queue_lock) { m_snd_params_queued = true; }
            if(m_snd_params_tsk==null) {
                Task tsk = new Task(delegate () {
                    while (true) {
                        lock (m_snd_params_queue_lock) { if (!m_snd_params_queued) break; m_snd_params_queued = false; }
                        List<MAVLinkParam> l = new List<MAVLinkParam>(p_params);
                        ushort k = 0;
                        ushort c = (ushort)l.Count;
                        while (true) {
                            SendParam(l[k],k,c);
                            k++;
                            if (k >= c) break;
                            Thread.Sleep(1);
                        }
                    }
                    //Console.WriteLine($"MAVLinkSystem> [{name}] parameters upload completed");
                    m_snd_params_tsk = null;
                });
                tsk.Start();
                m_snd_params_tsk = tsk;
            }            
        }
        private Task   m_snd_params_tsk        = null;
        private object m_snd_params_queue_lock = new object();
        private bool   m_snd_params_queued;
        #endregion

        /// <summary>
        /// Handler for incoming messages
        /// </summary>
        /// <param name="p_caller"></param>
        /// <param name="p_msg"></param>
        internal override void OnMessage(MAVLinkEntity p_caller,MAVLinkMessage p_msg) {
            base.OnMessage(p_caller,p_msg);            
        }

        /// <summary>
        /// Handler prior to send a heartbeat
        /// </summary>
        /// <param name="p_message"></param>
        virtual protected void OnHeartbeat(ref HEARTBEAT_MSG p_message) { }

        /// <summary>
        /// Handles keep-alive loop
        /// </summary>
        internal override void Update() {
            base.Update();
            if (alive) {
                m_hb_elapsed += network==null ? 0f : network.clock.deltaTime * 1000.0;
                if (m_hb_elapsed > heartbeatRate) {
                    m_hb_elapsed = 0f;
                    HEARTBEAT_MSG d = new HEARTBEAT_MSG() {
                        system_status   = (byte)state,
                        type            = (byte)type,
                        autopilot       = (byte)MAV_AUTOPILOT.GENERIC,
                        mavlink_version = 3
                    };
                    OnHeartbeat(ref d);
                    MAVLinkMessage msg = CreateMessage(MSG_ID.HEARTBEAT,d,false,systemId,componentId);
                    Send(msg);
                }
            }
        }

    }
}
