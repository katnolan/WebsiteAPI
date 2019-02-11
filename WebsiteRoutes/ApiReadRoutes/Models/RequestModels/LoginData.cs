using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ApiReadRoutes.Models
{
    public class LoginData
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string OSVersion { get; set; }
        public string DeviceModel { get; set; }
        public override string ToString()
        {
            var temp = this.GetType()
                           .GetProperties()
                           .Select(p => $"{p.Name}={HttpUtility.UrlEncode(p.GetValue(this).ToString())}");

            return string.Join("&", temp);
        }
    }
}
