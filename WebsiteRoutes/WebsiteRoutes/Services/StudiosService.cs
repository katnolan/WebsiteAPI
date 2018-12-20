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

            int numList = Results.Count();

            for (int i = 0; i < numList; i++)
            {
                var res = Results[i];
                foreach (BigQueryRow row in res)
                {
                    Studio studio = new Studio();
                    int numCols = res.Schema.Fields.Count();
                    for (int j = 0; j < numCols; j++)
                    {
                        Console.WriteLine($"{row.Schema.Fields[j].Name}: {row[j]}");
                        if (row.Schema.Fields[j].Name == "studioid")
                        {
                            studio.studioid = Convert.ToInt32(row[j]);
                        }
                        else if (row.Schema.Fields[j].Name == "studioname")
                        {
                            studio.studioname = row[j].ToString();
                        }
                        else if (row.Schema.Fields[j].Name == "clubid")
                        {
                            studio.clubid = Convert.ToInt32(row[j]);
                        }
                        else if (row.Schema.Fields[j].Name == "isActive")
                        {
                            studio.isActive = Convert.ToBoolean(row[j]);
                        }
                        else
                        {
                            break;
                        }
                    }

                    studiosList.Add(studio);
                }
            }


            Studio[] studios = studiosList.ToArray();
            return studios;

        }
    }
}