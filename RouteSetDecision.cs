
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text;
using System.Xml.Serialization;
namespace Escc.Gritting
{
    /// <summary>
    /// A decision taken on an action plan for routes within an entire gritting route set
    /// </summary>
    public class RouteSetDecision : GrittingDecision
    {
        /// <summary>
        /// Creates a new <see cref="GrittingDecision"/>
        /// </summary>
        public RouteSetDecision()
        {
            this.RouteSet = new GrittingRouteSet();
        }

        /// <summary>
        /// The route set to which the decision applies
        /// </summary>
        public GrittingRouteSet RouteSet { get; set; }

        private List<RouteDecision> routeDecisions = new List<RouteDecision>();

        /// <summary>
        /// The individual route decisions which make up the overall decision for the route set
        /// </summary>
        public IList<RouteDecision> RouteDecisions { get { return this.routeDecisions; } }

        /// <summary>
        /// Gets whether there are route decisions for all routes in the route set, and therefore this is a valid route set decision
        /// </summary>
        /// <returns></returns>
        public bool HasRouteDecisionsForAllRoutes()
        {
            if (this.RouteSet == null) throw new InvalidOperationException("RouteSet must be set before calling HasRouteDecisionsForAllRoutes()");

            var decisionsFoundForAllRoutesInSet = true;

            foreach (GrittingRoute route in RouteSet.Routes)
            {
                var decisionFoundForRoute = false;
                foreach (RouteDecision decision in routeDecisions)
                {
                    if (route.RouteName == decision.Route.RouteName)
                    {
                        decisionFoundForRoute = true;
                        break;
                    }
                }

                if (!decisionFoundForRoute)
                {
                    decisionsFoundForAllRoutesInSet = false;
                    break;
                }
            }

            return decisionsFoundForAllRoutesInSet;
        }

        /// <summary>
        /// Gets or sets the linked data URI representing the decision.
        /// </summary>
        /// <value>The URI.</value>
        [XmlIgnore]
        public Uri LinkedDataUri
        {
            get
            {
                var decisionUri = new StringBuilder("http://www.eastsussex.gov.uk/id/roadmaintenance/decision/");
                decisionUri.Append(DecisionTime.Year);
                decisionUri.Append("/");
                decisionUri.Append(DecisionTime.Month.ToString("00", CultureInfo.InvariantCulture));
                decisionUri.Append("/");
                decisionUri.Append(DecisionTime.Day.ToString("00", CultureInfo.InvariantCulture));
                decisionUri.Append("/");
                decisionUri.Append(this.DecisionId);

                return new Uri(decisionUri.ToString());
            }
        }

        /// <summary>
        /// Gets or sets the linked data URI representing the closure. Synonym for <seealso cref="LinkedDataUri"/> which is compatible with serialisation.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "value"), SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings"), XmlElement("LinkedDataUri")]
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public string LinkedDataUriSerialisable
        {
            get { return (this.LinkedDataUri != null) ? this.LinkedDataUri.ToString() : null; }
            set
            {
                // do nothing, URI is generated and this is only here for compatibility with serialisation
            }
        }
    }
}
