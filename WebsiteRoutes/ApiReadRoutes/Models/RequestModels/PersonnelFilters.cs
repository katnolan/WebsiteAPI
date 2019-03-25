using System.ComponentModel;

namespace ApiReadRoutes.Models
{
    public class PersonnelFilters
    {
        [DisplayName("conceptid")]
        public int? conceptid { get; set; }

        [DisplayName("personnelid")]
        public int? personnelid { get; set; }

        [DisplayName("personneltype")]
        public string personneltype { get; set; }

    }
}
