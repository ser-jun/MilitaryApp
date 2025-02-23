using System;
using System.Collections.Generic;

namespace MilitaryApp.Models;

public partial class Infrastructure
{
    public int BuildingId { get; set; }

    public string Name { get; set; } = null!;

    public int? UnitId { get; set; }

    public int? YearBuilt { get; set; }

    public virtual Militaryunit? Unit { get; set; }
}
