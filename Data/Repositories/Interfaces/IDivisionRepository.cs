using MilitaryApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilitaryApp.Data.Repositories.Interfaces
{
    public interface IDivisionRepository
    {
        Task<List<Division>> GetDivision();
        Task AddDivision(string name, int armyId);
        Task UpdateDivision(int divisionId, string newName, int newArmyId);
        Task DeleteDivision(int divisionId);
    }
}
