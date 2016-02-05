
namespace Escc.Gritting
{
    /// <summary>
    /// A decision taken on an action plan for gritting a particular route
    /// </summary>
    public class RouteDecision : GrittingDecision
    {
        /// <summary>
        /// Creates a new <see cref="RouteDecision"/>
        /// </summary>
        public RouteDecision()
        {
            this.Route = new GrittingRoute();
        }

        /// <summary>
        /// The route to which the decision applies
        /// </summary>
        public GrittingRoute Route { get; set; }
    }
}
