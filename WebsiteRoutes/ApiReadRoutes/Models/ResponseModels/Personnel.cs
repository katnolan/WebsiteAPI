using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiReadRoutes.Models
{
    public class Personnel
    {
        public int personnelid { get; set; }             // EmployeeId
        public string name { get; set; }
        public int clubid { get; set; }
        public long[] studioid { get; set; }
        public int personneltypeid { get; set; }          // Job Title
        public string personneltype { get; set; }

        public Personnel()
        {

        }

        public Personnel(int empid, string n, int club, long[] studio, int jobid, string job)
        {
            personnelid = empid;
            name = n;
            clubid = club;
            studioid = studio;
            personneltypeid = jobid;
            personneltype = job;

        }


    }
}
