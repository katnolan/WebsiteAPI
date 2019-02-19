using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;

namespace ApiReadRoutes.Models
{
    public class PersonnelFilters
    {
        [DisplayName("studioId")]
        public int? studioid { get; set; }

        [DisplayName("personnelId")]
        public int? personnelid { get; set; }

        [DisplayName("personnelType")]
        public string personneltype { get; set; }

    }
}
