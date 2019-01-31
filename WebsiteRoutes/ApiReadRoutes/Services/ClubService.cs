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
        

        public ClubService()
        {
            var bqq = new BigQueryQuery();
            var client = bqq.CreateClient();
            var job = bqq.CreateQueryJob(client, query);
            Results.Add(bqq.GetBigQueryResults(client, job));
        }


        public List<Club> GetClubs()
        {
            List<Club> clubList = new List<Club>();

            BigQueryResults result = Results[0];
            
            foreach(BigQueryRow row in result)
            {
                Club club = new Club
                {
                    clubid = Convert.ToInt32(row["clubid"]),
                    clubname = row["clubname"].ToString(),
                    csiid = Convert.ToInt32(row["csiid"]),
                    gpid = Convert.ToInt32(row["gpid"]),
                    location = row["location"].ToString(),
                    isActive = Convert.ToBoolean(row["isActive"])
                };


                clubList.Add(club);
            }

            return clubList;
        }
    }
}
