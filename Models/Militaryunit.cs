using System;
using System.Collections.Generic;

namespace MilitaryApp.Models;

public partial class Militaryunit
{
    public int? UnitId { get; set; }

    public string Name { get; set; } = null!;

    public int? CorpsId { get; set; }

    public int? CommanderId { get; set; }

    public virtual Officer? Commander { get; set; }

    public virtual Corps? Corps { get; set; }

    public virtual ICollection<Infrastructure> Infrastructures { get; set; } = new List<Infrastructure>();

    public virtual ICollection<Militarypersonnel> Militarypersonnel { get; set; } = new List<Militarypersonnel>();

    public virtual ICollection<Subunit> Subunits { get; set; } = new List<Subunit>();

    public virtual ICollection<Unitcombatequipment> Unitcombatequipments { get; set; } = new List<Unitcombatequipment>();

    public virtual ICollection<Unitweapon> Unitweapons { get; set; } = new List<Unitweapon>();
}
