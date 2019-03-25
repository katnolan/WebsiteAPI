using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using ApiReadRoutes.Models;
using ApiReadRoutes.Services;
using ApiReadRoutes.Utils;

namespace ApiReadRoutes.Controllers
{
    [Authorize]
    [Route("v2/personnel/{clubid}")]
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
        public ActionResult GetEmployees(int clubid, string personneltype, int? conceptid = null, int? personnelid = null)
        {

            _logger.LogInformation("Logging Info");

            PersonnelFilters filters = RequestHelper.GetPersonnelFilters(this.Request);

            List<Personnel> employees = new PersonnelService(clubid, filters).GetPersonnel();

            var emp = employees.Where((e) => e.clubId == clubid);
            return Ok(emp);
        }


    }
}
