using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static MAVLink;

#pragma warning disable CS8600
#pragma warning disable CS8602
#pragma warning disable CS8603
#pragma warning disable CS8604
#pragma warning disable CS8618
#pragma warning disable CS8625

namespace MAVLinkSharp {

    #region struct MAVLinkParam
    /// <summary>
    /// Struct that defines a single mavlink param to be exchanged by messages
    /// </summary>
    public struct MAVLinkParam {

        /// <summary>
        /// ID of the parameter.
        /// </summary>
        public string id;

        /// <summary>
        /// Param Type Enum
        /// </summary>
        public MAV_PARAM_TYPE type;

        /// <summary>
        /// Internals
        /// </summary>
        private ulong   m_u64;
        private long    m_i64;
        private double  m_f64;

        /// <summary>
        /// Generates the byte encoded id
        /// </summary>
        /// <returns></returns>
        public byte[] GetIdBytes() { return ASCIIEncoding.UTF8.GetBytes(id);  }

        /// <summary>
        /// Setter
        /// </summary>
        /// <param name="v"></param>
        public void SetByte  (byte   v) { type = MAV_PARAM_TYPE.UINT8 ;  m_u64 = v; }
        public void SetSByte (sbyte  v) { type = MAV_PARAM_TYPE.INT8  ;  m_i64 = v; }
        public void SetUShort(ushort v) { type = MAV_PARAM_TYPE.UINT16;  m_u64 = v; }
        public void SetShort (short  v) { type = MAV_PARAM_TYPE.INT16 ;  m_i64 = v; }
        public void SetUInt  (uint   v) { type = MAV_PARAM_TYPE.UINT32;  m_u64 = v; }
        public void SetInt   (int    v) { type = MAV_PARAM_TYPE.INT32 ;  m_i64 = v; }
        public void SetULong (ulong  v) { type = MAV_PARAM_TYPE.UINT64;  m_u64 = v; }
        public void SetLong  (long   v) { type = MAV_PARAM_TYPE.INT64 ;  m_i64 = v; }
        public void SetFloat (float  v) { type = MAV_PARAM_TYPE.REAL32;  m_f64 = v; }
        public void SetDouble(double v) { type = MAV_PARAM_TYPE.REAL64;  m_f64 = v; }

        /// <summary>
        /// Getter
        /// </summary>
        /// <param name="v"></param>
        public byte   GetByte  () { return (byte  )m_u64; }
        public sbyte  GetSByte () { return (sbyte )m_i64; }
        public ushort GetUShort() { return (ushort)m_u64; }
        public short  GetShort () { return (short )m_i64; }
        public uint   GetUInt  () { return (uint  )m_u64; }
        public int    GetInt   () { return (int   )m_i64; }
        public ulong  GetULong () { return (ulong )m_u64; }
        public long   GetLong  () { return (long  )m_i64; }
        public float  GetFloat () { return (float )m_f64; }
        public double GetDouble() { return (double)m_f64; }

        /// <summary>
        /// Populates and return a message struct
        /// </summary>
        /// <param name="p_index"></param>
        /// <param name="p_count"></param>
        /// <returns></returns>
        public PARAM_VALUE_MSG GetMessage(ushort p_index=0,ushort p_count=0) {
            //Why?
            //if (id == "_HASH_CHECK") p_index = ushort.MaxValue;
            PARAM_VALUE_MSG d = new PARAM_VALUE_MSG() {
                param_id    = GetIdBytes(),
                param_count = p_count,
                param_index = p_index,
                param_type = (byte)type                
            };
            switch (type) {
                case MAV_PARAM_TYPE.UINT8 : { byte   v = GetByte  (); d.param_value = (float)v; } break;
                case MAV_PARAM_TYPE.INT8  : { sbyte  v = GetSByte (); d.param_value = (float)v; } break;
                case MAV_PARAM_TYPE.UINT16: { ushort v = GetUShort(); d.param_value = (float)v; } break;
                case MAV_PARAM_TYPE.INT16 : { short  v = GetShort (); d.param_value = (float)v; } break;
                case MAV_PARAM_TYPE.UINT32: { uint   v = GetUInt  (); d.param_value = (float)v; } break;
                case MAV_PARAM_TYPE.INT32 : { int    v = GetInt   (); d.param_value = (float)v; } break;
                case MAV_PARAM_TYPE.UINT64: { ulong  v = GetULong (); d.param_value = (float)v; } break;
                case MAV_PARAM_TYPE.INT64 : { long   v = GetLong  (); d.param_value = (float)v; } break;
                case MAV_PARAM_TYPE.REAL32: { float  v = GetFloat (); d.param_value = (float)v; } break;
                case MAV_PARAM_TYPE.REAL64: { double v = GetDouble(); d.param_value = (float)v; } break;
            }
            return d;
        }

    }
    #endregion

