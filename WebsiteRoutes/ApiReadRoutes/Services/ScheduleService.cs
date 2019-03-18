using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Cloud.BigQuery.V2;
using ApiReadRoutes.Utils;
using ApiReadRoutes.Models;

namespace ApiReadRoutes.Services
{
    public class ScheduleService
    {
        public readonly List<BigQueryResults> Results = new List<BigQueryResults>();

        public ScheduleService(int clubid, ScheduleFilters classFilters)
        {
            string query = BuildQuery(clubid, classFilters);
                        

            var bqq = new BigQueryQuery();
            var client = bqq.CreateClient();
            var job = bqq.CreateQueryJob(client, query);
            Results.Add(bqq.GetBigQueryResults(client, job));
        }

        public static string BuildQuery(int clubid, ScheduleFilters cf)
        {
            string query = @"SELECT
                                    Classes.ClassId classid,
                                    Classes.ClubId clubid,
                                    IFNULL(ClassCategories.CategoryName,'') name,
                                    IFNULL(ClassCategories.Description, '') shortDescription,
                                    IFNULL(MovementTypes.MovementTypeId, 0) activityTypeId,
                                    ARRAY_AGG (Distinct IFNULL(Classes.EmployeeId, 0)) personnelid,
                                    ARRAY_AGG (Distinct IFNULL(CONCAT(Employees.FirstName, ' ', Employees.LastName),'')) personnelName,
                                    IFNULL(DATETIME(Classes.Date, Classes.TimeFrom), '1900-01-01') startDateTime,
                                    IFNULL(DATETIME(Classes.Date, Classes.TimeTo), '1900-01-01') endDateTime,
                                    IFNULL(ClassSchedules.ClassScheduleType,'') activityCode,
                                    IFNULL(Concepts.ConceptId, 0) conceptId,
                                    IFNULL(Concepts.Concept,'') conceptName,
                                    IFNULL(Classes.Attendance,0) booked,
                                    CASE WHEN ClassSchedules.ClassScheduleType = 'Program Registration' THEN ClassSchedules.DateFrom
                                        ELSE DATE('1900-01-01')
                                    END sessionBeginDate,
                                    CASE WHEN  ClassSchedules.ClassScheduleType = 'Program Registration' THEN ClassSchedules.DateTo
                                        ELSE DATE('1900-01-01')
                                    END sessionEndDate,
                                    CASE WHEN ClassCategories.NonMemberFlag = false THEN 'Member Only'
                                        ELSE 'Non-Member Allowed'
                                    END memberStatus,
                                    CASE WHEN ClassSchedules.MemberAmount > 0 then true
                                         WHEN ClassSchedules.NonMemberAmount > 0 then true
                                         ELSE false
                                    END isPaid,
                                    ARRAY_AGG (Distinct IFNULL(Classes.resourceId,0)) resourceId,
                                    IFNULL(Classes.Capacity,0) attendingCapacity,
                                    IFNULL(ClassSchedules.CSIScheduleGUID,'') scheduleGUID,
                                    IFNULL(ClassCategories.ClassTypeId,0) ClassTypeId,
                                    IFNULL(ClassCategories.FamilyFlag, false) FamilyFlag,
                                    IFNULL(ClassCategories.DropInFlag,false) isDropIn,
                                    IFNULL(Classes.Intensity, '') Intensity
                            FROM Data_Layer_Test.Classes
                            INNER JOIN Data_Layer_Test.ClassSchedules on Classes.ClassScheduleId = ClassSchedules.ClassScheduleId 
                            LEFT JOIN Data_Layer_Test.ClassCategories on ClassCategories.ClassCategoryId = ClassSchedules.ClassCategoryId and ClassCategories.ClubId = Classes.ClubId
                            LEFT JOIN Data_Layer_Test.MovementTypes on MovementTypes.MovementTypeId = ClassCategories.MovementTypeId
                            LEFT JOIN Data_Layer_Test.ClassTypes on ClassTypes.ClassTypeId = ClassCategories.ClassTypeId and ClassTypes.CSIServiceId = ClassCategories.ClassCategoryId
                            LEFT JOIN Data_Layer_Test.Concepts on Concepts.ConceptId = ClassTypes.ConceptId and Concepts.ClubId = ClassTypes.ClubId
                            LEFT JOIN Data_Layer_Test.Employees on Employees.CSIEmployeeId = Classes.EmployeeId
                            WHERE Classes.Date >= CURRENT_DATE() and 
                              ClassCategories.MovementTypeId is not null and 
                              Classes.ClubId = " + clubid.ToString();


            string groupQuery = @" GROUP BY ClassId,
                                     ClubId,
                                     CategoryName,
                                     Description,
                                     MovementTypes.MovementTypeId,
                                     Date,
                                     TimeFrom,
                                     TimeTo,
                                     ClassScheduleType,
                                     ConceptId,
                                     Concept,
                                     Attendance,
                                     ClassScheduleType,
                                     ClassSchedules.DateFrom,
                                     ClassSchedules.DateTo,
                                     NonMemberFlag,
                                     MemberAmount,
                                     NonMemberAmount,
                                     Capacity,
                                     CSIScheduleGUID,
                                     ClassTypeId,
                                     FamilyFlag,
                                     DropInFlag,
                                     Intensity
                                     order by classid ";



            if (cf == null)
            {
                return query + groupQuery;
            }
            else
            {
                string d = DateCheck(cf);
                string s = ConceptCheck(cf);
                string c = ClassCheck(cf);
                string p = PersonnelCheck(cf);
                string at = ActivityTypeCheck(cf);
                string st = StatusCheck(cf);
                string k = KeywordCheck(cf);
                string lo = LimitOffsetCheck(cf);
                string ct = ClassTypeCheck(cf);
              

                return query = query + d + s + c + p + at + st + k + ct + groupQuery + lo;
            }

        }

    

