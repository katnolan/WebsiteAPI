using System;
using System.Collections.Generic;
using Google.Cloud.BigQuery.V2;
using ApiReadRoutes.Models;
using ApiReadRoutes.Utils;

namespace ApiReadRoutes.Services
{
    public class ConceptService
    {
        public readonly List<BigQueryResults> Results = new List<BigQueryResults>();
        //@"SELECT TO_JSON_STRING(t, true) FROM(SELECT ClubId clubid, ClubName clubname, Location location, isActive FROM Data_Layer.Clubs WHERE DivisionId = 2) t";

        public ConceptService(int? clubid = null, int? conceptid = null, int? language = 0)
        {
            string query = "";
            if(language == 0 || language == 2)
            {
                query = @"SELECT ConceptId conceptid, IFNULL(Concept, '') conceptname, ClubId clubid, isActive from Data_Layer_Test.Concepts";

                if(clubid != null)
                {
                    query = query + " WHERE ClubId = " + clubid.ToString();
                }
                else if(clubid != null && conceptid != null)
                {
                    query = query + " WHERE ClubId = " + clubid.ToString() + " and ConceptId = " + conceptid.ToString();
                }
                else if (conceptid != null)
                {
                    query = query + " WHERE ConceptId = " + conceptid.ToString();
                }

            }
            else
            {
                query = @"SELECT ConceptId conceptid, IFNULL(FrenchConcept, '') conceptname, ClubId clubid, isActive from Data_Layer_Test.Concepts";

                if(clubid != null)
                {
                    query = query + " WHERE ClubId = " + clubid.ToString();
                }
                else if (clubid != null && conceptid != null)
                {
                    query = query + " WHERE ClubId = " + clubid.ToString() + " and ConceptId = " + conceptid.ToString();
                }
                else if (conceptid != null)
                {
                    query = query + " WHERE ConceptId = " + conceptid.ToString();
                }

            }
            


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
