using MAVLinkSharp.Bindings;
using System.Collections.Generic;

#pragma warning disable CS8618
#pragma warning disable CS8603
#pragma warning disable CS8600
#pragma warning disable CS1522

namespace MAVLinkSharp.Runtime {

    #region MAVLinkMsgId
    /// <summary>
    /// Enumeration for message ids
    /// </summary>
    public enum MAVLinkMsgId {
        Heartbeat                                  = 0     , //The heartbeat message shows that a system or component is present and responding. The type and autopilot fields (along with the message component id), allow the receiving system to treat further messages from this system appropriately (e.g. by laying out the user interface based on the autopilot). This microservice is documented at https://mavlink.io/en/services/heartbeat.html
        SysStatus                                  = 1     , //The general system state. If the system is following the MAVLink standard, the system state is mainly defined by three orthogonal states/modes: The system mode, which is either LOCKED (motors shut down and locked), MANUAL (system under RC control), GUIDED (system with autonomous position control, position setpoint controlled manually) or AUTO (system guided by path/waypoint planner). The NAV_MODE defined the current flight state: LIFTOFF (often an open-loop maneuver), LANDING, WAYPOINTS or VECTOR. This represents the internal navigation state machine. The system status shows whether the system is currently active or not and if an emergency occurred. During the CRITICAL and EMERGENCY states the MAV is still considered to be active, but should start emergency procedures autonomously. After a failure occurred it should first move from active to critical to allow manual intervention and then move to emergency after a certain timeout.
        SystemTime                                 = 2     , //The system time is the time of the master clock, typically the computer clock of the main onboard computer.
        Ping                                       = 4     , //A ping message either requesting or responding to a ping. This allows to measure the system latencies, including serial port, radio modem and UDP connections. The ping microservice is documented at https://mavlink.io/en/services/ping.html
        ChangeOperatorControl                      = 5     , //Request to control this MAV
        ChangeOperatorControlAck                   = 6     , //Accept / deny control of this MAV
        AuthKey                                    = 7     , //Emit an encrypted signature / key identifying this system. PLEASE NOTE: This protocol has been kept simple, so transmitting the key requires an encrypted channel for true safety.
        LinkNodeStatus                             = 8     , //Status generated in each node in the communication chain and injected into MAVLink stream.
        SetMode                                    = 11    , //Set the system mode, as defined by enum MAV_MODE. There is no target component id as the mode is by definition for the overall aircraft, not only for one component.
        ParamRequestRead                           = 20    , //Request to read the onboard parameter with the param_id string id. Onboard parameters are stored as key[const char*] -> value[float]. This allows to send a parameter to any other component (such as the GCS) without the need of previous knowledge of possible parameter names. Thus the same GCS can store different parameters for different autopilots. See also https://mavlink.io/en/services/parameter.html for a full documentation of QGroundControl and IMU code.
        ParamRequestList                           = 21    , //Request all parameters of this component. After this request, all parameters are emitted. The parameter microservice is documented at https://mavlink.io/en/services/parameter.html
        ParamValue                                 = 22    , //Emit the value of a onboard parameter. The inclusion of param_count and param_index in the message allows the recipient to keep track of received parameters and allows him to re-request missing parameters after a loss or timeout. The parameter microservice is documented at https://mavlink.io/en/services/parameter.html
        ParamSet                                   = 23    , //Set a parameter value (write new value to permanent storage). |         The receiving component should acknowledge the new parameter value by broadcasting a PARAM_VALUE message (broadcasting ensures that multiple GCS all have an up-to-date list of all parameters). If the sending GCS did not receive a PARAM_VALUE within its timeout time, it should re-send the PARAM_SET message. The parameter microservice is documented at https://mavlink.io/en/services/parameter.html. |         PARAM_SET may also be called within the context of a transaction (started with MAV_CMD_PARAM_TRANSACTION). Within a transaction the receiving component should respond with PARAM_ACK_TRANSACTION to the setter component (instead of broadcasting PARAM_VALUE), and PARAM_SET should be re-sent if this is ACK not received.
        GpsRawInt                                  = 24    , //The global position, as returned by the Global Positioning System (GPS). This is |                 NOT the global position estimate of the system, but rather a RAW sensor value. See message GLOBAL_POSITION_INT for the global position estimate.
        GpsStatus                                  = 25    , //The positioning status, as reported by GPS. This message is intended to display status information about each satellite visible to the receiver. See message GLOBAL_POSITION_INT for the global position estimate. This message can contain information for up to 20 satellites.
        ScaledImu                                  = 26    , //The RAW IMU readings for the usual 9DOF sensor setup. This message should contain the scaled values to the described units
        RawImu                                     = 27    , //The RAW IMU readings for a 9DOF sensor, which is identified by the id (default IMU1). This message should always contain the true raw values without any scaling to allow data capture and system debugging.
        RawPressure                                = 28    , //The RAW pressure readings for the typical setup of one absolute pressure and one differential pressure sensor. The sensor values should be the raw, UNSCALED ADC values.
        ScaledPressure                             = 29    , //The pressure readings for the typical setup of one absolute and differential pressure sensor. The units are as specified in each field.
        Attitude                                   = 30    , //The attitude in the aeronautical frame (right-handed, Z-down, X-front, Y-right).
        AttitudeQuaternion                         = 31    , //The attitude in the aeronautical frame (right-handed, Z-down, X-front, Y-right), expressed as quaternion. Quaternion order is w, x, y, z and a zero rotation would be expressed as (1 0 0 0).
        LocalPositionNed                           = 32    , //The filtered local position (e.g. fused computer vision and accelerometers). Coordinate frame is right-handed, Z-axis down (aeronautical frame, NED / north-east-down convention)
        GlobalPositionInt                          = 33    , //The filtered global position (e.g. fused GPS and accelerometers). The position is in GPS-frame (right-handed, Z-up). It |                is designed as scaled integer message since the resolution of float is not sufficient.
        RcChannelsScaled                           = 34    , //The scaled values of the RC channels received: (-100%) -10000, (0%) 0, (100%) 10000. Channels that are inactive should be set to UINT16_MAX.
        RcChannelsRaw                              = 35    , //The RAW values of the RC channels received. The standard PPM modulation is as follows: 1000 microseconds: 0%, 2000 microseconds: 100%. A value of UINT16_MAX implies the channel is unused. Individual receivers/transmitters might violate this specification.
        ServoOutputRaw                             = 36    , //Superseded by ACTUATOR_OUTPUT_STATUS. The RAW values of the servo outputs (for RC input from the remote, use the RC_CHANNELS messages). The standard PPM modulation is as follows: 1000 microseconds: 0%, 2000 microseconds: 100%.
        MissionRequestPartialList                  = 37    , //Request a partial list of mission items from the system/component. https://mavlink.io/en/services/mission.html. If start and end index are the same, just send one waypoint.
        MissionWritePartialList                    = 38    , //This message is sent to the MAV to write a partial list. If start index == end index, only one item will be transmitted / updated. If the start index is NOT 0 and above the current list size, this request should be REJECTED!
        MissionItem                                = 39    , //Message encoding a mission item. This message is emitted to announce |                 the presence of a mission item and to set a mission item on the system. The mission item can be either in x, y, z meters (type: LOCAL) or x:lat, y:lon, z:altitude. Local frame is Z-down, right handed (NED), global frame is Z-up, right handed (ENU). NaN may be used to indicate an optional/default value (e.g. to use the system's current latitude or yaw rather than a specific value). See also https://mavlink.io/en/services/mission.html.
        MissionRequest                             = 40    , //Request the information of the mission item with the sequence number seq. The response of the system to this message should be a MISSION_ITEM message. https://mavlink.io/en/services/mission.html
        MissionSetCurrent                          = 41    , //Set the mission item with sequence number seq as current item. This means that the MAV will continue to this mission item on the shortest path (not following the mission items in-between).
        MissionCurrent                             = 42    , //Message that announces the sequence number of the current active mission item. The MAV will fly towards this mission item.
        MissionRequestList                         = 43    , //Request the overall list of mission items from the system/component.
        MissionCount                               = 44    , //This message is emitted as response to MISSION_REQUEST_LIST by the MAV and to initiate a write transaction. The GCS can then request the individual mission item based on the knowledge of the total number of waypoints.
        MissionClearAll                            = 45    , //Delete all mission items at once.
        MissionItemReached                         = 46    , //A certain mission item has been reached. The system will either hold this position (or circle on the orbit) or (if the autocontinue on the WP was set) continue to the next waypoint.
        MissionAck                                 = 47    , //Acknowledgment message during waypoint handling. The type field states if this message is a positive ack (type=0) or if an error happened (type=non-zero).
        SetGpsGlobalOrigin                         = 48    , //Sets the GPS coordinates of the vehicle local origin (0,0,0) position. Vehicle should emit GPS_GLOBAL_ORIGIN irrespective of whether the origin is changed. This enables transform between the local coordinate frame and the global (GPS) coordinate frame, which may be necessary when (for example) indoor and outdoor settings are connected and the MAV should move from in- to outdoor.
        GpsGlobalOrigin                            = 49    , //Publishes the GPS coordinates of the vehicle local origin (0,0,0) position. Emitted whenever a new GPS-Local position mapping is requested or set - e.g. following SET_GPS_GLOBAL_ORIGIN message.
        ParamMapRc                                 = 50    , //Bind a RC channel to a parameter. The parameter should change according to the RC channel value.
        MissionRequestInt                          = 51    , //Request the information of the mission item with the sequence number seq. The response of the system to this message should be a MISSION_ITEM_INT message. https://mavlink.io/en/services/mission.html
        SafetySetAllowedArea                       = 54    , //Set a safety zone (volume), which is defined by two corners of a cube. This message can be used to tell the MAV which setpoints/waypoints to accept and which to reject. Safety areas are often enforced by national or competition regulations.
        SafetyAllowedArea                          = 55    , //Read out the safety zone the MAV currently assumes.
        AttitudeQuaternionCov                      = 61    , //The attitude in the aeronautical frame (right-handed, Z-down, X-front, Y-right), expressed as quaternion. Quaternion order is w, x, y, z and a zero rotation would be expressed as (1 0 0 0).
        NavControllerOutput                        = 62    , //The state of the navigation and position controller.
        GlobalPositionIntCov                       = 63    , //The filtered global position (e.g. fused GPS and accelerometers). The position is in GPS-frame (right-handed, Z-up). It  is designed as scaled integer message since the resolution of float is not sufficient. NOTE: This message is intended for onboard networks / companion computers and higher-bandwidth links and optimized for accuracy and completeness. Please use the GLOBAL_POSITION_INT message for a minimal subset.
        LocalPositionNedCov                        = 64    , //The filtered local position (e.g. fused computer vision and accelerometers). Coordinate frame is right-handed, Z-axis down (aeronautical frame, NED / north-east-down convention)
        RcChannels                                 = 65    , //The PPM values of the RC channels received. The standard PPM modulation is as follows: 1000 microseconds: 0%, 2000 microseconds: 100%.  A value of UINT16_MAX implies the channel is unused. Individual receivers/transmitters might violate this specification.
        RequestDataStream                          = 66    , //Request a data stream.
        DataStream                                 = 67    , //Data stream status information.
        ManualControl                              = 69    , //This message provides an API for manually controlling the vehicle using standard joystick axes nomenclature, along with a joystick-like input device. Unused axes can be disabled and buttons states are transmitted as individual on/off bits of a bitmask
        RcChannelsOverride                         = 70    , //The RAW values of the RC channels sent to the MAV to override info received from the RC radio. The standard PPM modulation is as follows: 1000 microseconds: 0%, 2000 microseconds: 100%. Individual receivers/transmitters might violate this specification.  Note carefully the semantic differences between the first 8 channels and the subsequent channels
        MissionItemInt                             = 73    , //Message encoding a mission item. This message is emitted to announce |                 the presence of a mission item and to set a mission item on the system. The mission item can be either in x, y, z meters (type: LOCAL) or x:lat, y:lon, z:altitude. Local frame is Z-down, right handed (NED), global frame is Z-up, right handed (ENU). NaN or INT32_MAX may be used in float/integer params (respectively) to indicate optional/default values (e.g. to use the component's current latitude, yaw rather than a specific value). See also https://mavlink.io/en/services/mission.html.
        VfrHud                                     = 74    , //Metrics typically displayed on a HUD for fixed wing aircraft.
        CommandInt                                 = 75    , //Message encoding a command with parameters as scaled integers. Scaling depends on the actual command value. NaN or INT32_MAX may be used in float/integer params (respectively) to indicate optional/default values (e.g. to use the component's current latitude, yaw rather than a specific value). The command microservice is documented at https://mavlink.io/en/services/command.html
        CommandLong                                = 76    , //Send a command with up to seven parameters to the MAV. The command microservice is documented at https://mavlink.io/en/services/command.html
        CommandAck                                 = 77    , //Report status of a command. Includes feedback whether the command was executed. The command microservice is documented at https://mavlink.io/en/services/command.html
        CommandCancel                              = 80    , //Cancel a long running command. The target system should respond with a COMMAND_ACK to the original command with result=MAV_RESULT_CANCELLED if the long running process was cancelled. If it has already completed, the cancel action can be ignored. The cancel action can be retried until some sort of acknowledgement to the original command has been received. The command microservice is documented at https://mavlink.io/en/services/command.html
        ManualSetpoint                             = 81    , //Setpoint in roll, pitch, yaw and thrust from the operator
        SetAttitudeTarget                          = 82    , //Sets a desired vehicle attitude. Used by an external controller to command the vehicle (manual controller or other system).
        AttitudeTarget                             = 83    , //Reports the current commanded attitude of the vehicle as specified by the autopilot. This should match the commands sent in a SET_ATTITUDE_TARGET message if the vehicle is being controlled this way.
        SetPositionTargetLocalNed                  = 84    , //Sets a desired vehicle position in a local north-east-down coordinate frame. Used by an external controller to command the vehicle (manual controller or other system).
        PositionTargetLocalNed                     = 85    , //Reports the current commanded vehicle position, velocity, and acceleration as specified by the autopilot. This should match the commands sent in SET_POSITION_TARGET_LOCAL_NED if the vehicle is being controlled this way.
        SetPositionTargetGlobalInt                 = 86    , //Sets a desired vehicle position, velocity, and/or acceleration in a global coordinate system (WGS84). Used by an external controller to command the vehicle (manual controller or other system).
        PositionTargetGlobalInt                    = 87    , //Reports the current commanded vehicle position, velocity, and acceleration as specified by the autopilot. This should match the commands sent in SET_POSITION_TARGET_GLOBAL_INT if the vehicle is being controlled this way.
        LocalPositionNedSystemGlobalOffset         = 89    , //The offset in X, Y, Z and yaw between the LOCAL_POSITION_NED messages of MAV X and the global coordinate frame in NED coordinates. Coordinate frame is right-handed, Z-axis down (aeronautical frame, NED / north-east-down convention)
        HilState                                   = 90    , //Sent from simulation to autopilot. This packet is useful for high throughput applications such as hardware in the loop simulations.
        HilControls                                = 91    , //Sent from autopilot to simulation. Hardware in the loop control outputs
        HilRcInputsRaw                             = 92    , //Sent from simulation to autopilot. The RAW values of the RC channels received. The standard PPM modulation is as follows: 1000 microseconds: 0%, 2000 microseconds: 100%. Individual receivers/transmitters might violate this specification.
        HilActuatorControls                        = 93    , //Sent from autopilot to simulation. Hardware in the loop control outputs (replacement for HIL_CONTROLS)
        OpticalFlow                                = 100   , //Optical flow from a flow sensor (e.g. optical mouse sensor)
        GlobalVisionPositionEstimate               = 101   , //Global position/attitude estimate from a vision source.
        VisionPositionEstimate                     = 102   , //Local position/attitude estimate from a vision source.
        VisionSpeedEstimate                        = 103   , //Speed estimate from a vision source.
        ViconPositionEstimate                      = 104   , //Global position estimate from a Vicon motion system source.
        HighresImu                                 = 105   , //The IMU readings in SI units in NED body frame
        OpticalFlowRad                             = 106   , //Optical flow from an angular rate flow sensor (e.g. PX4FLOW or mouse sensor)
        HilSensor                                  = 107   , //The IMU readings in SI units in NED body frame
        SimState                                   = 108   , //Status of simulation environment, if used
        RadioStatus                                = 109   , //Status generated by radio and injected into MAVLink stream.
        FileTransferProtocol                       = 110   , //File transfer protocol message: https://mavlink.io/en/services/ftp.html.
        Timesync                                   = 111   , //Time synchronization message.
        CameraTrigger                              = 112   , //Camera-IMU triggering and synchronisation message.
        HilGps                                     = 113   , //The global position, as returned by the Global Positioning System (GPS). This is |                  NOT the global position estimate of the system, but rather a RAW sensor value. See message GLOBAL_POSITION_INT for the global position estimate.
        HilOpticalFlow                             = 114   , //Simulated optical flow from a flow sensor (e.g. PX4FLOW or optical mouse sensor)
        HilStateQuaternion                         = 115   , //Sent from simulation to autopilot, avoids in contrast to HIL_STATE singularities. This packet is useful for high throughput applications such as hardware in the loop simulations.
        ScaledImu2                                 = 116   , //The RAW IMU readings for secondary 9DOF sensor setup. This message should contain the scaled values to the described units
        LogRequestList                             = 117   , //Request a list of available logs. On some systems calling this may stop on-board logging until LOG_REQUEST_END is called. If there are no log files available this request shall be answered with one LOG_ENTRY message with id = 0 and num_logs = 0.
        LogEntry                                   = 118   , //Reply to LOG_REQUEST_LIST
        LogRequestData                             = 119   , //Request a chunk of a log
        LogData                                    = 120   , //Reply to LOG_REQUEST_DATA
        LogErase                                   = 121   , //Erase all logs
        LogRequestEnd                              = 122   , //Stop log transfer and resume normal logging
        GpsInjectData                              = 123   , //Data for injecting into the onboard GPS (used for DGPS)
        Gps2Raw                                    = 124   , //Second GPS data.
        PowerStatus                                = 125   , //Power supply status
        SerialControl                              = 126   , //Control a serial port. This can be used for raw access to an onboard serial peripheral such as a GPS or telemetry radio. It is designed to make it possible to update the devices firmware via MAVLink messages or change the devices settings. A message with zero bytes can be used to change just the baudrate.
        GpsRtk                                     = 127   , //RTK GPS data. Gives information on the relative baseline calculation the GPS is reporting
        Gps2Rtk                                    = 128   , //RTK GPS data. Gives information on the relative baseline calculation the GPS is reporting
        ScaledImu3                                 = 129   , //The RAW IMU readings for 3rd 9DOF sensor setup. This message should contain the scaled values to the described units
        DataTransmissionHandshake                  = 130   , //Handshake message to initiate, control and stop image streaming when using the Image Transmission Protocol: https://mavlink.io/en/services/image_transmission.html.
        EncapsulatedData                           = 131   , //Data packet for images sent using the Image Transmission Protocol: https://mavlink.io/en/services/image_transmission.html.
        DistanceSensor                             = 132   , //Distance sensor information for an onboard rangefinder.
        TerrainRequest                             = 133   , //Request for terrain data and terrain status. See terrain protocol docs: https://mavlink.io/en/services/terrain.html
        TerrainData                                = 134   , //Terrain data sent from GCS. The lat/lon and grid_spacing must be the same as a lat/lon from a TERRAIN_REQUEST. See terrain protocol docs: https://mavlink.io/en/services/terrain.html
        TerrainCheck                               = 135   , //Request that the vehicle report terrain height at the given location (expected response is a TERRAIN_REPORT). Used by GCS to check if vehicle has all terrain data needed for a mission.
        TerrainReport                              = 136   , //Streamed from drone to report progress of terrain map download (initiated by TERRAIN_REQUEST), or sent as a response to a TERRAIN_CHECK request. See terrain protocol docs: https://mavlink.io/en/services/terrain.html
        ScaledPressure2                            = 137   , //Barometer readings for 2nd barometer
        AttPosMocap                                = 138   , //Motion capture attitude and position
        SetActuatorControlTarget                   = 139   , //Set the vehicle attitude and body angular rates.
        ActuatorControlTarget                      = 140   , //Set the vehicle attitude and body angular rates.
        Altitude                                   = 141   , //The current system altitude.
        ResourceRequest                            = 142   , //The autopilot is requesting a resource (file, binary, other type of data)
        ScaledPressure3                            = 143   , //Barometer readings for 3rd barometer
        FollowTarget                               = 144   , //Current motion information from a designated system
        ControlSystemState                         = 146   , //The smoothed, monotonic system state used to feed the control loops of the system.
        BatteryStatus                              = 147   , //Battery information. Updates GCS with flight controller battery status. Smart batteries also use this message, but may additionally send SMART_BATTERY_INFO.
        AutopilotVersion                           = 148   , //Version and capability of autopilot software. This should be emitted in response to a request with MAV_CMD_REQUEST_MESSAGE.
        LandingTarget                              = 149   , //The location of a landing target. See: https://mavlink.io/en/services/landing_target.html
        FenceStatus                                = 162   , //Status of geo-fencing. Sent in extended status stream when fencing enabled.
        MagCalReport                               = 192   , //Reports results of completed compass calibration. Sent until MAG_CAL_ACK received.
        EfiStatus                                  = 225   , //EFI status output
        EstimatorStatus                            = 230   , //Estimator status message including flags, innovation test ratios and estimated accuracies. The flags message is an integer bitmask containing information on which EKF outputs are valid. See the ESTIMATOR_STATUS_FLAGS enum definition for further information. The innovation test ratios show the magnitude of the sensor innovation divided by the innovation check threshold. Under normal operation the innovation test ratios should be below 0.5 with occasional values up to 1.0. Values greater than 1.0 should be rare under normal operation and indicate that a measurement has been rejected by the filter. The user should be notified if an innovation test ratio greater than 1.0 is recorded. Notifications for values in the range between 0.5 and 1.0 should be optional and controllable by the user.
        WindCov                                    = 231   , //Wind estimate from vehicle. Note that despite the name, this message does not actually contain any covariances but instead variability and accuracy fields in terms of standard deviation (1-STD).
        GpsInput                                   = 232   , //GPS sensor input message.  This is a raw sensor value sent by the GPS. This is NOT the global position estimate of the system.
        GpsRtcmData                                = 233   , //RTCM message for injecting into the onboard GPS (used for DGPS)
        HighLatency                                = 234   , //Message appropriate for high latency connections like Iridium
        HighLatency2                               = 235   , //Message appropriate for high latency connections like Iridium (version 2)
        Vibration                                  = 241   , //Vibration levels and accelerometer clipping
        HomePosition                               = 242   , // | 	Contains the home position. | 	The home position is the default position that the system will return to and land on. | 	The position must be set automatically by the system during the takeoff, and may also be explicitly set using MAV_CMD_DO_SET_HOME. | 	The global and local positions encode the position in the respective coordinate frames, while the q parameter encodes the orientation of the surface. | 	Under normal conditions it describes the heading and terrain slope, which can be used by the aircraft to adjust the approach. | 	The approach 3D vector describes the point to which the system should fly in normal flight mode and then perform a landing sequence along the vector. |         Note: this message can be requested by sending the MAV_CMD_REQUEST_MESSAGE with param1=242 (or the deprecated MAV_CMD_GET_HOME_POSITION command). |       
        SetHomePosition                            = 243   , // |         Sets the home position. | 	The home position is the default position that the system will return to and land on. |         The position is set automatically by the system during the takeoff (and may also be set using this message). |         The global and local positions encode the position in the respective coordinate frames, while the q parameter encodes the orientation of the surface. |         Under normal conditions it describes the heading and terrain slope, which can be used by the aircraft to adjust the approach. |         The approach 3D vector describes the point to which the system should fly in normal flight mode and then perform a landing sequence along the vector. |         Note: the current home position may be emitted in a HOME_POSITION message on request (using MAV_CMD_REQUEST_MESSAGE with param1=242). |       
        MessageInterval                            = 244   , // |         The interval between messages for a particular MAVLink message ID. |         This message is sent in response to the MAV_CMD_REQUEST_MESSAGE command with param1=244 (this message) and param2=message_id (the id of the message for which the interval is required). | 	It may also be sent in response to MAV_CMD_GET_MESSAGE_INTERVAL. | 	This interface replaces DATA_STREAM.
        ExtendedSysState                           = 245   , //Provides state for additional features
        AdsbVehicle                                = 246   , //The location and information of an ADSB vehicle
        Collision                                  = 247   , //Information about a potential collision
        V2Extension                                = 248   , //Message implementing parts of the V2 payload specs in V1 frames for transitional support.
        MemoryVect                                 = 249   , //Send raw controller memory. The use of this message is discouraged for normal packets, but a quite efficient way for testing new messages and getting experimental debug output.
        DebugVect                                  = 250   , //To debug something using a named 3D vector.
        NamedValueFloat                            = 251   , //Send a key-value pair as float. The use of this message is discouraged for normal packets, but a quite efficient way for testing new messages and getting experimental debug output.
        NamedValueInt                              = 252   , //Send a key-value pair as integer. The use of this message is discouraged for normal packets, but a quite efficient way for testing new messages and getting experimental debug output.
        Statustext                                 = 253   , //Status text message. These messages are printed in yellow in the COMM console of QGroundControl. WARNING: They consume quite some bandwidth, so use only for important status and error messages. If implemented wisely, these messages are buffered on the MCU and sent only at a limited rate (e.g. 10 Hz).
        Debug                                      = 254   , //Send a debug value. The index is used to discriminate between values. These values show up in the plot of QGroundControl as DEBUG N.
        SetupSigning                               = 256   , //Setup a MAVLink2 signing key. If called with secret_key of all zero and zero initial_timestamp will disable signing
        ButtonChange                               = 257   , //Report button state change.
        PlayTune                                   = 258   , //Control vehicle tone generation (buzzer).
        CameraInformation                          = 259   , //Information about a camera. Can be requested with a MAV_CMD_REQUEST_MESSAGE command.
        CameraSettings                             = 260   , //Settings of a camera. Can be requested with a MAV_CMD_REQUEST_MESSAGE command.
        StorageInformation                         = 261   , //Information about a storage medium. This message is sent in response to a request with MAV_CMD_REQUEST_MESSAGE and whenever the status of the storage changes (STORAGE_STATUS). Use MAV_CMD_REQUEST_MESSAGE.param2 to indicate the index/id of requested storage: 0 for all, 1 for first, 2 for second, etc.
        CameraCaptureStatus                        = 262   , //Information about the status of a capture. Can be requested with a MAV_CMD_REQUEST_MESSAGE command.
        CameraImageCaptured                        = 263   , //Information about a captured image. This is emitted every time a message is captured. |         MAV_CMD_REQUEST_MESSAGE can be used to (re)request this message for a specific sequence number or range of sequence numbers: |         MAV_CMD_REQUEST_MESSAGE.param2 indicates the sequence number the first image to send, or set to -1 to send the message for all sequence numbers. |         MAV_CMD_REQUEST_MESSAGE.param3 is used to specify a range of messages to send: |         set to 0 (default) to send just the the message for the sequence number in param 2, |         set to -1 to send the message for the sequence number in param 2 and all the following sequence numbers,  |         set to the sequence number of the final message in the range.
        FlightInformation                          = 264   , //Information about flight since last arming. |         This can be requested using MAV_CMD_REQUEST_MESSAGE. |       
        MountOrientation                           = 265   , //Orientation of a mount
        LoggingData                                = 266   , //A message containing logged data (see also MAV_CMD_LOGGING_START)
        LoggingDataAcked                           = 267   , //A message containing logged data which requires a LOGGING_ACK to be sent back
        LoggingAck                                 = 268   , //An ack for a LOGGING_DATA_ACKED message
        VideoStreamInformation                     = 269   , //Information about video stream. It may be requested using MAV_CMD_REQUEST_MESSAGE, where param2 indicates the video stream id: 0 for all streams, 1 for first, 2 for second, etc.
        VideoStreamStatus                          = 270   , //Information about the status of a video stream. It may be requested using MAV_CMD_REQUEST_MESSAGE.
        CameraFovStatus                            = 271   , //Information about the field of view of a camera. Can be requested with a MAV_CMD_REQUEST_MESSAGE command.
        CameraTrackingImageStatus                  = 275   , //Camera tracking status, sent while in active tracking. Use MAV_CMD_SET_MESSAGE_INTERVAL to define message interval.
        CameraTrackingGeoStatus                    = 276   , //Camera tracking status, sent while in active tracking. Use MAV_CMD_SET_MESSAGE_INTERVAL to define message interval.
        GimbalManagerInformation                   = 280   , //Information about a high level gimbal manager. This message should be requested by a ground station using MAV_CMD_REQUEST_MESSAGE.
        GimbalManagerStatus                        = 281   , //Current status about a high level gimbal manager. This message should be broadcast at a low regular rate (e.g. 5Hz).
        GimbalManagerSetAttitude                   = 282   , //High level message to control a gimbal's attitude. This message is to be sent to the gimbal manager (e.g. from a ground station). Angles and rates can be set to NaN according to use case.
        GimbalDeviceInformation                    = 283   , //Information about a low level gimbal. This message should be requested by the gimbal manager or a ground station using MAV_CMD_REQUEST_MESSAGE. The maximum angles and rates are the limits by hardware. However, the limits by software used are likely different/smaller and dependent on mode/settings/etc..
        GimbalDeviceSetAttitude                    = 284   , //Low level message to control a gimbal device's attitude. This message is to be sent from the gimbal manager to the gimbal device component. Angles and rates can be set to NaN according to use case.
        GimbalDeviceAttitudeStatus                 = 285   , //Message reporting the status of a gimbal device. This message should be broadcasted by a gimbal device component. The angles encoded in the quaternion are relative to absolute North if the flag GIMBAL_DEVICE_FLAGS_YAW_LOCK is set (roll: positive is rolling to the right, pitch: positive is pitching up, yaw is turn to the right) or relative to the vehicle heading if the flag is not set. This message should be broadcast at a low regular rate (e.g. 10Hz).
        AutopilotStateForGimbalDevice              = 286   , //Low level message containing autopilot state relevant for a gimbal device. This message is to be sent from the gimbal manager to the gimbal device component. The data of this message server for the gimbal's estimator corrections in particular horizon compensation, as well as the autopilot's control intention e.g. feed forward angular control in z-axis.
        GimbalManagerSetPitchyaw                   = 287   , //High level message to control a gimbal's pitch and yaw angles. This message is to be sent to the gimbal manager (e.g. from a ground station). Angles and rates can be set to NaN according to use case.
        GimbalManagerSetManualControl              = 288   , //High level message to control a gimbal manually. The angles or angular rates are unitless; the actual rates will depend on internal gimbal manager settings/configuration (e.g. set by parameters). This message is to be sent to the gimbal manager (e.g. from a ground station). Angles and rates can be set to NaN according to use case.
        EscInfo                                    = 290   , //ESC information for lower rate streaming. Recommended streaming rate 1Hz. See ESC_STATUS for higher-rate ESC data.
        EscStatus                                  = 291   , //ESC information for higher rate streaming. Recommended streaming rate is ~10 Hz. Information that changes more slowly is sent in ESC_INFO. It should typically only be streamed on high-bandwidth links (i.e. to a companion computer).
        WifiConfigAp                               = 299   , //Configure WiFi AP SSID, password, and mode. This message is re-emitted as an acknowledgement by the AP. The message may also be explicitly requested using MAV_CMD_REQUEST_MESSAGE
        ProtocolVersion                            = 300   , //Version and capability of protocol version. This message can be requested with MAV_CMD_REQUEST_MESSAGE and is used as part of the handshaking to establish which MAVLink version should be used on the network. Every node should respond to a request for PROTOCOL_VERSION to enable the handshaking. Library implementers should consider adding this into the default decoding state machine to allow the protocol core to respond directly.
        AisVessel                                  = 301   , //The location and information of an AIS vessel
        UavcanNodeStatus                           = 310   , //General status information of an UAVCAN node. Please refer to the definition of the UAVCAN message "uavcan.protocol.NodeStatus" for the background information. The UAVCAN specification is available at http://uavcan.org.
        UavcanNodeInfo                             = 311   , //General information describing a particular UAVCAN node. Please refer to the definition of the UAVCAN service "uavcan.protocol.GetNodeInfo" for the background information. This message should be emitted by the system whenever a new node appears online, or an existing node reboots. Additionally, it can be emitted upon request from the other end of the MAVLink channel (see MAV_CMD_UAVCAN_GET_NODE_INFO). It is also not prohibited to emit this message unconditionally at a low frequency. The UAVCAN specification is available at http://uavcan.org.
        ParamExtRequestRead                        = 320   , //Request to read the value of a parameter with either the param_id string id or param_index. PARAM_EXT_VALUE should be emitted in response.
        ParamExtRequestList                        = 321   , //Request all parameters of this component. All parameters should be emitted in response as PARAM_EXT_VALUE.
        ParamExtValue                              = 322   , //Emit the value of a parameter. The inclusion of param_count and param_index in the message allows the recipient to keep track of received parameters and allows them to re-request missing parameters after a loss or timeout.
        ParamExtSet                                = 323   , //Set a parameter value. In order to deal with message loss (and retransmission of PARAM_EXT_SET), when setting a parameter value and the new value is the same as the current value, you will immediately get a PARAM_ACK_ACCEPTED response. If the current state is PARAM_ACK_IN_PROGRESS, you will accordingly receive a PARAM_ACK_IN_PROGRESS in response.
        ParamExtAck                                = 324   , //Response from a PARAM_EXT_SET message.
        ObstacleDistance                           = 330   , //Obstacle distances in front of the sensor, starting from the left in increment degrees to the right
        Odometry                                   = 331   , //Odometry message to communicate odometry information with an external interface. Fits ROS REP 147 standard for aerial vehicles (http://www.ros.org/reps/rep-0147.html).
        TrajectoryRepresentationWaypoints          = 332   , //Describe a trajectory using an array of up-to 5 waypoints in the local frame (MAV_FRAME_LOCAL_NED).
        TrajectoryRepresentationBezier             = 333   , //Describe a trajectory using an array of up-to 5 bezier control points in the local frame (MAV_FRAME_LOCAL_NED).
        CellularStatus                             = 334   , //Report current used cellular network status
        IsbdLinkStatus                             = 335   , //Status of the Iridium SBD link.
        CellularConfig                             = 336   , //Configure cellular modems. |         This message is re-emitted as an acknowledgement by the modem. |         The message may also be explicitly requested using MAV_CMD_REQUEST_MESSAGE.
        RawRpm                                     = 339   , //RPM sensor data message.
        UtmGlobalPosition                          = 340   , //The global position resulting from GPS and sensor fusion.
        DebugFloatArray                            = 350   , //Large debug/prototyping array. The message uses the maximum available payload for data. The array_id and name fields are used to discriminate between messages in code and in user interfaces (respectively). Do not use in production code.
        OrbitExecutionStatus                       = 360   , //Vehicle status report that is sent out while orbit execution is in progress (see MAV_CMD_DO_ORBIT).
        SmartBatteryInfo                           = 370   , //Smart Battery information (static/infrequent update). Use for updates from: smart battery to flight stack, flight stack to GCS. Use BATTERY_STATUS for smart battery frequent updates.
        GeneratorStatus                            = 373   , //Telemetry of power generation system. Alternator or mechanical generator.
        ActuatorOutputStatus                       = 375   , //The raw values of the actuator outputs (e.g. on Pixhawk, from MAIN, AUX ports). This message supersedes SERVO_OUTPUT_RAW.
        TimeEstimateToTarget                       = 380   , //Time/duration estimates for various events and actions given the current vehicle state and position.
        Tunnel                                     = 385   , //Message for transporting "arbitrary" variable-length data from one component to another (broadcast is not forbidden, but discouraged). The encoding of the data is usually extension specific, i.e. determined by the source, and is usually not documented as part of the MAVLink specification.
        CanFrame                                   = 386   , //A forwarded CAN frame as requested by MAV_CMD_CAN_FORWARD.
        CanfdFrame                                 = 387   , //A forwarded CANFD frame as requested by MAV_CMD_CAN_FORWARD. These are separated from CAN_FRAME as they need different handling (eg. TAO handling)
        CanFilterModify                            = 388   , //Modify the filter of what CAN messages to forward over the mavlink. This can be used to make CAN forwarding work well on low bandwidth links. The filtering is applied on bits 8 to 24 of the CAN id (2nd and 3rd bytes) which corresponds to the DroneCAN message ID for DroneCAN. Filters with more than 16 IDs can be constructed by sending multiple CAN_FILTER_MODIFY messages.
        OnboardComputerStatus                      = 390   , //Hardware status sent by an onboard computer.
        ComponentInformation                       = 395   , // |         Component information message, which may be requested using MAV_CMD_REQUEST_MESSAGE. |       
        ComponentMetadata                          = 397   , // |         Component metadata message, which may be requested using MAV_CMD_REQUEST_MESSAGE. |          |         This contains the MAVLink FTP URI and CRC for the component's general metadata file. |         The file must be hosted on the component, and may be xz compressed. |         The file CRC can be used for file caching. |          |         The general metadata file can be read to get the locations of other metadata files (COMP_METADATA_TYPE) and translations, which may be hosted either on the vehicle or the internet. |         For more information see: https://mavlink.io/en/services/component_information.html. |          |         Note: Camera components should use CAMERA_INFORMATION instead, and autopilots may use both this message and AUTOPILOT_VERSION. |       
        PlayTuneV2                                 = 400   , //Play vehicle tone/tune (buzzer). Supersedes message PLAY_TUNE.
        SupportedTunes                             = 401   , //Tune formats supported by vehicle. This should be emitted as response to MAV_CMD_REQUEST_MESSAGE.
        Event                                      = 410   , //Event message. Each new event from a particular component gets a new sequence number. The same message might be sent multiple times if (re-)requested. Most events are broadcast, some can be specific to a target component (as receivers keep track of the sequence for missed events, all events need to be broadcast. Thus we use destination_component instead of target_component).
        CurrentEventSequence                       = 411   , //Regular broadcast for the current latest event sequence number for a component. This is used to check for dropped events.
        RequestEvent                               = 412   , //Request one or more events to be (re-)sent. If first_sequence==last_sequence, only a single event is requested. Note that first_sequence can be larger than last_sequence (because the sequence number can wrap). Each sequence will trigger an EVENT or EVENT_ERROR response.
        ResponseEventError                         = 413   , //Response to a REQUEST_EVENT in case of an error (e.g. the event is not available anymore).
        WheelDistance                              = 9000  , //Cumulative distance traveled for each reported wheel.
        WinchStatus                                = 9005  , //Winch status.
        OpenDroneIdBasicId                         = 12900 , //Data for filling the OpenDroneID Basic ID message. This and the below messages are primarily meant for feeding data to/from an OpenDroneID implementation. E.g. https://github.com/opendroneid/opendroneid-core-c. These messages are compatible with the ASTM F3411 Remote ID standard and the ASD-STAN prEN 4709-002 Direct Remote ID standard. Additional information and usage of these messages is documented at https://mavlink.io/en/services/opendroneid.html.
        OpenDroneIdLocation                        = 12901 , //Data for filling the OpenDroneID Location message. The float data types are 32-bit IEEE 754. The Location message provides the location, altitude, direction and speed of the aircraft.
        OpenDroneIdAuthentication                  = 12902 , //Data for filling the OpenDroneID Authentication message. The Authentication Message defines a field that can provide a means of authenticity for the identity of the UAS (Unmanned Aircraft System). The Authentication message can have two different formats. For data page 0, the fields PageCount, Length and TimeStamp are present and AuthData is only 17 bytes. For data page 1 through 15, PageCount, Length and TimeStamp are not present and the size of AuthData is 23 bytes.
        OpenDroneIdSelfId                          = 12903 , //Data for filling the OpenDroneID Self ID message. The Self ID Message is an opportunity for the operator to (optionally) declare their identity and purpose of the flight. This message can provide additional information that could reduce the threat profile of a UA (Unmanned Aircraft) flying in a particular area or manner. This message can also be used to provide optional additional clarification in an emergency/remote ID system failure situation.
        OpenDroneIdSystem                          = 12904 , //Data for filling the OpenDroneID System message. The System Message contains general system information including the operator location/altitude and possible aircraft group and/or category/class information.
        OpenDroneIdOperatorId                      = 12905 , //Data for filling the OpenDroneID Operator ID message, which contains the CAA (Civil Aviation Authority) issued operator ID.
        OpenDroneIdMessagePack                     = 12915 , //An OpenDroneID message pack is a container for multiple encoded OpenDroneID messages (i.e. not in the format given for the above message descriptions but after encoding into the compressed OpenDroneID byte format). Used e.g. when transmitting on Bluetooth 5.0 Long Range/Extended Advertising or on WiFi Neighbor Aware Networking or on WiFi Beacon.
        OpenDroneIdArmStatus                       = 12918 , //Transmitter (remote ID system) is enabled and ready to start sending location and other required information. This is streamed by transmitter. A flight controller uses it as a condition to arm.
        OpenDroneIdSystemUpdate                    = 12919 , //Update the data in the OPEN_DRONE_ID_SYSTEM message with new location information. This can be sent to update the location information for the operator when no other information in the SYSTEM message has changed. This message allows for efficient operation on radio links which have limited uplink bandwidth while meeting requirements for update frequency of the operator location.
        HygrometerSensor                           = 12920 , //Temperature and humidity from hygrometer.
    }
    #endregion

