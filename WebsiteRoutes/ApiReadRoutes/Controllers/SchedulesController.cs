using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using ApiReadRoutes.Models;
using ApiReadRoutes.Services;
using ApiReadRoutes.Utils;

namespace ApiReadRoutes.Controllers
{
    //[Authorize]
    [Route("/v1/schedule/{clubid}")]
    [Produces("application/json")]
    [ApiController]
    public class SchedulesController : ControllerBase
    {
        private readonly ILogger<PersonnelController> _logger;
        public SchedulesController(ILogger<PersonnelController> logger)
        {
            _logger = logger;
        }

        //GET v1/schedule/{clubid}?startdate={startdate}&enddate={enddate}&studioid=[{studioid}]&classid=[{classid}]&personnelid=[{personnelid}]&activityType={activityType}&status={status}&limit={limit}&offset={offset}
        [HttpGet]
        public ActionResult GetSchedules(int clubid)
        {
            _logger.LogInformation("Logging Info");

            ScheduleFilters classFilters = RequestHelper.GetClassesFilters(Request);

            List<Schedule> classes = new ScheduleService(clubid, classFilters).GetClasses();
            return Ok(classes);
        }


    }
}