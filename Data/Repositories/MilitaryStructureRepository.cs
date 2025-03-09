using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MilitaryApp.Data;
using MilitaryApp.Data.Repositories;
using MilitaryApp.Models;
using MilitaryApp.DTO;

namespace MilitaryApp.Data.Repositories
{
    public class MilitaryStructureRepository<T> : BaseCrudAbstract<T> where T : class
    {
       
        public MilitaryStructureRepository(MilitaryDbContext context) : base(context)
        { 
        }
       
        public async Task<List<MilitaryStructureItem>> GetMilitaryStructure()
        {
            return await _context.Database
             .SqlQueryRaw<MilitaryStructureItem>("CALL GetMilitaryStructure()")
             .ToListAsync();
        }
        #region DeleteMethods
        public async Task DeleteArmy(int armyId)
        {
            var armyToDelete = await GetByIdAsync(armyId);  
                await DeleteAsync(armyToDelete); 
        }

        public async Task DeleteDivision(int divisionId)
        {
            var divisionToDelete = await GetByIdAsync(divisionId);  
                await DeleteAsync(divisionToDelete);  
        }

        public async Task DeleteCorps(int corpsId)
        {
            var corpsToDelete = await GetByIdAsync(corpsId);  
                await DeleteAsync(corpsToDelete);  
        }

        public async Task DeleteUnit(int unitId)
        {
            var unitToDelete = await GetByIdAsync(unitId);  
                await DeleteAsync(unitToDelete);  
        }
        #endregion
        public async Task AddArmy(string name)
        {
            var army = new Army { Name = name };
            await AddAsync((T)(object)army); 
        }

        public async Task AddDivision(string name, int armyId)
        {
            var division = new Division { Name = name, ArmyId = armyId };
            await AddAsync((T)(object)division); 
        }

        public async Task AddCorps(string name, int divisionId)
        {
            var corps = new Corps { Name = name, DivisionId = divisionId };
            await AddAsync((T)(object)corps); 
        }

        public async Task AddMilitaryUnit(string name, int corpsId)
        {
            var unit = new Militaryunit { Name = name, CorpsId = corpsId };
            await AddAsync((T)(object)unit); 
        }

    }
}
