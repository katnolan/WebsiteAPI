using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using ApiReadRoutes.Models;
using ApiReadRoutes.Services;

namespace ApiReadRoutes.Controllers
{
    [Route("v1/personnel/{clubid}")]
    [Produces("application/json")]
    [ApiController]
    public class PersonnelController : ControllerBase
    {
        private readonly ILogger<PersonnelController> _logger;
        public PersonnelController(ILogger<PersonnelController> logger)
        {
            _logger = logger;
        }
       

        

        // GET v1/personnel/{clubid}?studioid={studioid}&personnelid={personnelid}&personneltype={personneltype}
        [HttpGet]
        public ActionResult GetEmployees(int clubid, int? studioid = null, int? personnelid = null, string personneltype = null)
        {

            _logger.LogInformation("Logging Info");

            List<Personnel> employees = new PersonnelService(clubid, studioid, personnelid, personneltype).GetPersonnel();

            var emp = employees.Where((e) => e.clubid == clubid);
            return Ok(emp);
        }


    }
}
