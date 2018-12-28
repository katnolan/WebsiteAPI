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

namespace ApiReadRoutes.Controllers
{
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

        Studio[] studios = new StudioService().Get();


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
                var clubStudios = studios.TakeWhile((s) => s.clubid == clubid);
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

    }
}