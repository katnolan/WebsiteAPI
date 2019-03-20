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

        


        [HttpGet]
        public ActionResult GetConcepts(int? clubid = null, int? conceptid = null, int? language = 0)
        {
            _logger.LogInformation("Logging info");

            List<Concept> concepts = new ConceptService(clubid, conceptid, language).GetConcepts();

            return Ok(concepts);


        }

       

    }
}