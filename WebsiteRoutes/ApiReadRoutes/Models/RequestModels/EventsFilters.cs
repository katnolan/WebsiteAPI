﻿using System;
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

        [DisplayName("studioId")]
        public int? studioid { get; set; }
        
        [DisplayName("dateFrom")]
        public string dateFrom { get; set; }

        [DisplayName("dateTo")]
        public string dateTo { get; set; }

        [DisplayName("keyword")]
        public string keyword { get; set; }

        //public override string ToString()
        //{
        //    var temp = this.GetType()
        //                   .GetProperties()
        //                   .Select(p => $"{p.Name}={HttpUtility.UrlEncode(p.GetValue(this).ToString())}");

        //    return string.Join("&", temp);
        //}

    }
}