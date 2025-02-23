using System;
using System.Collections.Generic;

namespace MilitaryApp.Models;

public partial class Officer
{
    public int OfficerId { get; set; }

    public int? PersonnelId { get; set; }

    public string? Position { get; set; }

    public virtual ICollection<Militaryunit> Militaryunits { get; set; } = new List<Militaryunit>();

    public virtual Militarypersonnel? Personnel { get; set; }
}
