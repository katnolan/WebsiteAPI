using System;

namespace ApiReadRoutes.Models.ResponseModels
{
    public class ClubDetails
    {
        public int clubId { get; set; }
        public int csiId { get; set; }
        public int gpId { get; set; }
        public string clubName { get; set; }
        public string location { get; set; }
        public string zip { get; set; }
        public Int64 RitaID { get; set; }
        public Int64 MerchantID { get; set; }
        public int language { get; set; }
        public int timeZone { get; set; }
        public bool isActive { get; set; }

    }
}
