using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Escc.Gritting
{
    /// <summary>
    /// Is a gritter stopped or moving?
    /// </summary>
    public enum GritterStatus
    {
        /// <summary>
        /// The gritter is stopped with its engine off
        /// </summary>
        Stopped=1,

        /// <summary>
        /// The gritter's engine is on but it's not moving
        /// </summary>
        Idle=2,

        /// <summary>
        /// The gritter is moving
        /// </summary>
        Moving=3
    }
}
