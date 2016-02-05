using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Escc.Gritting
{
    /// <summary>
    /// A method of caching gritter data from an <see cref="IGritterDataSource" />
    /// </summary>
    public interface ICacheStrategy
    {
        /// <summary>
        /// Caches the gritters.
        /// </summary>
        /// <param name="gritters">The gritters.</param>
        /// <param name="seconds">How many seconds to cache the data for.</param>
        void CacheGritters(ICollection<Gritter> gritters, int seconds);

        /// <summary>
        /// Retrieves the gritters from the cache.
        /// </summary>
        /// <returns></returns>
        ICollection<Gritter> RetrieveGritters();
    }
}
