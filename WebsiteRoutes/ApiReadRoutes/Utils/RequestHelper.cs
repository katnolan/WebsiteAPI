using Microsoft.IdentityModel;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using System.Configuration;
using System.Net.Http;
using System.Text;
using System.Web;
using ApiReadRoutes.Models;


namespace ApiReadRoutes.Utils
{
    public class RequestHelper
    {
        private readonly EventsFilters _eventsFilters = new EventsFilters
        {
            studioid = null,
            dateFrom = null,
            dateTo = null,
            keyword = null
        };

        public static Dictionary<string, string> ParseUri(HttpRequest request)
        {
            NameValueCollection nvc = HttpUtility.ParseQueryString(request.QueryString.ToUriComponent());


            Dictionary<string, string> items = new Dictionary<string, string>();

            foreach (String s in nvc.AllKeys)
            {
                items.Add(s, nvc[s]);
            }

            return items;

        }

        public static EventsFilters GetEventFilters(HttpRequest request)
        {
            EventsFilters _eventsFilters = new EventsFilters
            {
                studioid = null,
                dateFrom = null,
                dateTo = null,
                keyword = null
            };

            Dictionary<string, string> valuePairs = ParseUri(request);
                
            if (valuePairs.Count > 0)
            {
                if(valuePairs.TryGetValue("studioId", out string s)) { _eventsFilters.studioid = Convert.ToInt32(valuePairs["studioId"]); } 
                if(valuePairs.TryGetValue("dateFrom", out string f)) { _eventsFilters.dateFrom = valuePairs["dateFrom"]; }
                if (valuePairs.TryGetValue("dateTo", out string t)) { _eventsFilters.dateTo = valuePairs["dateTo"]; }
                if(valuePairs.TryGetValue("keyword", out string k)) { _eventsFilters.keyword = valuePairs["keyword"]; }

                return _eventsFilters;

            }
            else
            {
                return null;
            }

        }

        public static ClassesFilters GetClassesFilters(HttpRequest request)
        {
            ClassesFilters _classFilters = new ClassesFilters
            {
                dateFrom = null,
                dateTo = null,
                studioid = null,
                classid = null,
                personnelid = null,
                activityType = null,
                keyword = null,
                limit = null,
                offset = null
            };

            Dictionary<string, string> valuePairs = ParseUri(request);

            if (valuePairs.Count > 0)
            {
                if (valuePairs.TryGetValue("dateFrom", out string f)) { _classFilters.dateFrom = Convert.ToDateTime(valuePairs["dateFrom"]); }
                if (valuePairs.TryGetValue("dateTo", out string t)) { _classFilters.dateTo = Convert.ToDateTime(valuePairs["dateTo"]); }
                if (valuePairs.TryGetValue("studioId", out string s)) { _classFilters.studioid = string.Join(",",valuePairs["studioId"]); }
                if (valuePairs.TryGetValue("classId", out string c)) { _classFilters.classid = string.Join(",", valuePairs["classId"]); }
                if (valuePairs.TryGetValue("personnelId", out string p)) { _classFilters.personnelid = string.Join(",", valuePairs["personnelId"]); }
                if (valuePairs.TryGetValue("activityType", out string at)) { _classFilters.activityType = Convert.ToInt32(valuePairs["activityType"]); }
                if (valuePairs.TryGetValue("keyword", out string k)) { _classFilters.keyword = valuePairs["keyword"]; }
                if (valuePairs.TryGetValue("limit", out string l)) { _classFilters.limit = Convert.ToInt32(valuePairs["limit"]); }
                if (valuePairs.TryGetValue("offset", out string o)) { _classFilters.offset = Convert.ToInt32(valuePairs["offset"]); }

                return _classFilters;
            }
            else
            {
                return null;
            }

        }


        public static PersonnelFilters GetPersonnelFilters(HttpRequest request)
        {
            PersonnelFilters _personnelFilters = new PersonnelFilters
            {
                studioid = null,
                personnelid = null,
                personneltype = null
            };

            Dictionary<string, string> valuePairs = ParseUri(request);

            if (valuePairs.Count > 0)
            {
                if (valuePairs.TryGetValue("studioid", out string s)) { _personnelFilters.studioid = Convert.ToInt32(valuePairs["studioid"]); }
                if (valuePairs.TryGetValue("personnelid", out string p)) { _personnelFilters.personnelid = Convert.ToInt32(valuePairs["personnelid"]); }
                if (valuePairs.TryGetValue("personneltype", out string f)) { _personnelFilters.personneltype = Convert.ToString(valuePairs["personneltype"]); }

                return _personnelFilters;
            }
            else
            {
                return null;
            }

        }

    }
}
