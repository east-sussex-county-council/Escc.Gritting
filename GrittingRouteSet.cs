using System;
using System.Collections.Generic;
using System.Globalization;

namespace Escc.Gritting
{
    /// <summary>
    /// A geographic area for which gritting decisions are taken
    /// </summary>
    public class GrittingRouteSet
    {
        /// <summary>
        /// Gets or sets the id of the route set
        /// </summary>
        public int RouteSetId { get; set; }

        /// <summary>
        /// Gets or sets the name of the route set, as displayed on website
        /// </summary>
        /// <value>The route set name.</value>
        public string RouteSetName { get; set; }

        /// <summary>
        /// Gets or sets the route set name used by highways staff.
        /// </summary>
        /// <value>The route set name.</value>
        public string RouteSetNameInternal { get; set; }

        private string domainName;

        /// <summary>
        /// The gritting domain of which the route set is a part
        /// </summary>
        public string GrittingDomainName
        {
            get
            {
                if (String.IsNullOrEmpty(domainName))
                {
                    var pos = RouteSetName.IndexOf(" - ");
                    if (pos > -1) domainName = RouteSetName.Substring(0, pos);
                }
                return domainName;
            }
        }

        /// <summary>
        /// Gets whether this route set represents the whole county.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is whole county; otherwise, <c>false</c>.
        /// </value>
        public bool IsWholeCounty
        {
            get
            {
                if (String.IsNullOrEmpty(this.RouteSetName)) return false;
                return this.RouteSetName.ToUpper(CultureInfo.CurrentCulture).Contains("WHOLE COUNTY");
            }
        }


        /// <summary>
        /// The type of route, reflecting how often it is gritted
        /// </summary>
        public GrittingRouteType RouteTypeToGrit
        {
            get
            {
                if (this.RouteSetName.ToUpperInvariant().Contains("PRIMARY")) return GrittingRouteType.Primary;
                else if (this.RouteSetName.ToUpperInvariant().Contains("SECONDARY")) return GrittingRouteType.Secondary;
                else return GrittingRouteType.All;
            }
        }

        private List<GrittingRoute> routes = new List<GrittingRoute>();

        /// <summary>
        /// The gritting routes within the route set
        /// </summary>
        public IList<GrittingRoute> Routes { get { return this.routes; } }

        /// <summary>
        /// Checks whether a route is in this set
        /// </summary>
        /// <param name="route"></param>
        /// <returns></returns>
        public bool Contains(GrittingRoute route)
        {
            if (route == null) throw new ArgumentNullException("route");

            var found = false;

            foreach (var routeInSet in this.routes)
            {
                if (routeInSet.RouteName == route.RouteName)
                {
                    found = true;
                    break;
                }
            }

            return found;
        }
    }
}
