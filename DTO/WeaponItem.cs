using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilitaryApp.DTO
{
    public class WeaponItem
    {
        public int WeaponId { get; set; }   
        public string? NameWeapon { get; set; }  
        public string? TypeWeapon { get; set; }
        public int UnitId { get; set; }
        public int Quantity { get; set; }
        public string? NameUnit { get; set; }
    }
}
