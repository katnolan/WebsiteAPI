using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ApiReadRoutes.Models
{
    public class ScheduleFilters
    {
        
        [DisplayName("dateFrom")]
        public DateTime? dateFrom { get; set; }

        [DisplayName("dateTo")]
        public DateTime? dateTo { get; set; }

        [DisplayName("studioId")]
        public string studioid { get; set; }

        [DisplayName("classId")]
        public string classid { get; set; }

        [DisplayName("instructorId")]
        public string personnelid { get; set; }

        [DisplayName("activityType")]
        public string activityType { get; set; }

        [DisplayName("status")]
        public string status { get; set; }

        [DisplayName("keyword")]
        public string keyword { get; set; }

        [DisplayName("limit")]
        public int? limit { get; set; }

        [DisplayName("offset")]
        public int? offset { get; set; }

        //public override string ToString()
        //{
        //    var temp = this.GetType()
        //                   .GetProperties()
        //                   .Select(p => $"{p.Name}={HttpUtility.UrlEncode(p.GetValue(this).ToString())}");

        //    return string.Join("&", temp);
        //}

    }
}
