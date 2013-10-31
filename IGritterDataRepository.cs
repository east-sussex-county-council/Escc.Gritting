
using System.Collections.Generic;

namespace Escc.Gritting
{
    /// <summary>
    /// A service to store and retrieve data about gritters
    /// </summary>
    public interface IGritterDataRepository
    {
        /// <summary>
        /// Reads data about all gritters including their current position
        /// </summary>
        /// <returns></returns>
        ICollection<Gritter> ReadAllGritters();

        /// <summary>
        /// Adds or updates data about a gritter
        /// </summary>
        /// <param name="gritter">The gritter.</param>
        void SaveGritter(Gritter gritter);
    }
}
