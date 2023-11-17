using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MAVLink;

#pragma warning disable CS8600
#pragma warning disable CS8602
#pragma warning disable CS8603
#pragma warning disable CS8618
#pragma warning disable CS8625

namespace MAVConsole {

    /// <summary>
    /// Class that implements a MAVLink most basic system, made of an id and signals the network its 'alive'
    /// </summary>
    public class MAVLinkComponent : MAVLinkLiveEntity {

        /// <summary>
        /// Reference to the parent system
        /// </summary>
        public MAVLinkSystem system {
            get { return m_system; }
            set {
                MAVLinkSystem s;
                s = m_system;
                if (s != null) s.ComponentRemove(this);
                s = value;
                if (s != null) s.ComponentAdd(this);
                //Associate only the network instance
                m_network = system.network;
                OnSystemChange();
            }
        }
        internal MAVLinkSystem m_system;

        /// <summary>
        /// Component ID
        /// </summary>
        public MAV_COMPONENT id { get { return (MAV_COMPONENT)base.componentId; } private set { base.componentId = (byte)value; } }

        /// <summary>
        /// CTOR.
        /// </summary>
        /// <param name="p_id"></param>
        /// <param name="p_type"></param>
        /// <param name="p_name"></param>
        public MAVLinkComponent(MAV_COMPONENT p_id,MAV_TYPE p_type,string p_name="") : base(p_type,p_name) {
            id = p_id;
            //Default to not live as not every component needs to signal its presence
            alive = false;
        }

        /// <summary>
        /// CTOR.
        /// </summary>
        /// <param name="p_id"></param>
        /// <param name="p_type"></param>
        /// <param name="p_name"></param>
        public MAVLinkComponent(MAV_COMPONENT p_id,string p_name = "") : this(p_id,MAV_TYPE.GENERIC,p_name) { }

        /// <summary>
        /// Handler for when system instance has changed
        /// </summary>
        virtual protected void OnSystemChange() { }

    }
}
