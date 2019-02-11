using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ApiReadRoutes.Models
{
    public class ClassesFilters
    {
        public string status { get; set; }
        public DateTime? dateFrom { get; set; }
        public DateTime? dateTo { get; set; }
        public string studioid { get; set; }
        public string classid { get; set; }
        public string personnelid { get; set; }
        public int? activityType { get; set; }
        public string keyword { get; set; }
        public int? limit { get; set; }
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
