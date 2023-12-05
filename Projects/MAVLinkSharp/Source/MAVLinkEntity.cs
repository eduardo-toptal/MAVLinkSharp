using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static MAVLink;
using static MAVLinkSharp.MAVLinkNetwork;

#pragma warning disable CS8600
#pragma warning disable CS8601
#pragma warning disable CS8602
#pragma warning disable CS8603
#pragma warning disable CS8618
#pragma warning disable CS8625

namespace MAVLinkSharp {

    #region class MAVLinkMessageFilter
    /// <summary>
    /// Class that describes a message filtering rule that allows entities to ignore/allowe messages based on regexp rules and message properties
    /// </summary>
    public class MAVLinkMessageFilter {

        /// <summary>
        /// Auxiliary filter creator
        /// </summary>
        /// <param name="p_property"></param>
        /// <param name="p_filter"></param>
        /// <returns></returns>
        static public MAVLinkMessageFilter FilterMessage(string p_filter) { return new MAVLinkMessageFilter() { property = Property.MessageId, filter = p_filter }; }
        static public MAVLinkMessageFilter FilterSender (string p_filter) { return new MAVLinkMessageFilter() { property = Property.Sender   , filter = p_filter }; }
        static public MAVLinkMessageFilter FilterAll    (string p_filter) { return new MAVLinkMessageFilter() { property = Property.MessageId | Property.Sender , filter = p_filter }; }

        #region enum Property
        /// <summary>
        /// Message Property to Validate
        /// </summary>
        public enum Property {
            /// <summary>
            /// Filter by MSG_ID
            /// </summary>
            MessageId = 1,
            /// <summary>
            /// Filter by Sender
            /// </summary>
            Sender    = 2,            
        }
        #endregion

        /// <summary>
        /// Property to Match
        /// </summary>
        public Property property;

        /// <summary>
        /// Regexp Filter
        /// </summary>
        public string filter {
            get { return m_filter; }
            set {
                m_filter = value;
                string f = string.IsNullOrEmpty(m_filter) ? ".*" : m_filter;
                try {
                    regex = new Regex(f,RegexOptions.IgnoreCase);
                } catch (System.Exception) {
                    
                    regex = new Regex(".*");
                }                
            }
        }
        private string m_filter;

        /// <summary>
        /// Returns the Regex for the chosen rule.
        /// </summary>
        public Regex regex { get; private set; }

        /// <summary>
        /// Checks if a sender and message are a match for this particular rule
        /// </summary>
        /// <param name="p_sender"></param>
        /// <param name="p_msg"></param>
        /// <returns></returns>
        public bool IsMatch(MAVLinkEntity p_sender,MAVLinkMessage p_msg) {
            string q = "";
            bool f_message = (property & Property.MessageId) != 0;
            bool f_sender  = (property & Property.Sender   ) != 0;
            bool match_message = f_message ? false : true;
            bool match_sender  = f_sender  ? false : true;
            if(f_message) { q = ((MSG_ID)p_msg.msgid).ToString();      match_message = string.IsNullOrEmpty(q) ? true : regex.IsMatch(q); }
            if(f_sender ) { q = p_sender == null ? "" : p_sender.name; match_sender  = string.IsNullOrEmpty(q) ? true : regex.IsMatch(q); } 
            return match_message && match_sender;
        }

    }
    #endregion

    /// <summary>
    /// Class that implements a connection node for mavlink messages exchanging
    /// </summary>
    public class MAVLinkEntity {

        #region static 
        /// <summary>
        /// Utility method to create new messages handling package sequencing
        /// </summary>
        /// <param name="p_parser"></param>
        /// <param name="p_msg_id"></param>
        /// <param name="p_data"></param>
        /// <param name="p_sign"></param>
        /// <param name="p_sys_id"></param>
        /// <param name="p_comp_id"></param>
        /// <returns></returns>
        static private MAVLinkMessage CreateMessage(MavlinkParse p_parser,MSG_ID p_msg_id,object p_data,bool p_sign = false,byte p_sys_id = 255,byte p_comp_id = (byte)MAV_COMPONENT.MAV_COMP_ID_MISSIONPLANNER) {
            //Atomically update next sequence
            int next_seq = 0;
            lock(m_msg_lock) { next_seq = m_msg_seq++; }
            //Create the message using the assigned parser
            MAVLinkMessage msg = new MAVLinkMessage();
            msg.buffer = p_parser.GenerateMAVLinkPacket20(p_msg_id,p_data,p_sign,p_sys_id,p_comp_id,next_seq);
            return msg;
        }
        static object m_msg_lock = new object();
        static int    m_msg_seq  = 0;
        #endregion

