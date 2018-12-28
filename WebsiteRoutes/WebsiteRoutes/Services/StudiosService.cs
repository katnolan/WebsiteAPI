using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Google.Cloud.BigQuery.V2;
using WebsiteRoutes.Models;

namespace WebsiteRoutes.Services
{
    public class StudiosService
    {
        public readonly List<BigQueryResults> Results = new List<BigQueryResults>();
        static private readonly string query = @"SELECT StudioId studioid, StudioName studioname, ClubId clubid, isActive from Data_Layer.Studios";
        //@"SELECT TO_JSON_STRING(t, true) FROM(SELECT ClubId clubid, ClubName clubname, Location location, isActive FROM Data_Layer.Clubs WHERE DivisionId = 2) t";

        public StudiosService()
        {
            var bqq = new BigQueryQuery();
            var client = bqq.CreateClient();
            var job = bqq.CreateQueryJob(client, query);
            Results.Add(bqq.GetBigQueryResults(client, job));


        }


        public Studio[] Get()
        {

            List<Studio> studiosList = new List<Studio>();

            var result = Results[0];

            foreach(BigQueryRow row in result)
            {
                Studio studio = new Studio();

                studio.studioid = Convert.ToInt32(row["studioid"]);
                studio.studioname = row["studioname"].ToString();
                studio.clubid = Convert.ToInt32(row["clubid"]);
                studio.isActive = Convert.ToBoolean(row["isActive"]);

                studiosList.Add(studio);
            }



            Studio[] studios = studiosList.ToArray();
            return studios;

        }
    }
}