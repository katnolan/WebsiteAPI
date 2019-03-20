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
        public string startDate { get; set; }
        public string endDate { get; set; }
        public double priceMember { get; set; }
        public double priceNonMember { get; set; }
        public bool canBook { get; set; }
        public int clubId { get; set; }
        public string location { get; set; }
        public int attending { get; set; }
        public int attendingCapacity { get; set; }
        public long[] personnelId { get; set; }
        public string scheduleGUID { get; set; }
        public long[] activityTypeId { get; set; }
        public bool familyFlag { get; set; }
        public long[] resourceId { get; set; }
        public int conceptId { get; set; }
        public string conceptName { get; set; }

        public Event()
        {

        }

        public Event(int eid, string n, string desc, int studio, string sdate, string edate, double nmamt, double mamt, bool nm, bool cb, int club, string loc,  int att, int cap, long[] employeeid, string guid, long[] at, bool fam, long[] res)
        {
            eventId = eid;
            name = n;
            description = desc;
            startDate = sdate;
            endDate = edate;
            priceNonMember = nmamt;
            priceMember = mamt;
            canBook = cb;
            clubId = club;
            location = loc;
            attending = att;
            attendingCapacity = cap;
            personnelId = employeeid;
            scheduleGUID = guid;
            activityTypeId = at;
            familyFlag = fam;
            resourceId = res;
            conceptId = studio;
        }
    }
}