    #region enum MAVLinkInputField
    /// <summary>
    /// Enumeration to associate an input axis and buttons
    /// </summary>
    public enum MAVLinkInputField {
        Throttle=0,
        Yaw,
        Pitch,
        Roll,
        Axis0,
        Axis1,
        Axis2,
        Axis3,
        Axis4,
        Axis5,
        Axis6,
        Axis7,
        Axis8,
        Axis9,
        Axis10,
        Axis11,
        Axis12,
        Button0,
        Button1,
        Button2,
        Button3,
        Button4,
        Button5,
        Button6,
        Button7,
        Button8,
        Button9,
        Button10,
        Button11,
        Button12
    }
    #endregion

    #region class MAVLinkInput
    /// <summary>
    /// Class that envelops input information coming from MAVLink messaging
    /// </summary>
    public class MAVLinkInput {

        /// <summary>
        /// List of input axis
        /// </summary>
        public double[] axis;

        /// <summary>
        /// List of deadzones associated with the axis
        /// </summary>
        public double[] deadzones;

        /// <summary>
        /// List of buttons
        /// </summary>
        public bool[] buttons;

        /// <summary>
        /// Internal
        /// </summary>
        private Dictionary<MAVLinkInputField,int> m_field_lut;

        /// <summary>
        /// CTOR.
        /// </summary>
        /// <param name="p_length"></param>
        public MAVLinkInput (int p_length) {
            Resize(p_length);
            m_field_lut = new Dictionary<MAVLinkInputField,int>();
        }

        /// <summary>
        /// Clears the field lookup
        /// </summary>
        public void Clear() {
            m_field_lut.Clear();
        }

        /// <summary>
        /// Assign an axis to a given value in the input array
        /// </summary>
        /// <param name="p_field"></param>
        /// <param name="p_index"></param>
        public void AssignAxis(MAVLinkInputField p_field,int p_index) { m_field_lut[p_field] = p_index; }

        /// <summary>
        /// Assign a button to a given value in the input array
        /// </summary>
        /// <param name="p_field"></param>
        /// <param name="p_index"></param>
        public void AssignButton(MAVLinkInputField p_field,int p_index) { m_field_lut[p_field] = p_index; }

        /// <summary>
        /// Assigns the deadzone for a given assigned axis.
        /// </summary>
        /// <param name="p_field"></param>
        /// <param name="p_value"></param>
        public void SetDeadzone(MAVLinkInputField p_field,double p_value) {
            if (deadzones == null) return;
            if (!m_field_lut.ContainsKey(p_field)) return;
            int idx = m_field_lut[p_field];
            if(idx<0)             return;
            if(idx>=axis.Length)  return;
            deadzones[idx] = p_value;
        }

        /// <summary>
        /// Returns the associated axis.
        /// </summary>
        /// <param name="p_field"></param>
        /// <param name="p_default"></param>
        /// <returns></returns>
        public double GetAxis(MAVLinkInputField p_field,bool p_mid,double p_default=0) {
            if (axis == null) return p_default;
            if (!m_field_lut.ContainsKey(p_field)) return p_default;
            int idx = m_field_lut[p_field];
            if(idx<0)             return p_default;
            if(idx>=axis.Length)  return p_default;
            double v = axis[idx];
            if (p_mid) v = (v-0.5)/0.5;
            double d = Math.Max(deadzones[idx],0);
            double s = v < 0 ? -1 : 1;
            v = ((Math.Abs(v) - d) / (1.0 - d));
            v = v < 0 ? 0 : (v > 1 ? 1 : v);
            return v*s;
        }

        /// <summary>
        /// Returns the associated axis
        /// </summary>
        /// <param name="p_field"></param>
        /// <param name="p_default"></param>
        /// <returns></returns>
        public double GetAxis(MAVLinkInputField p_field,double p_default = 0) { return GetAxis(p_field,false,p_default); }

        /// <summary>
        /// Returns the associated button.
        /// </summary>
        /// <param name="p_field"></param>
        /// <param name="p_default"></param>
        /// <returns></returns>
        public bool Getbutton(MAVLinkInputField p_field,bool p_default = false) {
            if (buttons == null) return p_default;
            if (!m_field_lut.ContainsKey(p_field)) return p_default;
            int idx = m_field_lut[p_field];
            if (idx < 0) return p_default;
            if (idx >= axis.Length) return p_default;
            return buttons[idx];
        }

        /// <summary>
        /// Resizes the amount of axis
        /// </summary>
        /// <param name="p_length"></param>
        public void Resize(int p_length) {
            axis      = new double[p_length];
            deadzones = new double[p_length];
            buttons   = new bool[p_length];
            for (int i = 0;i < p_length;i++) axis[i] = deadzones[i] = 0;
            for (int i = 0;i < p_length;i++) buttons [i] = false;
        }

    }
    #endregion

    /// <summary>
    /// Class that implements a MAVLink most basic system, made of an id and signals the network its 'alive'
    /// </summary>
    public class MAVLinkSystem : MAVLinkLiveEntity {

