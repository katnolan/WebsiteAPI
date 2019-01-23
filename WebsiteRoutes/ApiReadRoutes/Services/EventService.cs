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

        public EventService(int clubid, int? studioid = null, DateTime? dateFrom = null, DateTime? dateTo = null, string keyword = null)
        {

            DateTime df = Convert.ToDateTime(dateFrom);
            DateTime dt = Convert.ToDateTime(dateTo);

            string query = @"SELECT e.ScheduleId eventid, 
                                    IF(e.EventName is null, '', e.EventName) name,
                                    e.Description description,
                                    s.StudioId studioid,
                                    Date date,
                                    TimeFrom time,
                                    CAST(NonMemberAmount as FLOAT64) nonmemberamount,
                                    CAST(MemberAmount as FLOAT64) memberamount,
                                    NonMemberFlag nonmember,
                                    CanBook canbook,
                                    e.ClubId clubid,
                                    c.ClubName location,
                                    Attendance attendance,
                                    Capacity capacity,
                                    EmployeeId personnelid
                             FROM Data_Layer.Events e 
                             INNER JOIN Data_Layer.Resources r 
                                ON e.ResourceId = r.ResourceId
                             INNER JOIN Data_Layer.Studios s
                                ON r.StudioId = s.StudioId
                             INNER JOIN Data_Layer.Clubs c
                                ON e.ClubId = c.ClubId
                             WHERE DATETIME (e.Date, TimeFrom) >= CURRENT_DATETIME()
                               and e.ClubId = " + clubid.ToString();

            string queryStudio = " and s.StudioId = " + studioid.ToString();
            string queryKeyword = " and e.Description LIKE '%" + keyword + "%";
            string queryDateRange = " and DATETIME(e.Date, TimeFrom) >= '" + df.ToString("yyyy-MM-dd") + "' and DATETIME(e.Date, TimeFrom ) < DATETIME_ADD('" + dt.ToString("yyyy-MM-dd") + "', INTERVAL 1 DAY)";

            if (studioid != null && keyword != null && dateFrom != null)
            {
                query = query + queryKeyword + queryDateRange;
            }
            else if (studioid != null && dateFrom != null)
            {
                query = query + queryStudio + queryDateRange;
            }
            else if (studioid != null && keyword != null)
            {
                query = query + queryStudio + queryKeyword;
            }
            else if (dateFrom != null && keyword != null)
            {
                query = query + queryKeyword + queryDateRange;
            }
            else if (studioid != null)
            {
                query = query + queryStudio;
            }
            else if (dateFrom != null)
            {
                query = query + queryDateRange;
            }
            else if (keyword != null )
            {
                query = query + queryKeyword;
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

                e.eventId = Convert.ToInt32(row["eventid"]);
                e.name = row["name"].ToString();
                e.description = row["description"].ToString();
                e.studioId = Convert.ToInt32(row["studioid"]);
                e.startDate = row["date"].ToString();
                e.startTime = row["time"].ToString();
                e.priceMember = Convert.ToDouble(row["memberamount"]);
                e.priceNonMember = Convert.ToDouble(row["nonmemberamount"]);
                e.nonMember = Convert.ToBoolean(row["nonmember"]);
                e.canBook = Convert.ToBoolean(row["canbook"]);
                e.clubId = Convert.ToInt32(row["clubid"]);
                e.location = row["location"].ToString();                
                e.attending = Convert.ToInt32(row["attendance"]);
                e.attendingCapacity = Convert.ToInt32(row["capacity"]);
                e.personnelId = Convert.ToInt32(row["personnelid"]);
                

                events.Add(e);
            }

            return events;
        }
    }
}
