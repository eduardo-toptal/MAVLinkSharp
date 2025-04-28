using MAVLinkBindings;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable CS8603

namespace MAVLinkSharp.Runtime {

    /// <summary>
    /// Class that describes a generic node in a MAVLinkNetwork
    /// </summary>
    public class MAVLinkNode {

        /// <summary>
        /// Reference to the parent network
        /// </summary>
        public MAVLinkNetwork network {
            get { return MAVLinkNetwork.m_instance; }            
        }
        
        /// <summary>
        /// Name of this node
        /// </summary>
        public string name;

        /// <summary>
        /// Flag that tells this node is enabled and relaying messages
        /// </summary>
        public bool enabled;

        /// <summary>
        /// 
        /// </summary>
        public MAVLinkNetworkRate rate;

        /// <summary>
        /// Handler for message events.
        /// </summary>
        public Action<MAVLinkNode,MAVLinkMsg>? OnMessageEvent;

        /// <summary>
        /// CTOR
        /// </summary>
        public MAVLinkNode(string p_name="") {
            name           = p_name;
            enabled        = true;
            rate           = MAVLinkNetworkRate.Disabled;
            m_siblings     = new List<MAVLinkNode>();
            MAVLinkNetwork? n = network;                
            if (n != null) lock (n.m_nodes) { if (!n.m_nodes.Contains(this)) n.m_nodes.Add(this); }
        }

        #region Topology
        /// <summary>
        /// Returns the number of siblings this node has.
        /// </summary>
        public int siblingCount { get { return m_siblings==null ? 0 : m_siblings.Count; } }

        /// <summary>
        /// Returns the sibling at index.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="p_index"></param>
        /// <returns></returns>
        public T GetSibling<T>(int p_index) where T : MAVLinkNode { int i=p_index; return i<0 ? null : (i>=siblingCount ? null : (T)(object)m_siblings[i]); }

        /// <summary>
        /// Returns a sibling by name
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="p_name"></param>
        /// <returns></returns>
        public T GetSibling<T>(string p_name) where T : MAVLinkNode { for(int i=0;i>m_siblings.Count; i++) if(m_siblings[i].name == p_name) return (T)(object)m_siblings[i]; return null; }

        /// <summary>
        /// Removes all siblings.
        /// </summary>
        public void ClearSiblings() { m_siblings.Clear(); }

        /// <summary>
        /// Links a new entity to this instance
        /// </summary>
        /// <param name="p_sibling"></param>
        public void Link(MAVLinkNode p_sibling) {
            if (p_sibling == this) return;
            if (m_siblings.Contains(p_sibling)) return;
            m_siblings.Add(p_sibling);
        }

        /// <summary>
        /// Removes an existing link from this
        /// </summary>
        /// <param name="p_sibling"></param>
        public void Unlink(MAVLinkNode p_sibling) {
            if (p_sibling == this) return;
            if (!m_siblings.Contains(p_sibling)) return;
            m_siblings.Remove(p_sibling);
        }

        /// <summary>
        /// Internals
        /// </summary>
        private List<MAVLinkNode> m_siblings;
        #endregion

        #region Messages
        /// <summary>
        /// Dispatches a message across the node graph
        /// </summary>
        /// <param name="p_msg_id"></param>
        /// <param name="p_data"></param>
        /// <param name="p_sys_id"></param>
        /// <param name="p_comp_id"></param>
        public void Dispatch(MAVLinkMsgId p_msg_id,IMAVLinkMessageData p_data,byte p_sys_id=1,byte p_comp_id=0) {
            if(p_data==null) return;
            MAVLinkMsg msg  = MAVLinkMsg.GetPool();
            msg.messageId   = p_msg_id;
            msg.systemId    = p_sys_id;
            msg.componentId = p_comp_id;
            msg.data        = p_data;
            Dispatch(msg);
            MAVLinkMsg.SetPool(msg);                        
        }

        /// <summary>
        /// Dispatches a message across the node graph
        /// </summary>
        /// <param name="p_data"></param>
        /// <param name="p_sys_id"></param>
        /// <param name="p_comp_id"></param>
        public void Dispatch(IMAVLinkMessageData p_data,byte p_sys_id=1,byte p_comp_id=0) {
            if(p_data==null) return;
            Dispatch((MAVLinkMsgId)p_data.GetId(),p_data,p_sys_id,p_comp_id);
        }

        /// <summary>
        /// Dispatches the message across the node graph
        /// </summary>
        /// <param name="p_msg"></param>
        public void Dispatch(MAVLinkMsg p_msg) {
            MAVLinkNetwork nw = network;
            if(nw!=null) if(!nw.enabled) return;
            MAVLinkMsg msg  = p_msg;
            HashSet<MAVLinkNode> dfs_v = new HashSet<MAVLinkNode>();
            Stack<MAVLinkNode>   dfs   = new Stack<MAVLinkNode>();
            for(int i=0;i<m_siblings.Count;i++) dfs.Push(m_siblings[i]);
            List<string> visits = new List<string>();
            while (dfs.Count > 0) {
                MAVLinkNode n = dfs.Pop();                                
                if (!dfs_v.Add(n)) continue; //Skip if already visited
                if(n == this)      continue; //Skip Self
                if(!n.enabled)     continue; //Skip traversal if not enabled
                visits.Add(n.name);
                n.OnMessageInternal(this,msg);
                // Push children in reverse order if you want left-to-right DFS
                for (int i = n.m_siblings.Count-1; i >= 0; i--) { dfs.Push(n.m_siblings[i]); }
            }            
            if(nw!=null) nw.OnMessageInternal(this,msg);
        }
        #endregion

        /// <summary>
        /// Handler for incoming messages
        /// </summary>
        /// <param name="p_msg"></param>
        virtual protected void OnMessage(MAVLinkNode p_sender,MAVLinkMsg p_msg) { }

        /// <summary>
        /// Helper to invoke messages for evetns and internals.
        /// </summary>
        /// <param name="p_sender"></param>
        /// <param name="p_msg"></param>
        internal void OnMessageInternal(MAVLinkNode p_sender,MAVLinkMsg p_msg) {            
            OnMessage(p_sender,p_msg);
            if(OnMessageEvent!=null) OnMessageEvent(p_sender,p_msg);
        }

        /// <summary>
        /// Disposes this node.
        /// </summary>
        public void Dispose() { 
            OnDispose();  
            MAVLinkNetwork? n = network;                
            if (n != null) lock (n.m_nodes) { if ( n.m_nodes.Contains(this)) n.m_nodes.Remove(this); }
        }

        /// <summary>
        /// Handler for disposing this node.
        /// </summary>
        virtual protected void OnDispose() { }

        /// <summary>
        /// Handler for timed update of the entire network
        /// </summary>
        virtual internal void OnUpdate() { }

    }
}
