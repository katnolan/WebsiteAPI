using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ApiReadRoutes.Models
{
    public class EventsFilters
    {


        [DisplayName("conceptid")]
        public int? conceptid { get; set; }

        [DisplayName("resourceid")]
        public int? resourceid { get; set; }

        [DisplayName("keyword")]
        public string keyword { get; set; }

        [DisplayName("datefrom")]
        public string datefrom { get; set; }

        [DisplayName("dateto")]
        public string dateto { get; set; }



        //public override string ToString()
        //{
        //    var temp = this.GetType()
        //                   .GetProperties()
        //                   .Select(p => $"{p.Name}={HttpUtility.UrlEncode(p.GetValue(this).ToString())}");

        //    return string.Join("&", temp);
        //}

    }
}
