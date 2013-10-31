
using System.Collections.Generic;

namespace Escc.Gritting
{
    /// <summary>
    /// A service to store and retrieve data about gritters
    /// </summary>
    interface IGritterDataRepository
    {
        /// <summary>
        /// Reads data about all gritters including their current position
        /// </summary>
        /// <returns></returns>
        ICollection<Gritter> ReadAllGritters();
    }
}
