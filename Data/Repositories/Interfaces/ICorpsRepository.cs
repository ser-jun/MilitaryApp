using MilitaryApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilitaryApp.Data.Repositories.Interfaces
{
    public interface ICorpsRepository
    {
        Task<List<Corps>> GetCorps();
        Task AddCorps(string name, int divisionId);
        Task UpdateCorps(int corpsId, string newName, int newDivisionId);
        Task DeleteCorps(int corpsId);
    }
}
