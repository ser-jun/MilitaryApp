using System;
using System.Collections.Generic;

namespace MilitaryApp.Models;

public partial class Combatequipment
{
    public int EquipmentId { get; set; }

    public string Name { get; set; } = null!;

    public string Type { get; set; } = null!;

    public virtual ICollection<Unitcombatequipment> Unitcombatequipments { get; set; } = new List<Unitcombatequipment>();
}
