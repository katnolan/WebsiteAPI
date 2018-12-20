using WebsiteRoutes.Models;
using WebsiteRoutes.Services;
using System;
using System.Collections.Generic;
using Google.Cloud.BigQuery.V2;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebsiteRoutes.Controllers
{
    [Route("v1/clubs")]
    [AllowAnonymous]
    public class ClubsController : ApiController
    {

        Club[] clubs = new ClubsService().Get();

        [HttpGet]
        public HttpResponseMessage GetClubs(int? clubid = null)
        {
            

            if (clubid != null)
            {
                var club = clubs.FirstOrDefault((c) => c.clubid == clubid);
                return Request.CreateResponse(HttpStatusCode.OK, club);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, clubs);
            }
            
        }

    }
}
