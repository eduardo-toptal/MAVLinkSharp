using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

#pragma warning disable CS8600
#pragma warning disable CS8602
#pragma warning disable CS8603
#pragma warning disable CS8618
#pragma warning disable CS8625
#pragma warning disable CS8632

namespace MAVLinkSharp.Runtime {

    /// <summary>
    /// Enumeration describing 
    /// </summary>
    [Flags]
    public enum MAVLinkNetworkRate {
        Disabled  = 0,
        Realtime  = (1<<0),
        Rate1hz   = (1<<1),
        Rate5hz   = (1<<2),
        Rate10hz  = (1<<3),
        Rate20hz  = (1<<4),
        Rate60hz  = (1<<5),
        Rate100hz = (1<<6),
        Rate200hz = (1<<7),
    }

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
            /// Current rate the clock is being presented
            /// </summary>
            public MAVLinkNetworkRate rate;

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
        /// Single Instance
        /// </summary>
        static internal MAVLinkNetwork m_instance;

        /// <summary>
        /// Network name
        /// </summary>
        public string name;

        /// <summary>
        /// Flag that enables the network loops to run.
        /// </summary>
        public bool enabled;

        /// <summary>
        /// Handler for messages to be interacted by external code.
        /// </summary>
        public Action<MAVLinkNode,MAVLinkMsg> OnMessageEvent;

        /// <summary>
        /// Network timing information
        /// </summary>
        public Clock time { get; private set; }

        /// <summary>
        /// List of entities
        /// </summary>
        internal List<MAVLinkNode> m_nodes;
        internal Stopwatch m_clk_elapsed;
        internal Stopwatch m_clk_delta;
        private double t1hz,t5hz,t10hz,t20hz,t60hz,t100hz,t200hz;
        private bool   m_running;
        
        /// <summary>
        /// CTOR.
        /// </summary>
        /// <param name="p_name"></param>
        public MAVLinkNetwork(string p_name="") {
            if(m_instance!=null) throw new NotSupportedException("Only one instance of MAVLinkNetwork is allowed");
            MAVLinkCRC.Init();
            m_instance = this;
            name       = p_name;            
            m_nodes    = new List<MAVLinkNode>();
            enabled    = true;
            m_clk_elapsed = new Stopwatch();
            m_clk_delta   = new Stopwatch();
            m_running     = false;
        }

        /// <summary>
        /// Runs the network inner loops
        /// </summary>
        public void Start() {
            if(m_running) return;
            m_running = true;
            ThreadPool.QueueUserWorkItem(InternalLoop);
            m_clk_elapsed.Start();
            t1hz=t5hz=t10hz=t20hz=t60hz=t100hz=t200hz=0;
        }

        /// <summary>
        /// Returns the number of available entities
        /// </summary>
        public int nodeCount { get { return m_nodes.Count; } }

        /// <summary>
        /// Returns an entity by its index.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="p_index"></param>
        /// <returns></returns>
        public T GetNode<T>(int p_index) where T : MAVLinkNode { return (p_index < 0 ? default(T) : (p_index >= nodeCount ? default(T) : (T)m_nodes[p_index])); }

        /// <summary>
        /// Handler for dispatching incoming messages internally
        /// </summary>
        /// <param name="p_sender"></param>
        /// <param name="p_msg"></param>
        internal void OnMessageInternal(MAVLinkNode p_sender,MAVLinkMsg p_msg) {            
            if (!enabled) return;
            if (OnMessageEvent != null) OnMessageEvent(p_sender,p_msg);
        }

        /// <summary>
        /// Updates this network clocking
        /// </summary>
        internal void InternalLoop(object? so) {
            while(m_running) {
                if(!enabled) { Thread.Sleep(100); continue; }
                double dt   = m_clk_delta.Elapsed.TotalSeconds;
                ulong  t_ms = (ulong)m_clk_elapsed.Elapsed.TotalMilliseconds;
                ulong  t_us = (ulong)m_clk_elapsed.Elapsed.Ticks/10;
                m_clk_delta.Restart();
                double vd;

                MAVLinkNetworkRate msk_exec = MAVLinkNetworkRate.Realtime;
                vd = t1hz;    vd += dt; if(vd >   1.0000) { vd=0; msk_exec |= MAVLinkNetworkRate.Rate1hz;   } t1hz   = vd;
                vd = t5hz;    vd += dt; if(vd >   0.2000) { vd=0; msk_exec |= MAVLinkNetworkRate.Rate5hz;   } t5hz   = vd;
                vd = t10hz;   vd += dt; if(vd >   0.1000) { vd=0; msk_exec |= MAVLinkNetworkRate.Rate10hz;  } t10hz  = vd;
                vd = t20hz;   vd += dt; if(vd >   0.0500) { vd=0; msk_exec |= MAVLinkNetworkRate.Rate20hz;  } t20hz  = vd;
                vd = t60hz;   vd += dt; if(vd >   0.0166) { vd=0; msk_exec |= MAVLinkNetworkRate.Rate60hz;  } t60hz  = vd;
                vd = t100hz;  vd += dt; if(vd >   0.0100) { vd=0; msk_exec |= MAVLinkNetworkRate.Rate100hz; } t100hz = vd;
                vd = t200hz;  vd += dt; if(vd >   0.0050) { vd=0; msk_exec |= MAVLinkNetworkRate.Rate200hz; } t200hz = vd;
                
                List<MAVLinkNode> nl = m_nodes;

                //For all available flags
                MAVLinkNetworkRate msk_f = MAVLinkNetworkRate.Realtime;
                for(int k=0;k<7;k++) {
                    MAVLinkNetworkRate f = msk_exec & msk_f;
                    bool will_run = f != 0;
                    msk_f = (MAVLinkNetworkRate)((int)msk_f<<1);
                    if(!will_run) continue;
                    double exec_dt = dt;
                    switch(f) {
                        case MAVLinkNetworkRate.Disabled: continue;
                        case MAVLinkNetworkRate.Realtime: break;
                        case MAVLinkNetworkRate.Rate1hz:    exec_dt = 1.0000; break;
                        case MAVLinkNetworkRate.Rate5hz:    exec_dt = 0.2000; break;
                        case MAVLinkNetworkRate.Rate10hz:   exec_dt = 0.1000; break;
                        case MAVLinkNetworkRate.Rate20hz:   exec_dt = 0.0500; break;
                        case MAVLinkNetworkRate.Rate60hz:   exec_dt = 0.0166; break;
                        case MAVLinkNetworkRate.Rate100hz:  exec_dt = 0.0100; break;
                        case MAVLinkNetworkRate.Rate200hz:  exec_dt = 0.0050; break;
                    }
                    time = new Clock() {
                        rate      = f,
                        deltaTime = exec_dt,
                        elapsedMS = t_ms,
                        elapsedUS = t_us
                    };
                    lock(nl) for(int i=0;i<nl.Count;i++) if((nl[i].rate & f)!=0) if(nl[i].enabled) nl[i].OnUpdate();
                }
                Thread.Yield();
            }
        }

        /// <summary>
        /// Disposes the entire network and nodes.
        /// </summary>
        public void Dispose() {
            m_running = false; 
            List<MAVLinkNode> nl = m_nodes;
            lock(nl) for(int i=0;i<nl.Count;i++) nl[i].Dispose();
            m_instance = null;
        }

    }
}
