﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;
using ApiReadRoutes.Models;


namespace ApiReadRoutes.Utils
{
    public class RequestHelper
    {

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
                conceptid = null,
                resourceid = null,
                keyword = null,
                datefrom = null,
                dateto = null,
                language = 0
            };

            Dictionary<string, string> valuePairs = ParseUri(request);
                
            if (valuePairs.Count > 0)
            {
                if(valuePairs.TryGetValue("conceptid", out string s)) { _eventsFilters.conceptid = Convert.ToInt32(valuePairs["conceptid"]); } 
                if(valuePairs.TryGetValue("resourceid", out string r)) { _eventsFilters.resourceid = Convert.ToInt32(valuePairs["resourceid"]);  }
                if(valuePairs.TryGetValue("datefrom", out string f)) { _eventsFilters.datefrom = valuePairs["datefrom"].ToString(); }
                if (valuePairs.TryGetValue("dateto", out string t)) { _eventsFilters.dateto = valuePairs["dateto"].ToString(); }
                if(valuePairs.TryGetValue("keyword", out string k)) { _eventsFilters.keyword = valuePairs["keyword"]; }
                if(valuePairs.TryGetValue("language", out string l)) { _eventsFilters.language = Convert.ToInt32(valuePairs["language"]); } 

                return _eventsFilters;

            }
            else
            {
                return _eventsFilters;
            }

        }

        public static ScheduleFilters GetClassesFilters(HttpRequest request)
        {
            ScheduleFilters _classFilters = new ScheduleFilters
            {
                datefrom = null,
                dateto = null,
                conceptid = null,
                classid = null,
                instructorid = null,
                activitytype = null,
                keyword = null,
                limit = null,
                offset = null,
                classtypeid = null,
                language = 0
            };

            Dictionary<string, string> valuePairs = ParseUri(request);

            if (valuePairs.Count > 0)
            {
                if (valuePairs.TryGetValue("datefrom", out string f)) { _classFilters.datefrom = valuePairs["datefrom"].ToString(); }
                if (valuePairs.TryGetValue("dateto", out string t)) { _classFilters.dateto = valuePairs["dateto"].ToString(); }
                if (valuePairs.TryGetValue("conceptid", out string s)) { _classFilters.conceptid = valuePairs["conceptid"].ToString();}
                if (valuePairs.TryGetValue("classid", out string c)) { _classFilters.classid = valuePairs["classid"].ToString(); }
                if (valuePairs.TryGetValue("instructorid", out string p)) { _classFilters.instructorid = valuePairs["instructorid"].ToString(); }
                if (valuePairs.TryGetValue("activitytype", out string at)) { _classFilters.activitytype = valuePairs["activitytype"].ToString(); }
                if (valuePairs.TryGetValue("keyword", out string k)) { _classFilters.keyword = valuePairs["keyword"].ToString(); }
                if (valuePairs.TryGetValue("limit", out string l)) { _classFilters.limit = Convert.ToInt32(valuePairs["limit"]); }
                if (valuePairs.TryGetValue("offset", out string o)) { _classFilters.offset = Convert.ToInt32(valuePairs["offset"]); }
                if (valuePairs.TryGetValue("classtypeid", out string ct)) { _classFilters.classtypeid = Convert.ToInt32(valuePairs["classtypeid"]); }
                if (valuePairs.TryGetValue("language", out string lang)) { _classFilters.language = Convert.ToInt32(valuePairs["language"]);  } 

                return _classFilters;
            }
            else
            {
                return _classFilters;
            }

        }


        public static PersonnelFilters GetPersonnelFilters(HttpRequest request)
        {
            PersonnelFilters _personnelFilters = new PersonnelFilters
            {
                conceptid = null,
                personnelid = null,
                personneltype = null
            };

            Dictionary<string, string> valuePairs = ParseUri(request);

            if (valuePairs.Count > 0)
            {
                if (valuePairs.TryGetValue("conceptid", out string s)) { _personnelFilters.conceptid = Convert.ToInt32(valuePairs["conceptid"]); }
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
