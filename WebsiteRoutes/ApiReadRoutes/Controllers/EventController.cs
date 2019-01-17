﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ApiReadRoutes.Services;
using ApiReadRoutes.Models;


namespace ApiReadRoutes.Controllers
{
    [Route("v1/events/{clubid}")]
    [Produces("application/json")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly ILogger<EventsController> _logger;
        public EventsController(ILogger<EventsController> logger)
        {
            _logger = logger;
        }

        // GET v1/events
        [HttpGet]
        public ActionResult GetEvents(int clubid, int? studioid = null, int? month = null, string keyword = null)
        {
            _logger.LogInformation("Log Start");

            List<Event> events = new EventService(clubid, studioid, month, keyword).GetEvents();

            var ev = events.Where((e) => e.clubid == clubid);
            return Ok(ev);
        }


        
    }
}