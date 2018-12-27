using WebsiteRoutes.Models;
using WebsiteRoutes.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Google.Cloud.BigQuery.V2;


namespace WebsiteRoutes.Controllers
{
    [Route("v1/clubs")]
    [ApiController]
    public class ClubsController : ControllerBase
    {

        Club[] clubs = new ClubsService().Get();

        private readonly ILogger<ClubsController> _logger;
        public ClubsController(ILogger<ClubsController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public ActionResult GetClubs(int? clubid = null)
        {

            _logger.LogInformation("Logging Info");

            if (clubid != null)
            {
                var club = clubs.FirstOrDefault((c) => c.clubid == clubid);
                return Ok(club);
            }
            else
            {
                return Ok(clubs);
            }
            
        }

    }
}
