using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Google.Cloud.BigQuery.V2;

namespace WebsiteRoutes.Services
{
    public class PersonnelService
    {
        public readonly List<BigQueryResults> Results = new List<BigQueryResults>();
        static private readonly string query = @"SELECT EmployeeId personnelid FROM Data_Layer.Employees";
        //@"SELECT TO_JSON_STRING(t, true) FROM(SELECT ClubId clubid, ClubName clubname, Location location, isActive FROM Data_Layer.Clubs WHERE DivisionId = 2) t";

        public PersonnelService()
        {
            var bqq = new BigQueryQuery();
            var client = bqq.CreateClient();
            var job = bqq.CreateQueryJob(client, query);
            Results.Add(bqq.GetBigQueryResults(client, job));
        }
    }
}