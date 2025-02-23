using System;
using System.Collections.Generic;

namespace MilitaryApp.Models;

public partial class Army
{
    public int ArmyId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Division> Divisions { get; set; } = new List<Division>();
}
