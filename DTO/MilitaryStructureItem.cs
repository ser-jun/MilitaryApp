using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MilitaryApp.DTO
{
    public class MilitaryStructureItem
    {
        [Display(AutoGenerateField = false)]
        public int ArmyId { get; set; }
        public string ArmyName { get; set; } = null!;
        [Display(AutoGenerateField = false)]
        public int? DivisionId { get; set; }
        public string DivisionName { get; set; } = null!;
        [Display(AutoGenerateField = false)]
        public int? CorpsId { get; set; }
        public string CorpsName { get; set; } = null!;
        [Display(AutoGenerateField = false)]
        public int? UnitId { get; set; }
        public string UnitName { get; set; } = null!;

        public string ItemType =>
       UnitId.HasValue ? "Unit" :
       CorpsId.HasValue ? "Corps" :
       DivisionId.HasValue ? "Division" :
       "Army";
    }
}
