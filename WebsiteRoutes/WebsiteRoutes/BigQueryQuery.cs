using WebsiteRoutes.Models;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.BigQuery.V2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebsiteRoutes
{
    public class BigQueryQuery
    {
        private string jsonPath = "C:/Users/knporta/vaulted-charmer-205613-f580883b8ec2.json";
        private string projectId = "vaulted-charmer-205613";

        public BigQueryClient CreateClient()
        {
            BigQueryClient client = BigQueryClient.Create(projectId, GoogleCredential.FromFile(jsonPath));
            return client;
        }

        public BigQueryJob CreateQueryJob(BigQueryClient client, string query)
        {
            BigQueryJob job = client.CreateQueryJob(
                sql: query,
                parameters: null,
                options: new QueryOptions { UseQueryCache = false }
            );
            return job;
        }

        public void WriteResults(
            string query
        )
        {
            BigQueryClient client = CreateClient();
            BigQueryJob job = CreateQueryJob(client, query);
            job.PollUntilCompleted();

            foreach (BigQueryRow row in client.GetQueryResults(job.Reference))
            {
                Console.WriteLine($"{row["ClubId"]},{row["ClubName"]}");
            }
        }


        public BigQueryResults GetBigQueryResults(
            BigQueryClient client,
            BigQueryJob job
        )
        {

            job.PollUntilCompleted();

            BigQueryResults results = client.GetQueryResults(job.Reference);

            return results;

        }


    }
}