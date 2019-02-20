using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiReadRoutes.Models
{
    public class Schedule
    {
        public int classid { get; set; }
        public int clubid { get; set; }
        public string name { get; set; }
        public string shortDescription { get; set; }
        public int activityTypeId { get; set; }                //movementType
        public int personnelid { get; set; }
        public string personnelName { get; set; }
        public DateTime startDateTime { get; set; }
        public DateTime endDateTime { get; set; }
        public string activityCode { get; set; }                   //ClassScheduleType
        public int studioid { get; set; }
        public string studioName { get; set; }
        public string status { get; set; }                      //ClassStatus
        public DateTime sessionBeginDate { get; set; }          //If class is part of ProgReg
        public DateTime sessionEndDate { get; set; }            //If class is part of ProgReg
        public string memberStatus { get; set; }                //Member, Non-Member
        public decimal isPrice { get; set; }
        public int booked { get; set; }
        public int capacity { get; set; }
        public string intensity { get; set; }                         
    }
}
