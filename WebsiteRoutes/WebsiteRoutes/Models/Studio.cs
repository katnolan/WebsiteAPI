using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebsiteRoutes.Models
{
    public class Studio
    {
        public int studioid { get; set; }
        public string studioname { get; set; }
        public int clubid { get; set; }
        public bool isActive { get; set; }

        public Studio()
        {

        }

        public Studio(int id, string name, int club, bool ia)
        {
            studioid = id;
            studioname = name;
            clubid = club;
            isActive = ia;
        }

    }
}