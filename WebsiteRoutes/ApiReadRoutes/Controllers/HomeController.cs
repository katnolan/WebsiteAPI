using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace ApiReadRoutes.Controllers
{
    [Route("api/home")]
    public class HomeController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public HomeController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet]
        public ActionResult Index(DateTime? dateFrom = null, DateTime? dateTo = null)
        {
            var webRootPath = _hostingEnvironment.WebRootPath;
            var contentRootPath = _hostingEnvironment.ContentRootPath;

            var jsonpath = Path.Combine(contentRootPath, "Utils\\googleCredentials.json");

            DateTime df = Convert.ToDateTime(dateFrom);
            DateTime dt = Convert.ToDateTime(dateTo);

            string datetimetest = "DateFrom: " + df.ToString("yyyy-MM-dd") + " and DateTo: " + dt.ToString("yyyy-MM-dd");
                                 

            return Content(webRootPath + "\n" + contentRootPath + "\n" + jsonpath + "\n" + datetimetest);
        }
    }
}