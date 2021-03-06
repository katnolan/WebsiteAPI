﻿using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using ApiReadRoutes.Models;
using ApiReadRoutes.Services;
using ApiReadRoutes.Utils;

namespace ApiReadRoutes.Controllers
{
    [Authorize]
    [Route("/v2/schedule/{clubid}")]
    [Produces("application/json")]
    [ApiController]
    public class SchedulesController : ControllerBase
    {
        private readonly ILogger<SchedulesController> _logger;
        public SchedulesController(ILogger<SchedulesController> logger)
        {
            _logger = logger;
        }

        //GET v1/schedule/{clubid}?startdate={startdate}&enddate={enddate}&conceptid=[{conceptid}]&classid=[{classid}]&personnelid=[{personnelid}]&activityType={activityType}&keyword={keyword}&status={status}&limit={limit}&offset={offset}&classtypeid={classtypeid}
        [HttpGet]
        public ActionResult GetSchedules(int clubid, string activitytype, string status, string keyword, string datefrom = null, string dateto = null, int?[] conceptid = null, int?[] personnelid = null, int? limit = null, int? offset = null, int? classtypeid = null, int? language = 0)
        {
            _logger.LogInformation("Logging Info");

            ScheduleFilters classFilters = RequestHelper.GetClassesFilters(Request);

            List<Schedule> classes = new ScheduleService(clubid, classFilters).GetClasses();

            if(classes.Count() == 0)
            {
                return NotFound("No result");
            }
            else
            {
                return Ok(classes);
            }
            
        }


    }
}