        /// <summary>
        /// Flag that tells this entity is enabled and able to receive msgs and interact
        /// </summary>
        public bool enabled;

        /// <summary>
        /// Connection name
        /// </summary>
        public string name;

        /// <summary>
        /// List of allowed message filter (only messages whom match will be sent)
        /// </summary>
        public List<MAVLinkMessageFilter> allowed { get; set; }

        /// <summary>
        /// List of ignored message filter from all allowed messages they will be ignored by this list
        /// </summary>
        public List<MAVLinkMessageFilter> ignored { get; set; }

        /// <summary>
        /// Milisseconds between each sync
        /// </summary>
        public uint syncRate;

        /// <summary>
        /// Local clock instance for syncRate adjusted time tracking
        /// </summary>
        public Clock clock {
            get {
                if (network == null) return default;
                double sr = ((double)syncRate) / 1000.0;
                double dt = network.clock.deltaTime;
                if (dt <= 0f) dt = 0.001;
                double r = Math.Max(1,sr / dt);
                Clock res = network.clock;
                res.deltaTime = dt * r;
                return res;
            }
        }

        /// <summary>
        /// Handler for mavlink messages events to external entities
        /// </summary>
        public Action<MAVLinkEntity,MAVLinkMessage> OnMessageEvent;

        /// <summary>
        /// Handler for 
        /// </summary>
        private double m_rate_elapsed;

        /// <summary>
        /// Reference to the parent network
        /// </summary>
        public MAVLinkNetwork network {
            get { return m_network; }
            set {
                MAVLinkNetwork n;                
                n = m_network;
                if (n != null) lock (n.m_entities) { if ( n.m_entities.Contains(this)) n.m_entities.Remove(this); }
                n = value;
                if (n != null) lock (n.m_entities) { if (!n.m_entities.Contains(this)) n.m_entities.Add   (this); }                                
                m_network = n;
            }
        }
        internal MAVLinkNetwork m_network;

        /// <summary>
        /// Internal
        /// </summary>
        protected   List<MAVLinkEntity> m_siblings;
        private     MavlinkParse parser;
        private     Stopwatch m_clk;

        /// <summary>
        /// Create a new named connection.
        /// </summary>
        /// <param name="p_name"></param>
        public MAVLinkEntity(string p_name) {
            name        = p_name;
            allowed     = new List<MAVLinkMessageFilter>();
            ignored     = new List<MAVLinkMessageFilter>();
            m_siblings  = new List<MAVLinkEntity>();
            parser      = new MavlinkParse();
            m_clk       = new Stopwatch();
            m_rate_elapsed = 9999f;
            syncRate    = 0;
            enabled     = true;
        }

        /// <summary>
        /// CTOR.
        /// </summary>
        public MAVLinkEntity() : this("") { }

        #region Topology
        /// <summary>
        /// Links a new entity to this instance
        /// </summary>
        /// <param name="p_sibling"></param>
        public void Link(MAVLinkEntity p_sibling) {
            if (p_sibling == this) return;
            if (m_siblings.Contains(p_sibling)) return;
            m_siblings.Add(p_sibling);
        }

        /// <summary>
        /// Removes an existing link from this
        /// </summary>
        /// <param name="p_sibling"></param>
        public void Unlink(MAVLinkEntity p_sibling) {
            if (p_sibling == this) return;
            if (!m_siblings.Contains(p_sibling)) return;
            m_siblings.Remove(p_sibling);
        }

        /// <summary>
        /// Removes all occurrences of a given sibling by name
        /// </summary>
        /// <param name="p_name"></param>
        public void Unlink(string p_name) {
            for(int i=0;i<m_siblings.Count;i++) if (m_siblings[i].name == p_name) Unlink(m_siblings[i]);                        
        }

        /// <summary>
        /// Removes all siblings
        /// </summary>
        public void Clear() { m_siblings.Clear();  }
        #endregion

