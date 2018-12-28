using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiReadRoutes.Models
{
    public class Club
    {
        public int clubid { get; set; }
        public string clubname { get; set; }
        public string location { get; set; }
        public bool isActive { get; set; }


        public Club()
        {

        }

        public Club(int id, string name, string city, bool act)
        {
            clubid = id;
            clubname = name;
            location = city;
            isActive = act;

        }
    }
}
