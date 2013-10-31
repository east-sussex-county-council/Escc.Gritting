
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using Microsoft.ApplicationBlocks.Data;

namespace Escc.Gritting.SqlServer
{
    /// <summary>
    /// A service to store and retrieve data about gritters using Microsoft SQL Server
    /// </summary>
    public class SqlServerGritterRepository : IGritterDataRepository
    {
        /// <summary>
        /// Reads data about all gritters including their current position
        /// </summary>
        /// <returns></returns>
        public System.Collections.Generic.ICollection<Gritter> ReadAllGritters()
        {
            var gritters = new List<Gritter>();
            using (var reader = SqlHelper.ExecuteReader(ConfigurationManager.ConnectionStrings["GrittingReader"].ConnectionString, CommandType.StoredProcedure, "usp_Gritter_SelectAll"))
            {
                while (reader.Read())
                {
                    gritters.Add(new Gritter()
                        {
                            GritterId = reader["GritterId"].ToString(),
                            GritterName = reader["GritterName"].ToString(),
                            Latitude = Double.Parse(reader["Latitude"].ToString()),
                            Longitude = Double.Parse(reader["Longitude"].ToString())
                        });
                }
            }
            return gritters;
        }
    }
}
