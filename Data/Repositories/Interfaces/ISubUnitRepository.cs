using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilitaryApp.Data.Repositories.Interfaces
{
    public interface ISubUnitRepository
    {
        Task AddSubUnit(string name, int unitId);
        Task DeleteSubUnit(int subUnitId);
        Task UpdateSubUnit(int subUnitId, string newName, int newUnitId);
    }
}
