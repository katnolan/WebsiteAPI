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
    //[Authorize]
    [Route("v2/concepts")]
    [Produces("application/json")]
    [ApiController]
    public class ConceptsController : ControllerBase
    {
        private readonly ILogger<ConceptsController> _logger;
        public ConceptsController(ILogger<ConceptsController> logger)
        {
            _logger = logger;
        }

        List<Concept> concepts = new ConceptService().GetConcepts();


        [HttpGet]
        public ActionResult GetConcepts(int? clubid = null, int? conceptid = null)
        {
            _logger.LogInformation("Logging info");

            if (clubid != null && conceptid != null)
            {
                var conceptClub = concepts.FirstOrDefault((s) => s.conceptid == conceptid);
                return Ok(conceptClub);
            }
            else if (clubid != null)
            {

                var clubConcepts = concepts.Where<Concept>((s) => s.clubid == clubid);
                return Ok(clubConcepts);
            }
            else if (conceptid != null)
            {
                var concept = concepts.FirstOrDefault((s) => s.conceptid == conceptid);
                return Ok(concept);
            }
            else
            {
                return Ok(concepts);
            }

        }

        [HttpGet("{conceptid}")]
        public ActionResult Getconcept(int conceptid)
        {
            _logger.LogInformation("Logging info");

            var concept = concepts.FirstOrDefault((s) => s.conceptid == conceptid);
            return Ok(concept);

        }

        [HttpGet("{clubid}")]
        public ActionResult GetClubconcepts(int clubid)
        {
            _logger.LogInformation("Logging info");

            var clubConcepts = concepts.Where((s) => s.clubid == clubid);
            return Ok(clubConcepts);

        }

    }
}