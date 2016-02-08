
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using Dapper;

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
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["GrittingReader"].ConnectionString))
            {
                using (var reader = connection.ExecuteReader("usp_Gritter_SelectAll", commandType: CommandType.StoredProcedure))
                {
                    while (reader.Read())
                    {
                        gritters.Add(new Gritter()
                        {
                            GritterId = reader["GritterId"].ToString(),
                            GritterName = reader["GritterName"].ToString(),
                            Latitude = Double.Parse(reader["Latitude"].ToString(), CultureInfo.InvariantCulture),
                            Longitude = Double.Parse(reader["Longitude"].ToString(), CultureInfo.InvariantCulture),
                            Status = (GritterStatus) Enum.Parse(typeof (GritterStatus), reader["Status"].ToString())
                        });
                    }
                }
            }
            return gritters;
        }

        /// <summary>
        /// Adds or updates data about a gritter
        /// </summary>
        /// <param name="gritter">The gritter.</param>
        public void SaveGritter(Gritter gritter)
        {
            if (gritter == null) throw new ArgumentNullException("gritter");

            var parameters = new DynamicParameters();
            parameters.Add("@gritterId", gritter.GritterId);
            parameters.Add("@gritterName", gritter.GritterName);
            parameters.Add("@latitude", gritter.Latitude);
            parameters.Add("@longitude", gritter.Longitude);
            parameters.Add("@status", (int) gritter.Status);

            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["GrittingWriter"].ConnectionString))
            {
                connection.Execute("usp_Gritter_Save", parameters, commandType: CommandType.StoredProcedure);
            }
        }
    }
}
