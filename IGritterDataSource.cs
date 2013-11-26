
using System.Collections.Generic;

namespace Escc.Gritting
{
    /// <summary>
    /// A service to retrieve data about gritters
    /// </summary>
    public interface IGritterDataSource
    {
        /// <summary>
        /// Reads data about all gritters including their current position
        /// </summary>
        /// <returns></returns>
        ICollection<Gritter> ReadAllGritters();
    }
}
