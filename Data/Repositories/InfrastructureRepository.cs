using MilitaryApp.Data.Repositories.Interfaces;
using MilitaryApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilitaryApp.Data.Repositories
{
    public class InfrastructureRepository
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
           var infrastructureList =  await _infrastructureCrud.GetAllAsync();
            return infrastructureList.ToList();
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
        public async Task DeleteInfrastructure(int idInfrastructure)
        {
           var infrastructure =  await _infrastructureCrud.GetByIdAsync(idInfrastructure);
            await _infrastructureCrud.DeleteAsync(infrastructure);
        }
        public async Task UpdateInfrastructure(int selectedInfrastructureId,string newName, int newUnitId, int newYearBuild)
        {
            var infrastructure = await _infrastructureCrud.GetByIdAsync(selectedInfrastructureId);
            infrastructure.Name = newName;
            infrastructure.UnitId = newUnitId;
            infrastructure.YearBuilt = newYearBuild;
            await _infrastructureCrud.UpdateAsync(infrastructure);
        }
    }
}
