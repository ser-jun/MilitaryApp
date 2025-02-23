using System;
using System.Collections.Generic;

namespace MilitaryApp.Models;

public partial class Subunit
{
    public int SubunitId { get; set; }

    public int? UnitId { get; set; }

    public string SubunitName { get; set; } = null!;

    public virtual Militaryunit? Unit { get; set; }
}