        #region Messages
        /// <summary>
        /// Sends a message across the MAVLink entities graph
        /// </summary>
        /// <param name="p_msg"></param>
        /// <param name="p_send_to_self"></param>
        public void Send(MAVLinkMessage p_msg,bool p_send_to_self=false,bool p_force=false) {
            //Starts a BFS iteration of the network its made of a queue and list of visited
            //Add self even if not using
            List<MAVLinkEntity> ql = new List<MAVLinkEntity>() { this };                        
            //Add siblings
            ql.AddRange(m_siblings);
            Queue<MAVLinkEntity> q = new Queue<MAVLinkEntity>(ql);
            List<MAVLinkEntity>  v = new List<MAVLinkEntity>();            
            MSG_ID msg_id = (MSG_ID)p_msg.msgid;
            if(msg_id == MSG_ID.HIL_ACTUATOR_CONTROLS) {
                int i = 0;
            }
            //Iterate until queue is empty.
            while(q.Count>0) {
                //Fetch next node
                MAVLinkEntity n = q.Dequeue();
                //If already visited skip
                if (v.Contains(n)) continue;
                //Add to visited
                v.Add(n);
                //Ignore self if needed
                if (!p_send_to_self) if (n == this) continue;                
                //Enqueue siblings 
                for(int i=0;i<n.m_siblings.Count;i++) if (n.m_siblings[i]!=null)q.Enqueue(n.m_siblings[i]);                
                //If entity is not allowed to process the msg then continue
                bool is_allowed = p_force || IsMessageAllowed(n,p_msg);                
                //Invoke message handler
                if(is_allowed) n.OnMessageInternal(this,p_msg);
            }
            if (network != null) network.OnMessageInternal(this,p_msg);
        }

        /// <summary>
        /// Checks if a given message is allowed to be processed by the target entity
        /// </summary>
        /// <param name="p_target"></param>
        /// <param name="p_msg"></param>
        /// <returns></returns>
        protected bool IsMessageAllowed(MAVLinkEntity p_target,MAVLinkMessage p_msg) {
            //Locals
            MAVLinkEntity n = p_target;
            //Skip invalids
            if (n == null) return false;
            //If this entity has a specific rules for allowing some messages skip if the message don't match
            if (n.allowed != null) if(n.allowed.Count  > 0) if (!IsMatch(n.allowed,this,p_msg)) return false;
            //If this entity has a specific rules for ignoring some messages skip if the message match the filter
            if (n.ignored != null) if (n.ignored.Count > 0) if ( IsMatch(n.ignored,this,p_msg)) return false;            
            //Convert to live entity
            MAVLinkLiveEntity n_le = (n is MAVLinkLiveEntity) ? (MAVLinkLiveEntity)n : null;
            //If not of type, allow pass-thru
            if (n_le == null) return true;
            //Locals            
            byte n_sys = n_le.systemId;
            byte n_cmp = n_le.componentId;
            //Validate COMMAND messages                
            byte t_sys = 0; //Target SysId
            byte t_cmp = 0; //Target CompId
            //Fetch targets from message
            GetMessageTargets(p_msg,out t_sys,out t_cmp);
            //Block handling if not matching (if targets are 0 then its broadcast)
            if (t_sys > 0) if (t_sys != n_sys) return false;
            if (t_cmp > 0) if (t_cmp != n_cmp) return false;
            //All good
            return true;
        }

