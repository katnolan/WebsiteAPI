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

        public ScheduleService(int clubid, string activityType, string status, DateTime? startdate = null, DateTime? enddate = null, long[] studioid = null, long[] classid = null, long[] personnelid = null, int? limit = null, int? offset = null)
        {
            string query = @"SELECT Classes.ClassId classid,
                                    Classes.ClubId clubid,
                                    ClassSchedules.ClassName classname,
                                    ClassSchedules.Description shortDescription,
                                    MovementTypes.MovementTypeId activityTypeId,
                                    Classes.Employeeid personnelid,
                                    CONCAT(Employees.FirstName, ' ', Employees.LastName) personnelName,
                                    DATETIME(Classes.Date, Classes.StartTime) startDateTime,
                                    DATETIME(Classes.Date, Classes.EndTime) endDateTime,
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
                            INNER JOIN Data_Layer.ClassScheduleTypes on ClassScheduleTypes.ClassScheduleTypeId = ClassSchedules.ClassScheduleTypeId
                            INNER JOIN Data_Layer.ClassStatus on ClassStatus.ClassStatusId = Classes.ClassStatusId
                            INNER JOIN Data_Layer.MovementTypes on MovementType.MovementTypeId = Classes.MovementTypeId
                            INNER JOIN Data_Layer.Employees on Employees.EmployeeId = Classes.EmployeeId
                            INNER JOIN Data_Layer.Resources on Resources.ResourceId = Classes.ResourceId
                            INNER JOIN Data_Layer.Studios on Studios.StudioId = Resources.StudioId
                            INNER JOIN Data_Layer.Intensity on Intensity.IntensityId = Classes.IntensityId
                            WHERE Classes.ClubId = " + clubid.ToString();

            string queryActivityType = "";

            var bqq = new BigQueryQuery();
            var client = bqq.CreateClient();
            var job = bqq.CreateQueryJob(client, query);
            Results.Add(bqq.GetBigQueryResults(client, job));
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
