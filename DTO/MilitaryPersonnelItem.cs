using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilitaryApp.DTO
{
    public  class MilitaryPersonnelItem
    {
        public int? PersonnelId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Rank { get; set; }
        public string? Position { get; set; }
        public string? Specialties { get; set; }
        public string? Unit { get; set; }
    }
}
