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
        private readonly IHostingEnvironment _hostingEnvironment;

        private string jsonpath = "";

        public ClubsController(ILogger<ClubsController> logger, IHostingEnvironment hostingEnvironment)
        {
            _logger = logger;
            _hostingEnvironment = hostingEnvironment;
        }
        
        public ActionResult Index()
        {
            var contentRootPath = _hostingEnvironment.ContentRootPath;
            jsonpath = Path.Combine(contentRootPath, "Utils\\googleCredential.json");

            return Ok(jsonpath);
        }

        Club[] clubs = new ClubService().Get();

        // GET v1/clubs?clubid={clubid}
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

        [HttpGet("{clubid}")]
        public ActionResult GetClub(int clubid)
        {

            _logger.LogInformation("Logging Info");

            var club = clubs.FirstOrDefault((c) => c.clubid == clubid);
            return Ok(club);

        }

    }
}
