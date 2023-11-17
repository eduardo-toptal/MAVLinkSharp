using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MAVLink;

#pragma warning disable CS8600
#pragma warning disable CS8602
#pragma warning disable CS8603
#pragma warning disable CS8618
#pragma warning disable CS8625

namespace MAVLinkSharp {

    /// <summary>
    /// Class that implements a mavlink network made of entities handling and dispatching MAVLink messages 
    /// </summary>
    public class MAVLinkNetwork {

        #region struct Clock
        /// <summary>
        /// Clock Structure for time tracking.
        /// </summary>
        public struct Clock {

            /// <summary>
            /// Deltatime in seconds
            /// </summary>
            public double deltaTime;

            /// <summary>
            /// Elapsed seconds
            /// </summary>
            public double elapsed;

            /// <summary>
            /// Elapsed microsseconds
            /// </summary>
            public ulong elapsedUS;

            /// <summary>
            /// Elapsed millisseconds
            /// </summary>
            public ulong elapsedMS;

        }
        #endregion

        /// <summary>
        /// Network name
        /// </summary>
        public string name;

        /// <summary>
        /// Timing data
        /// </summary>
        public Clock clock;

        /// <summary>
        /// Elapsed microsseconds
        /// </summary>
        public ulong elapsedUS;

        /// <summary>
        /// Elapsed microsseconds
        /// </summary>
        public ulong elapsedMS;

        /// <summary>
        /// Handler for messages to be interacted by external code.
        /// </summary>
        public Action<MAVLinkEntity,MAVLinkMessage> OnMessageEvent;

        /// <summary>
        /// List of entities
        /// </summary>
        internal List<MAVLinkEntity> m_entities;
        internal Stopwatch m_clock;

        /// <summary>
        /// CTOR.
        /// </summary>
        /// <param name="p_name"></param>
        public MAVLinkNetwork(string p_name="") {
            name    = p_name;
            m_clock = new Stopwatch();
            clock = new Clock() {
                elapsed   = 0,
                elapsedUS = 0,
                deltaTime = 0
            };
            m_entities = new List<MAVLinkEntity>();
        }

        /// <summary>
        /// Returns the number of available entities
        /// </summary>
        public int childCount { get { return m_entities.Count; } }

        /// <summary>
        /// Returns an entity by its index.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="p_index"></param>
        /// <returns></returns>
        public T GetChild<T>(int p_index) where T : MAVLinkEntity { return (p_index < 0 ? default(T) : (p_index >= childCount ? default(T) : (T)m_entities[p_index])); }

        /// <summary>
        /// Returns a MAVLinkSystem by its id
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="p_id"></param>
        /// <returns></returns>
        public T GetSystemById<T>(byte p_id) where T : MAVLinkSystem {
            for(int i=0;i<m_entities.Count;i++) {
                MAVLinkEntity it = m_entities[i];
                if (!(it is T)) continue;
                T it_sys = (T)it;
                if (it_sys    == null) continue;
                if (it_sys.id == p_id) return it_sys;
            }
            return null;
        }

        /// <summary>
        /// Handler for dispatching incoming messages internally
        /// </summary>
        /// <param name="p_caller"></param>
        /// <param name="p_msg"></param>
        internal void OnMessageInternal(MAVLinkEntity p_caller,MAVLinkMessage p_msg) {
            if (OnMessageEvent != null) OnMessageEvent(p_caller,p_msg);
            OnMessage(p_caller,p_msg);
        }

        /// <summary>
        /// Handles incoming messages from all entities
        /// </summary>
        /// <param name="p_sender"></param>
        /// <param name="p_msg"></param>
        protected void OnMessage(MAVLinkEntity p_sender,MAVLinkMessage p_msg) {
            switch((MSG_ID)p_msg.msgid) {
                case MSG_ID.HEARTBEAT: {
                    HEARTBEAT_MSG d = p_msg.ToStructure<HEARTBEAT_MSG>();
                    byte sys_id = p_msg.sysid;
                    MAVLinkSystem sys = GetSystemById<MAVLinkSystem>(sys_id);
                    //If <null> external system and not-zero to skip broadcasts
                    if(sys_id>0)
                    if(sys==null) {
                        //Adds a placeholder for it
                        sys = new MAVLinkSystem(sys_id,(MAV_TYPE)d.type,((MAV_TYPE)d.type).ToString().ToLower());
                        //Make it not alive
                        sys.alive   = false;
                        sys.enabled = false;
                        //Assign to this network
                        sys.network = this;
                    }
                }
                break;
                case MSG_ID.SERIAL_CONTROL: {
                    SERIAL_CONTROL_MSG d = (SERIAL_CONTROL_MSG)p_msg.data;
                    string vs = System.Text.ASCIIEncoding.ASCII.GetString(d.data);
                    Console.WriteLine($"{name}> SERIAL [{vs}]");
                }
                break;
            }
        }

        /// <summary>
        /// Executes the update loop in all entities
        /// </summary>
        public void Update() {
            //Update internal clocks
            Stopwatch clk = m_clock;
            if (!clk.IsRunning) clk.Start();
            double t_s  = ((double)clk.ElapsedTicks / (double)Stopwatch.Frequency);            
            Clock last_timer = clock;           
            clock = new Clock() {
                elapsed   = t_s,
                elapsedUS = (ulong)(t_s * 1000000.0),
                elapsedMS = (ulong)(t_s * 1000.0),
                deltaTime = t_s - last_timer.elapsed
            };
            lock(m_entities) { for (int i=0;i<m_entities.Count;i++) if (m_entities[i].enabled)m_entities[i].Update();}
        }        
    }
}
