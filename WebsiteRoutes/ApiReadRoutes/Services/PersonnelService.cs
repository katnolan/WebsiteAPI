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
                                    Concepts,
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
                                         SELECT DISTINCT COALESCE(ClassTypes.ConceptId,0) as Studios
                                         FROM
                                         Data_Layer_Test.Events
                                         INNER JOIN Data_Layer_Test.Resources
                                         ON Resources.ResourceId = Events.ResourceId
                                         INNER JOIN Data_Layer_Test.ClassSchedules 
                                         ON ClassSchedules.ClassScheduleId = Events.ClassScheduleId
                                         INNER JOIN Data_Layer_Test.ClassCategories
                                         ON ClassCategories.ClassCategoryId = ClassSchedules.ClassCategoryId
                                         INNER JOIN Data_Layer_Test.ClassTypes
                                         ON ClassTypes.ClassTypeId = ClassCategories.ClassTypeId
                                         WHERE Events.Date = CURRENT_DATE()
                                           and Events.EmployeeId = Employees.CSIEmployeeId),
                                        ARRAY(
                                         SELECT DISTINCT COALESCE(ClassTypes.ConceptId,0) as Studios
                                         FROM
                                         Data_Layer_Test.Classes
                                         INNER JOIN Data_Layer_Test.Resources
                                         ON Resources.ResourceId = Classes.ResourceId
                                         INNER JOIN Data_Layer_Test.ClassSchedules 
                                         ON ClassSchedules.ClassScheduleId = Classes.ClassScheduleId
                                         INNER JOIN Data_Layer_Test.ClassCategories
                                         ON ClassCategories.ClassCategoryId = ClassSchedules.ClassCategoryId
                                         INNER JOIN Data_Layer_Test.ClassTypes
                                         ON ClassTypes.ClassTypeId = ClassCategories.ClassTypeId
                                         WHERE Classes.Date = CURRENT_DATE()
                                           and Classes.EmployeeId = Employees.CSIEmployeeId)
                                        ) Concepts,
                                        Employees.JobTitleId jobtitleid,
                                        CASE WHEN Employees.ClubID = 30 THEN RTRIM(JobTitles.FrenchName)
                                             ELSE RTRIM(JobTitles.EnglishName)
                                        END jobtitle,
                                        JobTitles.EnglishName             
                                      FROM
                                      Data_Layer_Test.Employees
                                        INNER JOIN Data_Layer_Test.JobTitles
                                        ON JobTitles.JobTitleId = Employees.JobTitleId
                        ) a
                        WHERE a.ClubId =  " + clubid.ToString();

            if(pf == null)
            {
                return query;
            }
            else
            {
                string c = ConceptCheck(pf);
                string p = PersonnelCheck(pf);
                string t = TypeCheck(pf);

                return query + c + p + t;
            }
        }


        public static string ConceptCheck(PersonnelFilters pf)
        {
            string queryConcept = " and " + pf.conceptid.ToString() + " IN UNNEST(a.Concepts)";

            if(pf.conceptid == null)
            {
                return "";
            }
            else
            {
                return queryConcept;
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
                    personnelId = Convert.ToInt32(row["employeeid"]),
                    name = row["employeename"].ToString(),
                    clubId = Convert.ToInt32(row["clubid"]),
                    conceptId = (long[])row["Concepts"],
                    personnelTypeId = Convert.ToInt32(row["jobtitleid"]),
                    personnelType = row["jobtitle"].ToString()
                };

                employeeList.Add(employee);
            }

            return employeeList;
        }
    }
}
