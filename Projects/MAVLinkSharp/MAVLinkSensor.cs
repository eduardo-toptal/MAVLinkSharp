using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MAVLink;

#pragma warning disable CS8625

namespace MAVConsole {

    /// <summary>
    /// Enumeration that describes the data channel to be written by the sensor
    /// </summary>
    public enum SensorChannel {
        /// <summary>
        /// Invalid channel
        /// </summary>
        Invalid  = -1,
        __Accelerometer_   = 0,
        AccelerometerX,
        AccelerometerY,
        AccelerometerZ,
        __Gyro_            = __Accelerometer_+15,
        GyroscopeX,
        GyroscopeY,
        GyroscopeZ,
        __Attitude_        = __Gyro_+15,
        Yaw,
        Pitch,
        Roll,
        YawSpeed,
        PitchSpeed,
        RollSpeed,
        QuatW,
        QuatX,
        QuatY,
        QuatZ,
        AirspeedIndicated,
        AirSpeedTrue,
        __Altitude_        = __Attitude_ + 15,
        AltitudeMonotonic,
        AltitudeMSL,
        AltitudeLocal,
        AltitudeRelative,
        AltitudeTerrain,
        AltitudeBottom,
        __Magnetometer_    = __Altitude_ + 15,
        MagnetometerX,
        MagnetometerY,
        MagnetometerZ,
        __Ambient_         = __Magnetometer_+15,
        PressureAbsolute,
        PressureDifferential,
        PressureAltitude,
        Temperature,
        __GPS_             = __Ambient_+15,
        LatitudeWGS,
        LongitudeWGS,
        AltitudeGPS,
        HDOP,
        VDOP,
        GroundSpeedGPS,
        VelocityNorth,
        VelocityEast,
        VelocityDown,
        CourseOverGround,
        FixType,
        SatelliteVisible,
        __Battery_        = __GPS_+15,
        BatteryId,
        BatteryFunction,
        BatteryRemaining,
        BatteryChargeState,
        BatteryTimeRemaining,
        BatteryCurrent,
        BatteryCurrentConsumed,
        BatteryEnergyConsumed,
        BatteryTemperature,
        BatteryType,
        BatteryVoltage,
        /// <summary>
        /// Any other channel outside the predicted ones
        /// </summary>
        __Custom_ = 8192,
        Custom0,
        Custom1,
        Custom2,
        Custom3,
        Custom4,
        Custom5,
        Custom6,
        Custom7,
        Custom8,
        Custom9
    }

    /// <summary>
    /// Class that extends a component to implement sensors functionalities. Providing different channel data reported back to the owner system
    /// </summary>
    public class MAVLinkSensor : MAVLinkComponent {

        static public float Noise(float x,float s) {
            float n = (float)((rnd.NextDouble() - 0.5) * 2.0);
            return x + (n * s);
        }
        static Random rnd = new Random();

        /// <summary>
        /// This sensor's flag
        /// </summary>
        public MAV_SYS_STATUS_SENSOR flag;

        /// <summary>
        /// Flag that tells this sensor is active or not.
        /// </summary>
        public bool active {
            get { return m_active; }
            set { m_active = value; if (system != null) system.OnSensorStatusChanged(this); }
        }
        private bool m_active;

        /// <summary>
        /// Flag that tells this sensor is active or not.
        /// </summary>
        public bool healthy {
            get { return m_healthy; }
            set { m_healthy = value; if (system != null) system.OnSensorStatusChanged(this); }
        }
        private bool m_healthy;

        /// <summary>
        /// Internals
        /// </summary>
        protected Dictionary<SensorChannel,byte  > m_u8_lut;
        protected Dictionary<SensorChannel,sbyte > m_i8_lut;
        protected Dictionary<SensorChannel,ushort> m_u16_lut;
        protected Dictionary<SensorChannel,short > m_i16_lut;
        protected Dictionary<SensorChannel,uint  > m_u32_lut;
        protected Dictionary<SensorChannel,int   > m_i32_lut;
        protected Dictionary<SensorChannel,ulong > m_u64_lut;
        protected Dictionary<SensorChannel,long  > m_i64_lut;
        protected Dictionary<SensorChannel,float > m_f_lut;
        protected Dictionary<SensorChannel,double> m_d_lut;
        protected Dictionary<SensorChannel,object> m_obj_lut;

        /// <summary>
        /// CTOR.
        /// </summary>
        /// <param name="p_flag"></param>
        /// <param name="p_name"></param>
        public MAVLinkSensor(MAV_SYS_STATUS_SENSOR p_flag,MAV_COMPONENT p_id,MAV_TYPE p_type,string p_name="") : base(p_id,p_type,p_name) {
            flag      = p_flag;
            m_active  = true;
            m_healthy = true;

            m_u8_lut   = new Dictionary<SensorChannel,byte   >();
            m_i8_lut   = new Dictionary<SensorChannel,sbyte  >();
            m_u16_lut  = new Dictionary<SensorChannel,ushort >();
            m_i16_lut  = new Dictionary<SensorChannel,short  >();
            m_u32_lut  = new Dictionary<SensorChannel,uint   >();
            m_i32_lut  = new Dictionary<SensorChannel,int    >();
            m_u64_lut  = new Dictionary<SensorChannel,ulong  >();
            m_i64_lut  = new Dictionary<SensorChannel,long   >();
            m_f_lut    = new Dictionary<SensorChannel,float  >();
            m_d_lut    = new Dictionary<SensorChannel,double >();
            m_obj_lut  = new Dictionary<SensorChannel,object >();

        }

