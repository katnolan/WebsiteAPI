using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Google.Cloud.BigQuery.V2;
using ApiReadRoutes.Utils;
using ApiReadRoutes.Models;

namespace ApiReadRoutes.Services
{
    public class ClubService
    {
        public readonly List<BigQueryResults> Results = new List<BigQueryResults>();
        static private readonly string query = @"SELECT ClubId clubid, ClubName clubname, CSIId csiid, GPId gpid, Location location, isActive FROM Data_Layer.Clubs WHERE DivisionId = 2";
        //@"SELECT TO_JSON_STRING(t, true) FROM(SELECT ClubId clubid, ClubName clubname, Location location, isActive FROM Data_Layer.Clubs WHERE DivisionId = 2) t";

        public ClubService()
        {
            var bqq = new BigQueryQuery();
            var client = bqq.CreateClient();
            var job = bqq.CreateQueryJob(client, query);
            Results.Add(bqq.GetBigQueryResults(client, job));


        }


        public Club[] Get()
        {

            List<Club> clubsList = new List<Club>();

            int numList = Results.Count();

            for (int i = 0; i < numList; i++)
            {
                var res = Results[i];
                foreach (BigQueryRow row in res)
                {
                    Club club = new Club();
                    int numCols = res.Schema.Fields.Count();
                    for (int j = 0; j < numCols; j++)
                    {
                        Console.WriteLine($"{row.Schema.Fields[j].Name}: {row[j]}");
                        if (row.Schema.Fields[j].Name == "clubid")
                        {
                            club.clubid = Convert.ToInt32(row[j]);
                        }
                        else if (row.Schema.Fields[j].Name == "clubname")
                        {
                            club.clubname = row[j].ToString();
                        }
                        else if (row.Schema.Fields[j].Name == "location")
                        {
                            club.location = row[j].ToString();
                        }
                        else if (row.Schema.Fields[j].Name == "isActive")
                        {
                            club.isActive = Convert.ToBoolean(row[j]);
                        }
                        else
                        {
                            break;
                        }
                    }

                    clubsList.Add(club);
                }
            }


            Club[] clubs = clubsList.ToArray();
            return clubs;

        }
    }
}
