
namespace Escc.Gritting
{
    /// <summary>
    /// A gritting lorry
    /// </summary>
    public class Gritter
    {
        /// <summary>
        /// An internal identifier for the gritter
        /// </summary>
        public string GritterId { get; set; }

        /// <summary>
        /// A friendly name given to the gritter
        /// </summary>
        public string GritterName { get; set; }

        /// <summary>
        /// The latitude of the gritter's current position
        /// </summary>
        public double Latitude { get; set; }

        /// <summary>
        /// The longitude of the gritter's current position
        /// </summary>
        public double Longitude { get; set; }
    }
}
