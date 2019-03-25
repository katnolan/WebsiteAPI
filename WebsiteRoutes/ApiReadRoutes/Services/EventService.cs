using System;
using System.Collections.Generic;
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
            string mainquery = "";
            string groupquery = "";
            if (ef.language == 0 || ef.language == 2)
            {
                mainquery = @" select e.eventid, 
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
                                      IFNULL(e.Capacity, 20) capacity,
                                      ARRAY_AGG(DISTINCT IFNULL(em.employeeid, 0)) personnelid,
                                      cs.CSIScheduleGUID scheduleGUID,
                                      ARRAY_AGG(DISTINCT IFNULL(cc.MovementTypeId,0)) activityTypeId,
                                      cc.FamilyFlag familyFlag,
                                      ARRAY_AGG(DISTINCT IFNULL(e.resourceId, 0)) resourceId,
                                      c.conceptId,
                                      IFNULL(c.Concept, '') conceptName
                               FROM Data_Layer_Test.Events e 
                             Left JOIN Data_Layer_Test.ClassSchedules cs
                                ON e.ClassScheduleId = cs.ClassScheduleId
                             left JOIN Data_Layer_Test.ClassCategories cc
                                ON cc.ClassCategoryId = cs.ClassCategoryId
                             LEFT JOIN Data_Layer_Test.ClassTypes ct
                                ON ct.ClassTypeId = cc.ClassTypeId and ct.CSIServiceId = cc.ClassCategoryId
                             LEFT JOIN Data_Layer_Test.Concepts c
                                ON c.ConceptId = ct.ConceptId
                             LEFT JOIN Data_Layer_Test.Resources r 
                                ON e.ResourceId = r.ResourceId
                             LEFT JOIN Data_Layer_Test.Employees em
                                ON em.CSIEmployeeId = e.EmployeeId
                             LEFT JOIN Data_Layer_Test.Clubs cl
                                ON e.ClubId = cl.ClubId
                                where DATETIME(e.Date, e.TimeFrom) >= CURRENT_DATETIME()
                                  and e.Clubid = " + clubid.ToString();

                groupquery = @" group by e.eventid,
                                         cc.CategoryName,
                                         cc.Description,
                                         e.Date,
                                         e.TimeFrom,
                                         e.TimeTo,
                                         cs.MemberAmount,
                                         cs.NonMemberAmount,
                                         e.CanBook,
                                         e.ClubId,
                                         cl.ClubName,
                                         e.Attendance,
                                         e.Capacity,
                                         cs.CSIScheduleGUID,
                                         cc.FamilyFlag,
                                         c.conceptId,
                                         c.Concept
                                order by  e.eventid";
            }
            else
            {
                mainquery = @" select e.eventid, 
                                      IFNULL(cc.FrenchCategoryName, '') name,
                                      IFNULL(cc.FrenchDescription, '') description,
                                      DATETIME(e.Date, e.TimeFrom) startdate,
                                      DATETIME(e.Date, e.TimeTo) enddate,
                                      CAST(MemberAmount as FLOAT64) memberamount,
                                      CAST(NonMemberAmount as FLOAT64) nonmemberamount,
                                      e.CanBook canbook,                                  
                                      e.ClubId clubid,
                                      cl.ClubName location,
                                      e.Attendance attendance,
                                      IFNULL(e.Capacity, 20) capacity,
                                      ARRAY_AGG(DISTINCT IFNULL(em.employeeid, 0)) personnelid,
                                      cs.CSIScheduleGUID scheduleGUID,
                                      ARRAY_AGG(DISTINCT IFNULL(cc.MovementTypeId,0)) activityTypeId,
                                      cc.FamilyFlag familyFlag,
                                      ARRAY_AGG(DISTINCT IFNULL(e.resourceId, 0)) resourceId,
                                      c.conceptId,
                                      IFNULL(c.FrenchConcept, '') conceptName
                               FROM Data_Layer_Test.Events e 
                             Left JOIN Data_Layer_Test.ClassSchedules cs
                                ON e.ClassScheduleId = cs.ClassScheduleId
                             left JOIN Data_Layer_Test.ClassCategories cc
                                ON cc.ClassCategoryId = cs.ClassCategoryId
                             LEFT JOIN Data_Layer_Test.ClassTypes ct
                                ON ct.ClassTypeId = cc.ClassTypeId and ct.CSIServiceId = cc.ClassCategoryId
                             LEFT JOIN Data_Layer_Test.Concepts c
                                ON c.ConceptId = ct.ConceptId
                             LEFT JOIN Data_Layer_Test.Resources r 
                                ON e.ResourceId = r.ResourceId
                             LEFT JOIN Data_Layer_Test.Employees em
                                ON em.CSIEmployeeId = e.EmployeeId
                             LEFT JOIN Data_Layer_Test.Clubs cl
                                ON e.ClubId = cl.ClubId
                                where DATETIME(e.Date, e.DateFrom) >= CURRENT_DATETIME()
                                  and e.Clubid = " + clubid.ToString();

                groupquery = @" group by e.eventid,
                                         cc.FrenchCategoryName,
                                         cc.FrenchDescription,
                                         e.Date,
                                         e.TimeFrom,
                                         e.TimeTo,
                                         cs.MemberAmount,
                                         cs.NonMemberAmount,
                                         e.CanBook,
                                         e.ClubId,
                                         cl.ClubName,
                                         e.Attendance,
                                         e.Capacity,
                                         cs.CSIScheduleGUID,
                                         cc.FamilyFlag,
                                         c.conceptId,
                                         c.FrenchConcept
                                order by  e.eventid";
            }

             


            if(ef == null)
            {
                if(eventid == null)
                {
                    return mainquery + groupquery;
                }
                else
                {
                    string e = EventCheck(eventid);
                    return mainquery + e + groupquery;
                }
            }
            else
            {
                string s = StudioCheck(ef);
                string d = DateCheck(ef);
                string k = KeywordCheck(ef);
                string r = ResourceCheck(ef);
                string e = EventCheck(eventid);

                return mainquery + s + d + k + r + e + groupquery ;

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

                string queryEvent = " and e.EventId = " + eventid.ToString();

                return queryEvent;
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
                string queryStudio = " and c.ConceptId = " + ef.conceptid.ToString();
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
                string queryKeyword = " and cc.CategoryName LIKE '%" + ef.keyword.ToString() + "%'";
                return queryKeyword;
            }


        }

        public static string DateCheck(EventsFilters ef)
        {
            DateTime df = Convert.ToDateTime(ef.datefrom);
            DateTime dt = Convert.ToDateTime(ef.dateto);
            

            if (ef.datefrom == null)
            {
                return "";
            }
            else if (ef.datefrom != null && ef.dateto == null)
            {
                dt = Convert.ToDateTime(ef.datefrom);

                string queryDate = " and DATETIME(e.Date, TimeFrom) >= '" + df.ToString("yyyy-MM-dd") + "' and DATETIME(e.Date, TimeFrom ) <= " + dt.ToString("yyyy-MM-dd");
                return queryDate;
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
                string queryResource = " and r.ResourceId = " + ef.resourceid.ToString();
                return queryResource;
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
                    personnelId = (long[])(row["personnelid"]),
                    scheduleGUID = row["scheduleGUID"].ToString(),
                    activityTypeId = (long[])(row["activityTypeId"]),
                    familyFlag = Convert.ToBoolean(row["familyFlag"]),
                    resourceId = (long[])(row["resourceId"]),
                    conceptId = Convert.ToInt32(row["conceptId"]),
                    conceptName = row["conceptName"].ToString()
                };


                events.Add(e);
            }

            return events;
        }
    }
}
