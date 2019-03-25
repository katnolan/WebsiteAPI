using System;

namespace ApiReadRoutes.Models
{
    public class Schedule
    {
        public int classId { get; set; }
        public int clubId { get; set; }
        public string name { get; set; }
        public string shortDescription { get; set; }
        public long[] personnelId { get; set; }
        public string[] personnelName { get; set; }
        public DateTime startDateTime { get; set; }
        public DateTime endDateTime { get; set; }
        public string activityCode { get; set; }                //ClassScheduleType
        public int activityTypeId { get; set; }                //movementType
        public int conceptId { get; set; }
        public string conceptName { get; set; }
        public int booked { get; set; }
        public DateTime sessionBeginDate { get; set; }          //If class is part of ProgReg
        public DateTime sessionEndDate { get; set; }            //If class is part of ProgReg
        public string memberStatus { get; set; }                //Member, Non-Member
        public bool isPaid { get; set; }
        public int attendingCapacity { get; set; }
        public string scheduleGUID { get; set; }
        public long[] resourceId { get; set; }
        public int classTypeId { get; set; }
        public bool familyFlag { get; set; }                    //Family class flag
        public bool isDropIn { get; set; }                      //Drop-in Class flag
        public string intensity { get; set; }                                          
    }
}
