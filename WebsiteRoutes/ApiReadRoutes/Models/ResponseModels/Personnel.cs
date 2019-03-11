using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiReadRoutes.Models
{
    public class Personnel
    {
        public int personnelId { get; set; }             // EmployeeId
        public string name { get; set; }
        public int clubId { get; set; }
        public long[] conceptId { get; set; }
        public int personnelTypeId { get; set; }          // Job Title
        public string personnelType { get; set; }

        public Personnel()
        {

        }

        public Personnel(int empid, string n, int club, long[] concept, int jobid, string job)
        {
            personnelId = empid;
            name = n;
            clubId = club;
            conceptId = concept;
            personnelTypeId = jobid;
            personnelType = job;

        }


    }
}
