using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Caching;

namespace Escc.Gritting
{
    /// <summary>
    /// Caches gritter data in the ASP.NET application cache
    /// </summary>
    public class ApplicationCacheStrategy : ICacheStrategy
    {
        private const string CacheKey = "Escc.Gritting.ApplicationCacheStrategy.Gritters";

        /// <summary>
        /// Caches the gritters.
        /// </summary>
        /// <param name="gritters">The gritters.</param>
        /// <param name="seconds">How many seconds to cache the data for.</param>
        public void CacheGritters(ICollection<Gritter> gritters, int seconds)
        {
            HttpContext.Current.Cache.Add(CacheKey, (object)gritters, (CacheDependency)null, DateTime.Now.AddSeconds((double)seconds), Cache.NoSlidingExpiration, CacheItemPriority.Normal, (CacheItemRemovedCallback)null);
        }

        /// <summary>
        /// Retrieves the gritters from the cache.
        /// </summary>
        /// <returns></returns>
        public ICollection<Gritter> RetrieveGritters()
        {
            return HttpContext.Current.Cache[CacheKey] as ICollection<Gritter>;
        }
    }
}
