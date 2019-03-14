using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Google.Cloud.BigQuery.V2;
using ApiReadRoutes.Utils;
using ApiReadRoutes.Models;
using ApiReadRoutes.Models.ResponseModels;

namespace ApiReadRoutes.Services
{
    public class ClubService
    {
        public readonly List<BigQueryResults> Results = new List<BigQueryResults>();

        

        public ClubService(int? siteid = null, int? clubid = null)
        {
            string query = @"SELECT ClubId clubid, ClubName clubname, CSIId csiid, GPId gpid, Location location, zip, RitaID, MerchantId, ExtraLanguage, TimeZone, isActive FROM Data_Layer_Test.Clubs WHERE DivisionId = 2";

            if(siteid != null && clubid == null)
            {
                query = query + " and CSIId = " + siteid.ToString();
            }
            else if(siteid == null && clubid != null)
            {
                query = query + " and ClubId = " + clubid.ToString();
            }
            else
            {
                query = query + "";
            }

            var bqq = new BigQueryQuery();
            var client = bqq.CreateClient();
            var job = bqq.CreateQueryJob(client, query);
            Results.Add(bqq.GetBigQueryResults(client, job));
        }


        public List<Club> GetClubs()
        {
            List<Club> clubList = new List<Club>();
            List<ClubDetails> clubDetails = new List<ClubDetails>();

            BigQueryResults result = Results[0];
            
            foreach(BigQueryRow row in result)
            {
                Club club = new Club
                {
                    clubid = Convert.ToInt32(row["clubid"]),
                    clubname = row["clubname"].ToString(),
                    location = row["location"].ToString(),
                    isActive = Convert.ToBoolean(row["isActive"]),
                    language = Convert.ToInt32(row["ExtraLanguage"])
                };

                clubList.Add(club);

            }

            return clubList;
        }


        public List<ClubDetails> GetClubDetails()
        {
            
            List<ClubDetails> clubDetails = new List<ClubDetails>();

            BigQueryResults result = Results[0];

            foreach (BigQueryRow row in result)
            {
                
                ClubDetails details = new ClubDetails
                {
                    clubId = Convert.ToInt32(row["clubid"]),
                    clubName = row["clubname"].ToString(),
                    csiId = Convert.ToInt32(row["csiid"]),
                    gpId = Convert.ToInt32(row["gpid"]),
                    location = row["location"].ToString(),
                    zip = row["zip"].ToString(),
                    RitaID = Convert.ToInt64(row["RitaID"]),
                    MerchantID = Convert.ToInt64(row["MerchantId"]),
                    language = Convert.ToInt32(row["ExtraLanguage"]),
                    timeZone = Convert.ToInt32(row["TimeZone"]),
                    isActive = Convert.ToBoolean(row["isActive"]),

                };


                clubDetails.Add(details);
            }

            return clubDetails;
        }
    }
}
