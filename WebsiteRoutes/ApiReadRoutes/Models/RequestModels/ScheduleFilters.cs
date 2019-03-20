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
        
        
        public string datefrom { get; set; }

        public string dateto { get; set; }

        
        public string conceptid { get; set; }

        public string classid { get; set; }

        public string instructorid { get; set; }

        public string activitytype { get; set; }

        public string status { get; set; }

        public string keyword { get; set; }

        public int? limit { get; set; }

        public int? offset { get; set; }

        public int? classtypeid { get; set; }

        public int? language { get; set; }
        //public override string ToString()
        //{
        //    var temp = this.GetType()
        //                   .GetProperties()
        //                   .Select(p => $"{p.Name}={HttpUtility.UrlEncode(p.GetValue(this).ToString())}");

        //    return string.Join("&", temp);
        //}

    }
}
