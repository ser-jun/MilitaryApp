using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilitaryApp.DTO
{
    public class EquipmentItem
    {
        public int EquipmentId { get; set; }
        public string? NameEquipment {  get; set; }  
        public string? TypeEquipment { get; set; }
        public int? UnitId { get; set; }
        public string? UnitName { get; set; }    
        public int? Quantity { get; set; }

    }
}
