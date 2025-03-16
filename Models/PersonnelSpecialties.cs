using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MilitaryApp.Models;

namespace MilitaryApp.Models
{
    public class PersonnelSpecialties
    {
        public int PersonnelId { get; set; }

        public int SpecialtyId { get; set; }

        public virtual Militarypersonnel Personnel { get; set; } = null!;

        public virtual Militaryspecialty Specialty { get; set; } = null!;
    }
}
