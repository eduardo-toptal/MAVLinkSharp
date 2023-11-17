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
    /// Class that implements an extension of a MAVLink System supporting the usual set of components and methods available to vehicles
    /// </summary>
    public class MAVLinkVehicle : MAVLinkSystem {

        /// <summary>
        /// CTOR.
        /// </summary>
        /// <param name="p_id"></param>
        /// <param name="p_name"></param>
        public MAVLinkVehicle(byte p_id,MAV_TYPE p_type,string p_name="") : base(p_id,p_type,p_name) { }

    }
}