        public static string DateCheck(ScheduleFilters cf)
        {
            DateTime df = Convert.ToDateTime(cf.datefrom);
            DateTime dt = Convert.ToDateTime(cf.dateto);
                       
            if (cf.datefrom == null && cf.dateto == null)
            {
                return "";
            }
            else if (cf.datefrom != null && cf.dateto == null )
            {
                dt = Convert.ToDateTime(cf.datefrom);

                string queryDates = " and Classes.Date >= '" + df.ToString("yyyy-MM-dd") + "' and Classes.Date < DATE_ADD('" + dt.ToString("yyyy-MM-dd") + "',INTERVAL 1 DAY)";

                return queryDates;

            }
            else
            {
                string queryDates = " and Classes.Date >= '" + df.ToString("yyyy-MM-dd") + "' and Classes.Date < DATE_ADD('" + dt.ToString("yyyy-MM-dd") + "',INTERVAL 1 DAY)";

                return queryDates;
            }
        }

        public static string ConceptCheck(ScheduleFilters cf)
        {

            if (cf.conceptid == null)
            {
                return "";
            }
            else
            {
                string queryConcept = " and Concepts.ConceptId in (" + string.Join(",", cf.conceptid) + ")";

                return queryConcept;
            }
        }

        public static string ClassCheck(ScheduleFilters cf)
        {
            if (cf.classid == null)
            {
                return "";
            }
            else
            {
                string queryClasses = " and Classes.ClassId in (" + string.Join(",", cf.classid) + ")";

                return queryClasses;
            }
        }

        public static string PersonnelCheck(ScheduleFilters cf)
        {
            


            if (cf.instructorid == null)
            {
                return "";
            }
            else
            {
                string queryPersonnel = " and Classes.EmployeeId in (" + string.Join(",", cf.instructorid) + ")";

                return queryPersonnel;
            }
        }

        public static string ActivityTypeCheck(ScheduleFilters cf)
        {
            
            if (cf.activitytype == null)
            {
                return "";
            }
            else
            {
                string queryActivityType = " and MovementTypes.MovementTypeId = " + cf.activitytype;

                return queryActivityType;
            }
        }

        public static string StatusCheck(ScheduleFilters cf)
        {
            
            if (cf.status == null)
            {
                return "";
            }
            else
            {
                string queryStatus = " and Classes.Status = " + cf.status;

                return queryStatus;
            }
        }

        public static string KeywordCheck(ScheduleFilters cf)
        {
            if (cf.keyword == null)
            {
                return "";
            }
            else
            {
                string queryKeyword = " and LOWER(ClassCategories.CategoryName) LIKE LOWER('%" + cf.keyword.ToString() + "%')";

                return queryKeyword;
            }


        }

        public static string LimitOffsetCheck(ScheduleFilters cf)
        {
            if (cf.limit == null && cf.offset == null)
            {
                return "";
            }
            else
            {
                if (cf.limit == null && cf.offset != null)
                {
                    return "";
                }
                else if(cf.limit != null && cf.offset == null)
                {
                    string queryLimit = " LIMIT " + cf.limit.ToString();

                    return queryLimit;
                }
                
                else
                {
                    string queryLimit = " LIMIT " + cf.limit.ToString() + " OFFSET " + cf.offset.ToString();

                    return queryLimit;
                }
                
            }
        }

        public static string ClassTypeCheck(ScheduleFilters cf)
        {
            if (cf.classtypeid == null)
            {
                return "";
            }
            else
            {
                string classTypeQuery = " and ClassTypes.ClassTypeId = " + cf.classtypeid.ToString();

                return (classTypeQuery);

            }
        }

        public List<Schedule> GetClasses()
        {
            List<Schedule> classList = new List<Schedule>();

            BigQueryResults result = Results[0];

            foreach (BigQueryRow row in result)
            {
                Schedule classes = new Schedule
                {
                    classId = Convert.ToInt32(row["classid"]),
                    clubId = Convert.ToInt32(row["clubid"]),
                    name = row["name"].ToString(),
                    shortDescription = row["shortDescription"].ToString(),
                    personnelId = (long[])(row["personnelid"]),
                    personnelName = (String[])row["personnelName"],
                    startDateTime = Convert.ToDateTime(row["startDateTime"]),
                    endDateTime = Convert.ToDateTime(row["endDateTime"]),
                    activityCode = row["activityCode"].ToString(),
                    activityTypeId = Convert.ToInt32(row["activityTypeId"]),
                    conceptId = Convert.ToInt32(row["conceptId"]),
                    conceptName = row["conceptName"].ToString(),
                    booked = Convert.ToInt32(row["booked"]),
                    sessionBeginDate = Convert.ToDateTime(row["sessionBeginDate"]),
                    sessionEndDate = Convert.ToDateTime(row["sessionEndDate"]),
                    memberStatus = row["memberStatus"].ToString(),
                    isPaid = Convert.ToBoolean(row["isPaid"].ToString()),
                    attendingCapacity = Convert.ToInt32(row["attendingCapacity"]),
                    scheduleGUID = row["scheduleGUID"].ToString(),
                    resourceId = (long[])row["resourceId"],
                    classTypeId = Convert.ToInt32(row["ClassTypeId"]),
                    familyFlag = Convert.ToBoolean(row["FamilyFlag"]),
                    isDropIn = Convert.ToBoolean(row["isDropIn"]),
                    intensity = row["Intensity"].ToString()
                };


                classList.Add(classes);
            }

            return classList;
        }
    }
}
