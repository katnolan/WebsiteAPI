using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Google.Cloud.BigQuery.V2;
using Google.Apis.Auth.OAuth2;


namespace ApiReadRoutes.Utils
{
    public class BigQueryQuery
    {
        private readonly string jsonpath = "Utils/googleCredentials.json";
        private string projectId = "vaulted-charmer-205613";

        public BigQueryClient CreateClient()
        {
            BigQueryClient client = BigQueryClient.Create(projectId, GoogleCredential.FromFile(jsonpath));
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
