using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebsiteRoutes.Models
{
    public class Personnel
    {
        public int personnelid { get; set; } //employeeid
        public int clubid { get; set; }
        public int studioid { get; set; }
        public string name { get; set; }
        public string personneltype { get; set; } //job title
        public string studioname { get; set; }
        public int classid { get; set; }
        public string classdescription { get; set; }
        public DateTime classdate { get; set; }
        public TimeSpan classtime { get; set; }

        public Personnel()
        {

        }

        public Personnel(int employeeid, int club, int studio, string employeename, string jobtitle, string name, int classd, string description, DateTime date, TimeSpan time)
        {
            personnelid = employeeid;
            clubid = club;
            studioid = studio;
            name = employeename;
            personneltype = jobtitle;
            studioname = name;
            classid = classd;
            classdescription = description;
            classdate = date;
            classtime = time;
        }
    }
}