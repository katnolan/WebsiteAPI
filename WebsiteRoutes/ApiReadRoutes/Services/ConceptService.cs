using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Cloud.BigQuery.V2;
using ApiReadRoutes.Models;
using ApiReadRoutes.Utils;

namespace ApiReadRoutes.Services
{
    public class ConceptService
    {
        public readonly List<BigQueryResults> Results = new List<BigQueryResults>();
        //@"SELECT TO_JSON_STRING(t, true) FROM(SELECT ClubId clubid, ClubName clubname, Location location, isActive FROM Data_Layer.Clubs WHERE DivisionId = 2) t";

        public ConceptService()
        {
            string query = @"SELECT ConceptId conceptid, Concept conceptname, ClubId clubid, isActive from Data_Layer_Test.Concepts";


            var bqq = new BigQueryQuery();
            var client = bqq.CreateClient();
            var job = bqq.CreateQueryJob(client, query);
            Results.Add(bqq.GetBigQueryResults(client, job));


        }

        public List<Concept> GetConcepts()
        {

            List<Concept> ConceptsList = new List<Concept>();

            BigQueryResults result = Results[0];

           foreach (BigQueryRow row in result)
           {
                Concept Concept = new Concept
                {
                    conceptid = Convert.ToInt32(row["conceptid"]),
                    conceptname = row["conceptname"].ToString(),
                    clubid = Convert.ToInt32(row["clubid"]),
                    isactive = Convert.ToBoolean(row["isActive"])
                };

                ConceptsList.Add(Concept);
            }

            return ConceptsList;

        }
    }
}