    /// <summary>
    /// Class that describes a MAVLink Message Container
    /// </summary>    
    public class MAVLinkMsg {
        
        #region Pool
        /// <summary>
        /// Fetch a memory pooled message instance
        /// </summary>
        /// <returns></returns>
        static public MAVLinkMsg GetPool() {            
            MAVLinkMsg msg = null;
            lock(m_pool) if(m_pool.Count>0) { msg = m_pool[0]; m_pool.RemoveAt(0); }
            if(msg==null) msg = new MAVLinkMsg();
            return msg;
        }

        /// <summary>
        /// Returns a memory pooled message instance
        /// </summary>
        /// <returns></returns>
        static public void SetPool(MAVLinkMsg p_instance) {               
            lock(m_pool) if(!m_pool.Contains(p_instance)) { m_pool.Add(p_instance); }
        }

        /// <summary>
        /// Message Pool
        /// </summary>
        static public List<MAVLinkMsg> m_pool;
        #endregion

        /// <summary>
        /// CTOR.
        /// </summary>
        static MAVLinkMsg() {
            m_pool = new List<MAVLinkMsg>();
            for(int i=0;i<1000;i++) m_pool.Add(new MAVLinkMsg());
        }

        #region MessageID to PayloadLength
        /// <summary>
        /// Returns the message payload size generated from the definition XML
        /// </summary>
        static public byte GetMessagePayloadLength(int p_msg_id) {
            switch(p_msg_id) {
                case 0     : return 9;      //HEARTBEAT
                case 1     : return 43;     //SYS_STATUS
                case 2     : return 12;     //SYSTEM_TIME
                case 4     : return 14;     //PING
                case 5     : return 28;     //CHANGE_OPERATOR_CONTROL
                case 6     : return 3;      //CHANGE_OPERATOR_CONTROL_ACK
                case 7     : return 32;     //AUTH_KEY
                case 8     : return 36;     //LINK_NODE_STATUS
                case 11    : return 6;      //SET_MODE
                case 20    : return 20;     //PARAM_REQUEST_READ
                case 21    : return 2;      //PARAM_REQUEST_LIST
                case 22    : return 25;     //PARAM_VALUE
                case 23    : return 23;     //PARAM_SET
                case 24    : return 52;     //GPS_RAW_INT
                case 25    : return 101;    //GPS_STATUS
                case 26    : return 24;     //SCALED_IMU
                case 27    : return 29;     //RAW_IMU
                case 28    : return 16;     //RAW_PRESSURE
                case 29    : return 16;     //SCALED_PRESSURE
                case 30    : return 28;     //ATTITUDE
                case 31    : return 48;     //ATTITUDE_QUATERNION
                case 32    : return 28;     //LOCAL_POSITION_NED
                case 33    : return 28;     //GLOBAL_POSITION_INT
                case 34    : return 22;     //RC_CHANNELS_SCALED
                case 35    : return 22;     //RC_CHANNELS_RAW
                case 36    : return 37;     //SERVO_OUTPUT_RAW
                case 37    : return 7;      //MISSION_REQUEST_PARTIAL_LIST
                case 38    : return 7;      //MISSION_WRITE_PARTIAL_LIST
                case 39    : return 38;     //MISSION_ITEM
                case 40    : return 5;      //MISSION_REQUEST
                case 41    : return 4;      //MISSION_SET_CURRENT
                case 42    : return 2;      //MISSION_CURRENT
                case 43    : return 3;      //MISSION_REQUEST_LIST
                case 44    : return 5;      //MISSION_COUNT
                case 45    : return 3;      //MISSION_CLEAR_ALL
                case 46    : return 2;      //MISSION_ITEM_REACHED
                case 47    : return 4;      //MISSION_ACK
                case 48    : return 21;     //SET_GPS_GLOBAL_ORIGIN
                case 49    : return 20;     //GPS_GLOBAL_ORIGIN
                case 50    : return 37;     //PARAM_MAP_RC
                case 51    : return 5;      //MISSION_REQUEST_INT
                case 54    : return 27;     //SAFETY_SET_ALLOWED_AREA
                case 55    : return 25;     //SAFETY_ALLOWED_AREA
                case 61    : return 72;     //ATTITUDE_QUATERNION_COV
                case 62    : return 26;     //NAV_CONTROLLER_OUTPUT
                case 63    : return 181;    //GLOBAL_POSITION_INT_COV
                case 64    : return 225;    //LOCAL_POSITION_NED_COV
                case 65    : return 42;     //RC_CHANNELS
                case 66    : return 6;      //REQUEST_DATA_STREAM
                case 67    : return 4;      //DATA_STREAM
                case 69    : return 18;     //MANUAL_CONTROL
                case 70    : return 38;     //RC_CHANNELS_OVERRIDE
                case 73    : return 38;     //MISSION_ITEM_INT
                case 74    : return 20;     //VFR_HUD
                case 75    : return 35;     //COMMAND_INT
                case 76    : return 33;     //COMMAND_LONG
                case 77    : return 10;     //COMMAND_ACK
                case 80    : return 4;      //COMMAND_CANCEL
                case 81    : return 22;     //MANUAL_SETPOINT
                case 82    : return 51;     //SET_ATTITUDE_TARGET
                case 83    : return 37;     //ATTITUDE_TARGET
                case 84    : return 53;     //SET_POSITION_TARGET_LOCAL_NED
                case 85    : return 51;     //POSITION_TARGET_LOCAL_NED
                case 86    : return 53;     //SET_POSITION_TARGET_GLOBAL_INT
                case 87    : return 51;     //POSITION_TARGET_GLOBAL_INT
                case 89    : return 28;     //LOCAL_POSITION_NED_SYSTEM_GLOBAL_OFFSET
                case 90    : return 56;     //HIL_STATE
                case 91    : return 42;     //HIL_CONTROLS
                case 92    : return 33;     //HIL_RC_INPUTS_RAW
                case 93    : return 81;     //HIL_ACTUATOR_CONTROLS
                case 100   : return 34;     //OPTICAL_FLOW
                case 101   : return 117;    //GLOBAL_VISION_POSITION_ESTIMATE
                case 102   : return 117;    //VISION_POSITION_ESTIMATE
                case 103   : return 57;     //VISION_SPEED_ESTIMATE
                case 104   : return 116;    //VICON_POSITION_ESTIMATE
                case 105   : return 63;     //HIGHRES_IMU
                case 106   : return 44;     //OPTICAL_FLOW_RAD
                case 107   : return 65;     //HIL_SENSOR
                case 108   : return 84;     //SIM_STATE
                case 109   : return 9;      //RADIO_STATUS
                case 110   : return 254;    //FILE_TRANSFER_PROTOCOL
                case 111   : return 16;     //TIMESYNC
                case 112   : return 12;     //CAMERA_TRIGGER
                case 113   : return 39;     //HIL_GPS
                case 114   : return 44;     //HIL_OPTICAL_FLOW
                case 115   : return 64;     //HIL_STATE_QUATERNION
                case 116   : return 24;     //SCALED_IMU2
                case 117   : return 6;      //LOG_REQUEST_LIST
                case 118   : return 14;     //LOG_ENTRY
                case 119   : return 12;     //LOG_REQUEST_DATA
                case 120   : return 97;     //LOG_DATA
                case 121   : return 2;      //LOG_ERASE
                case 122   : return 2;      //LOG_REQUEST_END
                case 123   : return 113;    //GPS_INJECT_DATA
                case 124   : return 57;     //GPS2_RAW
                case 125   : return 6;      //POWER_STATUS
                case 126   : return 81;     //SERIAL_CONTROL
                case 127   : return 35;     //GPS_RTK
                case 128   : return 35;     //GPS2_RTK
                case 129   : return 24;     //SCALED_IMU3
                case 130   : return 13;     //DATA_TRANSMISSION_HANDSHAKE
                case 131   : return 255;    //ENCAPSULATED_DATA
                case 132   : return 39;     //DISTANCE_SENSOR
                case 133   : return 18;     //TERRAIN_REQUEST
                case 134   : return 43;     //TERRAIN_DATA
                case 135   : return 8;      //TERRAIN_CHECK
                case 136   : return 22;     //TERRAIN_REPORT
                case 137   : return 16;     //SCALED_PRESSURE2
                case 138   : return 120;    //ATT_POS_MOCAP
                case 139   : return 43;     //SET_ACTUATOR_CONTROL_TARGET
                case 140   : return 41;     //ACTUATOR_CONTROL_TARGET
                case 141   : return 32;     //ALTITUDE
                case 142   : return 243;    //RESOURCE_REQUEST
                case 143   : return 16;     //SCALED_PRESSURE3
                case 144   : return 93;     //FOLLOW_TARGET
                case 146   : return 100;    //CONTROL_SYSTEM_STATE
                case 147   : return 54;     //BATTERY_STATUS
                case 148   : return 78;     //AUTOPILOT_VERSION
                case 149   : return 60;     //LANDING_TARGET
                case 162   : return 9;      //FENCE_STATUS
                case 192   : return 54;     //MAG_CAL_REPORT
                case 225   : return 69;     //EFI_STATUS
                case 230   : return 42;     //ESTIMATOR_STATUS
                case 231   : return 40;     //WIND_COV
                case 232   : return 65;     //GPS_INPUT
                case 233   : return 182;    //GPS_RTCM_DATA
                case 234   : return 40;     //HIGH_LATENCY
                case 235   : return 42;     //HIGH_LATENCY2
                case 241   : return 32;     //VIBRATION
                case 242   : return 60;     //HOME_POSITION
                case 243   : return 61;     //SET_HOME_POSITION
                case 244   : return 6;      //MESSAGE_INTERVAL
                case 245   : return 2;      //EXTENDED_SYS_STATE
                case 246   : return 38;     //ADSB_VEHICLE
                case 247   : return 19;     //COLLISION
                case 248   : return 254;    //V2_EXTENSION
                case 249   : return 36;     //MEMORY_VECT
                case 250   : return 30;     //DEBUG_VECT
                case 251   : return 18;     //NAMED_VALUE_FLOAT
                case 252   : return 18;     //NAMED_VALUE_INT
                case 253   : return 54;     //STATUSTEXT
                case 254   : return 9;      //DEBUG
                case 256   : return 42;     //SETUP_SIGNING
                case 257   : return 9;      //BUTTON_CHANGE
                case 258   : return 232;    //PLAY_TUNE
                case 259   : return 235;    //CAMERA_INFORMATION
                case 260   : return 13;     //CAMERA_SETTINGS
                case 261   : return 61;     //STORAGE_INFORMATION
                case 262   : return 22;     //CAMERA_CAPTURE_STATUS
                case 263   : return 255;    //CAMERA_IMAGE_CAPTURED
                case 264   : return 28;     //FLIGHT_INFORMATION
                case 265   : return 20;     //MOUNT_ORIENTATION
                case 266   : return 255;    //LOGGING_DATA
                case 267   : return 255;    //LOGGING_DATA_ACKED
                case 268   : return 4;      //LOGGING_ACK
                case 269   : return 213;    //VIDEO_STREAM_INFORMATION
                case 270   : return 19;     //VIDEO_STREAM_STATUS
                case 271   : return 52;     //CAMERA_FOV_STATUS
                case 275   : return 31;     //CAMERA_TRACKING_IMAGE_STATUS
                case 276   : return 49;     //CAMERA_TRACKING_GEO_STATUS
                case 280   : return 33;     //GIMBAL_MANAGER_INFORMATION
                case 281   : return 13;     //GIMBAL_MANAGER_STATUS
                case 282   : return 35;     //GIMBAL_MANAGER_SET_ATTITUDE
                case 283   : return 144;    //GIMBAL_DEVICE_INFORMATION
                case 284   : return 32;     //GIMBAL_DEVICE_SET_ATTITUDE
                case 285   : return 40;     //GIMBAL_DEVICE_ATTITUDE_STATUS
                case 286   : return 53;     //AUTOPILOT_STATE_FOR_GIMBAL_DEVICE
                case 287   : return 23;     //GIMBAL_MANAGER_SET_PITCHYAW
                case 288   : return 23;     //GIMBAL_MANAGER_SET_MANUAL_CONTROL
                case 290   : return 46;     //ESC_INFO
                case 291   : return 57;     //ESC_STATUS
                case 299   : return 98;     //WIFI_CONFIG_AP
                case 300   : return 22;     //PROTOCOL_VERSION
                case 301   : return 58;     //AIS_VESSEL
                case 310   : return 17;     //UAVCAN_NODE_STATUS
                case 311   : return 116;    //UAVCAN_NODE_INFO
                case 320   : return 20;     //PARAM_EXT_REQUEST_READ
                case 321   : return 2;      //PARAM_EXT_REQUEST_LIST
                case 322   : return 149;    //PARAM_EXT_VALUE
                case 323   : return 147;    //PARAM_EXT_SET
                case 324   : return 146;    //PARAM_EXT_ACK
                case 330   : return 167;    //OBSTACLE_DISTANCE
                case 331   : return 233;    //ODOMETRY
                case 332   : return 239;    //TRAJECTORY_REPRESENTATION_WAYPOINTS
                case 333   : return 109;    //TRAJECTORY_REPRESENTATION_BEZIER
                case 334   : return 10;     //CELLULAR_STATUS
                case 335   : return 24;     //ISBD_LINK_STATUS
                case 336   : return 84;     //CELLULAR_CONFIG
                case 339   : return 5;      //RAW_RPM
                case 340   : return 70;     //UTM_GLOBAL_POSITION
                case 350   : return 252;    //DEBUG_FLOAT_ARRAY
                case 360   : return 25;     //ORBIT_EXECUTION_STATUS
                case 370   : return 109;    //SMART_BATTERY_INFO
                case 373   : return 42;     //GENERATOR_STATUS
                case 375   : return 140;    //ACTUATOR_OUTPUT_STATUS
                case 380   : return 20;     //TIME_ESTIMATE_TO_TARGET
                case 385   : return 133;    //TUNNEL
                case 386   : return 16;     //CAN_FRAME
                case 387   : return 72;     //CANFD_FRAME
                case 388   : return 37;     //CAN_FILTER_MODIFY
                case 390   : return 238;    //ONBOARD_COMPUTER_STATUS
                case 395   : return 212;    //COMPONENT_INFORMATION
                case 397   : return 108;    //COMPONENT_METADATA
                case 400   : return 254;    //PLAY_TUNE_V2
                case 401   : return 6;      //SUPPORTED_TUNES
                case 410   : return 53;     //EVENT
                case 411   : return 3;      //CURRENT_EVENT_SEQUENCE
                case 412   : return 6;      //REQUEST_EVENT
                case 413   : return 7;      //RESPONSE_EVENT_ERROR
                case 9000  : return 137;    //WHEEL_DISTANCE
                case 9005  : return 34;     //WINCH_STATUS
                case 12900 : return 44;     //OPEN_DRONE_ID_BASIC_ID
                case 12901 : return 59;     //OPEN_DRONE_ID_LOCATION
                case 12902 : return 53;     //OPEN_DRONE_ID_AUTHENTICATION
                case 12903 : return 46;     //OPEN_DRONE_ID_SELF_ID
                case 12904 : return 54;     //OPEN_DRONE_ID_SYSTEM
                case 12905 : return 43;     //OPEN_DRONE_ID_OPERATOR_ID
                case 12915 : return 249;    //OPEN_DRONE_ID_MESSAGE_PACK
                case 12918 : return 51;     //OPEN_DRONE_ID_ARM_STATUS
                case 12919 : return 18;     //OPEN_DRONE_ID_SYSTEM_UPDATE
                case 12920 : return 5;      //HYGROMETER_SENSOR
            }            
            return 0;
        }
        #endregion

