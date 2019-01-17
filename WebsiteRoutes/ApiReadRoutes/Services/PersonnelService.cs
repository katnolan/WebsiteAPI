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


        public PersonnelService(int? clubid = null, int? studioid = null)
        {
            string query = @"SELECT
                EmployeeId employeeid,
                CONCAT(FirstName,' ',LastName) employeename,
                ClubId clubid,
                0 studioid,
                Employees.JobTitleId jobtitleid,
                CASE WHEN ClubID = 30 THEN FrenchName
                     ELSE EnglishName
                END jobtitle              
              FROM
              Data_Layer.Employees
                INNER JOIN Data_Layer.JobTitles
                ON JobTitles.JobTitleId = Employees.JobTitleId";

            if (clubid != null && studioid != null)
            {
                query = query + " WHERE ClubId=" + clubid.ToString() + " and StudioId=" + studioid.ToString();
            }
            else if (clubid != null)
            { 
                query = query + " WHERE ClubId=" + clubid.ToString();
            }
            else if (studioid != null)
            {
                query = query + " WHERE StudioId=" + studioid.ToString();
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
                employee.studioid = Convert.ToInt32(row["studioid"]);
                employee.personneltypeid = Convert.ToInt32(row["jobtitleid"]);
                employee.personneltype = row["jobtitle"].ToString();

                employeeList.Add(employee);
            }

            return employeeList;
        }
    }
}
