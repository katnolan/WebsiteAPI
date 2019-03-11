using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiReadRoutes.Models
{
    public class ClassTypes
    {
        public int id { get; set; }
        public string classType { get; set; }
        public int conceptId { get; set; }
        public string className { get; set; }
        public string description { get; set; }

    }
}
