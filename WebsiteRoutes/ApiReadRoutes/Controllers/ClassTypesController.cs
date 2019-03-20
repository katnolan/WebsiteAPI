using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using ApiReadRoutes.Models;
using ApiReadRoutes.Services;

namespace ApiReadRoutes.Controllers
{
    [Authorize]
    [Route("v2/classtypes/{clubid}")]
    [Produces("application/json")]
    [ApiController]
    public class ClassTypesController : ControllerBase
    {
        private readonly ILogger<ClassTypesController> _logger;
        public ClassTypesController(ILogger<ClassTypesController> logger)
        {
            _logger = logger;
        }


        //GET /v1/classtypes
        [HttpGet]
        public ActionResult GetClassTypes(int clubid, int? conceptid = null, int? language = 0)
        {
            _logger.LogInformation("Logging Information");

            List<ClassTypes> ct = new ClassTypesService(clubid, conceptid, language).GetClassTypes();

            if(conceptid == null)
            {
                return Ok(ct);
            }
            else
            {
                var conceptCT = ct.Where((c) => c.conceptId == conceptid);

                return Ok(conceptCT);
            }
        }
    }
}