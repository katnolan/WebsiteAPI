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

        public ScheduleService(int clubid, ClassesFilters classFilters)
        {
            string query = BuildQuery(clubid, classFilters);
                        

            var bqq = new BigQueryQuery();
            var client = bqq.CreateClient();
            var job = bqq.CreateQueryJob(client, query);
            Results.Add(bqq.GetBigQueryResults(client, job));
        }

        public static string BuildQuery(int clubid, ClassesFilters cf)
        {
            string query = @"SELECT Classes.ClassId classid,
                                    Classes.ClubId clubid,
                                    ClassSchedules.ClassName classname,
                                    ClassSchedules.Description shortDescription,
                                    MovementTypes.MovementTypeId activityTypeId,
                                    Classes.Employeeid personnelid,
                                    CONCAT(Employees.FirstName, ' ', Employees.LastName) personnelName,
                                    DATETIME(Classes.Date, Classes.TimeFrom) startDateTime,
                                    DATETIME(Classes.Date, Classes.TimeTo) endDateTime,
                                    ClassScheduleTypes.Description as activityCode,
                                    Resources.StudioId studioid,
                                    Studios.StudioName studioName,
                                    ClassStatus.Status,
                                    CASE WHEN ClassSchedules.ClassScheduleTypeID = 1 THEN ClassSchedules.DateFrom
                                        ELSE NULL
                                    END sesssionBeginDate,
                                    CASE WHEN  ClassSchedules.ClassScheduleTypeID = 1 THEN ClassSchedules.DateTo
                                        ELSE NULL
                                    END sessionEndDate,
                                    CASE WHEN ClassSchedules.MemberOnlyFlag = true THEN 'Member Only'
                                        ELSE 'Non-Member Allowed'
                                    END memberStatus,
                                    Classes.Amount price,
                                    Classes.AttendanceCount booked,
                                    Classes.Capacity capacity,
                                    Intensity.Intensity intensity
                            FROM Data_Layer.Classes
                            INNER JOIN Data_Layer.ClassSchedules on Classes.ClassScheduleId = ClassSchedules.ClassScheduleId
                            LEFT JOIN Data_Layer.ClassScheduleTypes on ClassScheduleTypes.ClassScheduleTypeId = ClassSchedules.ClassScheduleTypeId
                            LEFT JOIN Data_Layer.ClassStatus on ClassStatus.ClassStatusId = Classes.ClassStatusId
                            LEFT JOIN Data_Layer.MovementTypes on MovementTypes.MovementTypeId = Classes.MovementTypeId
                            LEFT JOIN Data_Layer.Employees on Employees.EmployeeId = Classes.EmployeeId
                            LEFT JOIN Data_Layer.Resources on Resources.ResourceId = Classes.ResourceId
                            LEFT JOIN Data_Layer.Studios on Studios.StudioId = Resources.StudioId
                            LEFT JOIN Data_Layer.Intensity on Intensity.IntensityId = Classes.IntensityId
                            WHERE DATETIME(Classes.Date, Classes.TimeFrom) >= CURRENT_DATETIME()
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
                string l = LimitCheck(cf);
                string o = OffsetCheck(cf);

                return query = query + d + s + c + p + at + st + k + l + o;
            }

        }

    

        public static string DateCheck(ClassesFilters cf)
        {
            DateTime df = Convert.ToDateTime(cf.dateFrom);
            DateTime dt = Convert.ToDateTime(cf.dateTo);
                       
            string queryDates = " and Classes.Date >= " + df.ToString("yyyy-MM-dd") + " and Classes.Date < " + dt.ToString("yyyy-MM-dd");

            if (df == null)
            {
                return "";
            }
            else
            {
                return queryDates;
            }
        }

        public static string StudioCheck(ClassesFilters cf)
        {

            string queryStudio = " and Classes.StudioId in (" + string.Join(",", cf.studioid.ToString()) + ")";

            if (cf.studioid == null)
            {
                return "";
            }
            else
            {
                return queryStudio;
            }
        }

        public static string ClassCheck(ClassesFilters cf)
        {
            string queryClasses = " and Classes.ClassId in (" + string.Join(",", cf.classid.ToString()) + ")";


            if (cf.classid == null)
            {
                return "";
            }
            else
            {
                return queryClasses;
            }
        }

        public static string PersonnelCheck(ClassesFilters cf)
        {
            string queryPersonnel = " and Classes.EmployeeId in (" + string.Join(",", cf.personnelid.ToString()) + ")";


            if (cf.personnelid == null)
            {
                return "";
            }
            else
            {
                return queryPersonnel;
            }
        }

        public static string ActivityTypeCheck(ClassesFilters cf)
        {
            string queryActivityType = " and MovementTypes.MovementTypeId = " + cf.activityType.ToString();


            if (cf.activityType == null)
            {
                return "";
            }
            else
            {
                return queryActivityType;
            }
        }

        public static string StatusCheck(ClassesFilters cf)
        {
            string queryStatus = " and ClassStatus.Status = " + cf.status;


            if (cf.status == null)
            {
                return "";
            }
            else
            {
                return queryStatus;
            }
        }

        public static string KeywordCheck(ClassesFilters cf)
        {
            string queryKeyword = " and e.Description LIKE '%" + cf.keyword.ToString() + "%";

            if (cf.keyword == null)
            {
                return "";
            }
            else
            {
                return queryKeyword;
            }


        }

        public static string LimitCheck(ClassesFilters cf)
        {
            string queryLimit = " LIMIT " + cf.limit.ToString();


            if (cf.limit == null)
            {
                return "";
            }
            else
            {
                return queryLimit;
            }
        }

        public static string OffsetCheck(ClassesFilters cf)
        {

            string queryOffset = " OFFSET " + cf.offset.ToString();

            if (cf.offset == null)
            {
                return "";
            }
            else
            {
                return queryOffset;
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
                    startDateTime = (DateTime)row["startDateTime"],
                    endDateTime = (DateTime)row["endDateTime"],
                    activityCode = row["activityCode"].ToString(),
                    studioid = Convert.ToInt32(row["studioid"]),
                    studioName = row["studioName"].ToString(),
                    status = row["Status"].ToString(),
                    sessionBeginDate = (DateTime)row["sessionBeginDate"],
                    sessionEndDate = (DateTime)row["sessionEndDate"],
                    memberStatus = row["memberStatus"].ToString(),
                    isPrice = Convert.ToDouble(row["price"]),
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
