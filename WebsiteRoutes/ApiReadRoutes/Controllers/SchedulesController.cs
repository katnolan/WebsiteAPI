using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using ApiReadRoutes.Models;
using ApiReadRoutes.Services;

namespace ApiReadRoutes.Controllers
{
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
        public ActionResult GetSchedules(int clubid, string activityType, string status, DateTime? startdate = null, DateTime? enddate = null, long[] studioid = null, long[] classid = null, long[] personnelid = null, int? limit = null, int? offset = null)
        {
            _logger.LogInformation("Logging Info");

            List<Schedule> classes = new ScheduleService(clubid, activityType, status, startdate, enddate, studioid, classid, personnelid, limit, offset).GetClasses();
            return Ok(classes);
        }


    }
}