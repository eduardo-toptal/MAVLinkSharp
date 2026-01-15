        
namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// Fuel types for use in FUEL_TYPE. Fuel types specify the units for the maximum, available and consumed fuel, and for the flow rates.
    /// </summary>    
    public enum MAVFuelTypeFlags {
        Unknown               = 0,           //Not specified. Fuel levels are normalized (i.e. maximum is 1, and other levels are relative to 1).
        Liquid                = 1,           //A generic liquid fuel. Fuel levels are in millilitres (ml). Fuel rates are in millilitres/second.
        Gas                   = 2            //A gas tank. Fuel levels are in kilo-Pascal (kPa), and flow rates are in milliliters per second (ml/s).
    }

}
