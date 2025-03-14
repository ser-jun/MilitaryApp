    using Microsoft.EntityFrameworkCore;
    using MilitaryApp.Data.Repositories.Interfaces;
    using MilitaryApp.DTO;
    using MilitaryApp.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    namespace MilitaryApp.Data.Repositories
    {
        public class MilitaryStructureRepository : IArmyRepository, IDivisionRepository, ICorpsRepository, IMilitaryUnitRepository, IMilitaryStructureRepository
        
        {
            private readonly MilitaryDbContext _context;
            private readonly BaseCrudAbstract<Army> _armyRepo;
            private readonly BaseCrudAbstract<Division> _divisionRepo;
            private readonly BaseCrudAbstract<Corps> _corpsRepo;
            private readonly BaseCrudAbstract<Militaryunit> _unitRepo;

            public MilitaryStructureRepository(MilitaryDbContext context)
            {
                _context = context;
                _armyRepo = new BaseCrudAbstract<Army>(_context);
                _divisionRepo = new BaseCrudAbstract<Division>(_context);
                _corpsRepo = new BaseCrudAbstract<Corps>(_context);
                _unitRepo = new BaseCrudAbstract<Militaryunit>(_context);
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
                var unit = new Militaryunit { Name = name, CorpsId = corpsId };
                await _unitRepo.AddAsync(unit);
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
            #endregion
        }
    }
