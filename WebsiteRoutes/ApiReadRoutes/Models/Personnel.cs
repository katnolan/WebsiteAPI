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
        public int studioid { get; set; }
        public int personnelType { get; set; }          // Job Title

        public Personnel()
        {

        }


    }
}
