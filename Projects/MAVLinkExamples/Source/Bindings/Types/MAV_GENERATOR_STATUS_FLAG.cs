        
namespace MAVLinkBindings {

    /// <summary>
    /// Flags to report status/failure cases for a power generator (used in GENERATOR_STATUS). Note that FAULTS are conditions that cause the generator to fail. Warnings are conditions that require attention before the next use (they indicate the system is not operating properly).
    /// </summary>    
    public enum MAVGeneratorStatusFlag {
        Off                                                        = 1,           //Generator is off.
        Ready                                                      = 2,           //Generator is ready to start generating power.
        Generating                                                 = 4,           //Generator is generating power.
        Charging                                                   = 8,           //Generator is charging the batteries (generating enough power to charge and provide the load).
        ReducedPower                                               = 16,          //Generator is operating at a reduced maximum power.
        Maxpower                                                   = 32,          //Generator is providing the maximum output.
        OvertempWarning                                            = 64,          //Generator is near the maximum operating temperature, cooling is insufficient.
        OvertempFault                                              = 128,         //Generator hit the maximum operating temperature and shutdown.
        ElectronicsOvertempWarning                                 = 256,         //Power electronics are near the maximum operating temperature, cooling is insufficient.
        ElectronicsOvertempFault                                   = 512,         //Power electronics hit the maximum operating temperature and shutdown.
        ElectronicsFault                                           = 1024,        //Power electronics experienced a fault and shutdown.
        PowersourceFault                                           = 2048,        //The power source supplying the generator failed e.g. mechanical generator stopped, tether is no longer providing power, solar cell is in shade, hydrogen reaction no longer happening.
        CommunicationWarning                                       = 4096,        //Generator controller having communication problems.
        CoolingWarning                                             = 8192,        //Power electronic or generator cooling system error.
        PowerRailFault                                             = 16384,       //Generator controller power rail experienced a fault.
        OvercurrentFault                                           = 32768,       //Generator controller exceeded the overcurrent threshold and shutdown to prevent damage.
        BatteryOverchargeCurrentFault                              = 65536,       //Generator controller detected a high current going into the batteries and shutdown to prevent battery damage.
        OvervoltageFault                                           = 131072,      //Generator controller exceeded it's overvoltage threshold and shutdown to prevent it exceeding the voltage rating.
        BatteryUndervoltFault                                      = 262144,      //Batteries are under voltage (generator will not start).
        StartInhibited                                             = 524288,      //Generator start is inhibited by e.g. a safety switch.
        MaintenanceRequired                                        = 1048576,     //Generator requires maintenance.
        WarmingUp                                                  = 2097152,     //Generator is not ready to generate yet.
        Idle                                                       = 4194304      //Generator is idle.
    }

}
