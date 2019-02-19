using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using ApiReadRoutes.Models;
using ApiReadRoutes.Services;

namespace ApiReadRoutes.Controllers
{
    //[Authorize]
    [Route("/v1/activitytype")]
    [Produces("application/json")]
    [ApiController]
    public class ActivityTypeController : ControllerBase
    {
        private readonly ILogger<ActivityTypeController> _logger;
        public ActivityTypeController(ILogger<ActivityTypeController> logger)
        {
            _logger = logger;
        }

        //GET /v1/activitytype?id={id}&type={type}
        [HttpGet]
        public ActionResult GetActivities(string type = null, int? id = null)
        {
            _logger.LogInformation("Logging Info");

            List<ActivityType> types = new ActivityTypeService(type, id).GetActivities();

            return Ok(types);
        }
    }
}