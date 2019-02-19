using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Google.Cloud.BigQuery.V2;
using ApiReadRoutes.Utils;
using ApiReadRoutes.Models;

namespace ApiReadRoutes.Services
{
    public class PersonnelService
    {
        public readonly List<BigQueryResults> Results = new List<BigQueryResults>();


        public PersonnelService(int clubid, PersonnelFilters pf)
        {
            string query = BuildQuery(clubid, pf);           
           

            var bqq = new BigQueryQuery();
            var client = bqq.CreateClient();
            var job = bqq.CreateQueryJob(client, query);
            Results.Add(bqq.GetBigQueryResults(client, job));
        }


        public static string BuildQuery(int clubid, PersonnelFilters pf)
        {
            string query = @"SELECT employeeid,
                                    employeename,
                                    clubid,
                                    Studios,
                                    jobtitleid,
                                    jobtitle
                            FROM
                            (
                                SELECT
                                        Employees.EmployeeId employeeid,
                                        CONCAT(FirstName,' ',LastName) employeename,
                                        Employees.ClubId clubid,
                                        ARRAY_CONCAT(
                                        ARRAY(
                                         SELECT DISTINCT COALESCE(Resources.StudioId,0) as Studios
                                         FROM
                                         Data_Layer.Events
                                         INNER JOIN Data_Layer.Resources
                                         ON Resources.ResourceId = Events.ResourceId
                                         WHERE Events.Date = CURRENT_DATE()
                                           and Events.EmployeeId = Employees.EmployeeId),
                                        ARRAY(
                                         SELECT DISTINCT COALESCE(Resources.StudioId,0) as Studios
                                         FROM
                                         Data_Layer.Classes
                                         INNER JOIN Data_Layer.Resources
                                         ON Resources.ResourceId = Classes.ResourceId
                                         WHERE Classes.Date = CURRENT_DATE()
                                           and Classes.EmployeeId = Employees.EmployeeId)
                                        ) Studios,
                                        Employees.JobTitleId jobtitleid,
                                        CASE WHEN Employees.ClubID = 30 THEN RTRIM(JobTitles.FrenchName)
                                             ELSE RTRIM(JobTitles.EnglishName)
                                        END jobtitle,
                                        JobTitles.EnglishName             
                                      FROM
                                      Data_Layer.Employees
                                        INNER JOIN Data_Layer.JobTitles
                                        ON JobTitles.JobTitleId = Employees.JobTitleId
                        ) a
                        WHERE a.ClubId =  " + clubid.ToString();

            if(pf == null)
            {
                return query;
            }
            else
            {
                string s = StudioCheck(pf);
                string p = PersonnelCheck(pf);
                string t = TypeCheck(pf);

                return query + s + p + t;
            }
        }


        public static string StudioCheck(PersonnelFilters pf)
        {
            string queryStudio = " and " + pf.studioid.ToString() + " IN UNNEST(a.Studios)";

            if(pf.studioid == null)
            {
                return "";
            }
            else
            {
                return queryStudio;
            }
 
        }

        public static string PersonnelCheck(PersonnelFilters pf)
        {
            string queryEmployee = " and a.EmployeeId = " + pf.personnelid.ToString();

            if (pf.personnelid == null)
            {
                return "";
            }
            else
            {
                return queryEmployee;
            }

        }

        public static string TypeCheck(PersonnelFilters pf)
        {
            string queryJob = " and RTRIM(a.EnglishName) = " + pf.personneltype;

            if (pf.personneltype == null)
            {
                return "";
            }
            else
            {
                return queryJob;
            }

        }

        public List<Personnel> GetPersonnel()
        {
            List<Personnel> employeeList = new List<Personnel>();

            BigQueryResults result = Results[0];
            
            foreach(BigQueryRow row in result)
            {
                Personnel employee = new Personnel
                {
                    personnelid = Convert.ToInt32(row["employeeid"]),
                    name = row["employeename"].ToString(),
                    clubid = Convert.ToInt32(row["clubid"]),
                    studioid = (long[])row["Studios"],
                    personneltypeid = Convert.ToInt32(row["jobtitleid"]),
                    personneltype = row["jobtitle"].ToString()
                };

                employeeList.Add(employee);
            }

            return employeeList;
        }
    }
}
