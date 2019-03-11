using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiReadRoutes.Models
{
    public class Event
    {
        public int eventId { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public int conceptId { get; set; }        
        public string startDate { get; set; }
        public string startTime { get; set; }
        public string endTime { get; set; }
        public double priceNonMember { get; set; }
        public double priceMember { get; set; }
        public bool nonMember { get; set; }
        public bool canBook { get; set; }
        public int clubId { get; set; }
        public string location { get; set; }
        public int attending { get; set; }
        public int attendingCapacity { get; set; }
        public int personnelId { get; set; }

        public Event()
        {

        }

        public Event(int eid, string n, string desc, int studio, string edate, string stime, string etime, double nmamt, double mamt, bool nm, bool cb, int club, string loc,  int att, int cap, int employeeid)
        {
            eventId = eid;
            name = n;
            description = desc;
            conceptId = studio;
            startDate = edate;
            startTime = stime;
            endTime = etime;
            priceNonMember = nmamt;
            priceMember = mamt;
            nonMember = nm;
            canBook = cb;
            clubId = club;
            location = loc;
            attending = att;
            attendingCapacity = cap;
            personnelId = employeeid;
        }
    }
}
