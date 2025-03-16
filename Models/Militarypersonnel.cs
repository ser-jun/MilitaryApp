using System;
using System.Collections.Generic;

namespace MilitaryApp.Models;

public partial class Militarypersonnel
{
    public int PersonnelId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Rank { get; set; } = null!;

    public int? UnitId { get; set; }

    public virtual ICollection<Enlistedpersonnel> Enlistedpersonnel { get; set; } = new List<Enlistedpersonnel>();

    public virtual ICollection<Officer> Officers { get; set; } = new List<Officer>();

    public virtual Militaryunit? Unit { get; set; }

    public virtual ICollection<PersonnelSpecialties> Specialties { get; set; } = new List<PersonnelSpecialties>();
}
