using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MilitaryApp.Models;
using MilitaryApp.DTO;

namespace MilitaryApp.Data.Repositories.Interfaces
{
    public interface IInfrastructureRepository
    {
        Task<List<InfrastructureItem>> LoadInfrastructureInfo();
        Task<List<InfrastructureItem>> GetBuildingsByUnitOrName(int? unitId, string? buildingName);
        Task AddInfrastructureItem(string name, int unitId, int yearBuild);
        Task DeleteInfrastructureItem(int idInfrastructure);
        Task UpdateInfrastructureItem(int selectedInfrastructureId, string newName, int newUnitId, int newYearBuild);
    }
}
