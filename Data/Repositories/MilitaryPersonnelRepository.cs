using Microsoft.EntityFrameworkCore;
using MilitaryApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MilitaryApp.DTO;
using MilitaryApp.Data.Repositories.Interfaces;
using MilitaryApp.Models;

namespace MilitaryApp.Data.Repositories
{
    public class MilitaryPersonnelRepository : IMilitaryPersonnelRepository
    {
        private readonly MilitaryDbContext _context;
        private readonly BaseCrudAbstract<Militarypersonnel> _personnelItem;
        private readonly BaseCrudAbstract<Officer> _officer;
        private readonly BaseCrudAbstract<Enlistedpersonnel> _enlistedPersonnel;
        private readonly BaseCrudAbstract<PersonnelSpecialties> _personnelSpecialty;
        public MilitaryPersonnelRepository (MilitaryDbContext context) 
        {
            _context = context;
            _personnelItem = new BaseCrudAbstract<Militarypersonnel>(_context);
            _officer = new BaseCrudAbstract<Officer>(_context);
            _enlistedPersonnel = new BaseCrudAbstract<Enlistedpersonnel> (_context);
            _personnelSpecialty = new BaseCrudAbstract<PersonnelSpecialties> (_context);
        }
        public async Task<List<MilitaryPersonnelItem>> GetMilitaryPersonnel()
        {
            return await _context.Database
            .SqlQueryRaw<MilitaryPersonnelItem>("CALL GetMilitaryPersonnelInfo()")
            .ToListAsync();
        }
        public async Task AddPersonnel(string name, string lastName, string rank, string post, int idSpeciality, int idUnit)
        {       
            var unit = await  _context.Militaryunits.FindAsync(idUnit);
            var personnelItem = new Militarypersonnel
            {
                FirstName = name,
                LastName = lastName,
                Rank = rank,
                Unit = unit
            };
           await _personnelItem.AddAsync(personnelItem);

            if (GetIndexFromEnum(rank)> 8)
            {
             await AddOfficer(personnelItem.PersonnelId, post);
            }
            else
            {
              await AddEnlistedPersonnel(personnelItem.PersonnelId, post);
            }
            await ConnectionBetweenPersonnelSpecialty(personnelItem.PersonnelId, idSpeciality);
        }
        private int GetIndexFromEnum(string rank)
        {
            MilitaryRank rankToIndex = (MilitaryRank)Enum.Parse(typeof(MilitaryRank), rank);
            return (int)rankToIndex;
        }
        private async Task AddOfficer(int personnelId, string post)
        {
            var officer = new Officer
            {
                PersonnelId = personnelId,
                Position = post
            };
            await _officer.AddAsync(officer);
        }
        private async Task AddEnlistedPersonnel (int personnelId, string post)
        {
            var enlistedPersonnel = new Enlistedpersonnel
            {
                PersonnelId = personnelId,
                Position = post
            };
            await _enlistedPersonnel.AddAsync(enlistedPersonnel);
        }
        private async Task ConnectionBetweenPersonnelSpecialty(int personnelId, int specialtyId)
        {
            var connectionPersSpetialty = new PersonnelSpecialties
            {
                PersonnelId = personnelId,
                SpecialtyId = specialtyId
            };
            await _personnelSpecialty.AddAsync(connectionPersSpetialty);
        }
        public async Task DeletePersonnel(int personnelId)
        {
            var personnel = await _personnelItem.GetByIdAsync(personnelId);
            await _personnelItem.DeleteAsync(personnel);
        }
        public async Task UpdatePersonnel()
        {

        }
    }
}
