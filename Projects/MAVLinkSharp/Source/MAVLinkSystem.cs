using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MAVLink;

#pragma warning disable CS8600
#pragma warning disable CS8602
#pragma warning disable CS8603
#pragma warning disable CS8604
#pragma warning disable CS8618
#pragma warning disable CS8625

namespace MAVConsole {

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
        /// Status Flags
        /// </summary>
        public Status status { get; private set; }

        /// <summary>
        /// Flag that tells if lock step must be applied.
        /// </summary>
        public bool lockstep;

        /// <summary>
        /// Internals
        /// </summary>
        internal List<MAVLinkComponent> m_components;
        internal Dictionary<SensorChannel,MAVLinkSensor> m_sensor_change_lut;
        internal object m_sensor_change_lock;
        internal HIL_GPS_MSG              hil_gps_d;        
        internal HIL_STATE_QUATERNION_MSG hil_quat_d;        
        internal float[]                  hil_quat_v = new float[4];
        internal List<MAVLinkSensor>      batt_list;
        internal int                      lockstep_wait_actuator;
        internal int                      lockstep_frame;
        internal bool                     lockstep_actuator_active;
        internal double                   lockstep_delay;
        
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
            lockstep     = true;
            lockstep_wait_actuator       = 1; 
            lockstep_actuator_active     = false;
            lockstep_delay               = 5.0;


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
        internal void OnSensortChannelChanged(MAVLinkSensor p_sensor,SensorChannel p_channel) {
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
                    /*
                    lockstep_actuator_active = true;
                    if (lockstep_wait_actuator > 0) break;
                    lockstep_wait_actuator = 16;
                    //Console.WriteLine($"[{lockstep_wait_actuator,3}][{lockstep_frame}] ACTUATOR");
                    lockstep_frame++;
                    //*/
                }
                break;

