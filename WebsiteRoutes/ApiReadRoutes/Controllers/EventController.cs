using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using ApiReadRoutes.Services;
using ApiReadRoutes.Models;
using ApiReadRoutes.Utils;


namespace ApiReadRoutes.Controllers
{
    //[Authorize]
    [Route("v2/events/{clubid}")]
    [Produces("application/json")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly ILogger<EventsController> _logger;
        private EventService _ef;

        public EventsController(ILogger<EventsController> logger)
        {
            _logger = logger;
            _ef = new EventService();
        }

        // GET v1/events
        [HttpGet]
        public ActionResult GetEvents(int clubid, string datefrom, string dateto, string keyword, int? conceptid = null)
        {
            _logger.LogInformation("Log Start");

            var eventFilters = RequestHelper.GetEventFilters(Request);
            
            List<Event> events = new EventService(clubid, eventFilters).GetEvents();

            var ev = events.Where((e) => e.clubId == clubid);
            return Ok(ev);
        }


        
    }
}