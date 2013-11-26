
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
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
                            Latitude = Double.Parse(reader["Latitude"].ToString(), CultureInfo.InvariantCulture),
                            Longitude = Double.Parse(reader["Longitude"].ToString(), CultureInfo.InvariantCulture),
                            Status = (GritterStatus)Enum.Parse(typeof(GritterStatus), reader["Status"].ToString())
                        });
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

            var id = new SqlParameter("@gritterId", gritter.GritterId);
            var name = new SqlParameter("@gritterName", gritter.GritterName);
            var latitude = new SqlParameter("@latitude", gritter.Latitude);
            var longitude = new SqlParameter("@longitude", gritter.Longitude);
            var status = new SqlParameter("@status", (int) gritter.Status);

            SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["GrittingWriter"].ConnectionString, CommandType.StoredProcedure, "usp_Gritter_Save", id, name, latitude, longitude, status);
        }
    }
}
