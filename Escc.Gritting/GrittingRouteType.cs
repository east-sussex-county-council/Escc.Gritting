namespace Escc.Gritting
{
    /// <summary>
    /// The type of gritting route which determines when it is gritted
    /// </summary>
    public enum GrittingRouteType
    {
        /// <summary>
        /// Route type not known
        /// </summary>
        Unknown,

        /// <summary>
        /// A primary gritting route
        /// </summary>
        Primary,

        /// <summary>
        /// A secondary gritting route
        /// </summary>
        Secondary,

        /// <summary>
        /// A route that encompasses primary and secondary routes in one
        /// </summary>
        All
    }
}