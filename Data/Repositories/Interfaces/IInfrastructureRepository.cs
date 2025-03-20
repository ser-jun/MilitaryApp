using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MilitaryApp.Models;

namespace MilitaryApp.Data.Repositories.Interfaces
{
    public interface IInfrastructureRepository
    {
        Task<List<Infrastructure>> LoadInfrastructureInfo();
        Task AddInfrastructureItem(string name, int unitId, int yearBuild);
        Task DeleteInfrastructure(int idInfrastructure);
        Task UpdateInfrastructure(int selectedInfrastructureId, string newName, int newUnitId, int newYearBuild);
    }
}
