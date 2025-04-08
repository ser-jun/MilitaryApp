using Microsoft.EntityFrameworkCore;
using MilitaryApp.Data.Repositories.Interfaces;
using MilitaryApp.DTO;
using MilitaryApp.Models;

namespace MilitaryApp.Data.Repositories
{
    public class MilitaryStructureRepository : IArmyRepository, IDivisionRepository, ICorpsRepository, IMilitaryUnitRepository, IMilitaryStructureRepository, ISubUnitRepository

    {
        private readonly MilitaryDbContext _context;
        private readonly BaseCrudAbstract<Army> _armyRepo;
        private readonly BaseCrudAbstract<Division> _divisionRepo;
        private readonly BaseCrudAbstract<Corps> _corpsRepo;
        private readonly BaseCrudAbstract<Militaryunit> _unitRepo;
        private readonly BaseCrudAbstract<Subunit> _subunitRepo;

        public MilitaryStructureRepository(MilitaryDbContext context)
        {
            _context = context;
            _armyRepo = new BaseCrudAbstract<Army>(_context);
            _divisionRepo = new BaseCrudAbstract<Division>(_context);
            _corpsRepo = new BaseCrudAbstract<Corps>(_context);
            _unitRepo = new BaseCrudAbstract<Militaryunit>(_context);
            _subunitRepo = new BaseCrudAbstract<Subunit>(_context);
 
        }

        public async Task<List<MilitaryStructureItem>> GetMilitaryStructure()
        {
            return await _context.Database
                .SqlQueryRaw<MilitaryStructureItem>("CALL GetMilitaryStructure()")
                .ToListAsync();
        }
        public async Task<List<MilitaryStructureItem>> GetFilterMilitaryStructure(int? armyId = null, int? divisionId = null, int? corpsId = null)
        {
            return await _context.Database
                .SqlQueryRaw<MilitaryStructureItem>(
                    "CALL GetMilitaryUnitsHierarchy({0}, {1}, {2})",
                    armyId.HasValue ? (object)armyId.Value : DBNull.Value,
                    divisionId.HasValue ? (object)divisionId.Value : DBNull.Value,
                    corpsId.HasValue ? (object)corpsId.Value : DBNull.Value)
                .ToListAsync();
        }
        public async Task<List<MilitaryStructureItem>> GetMinMaxCountUnit(string param)
        {
            return await _context.Database.SqlQueryRaw<MilitaryStructureItem>(
                "CALL GetMilitaryStructureWithUnitCount({0})", param).ToListAsync();
        }
        public async Task<List<Army>> GetArmy()
        {
             return (await _armyRepo.GetAllAsync()).ToList();
            //return await _context.Armies.Include(a => a.ArmyId).ToListAsync();
        }
        public async Task<List<Division>> GetDivision()
        {
            return (await _divisionRepo.GetAllAsync()).ToList();
        }
        public async Task<List<Corps>> GetCorps()
        {
            return (await _corpsRepo.GetAllAsync()).ToList();
        }

        #region DeleteMethods
        public async Task DeleteArmy(int armyId)
        {
            var armyToDelete = await _armyRepo.GetByIdAsync(armyId);
            if (armyToDelete != null)
                await _armyRepo.DeleteAsync(armyToDelete);
        }

        public async Task DeleteDivision(int divisionId)
        {
            var divisionToDelete = await _divisionRepo.GetByIdAsync(divisionId);
            if (divisionToDelete != null)
                await _divisionRepo.DeleteAsync(divisionToDelete);
        }

        public async Task DeleteCorps(int corpsId)
        {
            var corpsToDelete = await _corpsRepo.GetByIdAsync(corpsId);
            if (corpsToDelete != null)
                await _corpsRepo.DeleteAsync(corpsToDelete);
        }

        public async Task DeleteUnit(int unitId)
        {
            var unitToDelete = await _unitRepo.GetByIdAsync(unitId);
            if (unitToDelete != null)
                await _unitRepo.DeleteAsync(unitToDelete);
        }
        public async Task DeleteSubUnit(int subUnitId)
        {
            var subUnitToDelete = await _subunitRepo.GetByIdAsync(subUnitId);
            if (subUnitToDelete != null)
                await _subunitRepo.DeleteAsync(subUnitToDelete);
        }
        #endregion

        #region AddMethods
        public async Task AddArmy(string name)
        {
            var army = new Army { Name = name };
            await _armyRepo.AddAsync(army);
        }

        public async Task AddDivision(string name, int armyId)
        {
            var division = new Division { Name = name, ArmyId = armyId };
            await _divisionRepo.AddAsync(division);
        }

        public async Task AddCorps(string name, int divisionId)
        {
            var corps = new Corps { Name = name, DivisionId = divisionId };
            await _corpsRepo.AddAsync(corps);
        }

        public async Task AddMilitaryUnit(string name, int corpsId)
        {
            var unit = new Militaryunit
            { 
                Name = name,
                CorpsId = corpsId 
            };
            await _unitRepo.AddAsync(unit);
        }
        public async Task AddSubUnit(string name, int unitId)
        {
            var subUnit = new Subunit { SubunitName = name, UnitId = unitId };      
            await _subunitRepo.AddAsync(subUnit);
        }
        #endregion

        #region UpdateMethods
        public async Task UpdateArmy(int armyId, string newName)
        {
            var army = await _armyRepo.GetByIdAsync(armyId);
            if (army != null)
            {
                army.Name = newName;
                await _armyRepo.UpdateAsync(army);
            }
        }

        public async Task UpdateDivision(int divisionId, string newName, int newArmyId)
        {
            var division = await _divisionRepo.GetByIdAsync(divisionId);
            if (division != null)
            {
                division.Name = newName;
                division.ArmyId = newArmyId;
                await _divisionRepo.UpdateAsync(division);
            }
        }

        public async Task UpdateCorps(int corpsId, string newName, int newDivisionId)
        {
            var corps = await _corpsRepo.GetByIdAsync(corpsId);
            if (corps != null)
            {
                corps.Name = newName;
                corps.DivisionId = newDivisionId;
                await _corpsRepo.UpdateAsync(corps);
            }
        }

        public async Task UpdateMilitaryUnit(int unitId, string newName, int newCorpsId)
        {
            var unit = await _unitRepo.GetByIdAsync(unitId);
            if (unit != null)
            {
                unit.Name = newName;
                unit.CorpsId = newCorpsId;
                await _unitRepo.UpdateAsync(unit);
            }
        }
        public async Task UpdateSubUnit(int subUnitId, string newName, int newUnitId)
        {
            var subUnit = await _subunitRepo.GetByIdAsync(subUnitId);
            if (subUnit != null)
            {
                subUnit.SubunitName = newName;
                subUnit.UnitId = newUnitId;
                await _subunitRepo.UpdateAsync(subUnit);
            }
        }
        #endregion
        public async Task SetTriggersState(bool enable)
        {
            await _context.Database.ExecuteSqlRawAsync(
                "UPDATE AppSettings SET SettingValue = {0} WHERE SettingName = 'AutoCreateSubunits'",
                enable);
        }
    }
}
