using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilitaryApp.DTO
{
    public class InfrastructureItem
    {
            public int BuildingId { get; set; }
            public string Name { get; set; }
            public int? YearBuilt { get; set; }
            public int? UnitId { get; set; }
            public string UnitName { get; set; } 
    }
}