        #region struct SensorStatus
        /// <summary>
        /// Struct to group sensor status flags
        /// </summary>
        public struct SensorStatus {
            /// <summary>
            /// Sensor Exists
            /// </summary>
            public MAV_SYS_STATUS_SENSOR available;
            /// <summary>
            /// Sensort is enabled
            /// </summary>
            public MAV_SYS_STATUS_SENSOR enabled;
            /// <summary>
            /// Sensor is healthy
            /// </summary>
            public MAV_SYS_STATUS_SENSOR healthy;

            /// <summary>
            /// Changes the bitmask
            /// </summary>
            /// <param name="f"></param>
            /// <param name="v"></param>
            public void SetAvailable(MAV_SYS_STATUS_SENSOR f,bool v) { available = v ? (available | f) : (available & ~f); }

            /// <summary>
            /// Changes the bitmask
            /// </summary>
            /// <param name="f"></param>
            /// <param name="v"></param>
            public void SetEnabled(MAV_SYS_STATUS_SENSOR f,bool v) { enabled = v ? (enabled | f) : (enabled & ~f); }

            /// <summary>
            /// Changes the bitmask
            /// </summary>
            /// <param name="f"></param>
            /// <param name="v"></param>
            public void SetHealthy(MAV_SYS_STATUS_SENSOR f,bool v) { healthy = v ? (healthy | f) : (healthy & ~f); }

        }
        #endregion

        #region class Status
        /// <summary>
        /// Class to group status flags
        /// </summary>
        public class Status {

            /// <summary>
            /// Sensors Status Flags
            /// </summary>
            public SensorStatus sensors;

            /// <summary>
            /// CTOR.
            /// </summary>
            public Status() {
                sensors = new SensorStatus() {
                    available = 0,
                    enabled   = 0,
                    healthy   = 0
                };
            }

        }
        #endregion

        /// <summary>
        /// System ID
        /// </summary>
        public byte id { get { return base.systemId; } private set { base.systemId = value; } }

        /// <summary>
        /// Autopilot enumeration
        /// </summary>
        public MAV_AUTOPILOT autopilot { get; set; }

        /// <summary>
        /// Autopilot enumeration
        /// </summary>
        public MAV_MODE mode { get; set; }

        /// <summary>
        /// flag that tells the system is armed.
        /// </summary>
        public bool armed { get { return ((byte)mode & (byte)MAV_MODE_FLAG.SAFETY_ARMED) != 0; } }

        /// <summary>
        /// Flag that tells this system is receiving actuators in lockstep mode.
        /// </summary>
        public bool lockstep { get; set; }

        /// <summary>
        /// Returns the isolated flight mode flag
        /// </summary>
        public MAV_MODE_FLAG flightMode { 
            get { 
                switch(mode) {
                    case MAV_MODE.AUTO_ARMED:      case MAV_MODE.AUTO_DISARMED:         return MAV_MODE_FLAG.AUTO_ENABLED;
                    case MAV_MODE.MANUAL_ARMED:    case MAV_MODE.MANUAL_DISARMED:       return MAV_MODE_FLAG.MANUAL_INPUT_ENABLED;
                    case MAV_MODE.STABILIZE_ARMED: case MAV_MODE.STABILIZE_DISARMED:    return MAV_MODE_FLAG.STABILIZE_ENABLED;
                    case MAV_MODE.GUIDED_ARMED:    case MAV_MODE.GUIDED_DISARMED:       return MAV_MODE_FLAG.GUIDED_ENABLED;
                    case MAV_MODE.TEST_ARMED:      case MAV_MODE.TEST_DISARMED:         return MAV_MODE_FLAG.TEST_ENABLED;
                    case MAV_MODE.PREFLIGHT: break;
                }
                return (MAV_MODE_FLAG)0;
            } 
        }

        /// <summary>
        /// Reference to the input data.
        /// </summary>
        public MAVLinkInput input { get; private set; }

        /// <summary>
        /// List of actuator values;
        /// </summary>
        public double[] actuators;

        /// <summary>
        /// Populates actuator values into the list.
        /// </summary>
        /// <param name="p_list"></param>
        public void SetActuators(double[] p_list) {
            int c = (p_list==null || actuators==null) ? 0 : Math.Min(p_list.Length, actuators.Length);
            for(int i=0;i< c;i++) p_list[i] = actuators[i];
        }

        /// <summary>
        /// Status Flags
        /// </summary>
        public Status status { get; private set; }

        /// <summary>
        /// Internals
        /// </summary>
        internal List<MAVLinkComponent> m_components;
        internal Dictionary<SensorChannel,MAVLinkSensor> m_sensor_change_lut;
        internal object m_sensor_change_lock;
        internal HIL_GPS_MSG              hil_gps_d;
        internal HIL_STATE_QUATERNION_MSG hil_quat_d;
        internal HIL_SENSOR_MSG           hil_sensor_d;
        internal float[]                  hil_quat_v = new float[4];
        internal List<MAVLinkSensor>      batt_list;               
        internal bool                     lockstep_wait_actuator;        
        internal bool                     has_sensor;
        internal bool                     has_quat;
        internal bool                     has_gps;        
        
