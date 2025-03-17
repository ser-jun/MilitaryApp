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
using System.Windows;

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
        private async Task DeleteOfficer(int idOfficer)
        {
            //var officer = await _officer.GetByIdAsync(idOfficer);

            var officer = await _context.Officers.FirstOrDefaultAsync(x => x.PersonnelId == idOfficer);
            await _officer.DeleteAsync(officer);
        }
        private async Task DeleteEnlistedPersonnel(int idEnlistedPersonnnel)
        {
            //var enlistedPersonnel = await _enlistedPersonnel.GetByIdAsync(idEnlistedPersonnnel);
            var enlistedPersonnel = await _context.Enlistedpersonnel.FirstOrDefaultAsync(x => x.PersonnelId == idEnlistedPersonnnel);
            await _enlistedPersonnel.DeleteAsync(enlistedPersonnel);
        }
        public async Task UpdateItem(int selectEntry, string newName, string newLastName, string newRank, string newPost, int newIdSpecialty, int newIdUnit)
        {
            var selectedPersonnel = await _personnelItem.GetByIdAsync(selectEntry);
            if (selectedPersonnel == null) return;

            int currentIndexRank = GetIndexFromEnum(selectedPersonnel.Rank);
            int newIndexRank = GetIndexFromEnum(newRank);

            await UpdatePersonnel(selectEntry, newName, newLastName, newRank, newIdUnit);

            if ((currentIndexRank <= 8 && newIndexRank > 8) || (currentIndexRank > 8 && newIndexRank <= 8))
            {
                if (currentIndexRank > 8)
                {
                    await DeleteOfficer(selectedPersonnel.PersonnelId);
                }
                else
                {
                    await DeleteEnlistedPersonnel(selectedPersonnel.PersonnelId);
                }

                if (newIndexRank > 8)
                {
                    await AddOfficer(selectedPersonnel.PersonnelId, newPost);// какие то траблы с добавлением именно тут и что-то в методоах update
                }
                else
                {
                    await AddEnlistedPersonnel(selectedPersonnel.PersonnelId, newPost);
                }
            }
            else
            {
                if (newIndexRank > 8)
                {
                    await UpdateOfficer(selectedPersonnel.PersonnelId, newPost);
                }
                else
                {
                    await UpdateEnlistedPersonnel(selectedPersonnel.PersonnelId, newPost);
                }
            }
            await ConnectionBetweenPersonnelSpecialty(selectedPersonnel.PersonnelId, newIdSpecialty);
        }

        private async Task UpdatePersonnel(int selectPerson, string newName, string newLastName, string newRank, int idNewUnit)

        {
            var personnel = await _personnelItem.GetByIdAsync(selectPerson);
            var newUnit = await _context.Militaryunits.FindAsync(idNewUnit);
            personnel.FirstName = newName;
            personnel.LastName = newLastName;
            personnel.Rank = newRank;
            personnel.Unit = newUnit;
            await _personnelItem.UpdateAsync(personnel);
        }
        private async Task UpdateOfficer(int selectedOfficer,  string newPosition)
        {
            var officer = await _officer.GetByIdAsync(selectedOfficer);
            officer.Position = newPosition;
            await _officer.UpdateAsync(officer);
        }
        private async Task UpdateEnlistedPersonnel(int selectedEnlistedPersonnel,  string newPosition)
        {
            var enlistedPersonnel = await _enlistedPersonnel.GetByIdAsync(selectedEnlistedPersonnel);
            enlistedPersonnel.Position = newPosition;
            await _enlistedPersonnel.UpdateAsync(enlistedPersonnel);
        }
    }
}
