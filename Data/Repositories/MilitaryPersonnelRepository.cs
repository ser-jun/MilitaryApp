using Microsoft.EntityFrameworkCore;
using MilitaryApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MilitaryApp.DTO;
using MilitaryApp.Data.Repositories.Interfaces;

namespace MilitaryApp.Data.Repositories
{
    public class MilitaryPersonnelRepository : IMilitaryPersonnelRepository
    {
        private readonly MilitaryDbContext _context;
        private readonly BaseCrudAbstract<MilitaryPersonnelItem> _personnelItem;
        public MilitaryPersonnelRepository (MilitaryDbContext context) 
        {
            _context = context;
            _personnelItem = new BaseCrudAbstract<MilitaryPersonnelItem>(_context);
        }
        public async Task<List<MilitaryPersonnelItem>> GetMilitaryPersonnel()
        {
            return await _context.Database
                .SqlQueryRaw<MilitaryPersonnelItem>("CALL GetMilitaryPersonnelInfo()").ToListAsync();
        }
        public async Task AddPersonnel(string name, string lastName, int rankId, string post, int idSpeciality, int idUnit)
        {
            var personnelItem = new MilitaryPersonnelItem
            {
                FirstName = name,
                LastName = lastName,
                Rank = rankId.ToString(),
                Position = post,
                Specialties = idSpeciality.ToString(),
                Unit = idUnit.ToString()
            };
           await _personnelItem.AddAsync(personnelItem);
        }
    }
}