        /// <summary>
        /// CTOR.
        /// </summary>
        /// <param name="p_id"></param>
        /// <param name="p_name"></param>
        public MAVLinkSystem(byte p_id,MAV_TYPE p_type,string p_name="") : base(p_type,p_name) {
            id           = p_id;
            componentId  = (byte)MAV_COMPONENT.MAV_COMP_ID_AUTOPILOT1;
            m_components = new List<MAVLinkComponent>();
            m_sensor_change_lut  = new Dictionary<SensorChannel,MAVLinkSensor>();
            m_sensor_change_lock = new object();
            alive        = true;
            mode         = (MAV_MODE)0;
            autopilot    = MAV_AUTOPILOT.GENERIC;
            status       = new Status();            
            
            lockstep_wait_actuator = false;
            
            input     = new MAVLinkInput(16);
            actuators = new double[16];

            //Thre is no 'fields updated' so we iteratively change fields and re-use the struct
            hil_gps_d = new HIL_GPS_MSG() {
                time_usec = 0,
                cog = ushort.MaxValue,
                eph = ushort.MaxValue,
                epv = ushort.MaxValue,
                vel = ushort.MaxValue,
                satellites_visible = 1,
                fix_type = 3
            };
            //Thre is no 'fields updated' so we iteratively change fields and re-use the struct
            hil_quat_d = new HIL_STATE_QUATERNION_MSG() {
                time_usec = 0,
                attitude_quaternion = new float[4],                
            };
            //Keep the past sensor data
            hil_sensor_d = new HIL_SENSOR_MSG() {
                time_usec=0
            };

            //Create battery changed list
            batt_list = new List<MAVLinkSensor>();
        }

        #region Components
        /// <summary>
        /// Returns the number of available components
        /// </summary>
        public int componentCount { get { return m_components.Count; } }

        /// <summary>
        /// Returns an entity by its index.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="p_index"></param>
        /// <returns></returns>
        public T GetComponent<T>(int p_index) where T : MAVLinkComponent { return (p_index < 0 ? default(T) : (p_index >= componentCount ? default(T) : (T)m_components[p_index])); }

        /// <summary>
        /// Returns a MAVLinkSystem by its id
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="p_id"></param>
        /// <returns></returns>
        public T GetComponentById<T>(MAV_COMPONENT p_id) where T : MAVLinkComponent {
            for (int i = 0;i < m_components.Count;i++) {
                MAVLinkComponent it = m_components[i];
                if (!(it is T)) continue;
                T it_cmp = (T)it;
                if (it_cmp == null) continue;
                if (it_cmp.id == p_id) return it_cmp;
            }
            return null;
        }

        /// <summary>
        /// Handler for component remove
        /// </summary>
        /// <param name="p_component"></param>
        internal void ComponentRemove(MAVLinkComponent p_component) {
            MAVLinkComponent       c  = p_component;
            List<MAVLinkComponent> cl = m_components;
            if (c == null) return;
            //Remove from list
            if (cl.Contains(c)) cl.Remove(c);
            //Unlink from graph relationship
            c.Unlink(c.m_system);
            //Invalidate system and id
            c.m_system = null;
            c.systemId = 0;
            //Update sensor status in the system (to not part of it)
            if (c is MAVLinkSensor) OnSensorStatusChanged(c as MAVLinkSensor);
        }

        /// <summary>
        /// Handler for component add
        /// </summary>
        /// <param name="p_component"></param>
        internal void ComponentAdd(MAVLinkComponent p_component) {
            MAVLinkComponent       c  = p_component;
            List<MAVLinkComponent> cl = m_components;
            if (c == null) return;
            //Adds the component
            if (!cl.Contains(c)) cl.Add(p_component);
            //Set its system
            c.m_system = this;
            //Link into the system graph and assign the id
            c.Link(c.m_system);
            c.systemId = id;
            //Notify the system
            if (c is MAVLinkSensor) OnSensorStatusChanged(c as MAVLinkSensor);
        }

        /// <summary>
        /// Handler for when the sensor changed its status
        /// </summary>
        /// <param name="p_sensor"></param>
        internal void OnSensorStatusChanged(MAVLinkSensor p_sensor) { }

        /// <summary>
        /// Handler called when sensor data has undergone changes
        /// </summary>
        /// <param name="p_sensor"></param>
        /// <param name="p_channel"></param>
        internal void OnSensorChannelChanged(MAVLinkSensor p_sensor,SensorChannel p_channel) {
            //Collects all changes
            lock(m_sensor_change_lock) {
                m_sensor_change_lut[p_channel] = p_sensor;
            }            
        }

        #endregion

