using System.Linq;
using System.Web;

namespace ApiReadRoutes.Models
{
    public class EventsFilters
    {


        public int? conceptid { get; set; }

        public int? resourceid { get; set; }

        public string keyword { get; set; }

        public string datefrom { get; set; }

        public string dateto { get; set; }

        public int? language { get; set; }


        public override string ToString()
        {
            var temp = this.GetType()
                           .GetProperties()
                           .Select(p => $"{p.Name}={HttpUtility.UrlEncode(p.GetValue(this).ToString())}");

            return string.Join("&", temp);
        }

    }
}