        /// <summary>
        /// Returns a flag telling if the channel is read/write by this sensor
        /// </summary>
        /// <param name="p_channel"></param>
        /// <returns></returns>
        public bool Contains(SensorChannel p_channel) {             
           if(m_u8_lut .ContainsKey(p_channel)) return true;
           if(m_i8_lut .ContainsKey(p_channel)) return true;
           if(m_u16_lut.ContainsKey(p_channel)) return true;
           if(m_i16_lut.ContainsKey(p_channel)) return true;
           if(m_u32_lut.ContainsKey(p_channel)) return true;
           if(m_i32_lut.ContainsKey(p_channel)) return true;
           if(m_u64_lut.ContainsKey(p_channel)) return true;
           if(m_i64_lut.ContainsKey(p_channel)) return true;
           if(m_f_lut  .ContainsKey(p_channel)) return true;
           if(m_d_lut  .ContainsKey(p_channel)) return true;
           if(m_obj_lut.ContainsKey(p_channel)) return true;
           return false;
        }

        /// <summary>
        /// Reads a channel data
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="p_channel"></param>
        /// <returns></returns>
        public byte   ReadByte  (SensorChannel p_channel,byte   p_default = 0)    { return m_u8_lut .ContainsKey(p_channel) ? (byte  )m_u8_lut [p_channel] : p_default; }
        public sbyte  ReadSByte (SensorChannel p_channel,sbyte  p_default = 0)    { return m_i8_lut .ContainsKey(p_channel) ? (sbyte )m_i8_lut [p_channel] : p_default; }
        public ushort ReadUShort(SensorChannel p_channel,ushort p_default = 0)    { return m_u16_lut.ContainsKey(p_channel) ? (ushort)m_u16_lut[p_channel] : p_default; }
        public short  ReadShort (SensorChannel p_channel,short  p_default = 0)    { return m_i16_lut.ContainsKey(p_channel) ? (short )m_i16_lut[p_channel] : p_default; }
        public uint   ReadUInt  (SensorChannel p_channel,uint   p_default = 0)    { return m_u32_lut.ContainsKey(p_channel) ? (uint  )m_u32_lut[p_channel] : p_default; }
        public int    ReadInt   (SensorChannel p_channel,int    p_default = 0)    { return m_i32_lut.ContainsKey(p_channel) ? (int   )m_i32_lut[p_channel] : p_default; }
        public ulong  ReadULong (SensorChannel p_channel,ulong  p_default = 0)    { return m_u64_lut.ContainsKey(p_channel) ? (ulong )m_u64_lut[p_channel] : p_default; }
        public long   ReadLong  (SensorChannel p_channel,long   p_default = 0)    { return m_i64_lut.ContainsKey(p_channel) ? (long  )m_i64_lut[p_channel] : p_default; }        
        public float  ReadFloat (SensorChannel p_channel,float  p_default = 0)    { return m_f_lut  .ContainsKey(p_channel) ? (float )m_f_lut  [p_channel] : p_default; }
        public double ReadDouble(SensorChannel p_channel,double p_default = 0)    { return m_d_lut  .ContainsKey(p_channel) ? (double)m_d_lut  [p_channel] : p_default; }
        public object Read      (SensorChannel p_channel,object p_default = null) { return m_obj_lut.ContainsKey(p_channel) ?         m_obj_lut[p_channel] : p_default; }

        /// <summary>
        /// Reads a channel data
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="p_channel"></param>
        /// <returns></returns>
        public void WriteByte  (SensorChannel p_channel,byte   p_value) { m_u8_lut [p_channel] = p_value; if (system != null) system.OnSensortChannelChanged(this,p_channel); }
        public void WriteSByte (SensorChannel p_channel,sbyte  p_value) { m_i8_lut [p_channel] = p_value; if (system != null) system.OnSensortChannelChanged(this,p_channel); }
        public void WriteUShort(SensorChannel p_channel,ushort p_value) { m_u16_lut[p_channel] = p_value; if (system != null) system.OnSensortChannelChanged(this,p_channel); }
        public void WriteShort (SensorChannel p_channel,short  p_value) { m_i16_lut[p_channel] = p_value; if (system != null) system.OnSensortChannelChanged(this,p_channel); }
        public void WriteUInt  (SensorChannel p_channel,uint   p_value) { m_u32_lut[p_channel] = p_value; if (system != null) system.OnSensortChannelChanged(this,p_channel); }
        public void WriteInt   (SensorChannel p_channel,int    p_value) { m_i32_lut[p_channel] = p_value; if (system != null) system.OnSensortChannelChanged(this,p_channel); }
        public void WriteULong (SensorChannel p_channel,ulong  p_value) { m_u64_lut[p_channel] = p_value; if (system != null) system.OnSensortChannelChanged(this,p_channel); }
        public void WriteLong  (SensorChannel p_channel,long   p_value) { m_i64_lut[p_channel] = p_value; if (system != null) system.OnSensortChannelChanged(this,p_channel); }        
        public void WriteFloat (SensorChannel p_channel,float  p_value) { m_f_lut  [p_channel] = p_value; if (system != null) system.OnSensortChannelChanged(this,p_channel); }
        public void WriteDouble(SensorChannel p_channel,double p_value) { m_d_lut  [p_channel] = p_value; if (system != null) system.OnSensortChannelChanged(this,p_channel); }
        public void Write      (SensorChannel p_channel,object p_value) { m_obj_lut[p_channel] = p_value; if (system != null) system.OnSensortChannelChanged(this,p_channel); }

    }
}