        #region MAVLink
        /// <summary>
        /// Sets the system mode.
        /// </summary>
        /// <param name="p_flag"></param>
        public void SetMode(MAV_MODE p_flag) {
            COMMAND_LONG_MSG d = new COMMAND_LONG_MSG() {
                command       = (byte)MAV_CMD.DO_SET_MODE,
                param1        = (byte)p_flag                
            };
            MAVLinkMessage msg = CreateMessage(MSG_ID.COMMAND_LONG,d,false,systemId,componentId);
            mode = p_flag;
            Send(msg);
        }

        /// <summary>
        /// Handler to intercept a heartbeat and modify it before sending
        /// </summary>
        /// <param name="p_message"></param>
        protected override void OnHeartbeat(ref HEARTBEAT_MSG p_message) {
            p_message.base_mode = (byte)mode;
            p_message.autopilot = (byte)autopilot;
        }

        /// <summary>
        /// Handler for messages coming to this system
        /// </summary>
        /// <param name="p_caller"></param>
        /// <param name="p_msg"></param>
        override protected void OnMessage(MAVLinkEntity p_caller,MAVLinkMessage p_msg) {
            //Send msg to base
            base.OnMessage(p_caller,p_msg);

            switch ((MSG_ID)p_msg.msgid) {

                case MSG_ID.HIL_ACTUATOR_CONTROLS: {
                    //Lockstep enabled and actuator just sennt, skip
                    if (lockstep) if (!lockstep_wait_actuator) break;                    
                    HIL_ACTUATOR_CONTROLS_MSG d = (HIL_ACTUATOR_CONTROLS_MSG)p_msg.data;
                    mode     = (MAV_MODE)d.mode;
                    lockstep = (d.flags & 0x1) != 0;
                    ulong t_us = network == null ? 0 : network.clock.elapsedUS;
                    int c = Math.Min(d.controls.Length,actuators.Length);
                    for(int i=0;i<c;i++) actuators[i] = d.controls[i];
                    lockstep_wait_actuator = false;

                    UnityEngine.Debug.Log($"[{d.time_usec/1000}ms] hil_actuators");
                }
                break;

                case MSG_ID.MANUAL_CONTROL: {
                    MANUAL_CONTROL_MSG d = (MANUAL_CONTROL_MSG)p_msg.data;
                    double v;
                    double x, y, z, r;
                    v = 0f; if (d.x != short.MaxValue) v = ((double)d.x) / 1000.0; x = (float)v;
                    v = 0f; if (d.y != short.MaxValue) v = ((double)d.y) / 1000.0; y = (float)v;
                    v = 0f; if (d.z != short.MaxValue) v = ((double)d.z) / 1000.0; z = (float)v;
                    v = 0f; if (d.r != short.MaxValue) v = ((double)d.r) / 1000.0; r = (float)v;

                    int k = 0;

                    input.axis[k++] = x;
                    input.axis[k++] = y;
                    input.axis[k++] = z;
                    input.axis[k++] = r;

                    ushort msk = d.buttons;
                    int c = Math.Min(input.buttons.Length,16);
                    for (int i = 0;i < c;i++) {
                        bool f = (msk & 1) != 0;
                        msk = (ushort)(msk >> 1);
                        input.buttons[i] = f;
                    }

                }
                break;

                case MSG_ID.COMMAND_LONG: {
                    COMMAND_LONG_MSG cmd_d = (COMMAND_LONG_MSG)p_msg.data;
                    //switch((MAV_CMD)cmd_d.command) { }
                }
                break;
            }

            //Check msg sysid and skip if not matching
            //Validate COMMAND messages                
            byte t_sys = 0; //Target SysId
            byte t_cmp = 0; //Target CompId
            //Fetch targets from message
            GetMessageTargets(p_msg,out t_sys,out t_cmp);            
            //Iterate components and call the message handler if matching sys/comp ids
            for(int i=0;i<m_components.Count;i++) {
                MAVLinkComponent it = m_components[i];
                if(it== null) continue;
                //Skip non matching component id
                //Block handling if not matching (if targets are 0 then its broadcast)
                if (it.systemId    > 0) if (t_sys > 0) if (t_sys != it.systemId   ) continue;
                if (it.componentId > 0) if (t_cmp > 0) if (t_cmp != it.componentId) continue;
                it.OnSystemMessageInternal(p_msg);
            }
            
        }

        #endregion

        /// <summary>
        /// Special case for System and Components where components updates are related to the owner system
        /// </summary>
        override public void Update() {
            base.Update();
            for (int i = 0;i < m_components.Count;i++) {
                if(m_components[i]!=null) m_components[i].Update();
            }
        }

