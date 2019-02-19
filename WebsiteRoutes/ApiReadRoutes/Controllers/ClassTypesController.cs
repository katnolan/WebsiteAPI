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
    //[Authorize]
    [Route("v1/classtypes")]
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
        public ActionResult GetClassTypes()
        {
           _logger.LogInformation("Logging Information");


            List<ClassTypes> ct = new ClassTypesService().GetClassTypes();

           return Ok(ct);
        }
    }
}