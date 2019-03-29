using System;
using System.Collections.Generic;
using Google.Cloud.BigQuery.V2;
using ApiReadRoutes.Utils;
using ApiReadRoutes.Models;

namespace ApiReadRoutes.Services
{
    public class ActivityTypeService
    {
        public readonly List<BigQueryResults> Results = new List<BigQueryResults>();

        public ActivityTypeService(string type = null, int? id = null)
        {
            string query = @"SELECT 
                            MovementTypeId activityTypeId,
                            MovementType activityType
                         FROM
                            Data_Layer_Test.MovementTypes
                         WHERE IsActive = true";

            string queryType = " and MovementType = " + type;
            string queryId = " and MovementTypeId = " + id.ToString();

            if(type != null && id != null)
            {
                query = query + queryType + queryId;
            }
            else if(type != null)
            {
                query = query + queryType;
            }
            else if(id != null)
            {
                query = query + queryId;
            }

            var bqq = new BigQueryQuery();
            var client = bqq.CreateClient();
            var job = bqq.CreateQueryJob(client, query);
            Results.Add(bqq.GetBigQueryResults(client, job));

        }

        public List<ActivityType> GetActivities()
        {
            List<ActivityType> activityTypes = new List<ActivityType>();

            BigQueryResults results = Results[0];

            foreach(BigQueryRow row in results)
            {
                ActivityType type = new ActivityType
                {
                    activityTypeId = Convert.ToInt32(row["activityTypeId"]),
                    activityType = row["activityType"].ToString()
                };

                activityTypes.Add(type);
            }
            return (activityTypes);
        }
       

    }
}
