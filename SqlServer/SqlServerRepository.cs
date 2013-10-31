
namespace Escc.Gritting.SqlServer
{
    /// <summary>
    /// A service to store and retrieve data about gritters using Microsoft SQL Server
    /// </summary>
    public class SqlServerRepository : IGritterDataRepository
    {
        /// <summary>
        /// Reads data about all gritters including their current position
        /// </summary>
        /// <returns></returns>
        public System.Collections.Generic.ICollection<Gritter> ReadAllGritters()
        {
            throw new System.NotImplementedException();
        }
    }
}
