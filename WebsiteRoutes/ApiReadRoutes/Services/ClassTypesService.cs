using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Cloud.BigQuery.V2;
using ApiReadRoutes.Models;
using ApiReadRoutes.Utils;

namespace ApiReadRoutes.Services
{
    public class ClassTypesService
    {
        public readonly List<BigQueryResults> Results = new List<BigQueryResults>();

        public ClassTypesService()
        {
            string query = @"SELECT cc.CategoryId as id, 
                                    cc.CategoryName as name, 
                                    cc.MovementTypeId as activityTypeId, 
                                    a.MovementType as activityType
                            FROM Data_Layer.ClassCategories cc
                            INNER JOIN Data_Layer.MovementTypes a on a.MovementTypeId = cc.MovementTypeId";

            var bqq = new BigQueryQuery();
            var client = bqq.CreateClient();
            var job = bqq.CreateQueryJob(client, query);
            Results.Add(bqq.GetBigQueryResults(client, job));
        }

        public List<ClassTypes> GetClassTypes()
        {
            List<ClassTypes> classTypes = new List<ClassTypes>();

            BigQueryResults res = Results[0];

            foreach (BigQueryRow row in res)
            {
                ClassTypes types = new ClassTypes
                {
                    id = Convert.ToInt32(row["id"]),
                    name = row["name"].ToString(),
                    activityTypeId = Convert.ToInt32(row["activityTypeId"]),
                    activityType = row["activityType"].ToString()
                };

                classTypes.Add(types);
            }

            return classTypes;
        }
    }
}
