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
    [Route("v2/concepts/")]
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
        public ActionResult GetConcepts(int? clubid, int? conceptid = null)
        {
            _logger.LogInformation("Logging info");

            if (clubid != null && conceptid != null)
            {
                var clubConcept = concepts.Where((c) => c.conceptid == conceptid && c.clubid == clubid);
                return Ok(clubConcept);
            }
            else if (conceptid != null)
            {
                var clubConcept = concepts.Where((s) => s.conceptid == conceptid);
                return Ok(clubConcept);
            }
            else if (clubid != null)
            {
                var clubConcept = concepts.Where((c) => c.clubid == clubid);
                return Ok(clubConcept);
            }
            else
            {
                return Ok(concepts);
            }

        }

       

    }
}