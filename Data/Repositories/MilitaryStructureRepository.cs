using Microsoft.EntityFrameworkCore;
using MilitaryApp.DTO;
using MilitaryApp.Models;

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
        #region AddMethods
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
        #endregion
        #region UpdateMethods
        public async Task UpdateArmy(int armyId, string newName)
        {
            var army = await GetByIdAsync(armyId);
            ((Army)(object)army).Name = newName;
            await UpdateAsync(army);
        }
        public async Task UpdateDivision(int divisionId, string newName, int newArmyId)
        {
            var division = await GetByIdAsync(divisionId);
             ((Division)(object)division).Name = newName;
            ((Division)(object)division).ArmyId = newArmyId;
            await UpdateAsync(division);
        }
        public async Task UpdateCorps(int corpsId, string newName, int newDivisionId)
        {
            var corps = await GetByIdAsync(corpsId);
            ((Corps)(object)corps).Name = newName;
            ((Corps)(object)corps).DivisionId = newDivisionId;
            await UpdateAsync(corps);   
        }
        public async Task UpdateMilitaryUnit(int unitId, string newName, int newCorpsId)
        {
            var militaryUnit = await GetByIdAsync(unitId);
            ((Militaryunit)(object)militaryUnit).Name = newName;
            ((Militaryunit)(object)militaryUnit).CorpsId = newCorpsId;
            await UpdateAsync(militaryUnit);
        }
        #endregion

    }
}