        #region MessageID to Instance
        /// <summary>
        /// Returns the message instance to be populated w/ data
        /// </summary>
        static public IMAVLinkMessageData GetMessageInstance(int p_msg_id) {
            switch(p_msg_id) {
                case 0     : return new HeartbeatData();
                case 1     : return new SysStatusData();
                case 2     : return new SystemTimeData();
                case 4     : return new PingData();
                case 5     : return new ChangeOperatorControlData();
                case 6     : return new ChangeOperatorControlAckData();
                case 7     : return new AuthKeyData();
                case 8     : return new LinkNodeStatusData();
                case 11    : return new SetModeData();
                case 20    : return new ParamRequestReadData();
                case 21    : return new ParamRequestListData();
                case 22    : return new ParamValueData();
                case 23    : return new ParamSetData();
                case 24    : return new GpsRawIntData();
                case 25    : return new GpsStatusData();
                case 26    : return new ScaledImuData();
                case 27    : return new RawImuData();
                case 28    : return new RawPressureData();
                case 29    : return new ScaledPressureData();
                case 30    : return new AttitudeData();
                case 31    : return new AttitudeQuaternionData();
                case 32    : return new LocalPositionNedData();
                case 33    : return new GlobalPositionIntData();
                case 34    : return new RcChannelsScaledData();
                case 35    : return new RcChannelsRawData();
                case 36    : return new ServoOutputRawData();
                case 37    : return new MissionRequestPartialListData();
                case 38    : return new MissionWritePartialListData();
                case 39    : return new MissionItemData();
                case 40    : return new MissionRequestData();
                case 41    : return new MissionSetCurrentData();
                case 42    : return new MissionCurrentData();
                case 43    : return new MissionRequestListData();
                case 44    : return new MissionCountData();
                case 45    : return new MissionClearAllData();
                case 46    : return new MissionItemReachedData();
                case 47    : return new MissionAckData();
                case 48    : return new SetGpsGlobalOriginData();
                case 49    : return new GpsGlobalOriginData();
                case 50    : return new ParamMapRcData();
                case 51    : return new MissionRequestIntData();
                case 54    : return new SafetySetAllowedAreaData();
                case 55    : return new SafetyAllowedAreaData();
                case 61    : return new AttitudeQuaternionCovData();
                case 62    : return new NavControllerOutputData();
                case 63    : return new GlobalPositionIntCovData();
                case 64    : return new LocalPositionNedCovData();
                case 65    : return new RcChannelsData();
                case 66    : return new RequestDataStreamData();
                case 67    : return new DataStreamData();
                case 69    : return new ManualControlData();
                case 70    : return new RcChannelsOverrideData();
                case 73    : return new MissionItemIntData();
                case 74    : return new VfrHudData();
                case 75    : return new CommandIntData();
                case 76    : return new CommandLongData();
                case 77    : return new CommandAckData();
                case 80    : return new CommandCancelData();
                case 81    : return new ManualSetpointData();
                case 82    : return new SetAttitudeTargetData();
                case 83    : return new AttitudeTargetData();
                case 84    : return new SetPositionTargetLocalNedData();
                case 85    : return new PositionTargetLocalNedData();
                case 86    : return new SetPositionTargetGlobalIntData();
                case 87    : return new PositionTargetGlobalIntData();
                case 89    : return new LocalPositionNedSystemGlobalOffsetData();
                case 90    : return new HilStateData();
                case 91    : return new HilControlsData();
                case 92    : return new HilRcInputsRawData();
                case 93    : return new HilActuatorControlsData();
                case 100   : return new OpticalFlowData();
                case 101   : return new GlobalVisionPositionEstimateData();
                case 102   : return new VisionPositionEstimateData();
                case 103   : return new VisionSpeedEstimateData();
                case 104   : return new ViconPositionEstimateData();
                case 105   : return new HighresImuData();
                case 106   : return new OpticalFlowRadData();
                case 107   : return new HilSensorData();
                case 108   : return new SimStateData();
                case 109   : return new RadioStatusData();
                case 110   : return new FileTransferProtocolData();
                case 111   : return new TimesyncData();
                case 112   : return new CameraTriggerData();
                case 113   : return new HilGpsData();
                case 114   : return new HilOpticalFlowData();
                case 115   : return new HilStateQuaternionData();
                case 116   : return new ScaledImu2Data();
                case 117   : return new LogRequestListData();
                case 118   : return new LogEntryData();
                case 119   : return new LogRequestDataData();
                case 120   : return new LogDataData();
                case 121   : return new LogEraseData();
                case 122   : return new LogRequestEndData();
                case 123   : return new GpsInjectDataData();
                case 124   : return new Gps2RawData();
                case 125   : return new PowerStatusData();
                case 126   : return new SerialControlData();
                case 127   : return new GpsRtkData();
                case 128   : return new Gps2RtkData();
                case 129   : return new ScaledImu3Data();
                case 130   : return new DataTransmissionHandshakeData();
                case 131   : return new EncapsulatedDataData();
                case 132   : return new DistanceSensorData();
                case 133   : return new TerrainRequestData();
                case 134   : return new TerrainDataData();
                case 135   : return new TerrainCheckData();
                case 136   : return new TerrainReportData();
                case 137   : return new ScaledPressure2Data();
                case 138   : return new AttPosMocapData();
                case 139   : return new SetActuatorControlTargetData();
                case 140   : return new ActuatorControlTargetData();
                case 141   : return new AltitudeData();
                case 142   : return new ResourceRequestData();
                case 143   : return new ScaledPressure3Data();
                case 144   : return new FollowTargetData();
                case 146   : return new ControlSystemStateData();
                case 147   : return new BatteryStatusData();
                case 148   : return new AutopilotVersionData();
                case 149   : return new LandingTargetData();
                case 162   : return new FenceStatusData();
                case 192   : return new MagCalReportData();
                case 225   : return new EfiStatusData();
                case 230   : return new EstimatorStatusData();
                case 231   : return new WindCovData();
                case 232   : return new GpsInputData();
                case 233   : return new GpsRtcmDataData();
                case 234   : return new HighLatencyData();
                case 235   : return new HighLatency2Data();
                case 241   : return new VibrationData();
                case 242   : return new HomePositionData();
                case 243   : return new SetHomePositionData();
                case 244   : return new MessageIntervalData();
                case 245   : return new ExtendedSysStateData();
                case 246   : return new AdsbVehicleData();
                case 247   : return new CollisionData();
                case 248   : return new V2ExtensionData();
                case 249   : return new MemoryVectData();
                case 250   : return new DebugVectData();
                case 251   : return new NamedValueFloatData();
                case 252   : return new NamedValueIntData();
                case 253   : return new StatustextData();
                case 254   : return new DebugData();
                case 256   : return new SetupSigningData();
                case 257   : return new ButtonChangeData();
                case 258   : return new PlayTuneData();
                case 259   : return new CameraInformationData();
                case 260   : return new CameraSettingsData();
                case 261   : return new StorageInformationData();
                case 262   : return new CameraCaptureStatusData();
                case 263   : return new CameraImageCapturedData();
                case 264   : return new FlightInformationData();
                case 265   : return new MountOrientationData();
                case 266   : return new LoggingDataData();
                case 267   : return new LoggingDataAckedData();
                case 268   : return new LoggingAckData();
                case 269   : return new VideoStreamInformationData();
                case 270   : return new VideoStreamStatusData();
                case 271   : return new CameraFovStatusData();
                case 275   : return new CameraTrackingImageStatusData();
                case 276   : return new CameraTrackingGeoStatusData();
                case 280   : return new GimbalManagerInformationData();
                case 281   : return new GimbalManagerStatusData();
                case 282   : return new GimbalManagerSetAttitudeData();
                case 283   : return new GimbalDeviceInformationData();
                case 284   : return new GimbalDeviceSetAttitudeData();
                case 285   : return new GimbalDeviceAttitudeStatusData();
                case 286   : return new AutopilotStateForGimbalDeviceData();
                case 287   : return new GimbalManagerSetPitchyawData();
                case 288   : return new GimbalManagerSetManualControlData();
                case 290   : return new EscInfoData();
                case 291   : return new EscStatusData();
                case 299   : return new WifiConfigApData();
                case 300   : return new ProtocolVersionData();
                case 301   : return new AisVesselData();
                case 310   : return new UavcanNodeStatusData();
                case 311   : return new UavcanNodeInfoData();
                case 320   : return new ParamExtRequestReadData();
                case 321   : return new ParamExtRequestListData();
                case 322   : return new ParamExtValueData();
                case 323   : return new ParamExtSetData();
                case 324   : return new ParamExtAckData();
                case 330   : return new ObstacleDistanceData();
                case 331   : return new OdometryData();
                case 332   : return new TrajectoryRepresentationWaypointsData();
                case 333   : return new TrajectoryRepresentationBezierData();
                case 334   : return new CellularStatusData();
                case 335   : return new IsbdLinkStatusData();
                case 336   : return new CellularConfigData();
                case 339   : return new RawRpmData();
                case 340   : return new UtmGlobalPositionData();
                case 350   : return new DebugFloatArrayData();
                case 360   : return new OrbitExecutionStatusData();
                case 370   : return new SmartBatteryInfoData();
                case 373   : return new GeneratorStatusData();
                case 375   : return new ActuatorOutputStatusData();
                case 380   : return new TimeEstimateToTargetData();
                case 385   : return new TunnelData();
                case 386   : return new CanFrameData();
                case 387   : return new CanfdFrameData();
                case 388   : return new CanFilterModifyData();
                case 390   : return new OnboardComputerStatusData();
                case 395   : return new ComponentInformationData();
                case 397   : return new ComponentMetadataData();
                case 400   : return new PlayTuneV2Data();
                case 401   : return new SupportedTunesData();
                case 410   : return new EventData();
                case 411   : return new CurrentEventSequenceData();
                case 412   : return new RequestEventData();
                case 413   : return new ResponseEventErrorData();
                case 9000  : return new WheelDistanceData();
                case 9005  : return new WinchStatusData();
                case 12900 : return new OpenDroneIdBasicIdData();
                case 12901 : return new OpenDroneIdLocationData();
                case 12902 : return new OpenDroneIdAuthenticationData();
                case 12903 : return new OpenDroneIdSelfIdData();
                case 12904 : return new OpenDroneIdSystemData();
                case 12905 : return new OpenDroneIdOperatorIdData();
                case 12915 : return new OpenDroneIdMessagePackData();
                case 12918 : return new OpenDroneIdArmStatusData();
                case 12919 : return new OpenDroneIdSystemUpdateData();
                case 12920 : return new HygrometerSensorData();
            }            
            return null;
        }
        #endregion

