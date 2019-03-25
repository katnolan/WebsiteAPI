using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using ApiReadRoutes.Models;
using ApiReadRoutes.Models.ResponseModels;
using ApiReadRoutes.Services;

namespace ApiReadRoutes.Controllers
{
    [Authorize]
    [Route("v2/clubs")]
    [Produces("application/json")]
    [ApiController]
    public class ClubsController : ControllerBase
    {
        private readonly ILogger<ClubsController> _logger;
        public ClubsController(ILogger<ClubsController> logger)
        {
            _logger = logger;
        }
       

        

        // GET v2/clubs?clubid={clubid}
        [HttpGet("")]
        public ActionResult GetClubs(int? clubid = null)
        {

            _logger.LogInformation("Logging Clubs Info");

            List<Club> clubs = new ClubService(null, clubid).GetClubs();

            return Ok(clubs);
            
        }

        // GET v2/clubs/details?siteid={siteid}&clubid={clubid}
        [HttpGet("details")]
        public ActionResult GetClubDetails(int? siteid = null, int? clubid = null)
        {
            _logger.LogInformation("Logging ClubDetails Info");

            List<ClubDetails> details = new ClubService(siteid, clubid).GetClubDetails();

            return Ok(details);

        }


    }
}
