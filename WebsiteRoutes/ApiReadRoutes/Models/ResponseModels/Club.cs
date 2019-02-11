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
        public int csiid { get; set; }
        public int gpid { get; set; }
        public string location { get; set; }
        public bool isActive { get; set; }


        public Club()
        {

        }

        public Club(int id, string name, int cid, int gid, string city, bool act)
        {
            clubid = id;
            clubname = name;
            csiid = cid;
            gpid = gid;
            location = city;
            isActive = act;

        }
    }
}
