using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Cloud.BigQuery.V2;
using ApiReadRoutes.Models;
using ApiReadRoutes.Utils;

namespace ApiReadRoutes.Services
{
    public class StudioService
    {
        public readonly List<BigQueryResults> Results = new List<BigQueryResults>();
        static private readonly string query = @"SELECT StudioId studioid, StudioName studioname, ClubId clubid, isActive from Data_Layer.Studios";
        //@"SELECT TO_JSON_STRING(t, true) FROM(SELECT ClubId clubid, ClubName clubname, Location location, isActive FROM Data_Layer.Clubs WHERE DivisionId = 2) t";

        public StudioService()
        {
            var bqq = new BigQueryQuery();
            var client = bqq.CreateClient();
            var job = bqq.CreateQueryJob(client, query);
            Results.Add(bqq.GetBigQueryResults(client, job));


        }

        public List<Studio> GetStudios()
        {

            List<Studio> studiosList = new List<Studio>();

            BigQueryResults result = Results[0];

           foreach (BigQueryRow row in result)
           {
                Studio studio = new Studio
                {
                    studioid = Convert.ToInt32(row["studioid"]),
                    studioname = row["studioname"].ToString(),
                    clubid = Convert.ToInt32(row["clubid"]),
                    isActive = Convert.ToBoolean(row["isActive"])
                };

                studiosList.Add(studio);
            }

            return studiosList;

        }
    }
}