                case MSG_ID.COMMAND_LONG: {
                    COMMAND_LONG_MSG cmd_d = (COMMAND_LONG_MSG)p_msg.data;
                    //switch((MAV_CMD)cmd_d.command) { }
                }
                break;
            }

            //Check msg sysid and skip if not matching
            byte sys_id = p_msg.sysid;            
            if(sys_id>0) if (sys_id != id) return;            
            byte cmp_id = p_msg.compid;
            for(int i=0;i<m_components.Count;i++) {
                MAVLinkComponent it = m_components[i];
                if(it== null) continue;
                //Skip non matching component id
                if (cmp_id > 0) if ((byte)it.id != cmp_id) continue;
                it.OnMessageInternal(p_caller,p_msg);
            }
            
        }

        #endregion

        /// <summary>
        /// Special case for System and Components where components updates are related to the owner system
        /// </summary>
        override internal void Update() {
            base.Update();
            for (int i = 0;i < m_components.Count;i++) m_components[i].Update();
        }

        /// <summary>
        /// Internal update loop
        /// </summary>
        protected override void OnUpdate() {

            base.OnUpdate();

            HIL_SENSOR_MSG           l_hil_sensors   = default;
            HIL_GPS_MSG              l_hil_gps       = default;
            HIL_STATE_QUATERNION_MSG l_hil_quat      = default;
            
            bool has_sensor   = false;            
            bool has_gps      = false;
            bool has_quat     = false;
            

            //Clear changed batteries
            batt_list.Clear();

            lock (m_sensor_change_lock) {
                //If there are sensor changes
                if(m_sensor_change_lut.Count > 0) {
                    //Global microsseconds
                    ulong t_us = network == null ? 0 : network.clock.elapsedUS;
                    //For sensors create empty and flag the changes
                    l_hil_sensors  = new HIL_SENSOR_MSG()  { time_usec = t_us, fields_updated = 0 };
                    //For others re-use latest sample
                    l_hil_gps   = hil_gps_d;  l_hil_gps .time_usec = t_us;
                    l_hil_quat  = hil_quat_d; l_hil_quat.time_usec = t_us;
                    //Iterate sensor's channel changes
                    foreach (KeyValuePair<SensorChannel,MAVLinkSensor> it in m_sensor_change_lut) {
                        SensorChannel sch = it.Key;
                        MAVLinkSensor            s   = it.Value;
                        switch(sch) {
                            case SensorChannel.AccelerometerX         : { l_hil_sensors.xacc           = s.ReadFloat(sch); l_hil_quat.xacc = (short)(l_hil_sensors.xacc*1000f/9.81f); l_hil_sensors.fields_updated |= (uint)HIL_SENSORS_UPDATED.XACC;   has_sensor = has_quat = true; } break;
                            case SensorChannel.AccelerometerY         : { l_hil_sensors.yacc           = s.ReadFloat(sch); l_hil_quat.yacc = (short)(l_hil_sensors.yacc*1000f/9.81f); l_hil_sensors.fields_updated |= (uint)HIL_SENSORS_UPDATED.YACC;   has_sensor = has_quat = true; } break;
                            case SensorChannel.AccelerometerZ         : { l_hil_sensors.zacc           = s.ReadFloat(sch); l_hil_quat.zacc = (short)(l_hil_sensors.zacc*1000f/9.81f); l_hil_sensors.fields_updated |= (uint)HIL_SENSORS_UPDATED.ZACC;   has_sensor = has_quat = true; } break;
                            
                            case SensorChannel.GyroscopeX             : { l_hil_sensors.xgyro          = s.ReadFloat(sch); l_hil_sensors.fields_updated |= (uint)HIL_SENSORS_UPDATED.XGYRO;         has_sensor = true; } break;
                            case SensorChannel.GyroscopeY             : { l_hil_sensors.ygyro          = s.ReadFloat(sch); l_hil_sensors.fields_updated |= (uint)HIL_SENSORS_UPDATED.YGYRO;         has_sensor = true; } break;
                            case SensorChannel.GyroscopeZ             : { l_hil_sensors.zgyro          = s.ReadFloat(sch); l_hil_sensors.fields_updated |= (uint)HIL_SENSORS_UPDATED.ZGYRO;         has_sensor = true; } break;
                            
                            case SensorChannel.MagnetometerX          : { l_hil_sensors.xmag           = s.ReadFloat(sch); l_hil_sensors.fields_updated |= (uint)HIL_SENSORS_UPDATED.XMAG;          has_sensor = true; } break;
                            case SensorChannel.MagnetometerY          : { l_hil_sensors.ymag           = s.ReadFloat(sch); l_hil_sensors.fields_updated |= (uint)HIL_SENSORS_UPDATED.YMAG;          has_sensor = true; } break;
                            case SensorChannel.MagnetometerZ          : { l_hil_sensors.zmag           = s.ReadFloat(sch); l_hil_sensors.fields_updated |= (uint)HIL_SENSORS_UPDATED.ZMAG;          has_sensor = true; } break;
                            
                            case SensorChannel.PressureAbsolute       : { l_hil_sensors.abs_pressure   = s.ReadFloat(sch); l_hil_sensors.fields_updated |= (uint)HIL_SENSORS_UPDATED.ABS_PRESSURE;  has_sensor = true; } break;
                            case SensorChannel.PressureAltitude       : { l_hil_sensors.pressure_alt   = s.ReadFloat(sch); l_hil_sensors.fields_updated |= (uint)HIL_SENSORS_UPDATED.PRESSURE_ALT;  has_sensor = true; } break;
                            case SensorChannel.PressureDifferential   : { l_hil_sensors.diff_pressure  = s.ReadFloat(sch); l_hil_sensors.fields_updated |= (uint)HIL_SENSORS_UPDATED.DIFF_PRESSURE; has_sensor = true; } break;

                            case SensorChannel.Temperature            : { l_hil_sensors.temperature    = s.ReadFloat(sch); l_hil_sensors.fields_updated |= (uint)HIL_SENSORS_UPDATED.TEMPERATURE;   has_sensor = true; } break;

                            case SensorChannel.YawSpeed               : { l_hil_quat.yawspeed               = s.ReadFloat(sch); has_quat = true; } break;
                            case SensorChannel.PitchSpeed             : { l_hil_quat.pitchspeed             = s.ReadFloat(sch); has_quat = true; } break;
                            case SensorChannel.RollSpeed              : { l_hil_quat.rollspeed              = s.ReadFloat(sch); has_quat = true; } break;
                            case SensorChannel.QuatW                  : { l_hil_quat.attitude_quaternion[0] = s.ReadFloat(sch); has_quat = true; } break;
                            case SensorChannel.QuatX                  : { l_hil_quat.attitude_quaternion[1] = s.ReadFloat(sch); has_quat = true; } break;
                            case SensorChannel.QuatY                  : { l_hil_quat.attitude_quaternion[2] = s.ReadFloat(sch); has_quat = true; } break;
                            case SensorChannel.QuatZ                  : { l_hil_quat.attitude_quaternion[3] = s.ReadFloat(sch); has_quat = true; } break;
                            case SensorChannel.AirspeedIndicated      : { l_hil_quat.ind_airspeed           = (ushort)s.ReadFloat(sch); has_quat = true; } break;
                            case SensorChannel.AirSpeedTrue           : { l_hil_quat.true_airspeed          = (ushort)s.ReadFloat(sch); has_quat = true; } break;

                            case SensorChannel.LatitudeWGS            : { l_hil_gps.lat                  = l_hil_quat.lat = s.ReadInt   (sch);    has_gps = has_quat = true; } break;
                            case SensorChannel.LongitudeWGS           : { l_hil_gps.lon                  = l_hil_quat.lon = s.ReadInt   (sch);    has_gps = has_quat = true; } break;
                            case SensorChannel.AltitudeGPS            : { l_hil_gps.alt                  = l_hil_quat.alt = s.ReadInt   (sch);    has_gps = has_quat = true; } break;
                            case SensorChannel.GroundSpeedGPS         : { l_hil_gps.vel                  = s.ReadUShort(sch);    has_gps = true; } break;
                            case SensorChannel.CourseOverGround       : { l_hil_gps.cog                  = s.ReadUShort(sch);    has_gps = true; } break;
                            case SensorChannel.FixType                : { l_hil_gps.fix_type             = s.ReadByte  (sch);    has_gps = true; } break;
                            case SensorChannel.SatelliteVisible       : { l_hil_gps.satellites_visible   = s.ReadByte  (sch);    has_gps = true; } break;
                            case SensorChannel.HDOP                   : { l_hil_gps.eph                  = s.ReadUShort(sch);    has_gps = true; } break;
                            case SensorChannel.VDOP                   : { l_hil_gps.epv                  = s.ReadUShort(sch);    has_gps = true; } break;
                            case SensorChannel.VelocityNorth          : { l_hil_gps.vn                   = l_hil_quat.vx = s.ReadShort (sch);    has_gps = has_quat = true; } break;
                            case SensorChannel.VelocityEast           : { l_hil_gps.ve                   = l_hil_quat.vy = s.ReadShort (sch);    has_gps = has_quat = true; } break;
                            case SensorChannel.VelocityDown           : { l_hil_gps.vd                   = l_hil_quat.vz = s.ReadShort (sch);    has_gps = has_quat = true; } break;

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
                    //Re-assign struct with new values
                    hil_quat_d = l_hil_quat;
                    hil_gps_d  = l_hil_gps;
                    
                }
                //Clear the sensor changes
                m_sensor_change_lut.Clear();
            }

            MAVLinkMessage msg = null;

            //Lock Step Gatekeep
            bool send_hil = true;
            //if(lockstep && lockstep_actuator_active) if(lockstep_wait_actuator<=0) { send_hil = false;  }
            //if (lockstep_actuator_active) if (lockstep_delay > 0.0) { lockstep_delay -= clock.deltaTime; send_hil = true; }

            if(send_hil) {
                if (has_sensor) {  msg = CreateMessage(MSG_ID.HIL_SENSOR          ,l_hil_sensors ,false,id,componentId); Send(msg);  }                            
                if (has_quat  ) {  msg = CreateMessage(MSG_ID.HIL_STATE_QUATERNION,l_hil_quat    ,false,id,componentId); Send(msg);  }
                if (has_gps   ) {  msg = CreateMessage(MSG_ID.HIL_GPS             ,l_hil_gps     ,false,id,componentId); Send(msg);  }
                //Console.WriteLine($"[{lockstep_wait_actuator,3}][{lockstep_frame}] SENSORS");
                //lockstep_wait_actuator--;
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
