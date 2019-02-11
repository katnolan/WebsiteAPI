using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using ApiReadRoutes.Services;
using ApiReadRoutes.Models;
using Microsoft.AspNetCore.Authorization;

namespace ApiReadRoutes.Controllers
{
    [Authorize]
    [Route("v1/studios")]
    [Produces("application/json")]
    [ApiController]
    public class StudiosController : ControllerBase
    {
        private readonly ILogger<StudiosController> _logger;
        public StudiosController(ILogger<StudiosController> logger)
        {
            _logger = logger;
        }

        List<Studio> studios = new StudioService().GetStudios();


        [HttpGet]
        public ActionResult GetStudios(int? clubid = null, int? studioid = null)
        {
            _logger.LogInformation("Logging info");

            if (clubid != null && studioid != null)
            {
                var studioClub = studios.FirstOrDefault((s) => s.studioid == studioid);
                return Ok(studioClub);
            }
            else if (clubid != null)
            {

                var clubStudios = studios.Where<Studio>((s) => s.clubid == clubid);
                return Ok(clubStudios);
            }
            else if (studioid != null)
            {
                var studio = studios.FirstOrDefault((s) => s.studioid == studioid);
                return Ok(studio);
            }
            else
            {
                return Ok(studios);
            }

        }

        [HttpGet("{studioid}")]
        public ActionResult GetStudio(int studioid)
        {
            _logger.LogInformation("Logging info");

            var studio = studios.FirstOrDefault((s) => s.studioid == studioid);
            return Ok(studio);

        }

        [HttpGet("{clubid}")]
        public ActionResult GetClubStudios(int clubid)
        {
            _logger.LogInformation("Logging info");

            var clubStudios = studios.Where((s) => s.clubid == clubid);
            return Ok(clubStudios);

        }

    }
}