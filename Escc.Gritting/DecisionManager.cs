using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using Dapper;
using Exceptionless;
using log4net;
namespace Escc.Gritting
{
    /// <summary>
    /// Read and write gritting decisions
    /// </summary>
    public static class DecisionManager
    {
        #region Read from disk

        /// <summary>
        /// Method for reading a decision file and saving it to a database
        /// </summary>
        /// <param name="fileName">Path to decision file</param>
        /// <param name="log">log4Net logger</param>
        /// <returns><c>true</c> if file found and parsed, <c>false</c> otherwise</returns>
        public delegate void DecisionFileReader(string fileName, ILog log);

        /// <summary>
        /// Reads and processes any decision files in the configured folder
        /// </summary>
        /// <param name="fileReaderStrategy"></param>
        /// <param name="log"></param>
        public static void ReadDecisionFile(DecisionManager.DecisionFileReader fileReaderStrategy, ILog log)
        {
            if (fileReaderStrategy == null) throw new ArgumentNullException("fileReaderStrategy");
            if (log == null) throw new ArgumentNullException("log");

            try
            {
                // Get folder and file pattern from web.config so they're easy to update
                string folder = ConfigurationManager.AppSettings["DecisionsFolder"];
                if (String.IsNullOrEmpty(folder)) throw new ConfigurationErrorsException("DecisionsFolder setting is missing from appSettings in app.config");

                string filePattern = ConfigurationManager.AppSettings["DecisionFiles"];
                if (String.IsNullOrEmpty(filePattern)) throw new ConfigurationErrorsException("DecisionFiles setting is missing from appSettings in app.config");

                // Look in the folder, deal with and then delete each file
                var filenames = Directory.GetFiles(folder, filePattern, SearchOption.TopDirectoryOnly);
                foreach (string filename in filenames)
                {
                    // Log that we're working on this file
                    Console.WriteLine(String.Format(CultureInfo.CurrentCulture, Properties.Resources.LogReadingFile, filename));
                    log.Info(String.Format(CultureInfo.CurrentCulture, Properties.Resources.LogReadingFile, filename));

                    if (File.Exists(filename))
                    {
                        fileReaderStrategy(filename, log);

                        var delete = String.IsNullOrEmpty(ConfigurationManager.AppSettings["DeleteProcessedFiles"]);
                        try
                        {
                            if (!delete) delete = Boolean.Parse(ConfigurationManager.AppSettings["DeleteProcessedFiles"]);
                        }
                        catch (FormatException ex)
                        {
                            throw new ConfigurationErrorsException("DeleteProcessedFiles setting in app.config appSettings must be a boolean", ex);
                        }

                        if (delete)
                        {
                            Console.WriteLine(String.Format(CultureInfo.CurrentCulture, Properties.Resources.LogDeletingFile, filename));
                            log.Info(String.Format(CultureInfo.CurrentCulture, Properties.Resources.LogDeletingFile, filename));

                            File.Delete(filename);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                log.Error(ex.Message, ex);

                ex.ToExceptionless().Submit();

                return;
            }
        }

        #endregion // Read from disk

        #region Save decisions

        /// <summary>
        /// Save a route decision
        /// </summary>
        /// <param name="decision">The decision to save</param>
        /// <param name="decisionText">The original route decision text from the supplier's system, if any.</param>
        public static void SaveRouteDecision(RouteDecision decision, string decisionText)
        {
            if (decision == null) throw new ArgumentNullException("decision");

            var parameters = new DynamicParameters();
            parameters.Add("@decisionText", decisionText);
            parameters.Add("@supplierDecisionId", decision.SupplierDecisionId);
            parameters.Add("@messageType", decision.MessageType);
            parameters.Add("@routeName", decision.Route.RouteName);
            parameters.Add("@actionOriginal", decision.OriginalAction);
            parameters.Add("@actionWeb", decision.Action);
            parameters.Add("@decisionTime", decision.DecisionTime);
            parameters.Add("@actionTime", decision.ActionTime);
            parameters.Add("@notes", decision.Notes);

            using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GrittingDecisionWriter"].ConnectionString))
            {
                conn.Execute("usp_RouteDecision_Save", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        /// <summary>
        /// Save a decision for a route set
        /// </summary>
        /// <param name="decision">The decision to save</param>
        /// <param name="decisionText">The original decision text from the supplier's system, if any.</param>
        public static void SaveRouteSetDecision(RouteSetDecision decision, string decisionText)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@decisionText", decisionText);
            parameters.Add("@messageType", decision.MessageType);
            parameters.Add("@routeSetInternal", decision.RouteSet.RouteSetNameInternal);
            parameters.Add("@routeSetWeb", decision.RouteSet.RouteSetName);
            parameters.Add("@actionOriginal", decision.OriginalAction);
            parameters.Add("@actionWeb", decision.Action);
            parameters.Add("@decisionTime", decision.DecisionTime);
            parameters.Add("@actionTime", decision.ActionTime);
            parameters.Add("@notes", decision.Notes);
            parameters.Add("@routeSetDecisionId", null, DbType.Int32, ParameterDirection.Output);

            using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GrittingDecisionWriter"].ConnectionString))
            {
                conn.Execute("usp_RouteSetDecision_Save", parameters, commandType: CommandType.StoredProcedure);

                var decisionId = parameters.Get<int>("@routeSetDecisionId");
                foreach (RouteDecision route in decision.RouteDecisions)
                {
                    var routeParameters = new DynamicParameters();
                    routeParameters.Add("@routeSetDecisionId", decisionId);
                    routeParameters.Add("@supplierDecisionId", route.SupplierDecisionId);
                    conn.Execute("usp_RouteSetDecision_LinkToRouteDecision", routeParameters, commandType: CommandType.StoredProcedure);
                }
            }
        }

        /// <summary>
        /// Checks whether a decision has already been saved in the database
        /// </summary>
        /// <param name="decisionText">The decision text.</param>
        /// <returns></returns>
        public static bool DecisionExists(string decisionText)
        {
            using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GrittingDecisionReader"].ConnectionString))
            {
                var decisionCount = (int)conn.ExecuteScalar("usp_Decision_Exists", new { decisionText }, commandType: CommandType.StoredProcedure);
                return (decisionCount > 0);
            }
        }

        #endregion // Parse and save decisions

        #region Read gritting info from database

        /// <summary>
        /// Read all the route sets for the county
        /// </summary>
        /// <returns></returns>
        public static IList<GrittingRouteSet> ReadRouteSets()
        {
            var routeSets = new Dictionary<int, GrittingRouteSet>();

            using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GrittingDecisionReader"].ConnectionString))
            {
                using (var reader = conn.ExecuteReader("usp_RouteSet_SelectAllRoutes", commandType: CommandType.StoredProcedure))
                {
                    while (reader.Read())
                    {
                        var routeSetId = Int32.Parse(reader["RouteSetId"].ToString(), CultureInfo.InvariantCulture);
                        if (!routeSets.ContainsKey(routeSetId))
                        {
                            var routeSet = new GrittingRouteSet();
                            routeSet.RouteSetId = routeSetId;
                            routeSet.RouteSetName = reader["RouteSetName"].ToString();
                            routeSets.Add(routeSetId, routeSet);
                        }

                        var route = new GrittingRoute();
                        route.RouteName = reader["RouteName"].ToString();
                        routeSets[routeSetId].Routes.Add(route);
                    }
                }
            }

            return new List<GrittingRouteSet>(routeSets.Values);
        }


        /// <summary>
        /// Reads route set decisions from the last 26 hours.
        /// </summary>
        /// <returns></returns>
        public static IList<GrittingDecision> ReadLatestRouteSetDecisions()
        {
            var decisions = new List<GrittingDecision>();

            using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GrittingDecisionReader"].ConnectionString))
            {
                using (var reader = conn.ExecuteReader("usp_RouteSetDecision_SelectSummary", commandType: CommandType.StoredProcedure))
                {
                    while (reader.Read())
                    {
                        var decision = new RouteSetDecision();
                        decision.DecisionId = Int32.Parse(reader["RouteSetDecisionId"].ToString(), CultureInfo.InvariantCulture);
                        decision.RouteSet.RouteSetName = reader["RouteSet_Web"].ToString();
                        decision.Action = reader["Action_Web"].ToString();
                        decision.DecisionTime = DateTime.Parse(reader["DecisionTime"].ToString(), CultureInfo.CurrentCulture);
                        if (reader["ActionTime"] != DBNull.Value) decision.ActionTime = DateTime.Parse(reader["ActionTime"].ToString(), CultureInfo.CurrentCulture);
                        decisions.Add(decision);
                    }
                }
            }

            return decisions;
        }

        /// <summary>
        /// Reads a page of decisions about specific route sets in date order
        /// </summary>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <returns></returns>
        public static IList<GrittingDecision> ReadRouteSetDecisions(int pageSize, int pageNumber)
        {
            var ignore = -1;
            return ReadRouteSetDecisions(pageSize, pageNumber, out ignore);
        }

        /// <summary>
        /// Reads a page of decisions about specific route sets in date order
        /// </summary>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="totalDecisions">The total decisions.</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "2#")]
        public static IList<GrittingDecision> ReadRouteSetDecisions(int pageSize, int pageNumber, out int totalDecisions)
        {
            var decisions = new List<GrittingDecision>();

            var parameters = new DynamicParameters();
            if (pageSize == -1)
            {
                parameters.Add("@pageSize", null);
            }
            else
            {
                parameters.Add("@pageSize", pageSize);
            }
            if (pageNumber == -1)
            {
                parameters.Add("@pageNumber", null);
            }
            else
            {
                parameters.Add("@pageNumber", pageNumber);
            }
            parameters.Add("@totalDecisions", null, DbType.Int32, ParameterDirection.Output);

            // Read decisions. Important to include CommandType.StoredProcedure otherwise output parameter always null
            using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GrittingDecisionReader"].ConnectionString))
            {
                using (var reader = conn.ExecuteReader("usp_RouteSetDecision_Select", parameters, commandType: CommandType.StoredProcedure))
                {
                    while (reader.Read())
                    {
                        var decision = new RouteSetDecision();
                        decision.DecisionId = Int32.Parse(reader["RouteSetDecisionId"].ToString(), CultureInfo.InvariantCulture);
                        decision.RouteSet.RouteSetName = reader["RouteSet_Web"].ToString();
                        decision.OriginalAction = reader["Action_Original"].ToString();
                        decision.Action = reader["Action_Web"].ToString();
                        decision.DecisionTime = DateTime.Parse(reader["DecisionTime"].ToString(), CultureInfo.CurrentCulture);
                        if (reader["ActionTime"] != DBNull.Value) decision.ActionTime = DateTime.Parse(reader["ActionTime"].ToString(), CultureInfo.CurrentCulture);
                        decisions.Add(decision);
                    }
                }
            }

            // Wait until reader closed before getting access to output parameter
            totalDecisions =parameters.Get<int>("@totalDecisions");
            return decisions;
        }

        #endregion // Read processed decisions from database
    }
}
