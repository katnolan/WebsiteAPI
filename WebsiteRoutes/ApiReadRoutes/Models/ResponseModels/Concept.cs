using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiReadRoutes.Models
{
    public class Concept
    {
        public int conceptid { get; set; }
        public string conceptname { get; set; }
        public int clubid { get; set; }
        public bool isActive { get; set; }

        public Concept()
        {

        }

        public Concept(int id, string name, int club, bool ia)
        {
            conceptid = id;
            conceptname = name;
            clubid = club;
            isActive = ia;
        }

    }
}
