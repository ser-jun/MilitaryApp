﻿using MilitaryApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilitaryApp.Data.Repositories.Interfaces
{
    public interface IArmyRepository
    {
        Task<List<Army>> GetArmy();
        Task AddArmy(string name);
        Task UpdateArmy(int armyId, string newName);
        Task DeleteArmy(int armyId);
    }
}
