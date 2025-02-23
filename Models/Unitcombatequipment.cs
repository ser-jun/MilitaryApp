using System;
using System.Collections.Generic;

namespace MilitaryApp.Models;

public partial class Unitcombatequipment
{
    public int UnitId { get; set; }

    public int EquipmentId { get; set; }

    public int? Quantity { get; set; }

    public virtual Combatequipment Equipment { get; set; } = null!;

    public virtual Militaryunit Unit { get; set; } = null!;
}
