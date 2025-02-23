using System;
using System.Collections.Generic;

namespace MilitaryApp.Models;

public partial class Weapon
{
    public int WeaponId { get; set; }

    public string Name { get; set; } = null!;

    public string Type { get; set; } = null!;

    public virtual ICollection<Unitweapon> Unitweapons { get; set; } = new List<Unitweapon>();
}
