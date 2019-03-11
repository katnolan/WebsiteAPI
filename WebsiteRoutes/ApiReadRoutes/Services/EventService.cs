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

        public EventService()
        {

        }

        public EventService(int clubid, EventsFilters eventsFilters)
        {
            string query = BuildQuery(clubid, eventsFilters);


            var bqq = new BigQueryQuery();
            var client = bqq.CreateClient();
            var job = bqq.CreateQueryJob(client, query);
            Results.Add(bqq.GetBigQueryResults(client, job));
        }

        public static string BuildQuery(int clubid, EventsFilters ef)
        {
            string query = @"SELECT DISTINCT
                                    e.ScheduleId eventid, 
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



            if(ef == null)
            {
                return query;
            }
            else
            {
                string s = StudioCheck(ef);
                string d = DateCheck(ef);
                string k = KeywordCheck(ef);

                return query = query + s + d + k;

            }

        }

        public static string StudioCheck(EventsFilters ef)
        {
            

            if(ef.conceptid == null)
            {
                return "";
            }
            else
            {
                string queryStudio = " and s.StudioId = " + ef.conceptid.ToString();
                return queryStudio;
            }


        }
        public static string KeywordCheck(EventsFilters ef)
        {
            

            if(ef.keyword == null)
            {
                return "";
            }
            else
            {
                string queryKeyword = " and e.Description LIKE '%" + ef.keyword.ToString() + "%";
                return queryKeyword;
            }


        }

        public static string DateCheck(EventsFilters ef)
        {
            DateTime df = Convert.ToDateTime(ef.datefrom);
            DateTime dt = Convert.ToDateTime(ef.dateto);
            

            if (df == null)
            {
                return "";
            }
            else
            {
                string queryDateRange = " and DATETIME(e.Date, TimeFrom) >= '" + df.ToString("yyyy-MM-dd") + "' and DATETIME(e.Date, TimeFrom ) < DATETIME_ADD('" + dt.ToString("yyyy-MM-dd") + "', INTERVAL 1 DAY)";
                return queryDateRange;
            }

        }

        public List<Event> GetEvents()
        {
            List<Event> events = new List<Event>();

            BigQueryResults result = Results[0];

            foreach(BigQueryRow row in result)
            {


                Event e = new Event
                {
                    eventId = Convert.ToInt32(row["eventid"]),
                    name = row["name"].ToString(),
                    description = row["description"].ToString(),
                    conceptId = Convert.ToInt32(row["studioid"]),
                    startDate = row["date"].ToString(),
                    startTime = row["time"].ToString(),
                    priceMember = Convert.ToDouble(row["memberamount"]),
                    priceNonMember = Convert.ToDouble(row["nonmemberamount"]),
                    nonMember = Convert.ToBoolean(row["nonmember"]),
                    canBook = Convert.ToBoolean(row["canbook"]),
                    clubId = Convert.ToInt32(row["clubid"]),
                    location = row["location"].ToString(),
                    attending = Convert.ToInt32(row["attendance"]),
                    attendingCapacity = Convert.ToInt32(row["capacity"]),
                    personnelId = Convert.ToInt32(row["personnelid"])
                };


                events.Add(e);
            }

            return events;
        }
    }
}
