﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebsiteRoutes.Services;
using WebsiteRoutes.Models;

namespace WebsiteRoutes.Controllers
{
    [Route("v1/studios")]
    [ApiController]
    public class StudiosController : ControllerBase
    {
        private readonly ILogger<StudiosController> _logger;
        public StudiosController(ILogger<StudiosController> logger)
        {
            _logger = logger;
        }

        Studio[] studios = new StudiosService().Get();


        [HttpGet]
        public ActionResult GetStudios(int? clubid = null, int? studioid = null)
        {
            _logger.LogInformation("Logging info");

            if (clubid != null)
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
