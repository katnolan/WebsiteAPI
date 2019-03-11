using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiReadRoutes.Models.ResponseModels
{
    public class Resource
    {
        public int resourceId { get; set; }
        public string resourceName { get; set; }
        public int clubId { get; set; }
        public bool isActive { get; set; }


    }
}
