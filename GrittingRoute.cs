
using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
namespace Escc.Gritting
{
    /// <summary>
    /// A gritting route
    /// </summary>
    public class GrittingRoute
    {
        /// <summary>
        /// Gets or sets the route name used by highways staff.
        /// </summary>
        /// <value>The route name.</value>
        public string RouteName { get; set; }

        /// <summary>
        /// The type of route, reflecting how often it is gritted
        /// </summary>
        public GrittingRouteType RouteTypeToGrit { get; set; }


        /// <summary>
        /// Gets or sets the linked data URI representing the gritting route.
        /// </summary>
        /// <value>The URI.</value>
        [XmlIgnore]
        public Uri LinkedDataUri
        {
            get
            {
                var routeUri = new StringBuilder("http://www.eastsussex.gov.uk/id/roadmaintenance/grittingroute/");
                routeUri.Append(Regex.Replace(Regex.Replace(this.RouteName.ToLower(CultureInfo.CurrentCulture), "[^a-z ]", String.Empty), @"\s+", "-"));
                return new Uri(routeUri.ToString());
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