        #region class Signature
        /// <summary>
        /// Signature data structure
        /// </summary>
        public class Signature {

            /// <summary>
            /// Signature Link Id
            /// </summary>
            public byte linkId;

            /// <summary>
            /// Signature Timestamp 6 bytes
            /// </summary>
            public ulong timestamp;

            /// <summary>
            /// Signature Hash 6 bytes
            /// </summary>
            public ulong hash;

        }
        #endregion

        /// <summary>
        /// Flag that tells this header is valid.
        /// </summary>
        public bool valid { get { return version>0; } }

        /// <summary>
        /// MAVLink Version
        /// </summary>
        public int version;

        /// <summary>
        /// Payload Byte Length
        /// </summary>
        public byte payloadLength;

        /// <summary>
        /// Incompatibility Mask
        /// </summary>
        public byte incompatibilityFlags;

        /// <summary>
        /// Incompatibility Mask
        /// </summary>
        public byte compatibilityFlags;

        /// <summary>
        /// Sequence Index
        /// </summary>
        public byte sequence;

        /// <summary>
        /// System Id
        /// </summary>
        public byte systemId;

        /// <summary>
        /// System Id
        /// </summary>
        public byte componentId;

        /// <summary>
        /// Message Id
        /// </summary>
        public MAVLinkMsgId messageId; 

        /// <summary>
        /// Reference to the payload data
        /// </summary>
        public IMAVLinkMessageData data;

        /// <summary>
        /// Flag that tells there is signature info
        /// </summary>
        public bool isSigned { get { return (incompatibilityFlags & 0x1) != 0; } }

        /// <summary>
        /// Reference to signature data
        /// </summary>        
        public Signature signature { get; internal set; }

        /// <summary>
        /// Return the string representation
        /// </summary>
        /// <returns></returns>
        public override string ToString() {
            return $"MAVLinkMsg.{messageId} | {payloadLength}/{MAVLinkMsg.GetMessagePayloadLength((int)messageId)} bytes | sys: {systemId} comp: {componentId}";
        }

    }
}
