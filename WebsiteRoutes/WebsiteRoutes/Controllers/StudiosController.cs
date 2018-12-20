using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebsiteRoutes.Services;
using WebsiteRoutes.Models;

namespace WebsiteRoutes.Controllers
{
    [Route("v1/studios")]
    [AllowAnonymous]
    public class StudiosController : ApiController
    {

        Studio[] studios = new StudiosService().Get();


        [HttpGet]
        public HttpResponseMessage GetStudios(int? clubid = null, int? studioid = null)
        {


            if (clubid != null)
            {
                var clubStudios = studios.TakeWhile((s) => s.clubid == clubid);
                return Request.CreateResponse(HttpStatusCode.OK, clubStudios);
            }
            else if (studioid != null)
            {
                var studio = studios.FirstOrDefault((s) => s.studioid == studioid);
                return Request.CreateResponse(HttpStatusCode.OK, studio);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, studios);
            }

        }


    }
}
