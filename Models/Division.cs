using System;
using System.Collections.Generic;

namespace MilitaryApp.Models;

public partial class Division
{
    public int DivisionId { get; set; }

    public string Name { get; set; } = null!;

    public int? ArmyId { get; set; }

    public virtual Army? Army { get; set; }

    public virtual ICollection<Corps> Corps { get; set; } = new List<Corps>();
}
