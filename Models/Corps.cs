using System;
using System.Collections.Generic;

namespace MilitaryApp.Models;

public partial class Corps
{
    public int CorpsId { get; set; }

    public string Name { get; set; } = null!;

    public int? DivisionId { get; set; }

    public virtual Division? Division { get; set; }

    public virtual ICollection<Militaryunit> Militaryunits { get; set; } = new List<Militaryunit>();
}
