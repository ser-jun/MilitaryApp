using MilitaryApp.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilitaryApp.Data.Repositories.Interfaces
{
    public interface IMilitaryStructureRepository 
    {
        Task<List<MilitaryStructureItem>> GetMinMaxCountUnit(string param);
        Task<List<MilitaryStructureItem>> GetMilitaryStructure();
        Task<List<MilitaryStructureItem>> GetFilterMilitaryStructure(int? armyId = null, int? divisionId = null, int? corpsId = null);
        Task SetTriggersState(bool enable);
    }
}
