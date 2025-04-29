        
namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// Possible actions an aircraft can take to avoid a collision.
    /// </summary>    
    public enum MAVCollisionActionFlags {
        None                                    = 0,           //Ignore any potential collisions
        Report                                  = 1,           //Report potential collision
        AscendOrDescend                         = 2,           //Ascend or Descend to avoid threat
        MoveHorizontally                        = 3,           //Move horizontally to avoid threat
        MovePerpendicular                       = 4,           //Aircraft to move perpendicular to the collision's velocity vector
        Rtl                                     = 5,           //Aircraft to fly directly back to its launch point
        Hover                                   = 6            //Aircraft to stop in place
    }

}
