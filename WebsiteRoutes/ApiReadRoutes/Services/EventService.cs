using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Cloud.BigQuery.V2;
using ApiReadRoutes.Utils;
using ApiReadRoutes.Models;

namespace ApiReadRoutes.Services
{
    public class EventService
    {
        public readonly List<BigQueryResults> Results = new List<BigQueryResults>();

        public EventService(int clubid, int? studioid = null, int? month = null, string keyword = null)
        {
            string query = @"SELECT EventId eventid, 
                                    e.ClubId clubid,
                                    e.Description description,
                                    Date date,
                                    TimeFrom time,
                                    s.StudioId studioid,
                                    EmployeeId personnelid,
                                    NonMemberFlag nonmember,
                                    CanBook canbook,
                                    CAST(NonMemberAmount as FLOAT64) nonmemberamount,
                                    CAST(MemberAmount as FLOAT64) memberamount,
                                    Attendance attendance
                             FROM Data_Layer.Events e 
                             INNER JOIN Data_Layer.Resources r 
                                ON e.ResourceId = r.ResourceId
                             INNER JOIN Data_Layer.Studios s
                                ON r.StudioId = s.StudioId
                             WHERE DATETIME (e.Date, TimeFrom) >= CURRENT_DATETIME()
                               and e.ClubId = " + clubid.ToString();

            if (studioid != null && month != null && keyword != null)
            {
                query = query + " and s.StudioId = " + studioid.ToString() + " and EXTRACT( MONTH, DATETIME(e.Date) ) = " + month.ToString() + " and e.Description LIKE '%" + keyword + "%";
            }
            else if (studioid != null)
            {
                query = query + " and s.StudioId = " + studioid.ToString();
            }
            else if (month != null)
            {
                query = query + " and EXTRACT( MONTH, DATETIME(e.Date) ) = " + month.ToString();
            }
            else if (keyword != null )
            {
                query = query + " and e.Description LIKE '%" + keyword + "%";
            }



            var bqq = new BigQueryQuery();
            var client = bqq.CreateClient();
            var job = bqq.CreateQueryJob(client, query);
            Results.Add(bqq.GetBigQueryResults(client, job));
        }

        public List<Event> GetEvents()
        {
            List<Event> events = new List<Event>();

            BigQueryResults result = Results[0];

            foreach(BigQueryRow row in result)
            {


                Event e = new Event();

                e.eventid = Convert.ToInt32(row["eventid"]);
                e.description = row["description"].ToString();
                e.clubid = Convert.ToInt32(row["clubid"]);
                e.studioid = Convert.ToInt32(row["studioid"]);
                e.personnelid = Convert.ToInt32(row["personnelid"]);
                e.date = row["date"].ToString();
                e.time = row["time"].ToString();
                e.nonmember = Convert.ToBoolean(row["nonmember"]);
                e.canbook = Convert.ToBoolean(row["canbook"]);
                e.memberamount = Convert.ToDouble(row["memberamount"]);
                e.nonmemberamount = Convert.ToDouble(row["nonmemberamount"]);
                e.attendance = Convert.ToInt32(row["attendance"]);

                events.Add(e);
            }

            return events;
        }
    }
}
