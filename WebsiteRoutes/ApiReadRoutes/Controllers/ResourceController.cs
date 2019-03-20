using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using ApiReadRoutes.Models.ResponseModels;
using ApiReadRoutes.Services;

namespace ApiReadRoutes.Controllers
{
    [Authorize]
    [Route("v1/resources/{clubid}")]
    [Produces("application/json")]
    [ApiController]
    public class ResourceController : ControllerBase
    {
        private readonly ILogger<ResourceController> _logger;

        public ResourceController(ILogger<ResourceController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public ActionResult GetResources(int clubid, int? resourceid = null)
        {
            _logger.LogInformation("Logging Information");

            List<Resource> resources = new ResourceService(clubid, resourceid).GetResources();

            return Ok(resources);
            
        }
    }
}