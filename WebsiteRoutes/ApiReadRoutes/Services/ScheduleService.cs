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
            string query = @"SELECT DISTINCT
       Classes.ClassId classid,
                                    Classes.ClubId clubid,
                                    ClassSchedules.ClassName classname,
                                    ClassSchedules.Description shortDescription,
                                    IFNULL(MovementTypes.MovementTypeId, 0) activityTypeId,
                                    IFNULL(Classes.Employeeid, 0) personnelid,
                                    IFNULL(CONCAT(Employees.FirstName, ' ', Employees.LastName),'') personnelName,
                                    DATETIME(Classes.Date, Classes.TimeFrom) startDateTime,
                                    DATETIME(Classes.Date, Classes.TimeTo) endDateTime,
                                    ClassScheduleTypes.Description as activityCode,
                                    IFNULL(Resources.StudioId, 0) studioid,
                                    IFNULL(Studios.StudioName,'') studioName,
                                    ClassStatus.Status,
                                    CASE WHEN ClassSchedules.ClassScheduleTypeID = 1 THEN ClassSchedules.DateFrom
                                        ELSE DATE('1900-01-01')
                                    END sessionBeginDate,
                                    CASE WHEN  ClassSchedules.ClassScheduleTypeID = 1 THEN ClassSchedules.DateTo
                                        ELSE DATE('1900-01-01')
                                    END sessionEndDate,
                                    CASE WHEN ClassSchedules.MemberOnlyFlag = true THEN 'Member Only'
                                        ELSE 'Non-Member Allowed'

                                    END memberStatus,
                                    Classes.Amount price,
                                    Classes.AttendanceCount booked,
                                    Classes.Capacity capacity,
                                    IFNULL(Intensity.Intensity,'') intensity
                            FROM Data_Layer.Classes
                            INNER JOIN Data_Layer.ClassSchedules on Classes.ClassScheduleId = ClassSchedules.ClassScheduleId
                            LEFT JOIN Data_Layer.ClassCategories on ClassCategories.CategoryId = ClassSchedules.CategoryId
                            LEFT JOIN Data_Layer.ClassScheduleTypes on ClassScheduleTypes.ClassScheduleTypeId = ClassSchedules.ClassScheduleTypeId
                            LEFT JOIN Data_Layer.ClassStatus on ClassStatus.ClassStatusId = Classes.ClassStatusId
                            LEFT JOIN Data_Layer.MovementTypes on MovementTypes.MovementTypeId = Classes.MovementTypeId
                            LEFT JOIN Data_Layer.Employees on Employees.EmployeeId = Classes.EmployeeId
                            LEFT JOIN Data_Layer.Resources on Resources.ResourceId = Classes.ResourceId
                            LEFT JOIN Data_Layer.Studios on Studios.StudioId = Resources.StudioId
                            LEFT JOIN Data_Layer.Intensity on Intensity.IntensityId = Classes.IntensityId
                            WHERE Classes.Date >= CURRENT_DATE()
                              and ClassCategories.MovementTypeId is not null
                              and Classes.ClubId = " + clubid.ToString();



            if (cf == null)
            {
                return query;
            }
            else
            {
                string d = DateCheck(cf);
                string s = StudioCheck(cf);
                string c = ClassCheck(cf);
                string p = PersonnelCheck(cf);
                string at = ActivityTypeCheck(cf);
                string st = StatusCheck(cf);
                string k = KeywordCheck(cf);
                string lo = LimitOffsetCheck(cf);
              

                return query = query + d + s + c + p + at + st + k + lo;
            }

        }

    

        public static string DateCheck(ScheduleFilters cf)
        {
            DateTime df = Convert.ToDateTime(cf.dateFrom);
            DateTime dt = Convert.ToDateTime(cf.dateTo);
                       
            if (cf.dateFrom == null || cf.dateTo == null)
            {
                return "";
            }
            else
            {
                string queryDates = " and Classes.Date >= '" + df.ToString("yyyy-MM-dd") + "' and Classes.Date < DATE_ADD('" + dt.ToString("yyyy-MM-dd") + "',INTERVAL 1 DAY)";

                return queryDates;
            }
        }

        public static string StudioCheck(ScheduleFilters cf)
        {

            if (cf.studioid == null)
            {
                return "";
            }
            else
            {
                string queryStudio = " and Resources.StudioId in (" + string.Join(",", cf.studioid) + ")";

                return queryStudio;
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
            


            if (cf.personnelid == null)
            {
                return "";
            }
            else
            {
                string queryPersonnel = " and Classes.EmployeeId in (" + string.Join(",", cf.personnelid) + ")";

                return queryPersonnel;
            }
        }

        public static string ActivityTypeCheck(ScheduleFilters cf)
        {
            
            if (cf.activityType == null)
            {
                return "";
            }
            else
            {
                string queryActivityType = " and MovementTypes.MovementTypeId = " + cf.activityType;

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
                string queryStatus = " and ClassStatus.Status = " + cf.status;

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
                string queryKeyword = " and LOWER(ClassSchedules.ClassName) LIKE LOWER('%" + cf.keyword + "%')";

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
                if(cf.limit == null && cf.offset != null)
                {
                    return "";
                }
                else
                {
                    string queryLimit = " LIMIT " + cf.limit.ToString() + " OFFSET " + cf.offset.ToString();

                    return queryLimit;
                }
                
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
                    classid = Convert.ToInt32(row["classid"]),
                    clubid = Convert.ToInt32(row["clubid"]),
                    name = row["classname"].ToString(),
                    shortDescription = row["shortDescription"].ToString(),
                    activityTypeId = Convert.ToInt32(row["activityTypeId"]),
                    personnelid = Convert.ToInt32(row["personnelid"]),
                    personnelName = row["personnelName"].ToString(),
                    startDateTime = Convert.ToDateTime(row["startDateTime"]),
                    endDateTime = Convert.ToDateTime(row["endDateTime"]),
                    activityCode = row["activityCode"].ToString(),
                    studioid = Convert.ToInt32(row["studioid"]),
                    studioName = row["studioName"].ToString(),
                    status = row["Status"].ToString(),
                    sessionBeginDate = Convert.ToDateTime(row["sessionBeginDate"]),
                    sessionEndDate = Convert.ToDateTime(row["sessionEndDate"]),
                    memberStatus = row["memberStatus"].ToString(),
                    isPrice = Convert.ToDecimal(row["price"].ToString()),
                    booked = Convert.ToInt32(row["booked"]),
                    capacity = Convert.ToInt32(row["capacity"]),
                    intensity = row["intensity"].ToString()
                };


                classList.Add(classes);
            }

            return classList;
        }
    }
}
