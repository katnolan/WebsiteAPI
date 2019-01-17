using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiReadRoutes.Models
{
    public class Event
    {
        public int eventid { get; set; }
        public string description { get; set; }
        public int clubid { get; set; }
        public int studioid { get; set; }
        public int personnelid { get; set; }
        public string date { get; set; }
        public string time { get; set; }
        public bool nonmember { get; set; }
        public bool canbook { get; set; }
        public double nonmemberamount { get; set; }
        public double memberamount { get; set; }
        public int attendance { get; set; }

        public Event()
        {

        }

        public Event(int eid, string desc, int club, int studio, int employeeid, string edate, string etime, bool nm, bool cb, double nmamt, double mamt, int att)
        {
            eventid = eid;
            description = desc;
            clubid = club;
            studioid = studio;
            personnelid = employeeid;
            date = edate;
            time = etime;
            nonmember = nm;
            canbook = cb;
            nonmemberamount = nmamt;
            memberamount = mamt;
            attendance = att;

        }
    }
}
