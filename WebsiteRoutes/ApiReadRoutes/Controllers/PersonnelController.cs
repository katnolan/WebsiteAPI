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
    [Route("v1/personnel")]
    [Produces("application/json")]
    [ApiController]
    public class PersonnelController : ControllerBase
    {
        private readonly ILogger<PersonnelController> _logger;
        public PersonnelController(ILogger<PersonnelController> logger)
        {
            _logger = logger;
        }
       

        

        // GET v1/clubs?clubid={clubid}
        [HttpGet]
        public ActionResult GetEmployees(int? clubid = null, int? studioid = null)
        {

            _logger.LogInformation("Logging Info");

            List<Personnel> employees = new PersonnelService(clubid, studioid).GetPersonnel();

            if (clubid != null && studioid != null)
            {
                var employee = employees.Where((s) => s.studioid == studioid);
                return Ok(employee);
            }
            else if(clubid != null)
            {
                var employee = employees.Where((e) => e.clubid == clubid);
                return Ok(employee);
            }
            else if(studioid != null)
            {
                var employee = employees.Where((s) => s.studioid == studioid);
                return Ok(employee);
            }
            else
            {
                return Ok(employees);
            }
            
        }

        [HttpGet("{clubid}")]
        public ActionResult GetEmployee(int? clubid = null)
        {

            _logger.LogInformation("Logging Info");

            List<Personnel> employees = new PersonnelService(clubid, null).GetPersonnel();

            var employee = employees.Where((e) => e.clubid == clubid);
            return Ok(employee);

        }


        [HttpGet("{studioid}")]
        public ActionResult GetStudioEmployees(int? studioid = null)
        {

            _logger.LogInformation("Logging Info");

            List<Personnel> employees = new PersonnelService(null, studioid).GetPersonnel();

            var employee = employees.Where((e) => e.studioid == studioid);
            return Ok(employee);

        }
    }
}
