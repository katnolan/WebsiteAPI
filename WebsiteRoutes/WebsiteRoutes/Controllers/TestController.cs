using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace WebsiteRoutes.Controllers
{
    [Route("api/test")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly ILogger<TestController> _logger;
        public TestController(ILogger<TestController> logger)
        {
            _logger = logger;
        }


        // GET api/test
        [HttpGet]
        public ActionResult Get()
        {
            _logger.LogInformation("Logging info");
            return Ok("Hello");
        }


        // GET 
        [HttpGet("{id}")]
        public ActionResult Get(int id)
        {
            _logger.LogInformation("Logging info");

            if(id > 5)
            {
                return Ok("Greater than 5");
            }
            else
            {
                return Ok("Less than 5");
            }
        }
    }
}
