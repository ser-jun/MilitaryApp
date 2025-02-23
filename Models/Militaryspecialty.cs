using System;
using System.Collections.Generic;

namespace MilitaryApp.Models;

public partial class Militaryspecialty
{
    public int SpecialtyId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Militarypersonnel> Personnel { get; set; } = new List<Militarypersonnel>();
}
