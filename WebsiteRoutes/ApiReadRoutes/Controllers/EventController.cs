using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using ApiReadRoutes.Services;
using ApiReadRoutes.Models;
using ApiReadRoutes.Utils;


namespace ApiReadRoutes.Controllers
{
    [Authorize]
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

        // GET v2/events/{clubid}
        [HttpGet]
        public ActionResult GetEvents(int clubid, string datefrom, string dateto, string keyword, int? conceptid = null, int? resourceid = null, int? language = 0)
        {
            _logger.LogInformation("Log Start");

            var eventFilters = RequestHelper.GetEventFilters(Request);
            
            List<Event> events = new EventService(clubid, eventFilters, null).GetEvents();

            if(events.Count() == 0)
            {
                return NotFound("No result");
            }
            else
            {
                return Ok(events);
            }
            
        }

        //GET v2/events/{clubid}/{eventid}
        [HttpGet("{eventid}")]
        public ActionResult GetEventId (int clubid, int eventid)
        {
            _logger.LogInformation("Log Events API");

            var eventFilters = RequestHelper.GetEventFilters(Request);

            List<Event> events = new EventService(clubid, eventFilters, eventid).GetEvents();

            if(events.Count() == 0)
            {
                return NotFound("No result");
            }
            else
            {
                return Ok(events);
            }
            

            
        }

        
    }
}