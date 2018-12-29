using System;
using System.Collections.Generic;
using System.IO;
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
    [Route("v1/clubs")]
    [Produces("application/json")]
    [ApiController]
    public class ClubsController : ControllerBase
    {
        private readonly ILogger<ClubsController> _logger;
        public ClubsController(ILogger<ClubsController> logger)
        {
            _logger = logger;
        }
       

        List<Club> clubs = new ClubService().GetClubs();

        // GET v1/clubs?clubid={clubid}
        [HttpGet]
        public ActionResult GetClubs(int? clubid = null)
        {

            _logger.LogInformation("Logging Info");

            if(clubid != null)
            {
                var club = clubs.FirstOrDefault((c) => c.clubid == clubid);
                return Ok(club);
            }
            else
            {
                return Ok(clubs);
            }
            
        }

        [HttpGet("{clubid}")]
        public ActionResult GetClub(int? clubid = null)
        {

            _logger.LogInformation("Logging Info");

            var club = clubs.FirstOrDefault((c) => c.clubid == clubid);
            return Ok(club);

        }

    }
}
