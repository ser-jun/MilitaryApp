using System;
using System.Collections.Generic;

namespace MilitaryApp.Models;

public partial class Enlistedpersonnel
{
    public int EnlistedId { get; set; }

    public int? PersonnelId { get; set; }

    public string? Position { get; set; }

    public virtual Militarypersonnel? Personnel { get; set; }
}
