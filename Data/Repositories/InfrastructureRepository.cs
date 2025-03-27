using MilitaryApp.Data.Repositories.Interfaces;
using MilitaryApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore; 

namespace MilitaryApp.Data.Repositories
{
    public class InfrastructureRepository : IInfrastructureRepository
    {
        private readonly MilitaryDbContext _context;
        private readonly BaseCrudAbstract<Infrastructure> _infrastructureCrud;
        public InfrastructureRepository(MilitaryDbContext context) 
        {
        _context = context;
            _infrastructureCrud = new BaseCrudAbstract<Infrastructure>(context);
        }

        public async Task<List<Infrastructure>> LoadInfrastructureInfo()
        {
            return await _context.Infrastructures
                .Include(i => i.Unit) 
                .ToListAsync();
        }
        public async Task<List<Infrastructure>> GetBuildingsByUnitOrName(int? unitId, string? buildingName)
        {
            return await _context.Database.SqlQueryRaw<Infrastructure>("CALL GetBuildingsByUnitAndName({0},{1})",
                unitId ?? (object)DBNull.Value,
                buildingName ?? (object)DBNull.Value).ToListAsync();
        }
        public async Task AddInfrastructureItem(string name, int unitId, int yearBuild)
        {
            var infrastructure = new Infrastructure
            {
                Name = name,
                UnitId = unitId,
                YearBuilt = yearBuild
            };
            await _infrastructureCrud.AddAsync(infrastructure);
        }
        public async Task DeleteInfrastructureItem(int idInfrastructure)
        {
           var infrastructure =  await _infrastructureCrud.GetByIdAsync(idInfrastructure);
            await _infrastructureCrud.DeleteAsync(infrastructure);
        }
        public async Task UpdateInfrastructureItem(int selectedInfrastructureId,string newName, int newUnitId, int newYearBuild)
        {
            var infrastructure = await _infrastructureCrud.GetByIdAsync(selectedInfrastructureId);
            infrastructure.Name = newName;
            infrastructure.UnitId = newUnitId;
            infrastructure.YearBuilt = newYearBuild;
            await _infrastructureCrud.UpdateAsync(infrastructure);
        }
    }
}
