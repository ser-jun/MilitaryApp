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
        [Key, Column(Order = 0)]
        public int PersonnelId { get; set; }
        [Key, Column(Order = 1)]
        public int SpecialtyId { get; set; }

        [ForeignKey(nameof(PersonnelId))]
        public virtual Militarypersonnel Personnel { get; set; }

        [ForeignKey(nameof(SpecialtyId))]
        public virtual Militaryspecialty Specialty { get; set; }
    }
}
