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

        public EventService(int clubid, EventsFilters eventsFilters, int? eventid = null)
        {
            string query = BuildQuery(clubid, eventsFilters, eventid);


            var bqq = new BigQueryQuery();
            var client = bqq.CreateClient();
            var job = bqq.CreateQueryJob(client, query);
            Results.Add(bqq.GetBigQueryResults(client, job));
        }

        public static string BuildQuery(int clubid, EventsFilters ef, int? eventid = null)
        {
            string query = @"SELECT DISTINCT
                                    e.EventId eventid, 
                                    IFNULL(cc.CategoryName, '') name,
                                    IFNULL(cc.Description, '') description,
                                    DATETIME(e.Date, e.TimeFrom) startdate,
                                    DATETIME(e.Date, e.TimeTo) enddate,
                                    CAST(MemberAmount as FLOAT64) memberamount,
                                    CAST(NonMemberAmount as FLOAT64) nonmemberamount,
                                    e.CanBook canbook,                                  
                                    e.ClubId clubid,
                                    cl.ClubName location,
                                    e.Attendance attendance,
                                    e.Capacity capacity,
                                    IFNULL(em.EmployeeId,0) personnelid,
                                    cs.CSIScheduleGUID scheduleGUID,
                                    cc.MovementTypeId  activityTypeId,
                                    cc.FamilyFlag familyFlag,
                                    e.resourceId,
                                    c.conceptId
                             FROM Data_Layer_Test.Events e 
                             Left JOIN Data_Layer_Test.ClassSchedules cs
                                ON e.ClassScheduleId = cs.ClassScheduleId
                             left JOIN Data_Layer_Test.ClassCategories cc
                                ON cc.ClassCategoryId = cs.ClassCategoryId
                             LEFT JOIN Data_Layer_Test.ClassTypes ct
                                ON ct.ClassTypeId = cc.ClassTypeId
                             LEFT JOIN Data_Layer_Test.Concepts c
                                ON c.ConceptId = ct.ConceptId
                             LEFT JOIN Data_Layer_Test.Resources r 
                                ON e.ResourceId = r.ResourceId
                             LEFT JOIN Data_Layer_Test.Employees em
                                ON em.CSIEmployeeId = e.EmployeeId
                             LEFT JOIN Data_Layer_Test.Clubs cl
                                ON e.ClubId = cl.ClubId
                             WHERE --DATETIME (e.Date, TimeFrom) >= CURRENT_DATETIME()
                               --and 
                               e.ClubId = " + clubid.ToString();



            if(ef == null)
            {
                if(eventid == null)
                {
                    return query;
                }
                else
                {
                    string e = EventCheck(eventid);
                    return query = query + e;
                }
            }
            else
            {
                string s = StudioCheck(ef);
                string d = DateCheck(ef);
                string k = KeywordCheck(ef);
                string r = ResourceCheck(ef);
                string e = EventCheck(eventid);

                return query = query + s + d + k + r + e;

            }

        }

        public static string EventCheck(int? eventid)
        {
            if (eventid == null)
            {
                return "";
            }
            else
            {

                string e = " and e.EventId = " + eventid.ToString();

                return e;
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

        public static string ResourceCheck(EventsFilters ef)
        {


            if (ef.resourceid == null)
            {
                return "";
            }
            else
            {
                string queryStudio = " and s.StudioId = " + ef.resourceid.ToString();
                return queryStudio;
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
                    startDate = row["startdate"].ToString(),
                    endDate = row["enddate"].ToString(),
                    priceMember = Convert.ToDouble(row["memberamount"]),
                    priceNonMember = Convert.ToDouble(row["nonmemberamount"]),
                    canBook = Convert.ToBoolean(row["canbook"]),
                    clubId = Convert.ToInt32(row["clubid"]),
                    location = row["location"].ToString(),
                    attending = Convert.ToInt32(row["attendance"]),
                    attendingCapacity = Convert.ToInt32(row["capacity"]),
                    personnelId = Convert.ToInt32(row["personnelid"]),
                    scheduleGUID = row["scheduleGUID"].ToString(),
                    activityTypeId = Convert.ToInt32(row["activityTypeId"]),
                    familyFlag = Convert.ToBoolean(row["familyFlag"]),
                    resourceId = Convert.ToInt32(row["resourceId"]),
                    conceptId = Convert.ToInt32(row["conceptId"])
                };


                events.Add(e);
            }

            return events;
        }
    }
}
