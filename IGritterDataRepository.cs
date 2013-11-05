

namespace Escc.Gritting
{
    /// <summary>
    /// A service to store and retrieve data about gritters
    /// </summary>
    public interface IGritterDataRepository : IGritterDataSource
    {
        /// <summary>
        /// Adds or updates data about a gritter
        /// </summary>
        /// <param name="gritter">The gritter.</param>
        void SaveGritter(Gritter gritter);
    }
}
