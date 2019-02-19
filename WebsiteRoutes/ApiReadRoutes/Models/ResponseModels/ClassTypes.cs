using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiReadRoutes.Models
{
    public class ClassTypes
    {
        public int id { get; set; }
        public string name { get; set; }
        //public string description { get; set; }
        public int activityTypeId { get; set; }
        public string activityType { get; set; }

    }
}