        /// <summary>
        /// Utility to extract the target system and component from given type of messages
        /// </summary>
        /// <param name="p_msg"></param>
        /// <param name="p_sys"></param>
        /// <param name="p_cmp"></param>
        protected void GetMessageTargets(MAVLinkMessage p_msg,out byte p_sys,out byte p_cmp) {
            //Message id enum
            MSG_ID msg_id = (MSG_ID)p_msg.msgid;
            //Validate COMMAND messages                
            byte t_sys = 0; //Target SysId
            byte t_cmp = 0; //Target CompId
            //Collect targets
            switch (msg_id) {
                //If COMMAND need to filter by system and component
                case MSG_ID.COMMAND_ACK:                  { COMMAND_ACK_MSG                  d = p_msg.ToStructure<COMMAND_ACK_MSG                 >(); t_sys = d.target_system; t_cmp = d.target_component; } break;
                case MSG_ID.COMMAND_INT:                  { COMMAND_INT_MSG                  d = p_msg.ToStructure<COMMAND_INT_MSG                 >(); t_sys = d.target_system; t_cmp = d.target_component; } break;
                case MSG_ID.COMMAND_LONG:                 { COMMAND_LONG_MSG                 d = p_msg.ToStructure<COMMAND_LONG_MSG                >(); t_sys = d.target_system; t_cmp = d.target_component; } break;
                case MSG_ID.PARAM_REQUEST_LIST:           { PARAM_REQUEST_LIST_MSG           d = p_msg.ToStructure<PARAM_REQUEST_LIST_MSG          >(); t_sys = d.target_system; t_cmp = d.target_component; } break;
                case MSG_ID.PARAM_REQUEST_READ:           { PARAM_REQUEST_READ_MSG           d = p_msg.ToStructure<PARAM_REQUEST_READ_MSG          >(); t_sys = d.target_system; t_cmp = d.target_component; } break;
                case MSG_ID.MISSION_REQUEST_LIST:         { MISSION_REQUEST_LIST_MSG         d = p_msg.ToStructure<MISSION_REQUEST_LIST_MSG        >(); t_sys = d.target_system; t_cmp = d.target_component; } break;
                case MSG_ID.MISSION_REQUEST_PARTIAL_LIST: { MISSION_REQUEST_PARTIAL_LIST_MSG d = p_msg.ToStructure<MISSION_REQUEST_PARTIAL_LIST_MSG>(); t_sys = d.target_system; t_cmp = d.target_component; } break;
                case MSG_ID.MANUAL_CONTROL:               { MANUAL_CONTROL_MSG               d = p_msg.ToStructure<MANUAL_CONTROL_MSG              >(); t_sys = d.target;        t_cmp =                  0; } break;
            }
            p_sys = t_sys;
            p_cmp = t_cmp;
        }

        /// <summary>
        /// Utility method to generate a mavlink message
        /// </summary>
        /// <param name="p_msg_id"></param>
        /// <param name="p_data"></param>
        /// <param name="p_sign"></param>
        /// <param name="p_sys_id"></param>
        /// <param name="p_comp_id"></param>
        /// <returns></returns>
        public MAVLinkMessage CreateMessage(MSG_ID p_msg_id,object p_data,bool p_sign = false,byte p_sys_id = 255,byte p_comp_id = (byte)MAV_COMPONENT.MAV_COMP_ID_MISSIONPLANNER) {
            return CreateMessage(parser,p_msg_id,p_data,p_sign,p_sys_id,p_comp_id);
        }

        /// <summary>
        /// Utility method to check a list of filters for a match
        /// </summary>
        /// <param name="p_filters"></param>
        /// <param name="p_sender"></param>
        /// <param name="p_msg"></param>
        /// <returns></returns>
        internal bool IsMatch(IList<MAVLinkMessageFilter> p_filters,MAVLinkEntity p_sender,MAVLinkMessage p_msg) {
            if (p_filters       == null) return false;
            if (p_filters.Count <=    0) return true;
            for (int i = 0;i < p_filters.Count;i++) {
                if (p_filters[i] == null) continue;
                if (p_filters[i].IsMatch(p_sender,p_msg)) return true;
            }
            return false;
        }
        #endregion

        #region Virtuals

        /// <summary>
        /// Handler for dispatching incoming messages internally
        /// </summary>
        /// <param name="p_caller"></param>
        /// <param name="p_msg"></param>
        internal void OnMessageInternal(MAVLinkEntity p_caller,MAVLinkMessage p_msg) { 
            if(OnMessageEvent!=null) OnMessageEvent(p_caller,p_msg);
            OnMessage(p_caller,p_msg); 
        }

        /// <summary>
        /// Handles an incoming message
        /// </summary>
        /// <param name="p_msg"></param>
        virtual protected void OnMessage(MAVLinkEntity p_caller,MAVLinkMessage p_msg) { }

        /// <summary>
        /// Handler for updates
        /// </summary>
        virtual protected void OnUpdate() { }

        /// <summary>
        /// Handles internal logic updates
        /// </summary>
        virtual public void Update() {
            uint sr = syncRate;
            if(sr>0) {
                double dt = network == null ? 0.0 : network.clock.deltaTime;
                m_rate_elapsed += dt * 1000.0;
                if (m_rate_elapsed < sr) return;
                m_rate_elapsed = 0;
            }            
            OnUpdate();
        }
        #endregion

    }
}
