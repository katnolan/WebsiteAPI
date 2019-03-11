using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Cloud.BigQuery.V2;
using ApiReadRoutes.Models;
using ApiReadRoutes.Utils;
using ApiReadRoutes.Models.ResponseModels;

namespace ApiReadRoutes.Services
{
    public class ResourceService
    {
        public readonly List<BigQueryResults> Results = new List<BigQueryResults>();

        public ResourceService(int clubid, int? resourceid = null)
        {
            string query = @"SELECT ResourceId,
                                    ResourceName,
                                    ClubId,
                                    true as isActive
                             FROM Data_Layer_Test.Resources 
                             WHERE ClubId =" + clubid.ToString();

            if(resourceid != null)
            {
                query = query + " and ResourceId = " + resourceid.ToString();
            }

            var bqq = new BigQueryQuery();
            var client = bqq.CreateClient();
            var job = bqq.CreateQueryJob(client, query);
            Results.Add(bqq.GetBigQueryResults(client, job));
        }

        public List<Resource> GetResources()
        {
            List<Resource> resource = new List<Resource>();

            BigQueryResults result = Results[0];

            foreach (BigQueryRow row in result)
            {
                Resource res = new Resource
                {
                    resourceId = Convert.ToInt32(row["ResourceId"]),
                    resourceName = row["ResourceName"].ToString(),
                    clubId = Convert.ToInt32(row["ClubId"]),
                    isActive = Convert.ToBoolean(row["isActive"]),
                };

                resource.Add(res);
            }

            return resource;
        }
    }
}
