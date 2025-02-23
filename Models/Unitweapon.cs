using System;
using System.Collections.Generic;

namespace MilitaryApp.Models;

public partial class Unitweapon
{
    public int UnitId { get; set; }

    public int WeaponId { get; set; }

    public int? Quantity { get; set; }

    public virtual Militaryunit Unit { get; set; } = null!;

    public virtual Weapon Weapon { get; set; } = null!;
}
