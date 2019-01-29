using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Cloud.BigQuery.V2;


namespace TestBigQuery
{
    public class PersonnelService
    {
        public readonly List<BigQueryResults> Results = new List<BigQueryResults>();


        public PersonnelService(int clubid, int? studioid = null, int? personnelid = null, string personneltype = null)
        {
            string query = @"SELECT
                Employees.EmployeeId employeeid,
                CONCAT(FirstName,' ',LastName) employeename,
                Employees.ClubId clubid,
                ARRAY_CONCAT(
                ARRAY(
                 SELECT DISTINCT Resources.StudioId as Studios
                 FROM
                 Data_Layer.Events
                 INNER JOIN Data_Layer.Resources
                 ON Resources.ResourceId = Events.ResourceId
                 WHERE Events.Date = CURRENT_DATE()
                   and Events.EmployeeId = Employees.EmployeeId),
                ARRAY(
                 SELECT DISTINCT Resources.StudioId as Studios
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
                END jobtitle              
              FROM
              Data_Layer.Employees
                INNER JOIN Data_Layer.JobTitles
                ON JobTitles.JobTitleId = Employees.JobTitleId
              WHERE Employees.ClubId = " + clubid.ToString();

            string queryStudio = " and " + studioid.ToString() + " IN UNNEST(a.Studios)";
            string queryEmployee = " and Employees.EmployeeId = " + personnelid.ToString();
            string queryJob = " and RTRIM(JobTitles.EnglishName) = " + personneltype;


            if (studioid != null && personnelid != null && personneltype != null)
            {
                query = query + queryStudio + queryEmployee + queryJob;
            }
            else if (studioid != null && personnelid != null)
            {
                query = query + queryStudio + queryEmployee;
            }
            else if (studioid != null && personneltype != null)
            {
                query = query + queryStudio + queryJob;
            }
            else if (personneltype != null && personnelid != null)
            {
                query = query + queryEmployee + queryJob;
            }
            else if(studioid != null)
            {
                query = query + queryStudio;
            }
            else if(personnelid != null)
            {
                query = query + queryEmployee;
            }
            else if(personneltype != null)
            {
                query = query + queryJob;
            }
            
            
           

            var bqq = new BigQueryQuery();
            var client = bqq.CreateClient();
            var job = bqq.CreateQueryJob(client, query);
            Results.Add(bqq.GetBigQueryResults(client, job));
        }


        public List<Personnel> GetPersonnel()
        {
            List<Personnel> employeeList = new List<Personnel>();

            BigQueryResults result = Results[0];
            
            foreach(BigQueryRow row in result)
            {
                Personnel employee = new Personnel();

               
                employee.personnelid = Convert.ToInt32(row["employeeid"]);
                employee.name = row["employeename"].ToString();
                employee.clubid = Convert.ToInt32(row["clubid"]);
                employee.studioid = (long[])row["Studios"];
                employee.personneltypeid = Convert.ToInt32(row["jobtitleid"]);
                employee.personneltype = row["jobtitle"].ToString();

                employeeList.Add(employee);
            }

            return employeeList;
        }
    }
}
