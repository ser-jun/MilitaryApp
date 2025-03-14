using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilitaryApp.Data.Repositories.Interfaces
{
    public interface IMilitaryUnitRepository
    {
        Task AddMilitaryUnit(string name, int corpsId);
        Task UpdateMilitaryUnit(int unitId, string newName, int newCorpsId);
        Task DeleteUnit(int unitId);
    }
}