        /// <summary>
        /// Internal update loop
        /// </summary>
        protected override void OnUpdate() {

            base.OnUpdate();

            HIL_SENSOR_MSG           l_hil_sensors   = default;
            HIL_GPS_MSG              l_hil_gps       = default;
            HIL_STATE_QUATERNION_MSG l_hil_quat      = default;

            //Global microsseconds
            ulong t_us = network == null ? 0 : network.clock.elapsedUS;
            //if(lockstep_clock_us>0) t_us = lockstep_clock_us;
            //Assign current state
            l_hil_sensors = hil_sensor_d; l_hil_sensors.time_usec = t_us; 
            l_hil_gps     = hil_gps_d   ; l_hil_gps    .time_usec = t_us;
            l_hil_quat    = hil_quat_d  ; l_hil_quat   .time_usec = t_us;

            has_sensor   = false;
            has_gps      = false;
            has_quat     = false;

            //Clear changed batteries
            batt_list.Clear();

            //Init not sending hil data
            bool send_hil = false;

            //UnityEngine.Debug.Log($"[{t_us / 1000}ms] system update");

            lock (m_sensor_change_lock) {
                //If there are sensor changes
                if(m_sensor_change_lut.Count > 0) {

                    //Reset bit flags
                    l_hil_sensors.fields_updated = 0;

                    //Iterate sensor's channel changes
                    foreach (KeyValuePair<SensorChannel,MAVLinkSensor> it in m_sensor_change_lut) {
                        SensorChannel sch = it.Key;
                        MAVLinkSensor            s   = it.Value;

                        #region HIL Sensor Flags/Bits
                        //Update Change Flags and Dirty Bits
                        switch (sch) {
                            case SensorChannel.AccelX                 : { l_hil_sensors.fields_updated |= (uint)HIL_SENSORS_UPDATED.XACC;          has_sensor = has_quat = true; } break;
                            case SensorChannel.AccelY                 : { l_hil_sensors.fields_updated |= (uint)HIL_SENSORS_UPDATED.YACC;          has_sensor = has_quat = true; } break;
                            case SensorChannel.AccelZ                 : { l_hil_sensors.fields_updated |= (uint)HIL_SENSORS_UPDATED.ZACC;          has_sensor = has_quat = true; } break;                                                                                                                                                  
                            case SensorChannel.GyroX                  : { l_hil_sensors.fields_updated |= (uint)HIL_SENSORS_UPDATED.XGYRO;         has_sensor = has_quat = true; } break;
                            case SensorChannel.GyroY                  : { l_hil_sensors.fields_updated |= (uint)HIL_SENSORS_UPDATED.YGYRO;         has_sensor = has_quat = true; } break;
                            case SensorChannel.GyroZ                  : { l_hil_sensors.fields_updated |= (uint)HIL_SENSORS_UPDATED.ZGYRO;         has_sensor = has_quat = true; } break;                                                                                                                                                  
                            case SensorChannel.MagnetometerX          : { l_hil_sensors.fields_updated |= (uint)HIL_SENSORS_UPDATED.XMAG;          has_sensor = true;            } break;
                            case SensorChannel.MagnetometerY          : { l_hil_sensors.fields_updated |= (uint)HIL_SENSORS_UPDATED.YMAG;          has_sensor = true;            } break;
                            case SensorChannel.MagnetometerZ          : { l_hil_sensors.fields_updated |= (uint)HIL_SENSORS_UPDATED.ZMAG;          has_sensor = true;            } break;                            
                            case SensorChannel.PressureAbsolute       : { l_hil_sensors.fields_updated |= (uint)HIL_SENSORS_UPDATED.ABS_PRESSURE;  has_sensor = true; } break;
                            case SensorChannel.PressureAltitude       : { l_hil_sensors.fields_updated |= (uint)HIL_SENSORS_UPDATED.PRESSURE_ALT;  has_sensor = true; } break;
                            case SensorChannel.PressureDifferential   : { l_hil_sensors.fields_updated |= (uint)HIL_SENSORS_UPDATED.DIFF_PRESSURE; has_sensor = true; } break;
                            case SensorChannel.Temperature            : { l_hil_sensors.fields_updated |= (uint)HIL_SENSORS_UPDATED.TEMPERATURE;   has_sensor = true; } break;

                            case SensorChannel.YawSpeed               : { has_quat = true; } break;
                            case SensorChannel.PitchSpeed             : { has_quat = true; } break;
                            case SensorChannel.RollSpeed              : { has_quat = true; } break;
                            case SensorChannel.QuatW                  : { has_quat = true; } break;
                            case SensorChannel.QuatX                  : { has_quat = true; } break;
                            case SensorChannel.QuatY                  : { has_quat = true; } break;
                            case SensorChannel.QuatZ                  : { has_quat = true; } break;
                            case SensorChannel.AirspeedIndicated      : { has_quat = true; } break;
                            case SensorChannel.AirSpeedTrue           : { has_quat = true; } break;

                            case SensorChannel.LatitudeWGS            : { has_gps = has_quat = true; } break;
                            case SensorChannel.LongitudeWGS           : { has_gps = has_quat = true; } break;
                            case SensorChannel.AltitudeGPS            : { has_gps = has_quat = true; } break;
                            case SensorChannel.VelocityNorth          : { has_gps = has_quat = true; } break;
                            case SensorChannel.VelocityEast           : { has_gps = has_quat = true; } break;
                            case SensorChannel.VelocityDown           : { has_gps = has_quat = true; } break;
                            case SensorChannel.GroundSpeedGPS         : { has_gps = true;            } break;
                            case SensorChannel.CourseOverGround       : { has_gps = true;            } break;
                            case SensorChannel.FixType                : { has_gps = true;            } break;
                            case SensorChannel.SatelliteVisible       : { has_gps = true;            } break;
                            case SensorChannel.HDOP                   : { has_gps = true;            } break;
                            case SensorChannel.VDOP                   : { has_gps = true;            } break;
                        }
                        #endregion

                        #region HIL_SENSOR
                        //Update HIL Sensors
                        switch (sch) {
                            case SensorChannel.AccelX                 : { l_hil_sensors.xacc           = s.ReadFloat(sch); } break;
                            case SensorChannel.AccelY                 : { l_hil_sensors.yacc           = s.ReadFloat(sch); } break;
                            case SensorChannel.AccelZ                 : { l_hil_sensors.zacc           = s.ReadFloat(sch); } break;                            
                            case SensorChannel.GyroX                  : { l_hil_sensors.xgyro          = s.ReadFloat(sch); } break;
                            case SensorChannel.GyroY                  : { l_hil_sensors.ygyro          = s.ReadFloat(sch); } break;
                            case SensorChannel.GyroZ                  : { l_hil_sensors.zgyro          = s.ReadFloat(sch); } break;                            
                            case SensorChannel.MagnetometerX          : { l_hil_sensors.xmag           = s.ReadFloat(sch); } break;
                            case SensorChannel.MagnetometerY          : { l_hil_sensors.ymag           = s.ReadFloat(sch); } break;
                            case SensorChannel.MagnetometerZ          : { l_hil_sensors.zmag           = s.ReadFloat(sch); } break;                            
                            case SensorChannel.PressureAbsolute       : { l_hil_sensors.abs_pressure   = s.ReadFloat(sch); } break;
                            case SensorChannel.PressureAltitude       : { l_hil_sensors.pressure_alt   = s.ReadFloat(sch); } break;
                            case SensorChannel.PressureDifferential   : { l_hil_sensors.diff_pressure  = s.ReadFloat(sch); } break;
                            case SensorChannel.Temperature            : { l_hil_sensors.temperature    = s.ReadFloat(sch); } break;
                        }
                        #endregion

                        #region HIL_GPS
                        //Update HIL GPS
                        switch (sch) {
                            case SensorChannel.LatitudeWGS            : { l_hil_gps.lat                  = s.ReadInt   (sch);   } break;
                            case SensorChannel.LongitudeWGS           : { l_hil_gps.lon                  = s.ReadInt   (sch);   } break;
                            case SensorChannel.AltitudeGPS            : { l_hil_gps.alt                  = s.ReadInt   (sch);   } break;
                            case SensorChannel.GroundSpeedGPS         : { l_hil_gps.vel                  = s.ReadUShort(sch);   } break;
                            case SensorChannel.CourseOverGround       : { l_hil_gps.cog                  = s.ReadUShort(sch);   } break;
                            case SensorChannel.FixType                : { l_hil_gps.fix_type             = s.ReadByte  (sch);   } break;
                            case SensorChannel.SatelliteVisible       : { l_hil_gps.satellites_visible   = s.ReadByte  (sch);   } break;
                            case SensorChannel.HDOP                   : { l_hil_gps.eph                  = s.ReadUShort(sch);   } break;
                            case SensorChannel.VDOP                   : { l_hil_gps.epv                  = s.ReadUShort(sch);   } break;
                            case SensorChannel.VelocityNorth          : { l_hil_gps.vn                   = s.ReadShort (sch);   } break;
                            case SensorChannel.VelocityEast           : { l_hil_gps.ve                   = s.ReadShort (sch);   } break;
                            case SensorChannel.VelocityDown           : { l_hil_gps.vd                   = s.ReadShort (sch);   } break;
                        }
                        #endregion

                        #region HIL_STATE_QUATERNION
                        //Update HIL Quaternion
                        switch (sch) {
                            case SensorChannel.AccelX                 : { l_hil_quat.xacc                   = (short)(l_hil_sensors.xacc*1000f/9.81f);  } break; //mG
                            case SensorChannel.AccelY                 : { l_hil_quat.yacc                   = (short)(l_hil_sensors.yacc*1000f/9.81f);  } break; //mG
                            case SensorChannel.AccelZ                 : { l_hil_quat.zacc                   = (short)(l_hil_sensors.zacc*1000f/9.81f);  } break; //mG                        
                            case SensorChannel.GyroX                  : { l_hil_quat.rollspeed              = l_hil_sensors.xgyro;                      } break;
                            case SensorChannel.GyroY                  : { l_hil_quat.pitchspeed             = l_hil_sensors.ygyro;                      } break;
                            case SensorChannel.GyroZ                  : { l_hil_quat.yawspeed               = l_hil_sensors.zgyro;                      } break;                            
                            case SensorChannel.QuatW                  : { l_hil_quat.attitude_quaternion[0] = s.ReadFloat(sch);                         } break;
                            case SensorChannel.QuatX                  : { l_hil_quat.attitude_quaternion[1] = s.ReadFloat(sch);                         } break;
                            case SensorChannel.QuatY                  : { l_hil_quat.attitude_quaternion[2] = s.ReadFloat(sch);                         } break;
                            case SensorChannel.QuatZ                  : { l_hil_quat.attitude_quaternion[3] = s.ReadFloat(sch);                         } break;
                            case SensorChannel.AirspeedIndicated      : { l_hil_quat.ind_airspeed           = (ushort)s.ReadFloat(sch);                 } break;
                            case SensorChannel.AirSpeedTrue           : { l_hil_quat.true_airspeed          = (ushort)s.ReadFloat(sch);                 } break;
                            case SensorChannel.LatitudeWGS            : { l_hil_quat.lat                    = l_hil_gps.lat;                            } break;
                            case SensorChannel.LongitudeWGS           : { l_hil_quat.lon                    = l_hil_gps.lon;                            } break;
                            case SensorChannel.AltitudeGPS            : { l_hil_quat.alt                    = l_hil_gps.alt;                            } break;
                            case SensorChannel.VelocityNorth          : { l_hil_quat.vx                     = l_hil_gps.vn;                             } break;
                            case SensorChannel.VelocityEast           : { l_hil_quat.vy                     = l_hil_gps.ve;                             } break;
                            case SensorChannel.VelocityDown           : { l_hil_quat.vz                     = l_hil_gps.vd;                             } break;
                        }
                        #endregion

                        /*
                        case SensorChannel.BatteryId:
                        case SensorChannel.BatteryType:
                        case SensorChannel.BatteryFunction:
                        case SensorChannel.BatteryCurrent:
                        case SensorChannel.BatteryCurrentConsumed:
                        case SensorChannel.BatteryEnergyConsumed:
                        case SensorChannel.BatteryVoltage:
                        case SensorChannel.BatteryTemperature:
                        case SensorChannel.BatteryChargeState:
                        case SensorChannel.BatteryTimeRemaining:
                        case SensorChannel.BatteryRemaining: {
                            if (batt_list.Contains(it.Value)) break;
                            batt_list.Add(it.Value);
                        }
                        break;
                        //*/

                    }
                }
                //Re-assign struct with new values (or same if no changes)
                hil_quat_d   = l_hil_quat;
                hil_gps_d    = l_hil_gps;
                hil_sensor_d = l_hil_sensors;

                //If channels were written, send hil msg
                send_hil = has_gps || has_quat || has_sensor;

                //Clear the sensor changes
                m_sensor_change_lut.Clear();
            }

            MAVLinkMessage msg = null;

            //If lockstep enabled and waiting for actuators, skip sensors
            if(lockstep) if (lockstep_wait_actuator) { send_hil = false; }

            if (send_hil) {

                if (has_sensor) {                    
                    msg = CreateMessage(MSG_ID.HIL_SENSOR,hil_sensor_d,false,id,componentId);
                    Send(msg); 
                    has_sensor = false;
                    UnityEngine.Debug.Log($"[{hil_sensor_d.time_usec / 1000}ms] hil_sensors");
                }
                //if (has_quat  ) {  msg = CreateMessage(MSG_ID.HIL_STATE_QUATERNION,l_hil_quat    ,false,id,componentId); Send(msg); has_quat   = false;  }                
                //lockstep_log = $"[{lockstep_frame}][{l_hil_sensors.time_usec / 1000}ms] SENSOR -> ";
                

                if (has_gps) { 
                    msg = CreateMessage(MSG_ID.HIL_GPS,hil_gps_d,false,id,componentId); 
                    Send(msg); 
                    has_gps = false;
                    UnityEngine.Debug.Log($"[{hil_sensor_d.time_usec / 1000}ms] hil_gps");
                }

                //Send sensors wait for actuators.
                lockstep_wait_actuator = true;

            }

            /*
            for(int i=0;i<batt_list.Count;i++) {
                MAVLinkSensor it = batt_list[i];
                BATTERY_STATUS_MSG d = new BATTERY_STATUS_MSG() {
                    id                  = it.ReadByte (SensorChannel.BatteryId          ),
                    battery_function    = it.ReadByte (SensorChannel.BatteryFunction    ),
                    type                = it.ReadByte (SensorChannel.BatteryType        ), 
                    battery_remaining   = it.ReadSByte(SensorChannel.BatteryRemaining   ),
                    temperature         = it.ReadShort(SensorChannel.BatteryTemperature ),
                    voltages            = (ushort[])it.Read(SensorChannel.BatteryVoltage),
                    current_battery     = -1,
                    current_consumed    = -1,
                    energy_consumed     = -1,
                    charge_state        =  1,   
                    time_remaining      = -1
                };
                //msg = CreateMessage(MSG_ID.BATTERY_STATUS,d,false,id,componentId); Send(msg);                
            }
            //*/
        }

    }
}
