using Microsoft.EntityFrameworkCore;
using MilitaryApp.Data.Repositories.Interfaces;
using MilitaryApp.DTO;
using MilitaryApp.Models;
using System.Windows;
using System.Windows.Documents;

namespace MilitaryApp.Data.Repositories
{
    public class MilitaryPersonnelRepository : IMilitaryPersonnelRepository, IOfficerRepository, IEnlistedPersonnel
    {
        private readonly MilitaryDbContext _context;
        private readonly BaseCrudAbstract<Militarypersonnel> _personnelItem;
        private readonly BaseCrudAbstract<Officer> _officer;
        private readonly BaseCrudAbstract<Enlistedpersonnel> _enlistedPersonnel;
        private readonly BaseCrudAbstract<PersonnelSpecialties> _personnelSpecialty;
        public MilitaryPersonnelRepository(MilitaryDbContext context)
        {
            _context = context;
            _personnelItem = new BaseCrudAbstract<Militarypersonnel>(_context);
            _officer = new BaseCrudAbstract<Officer>(_context);
            _enlistedPersonnel = new BaseCrudAbstract<Enlistedpersonnel>(_context);
            _personnelSpecialty = new BaseCrudAbstract<PersonnelSpecialties>(_context);
        }
        public async Task<List<MilitaryPersonnelItem>> GetMilitaryPersonnel()
        {
            return await _context.Database
            .SqlQueryRaw<MilitaryPersonnelItem>("CALL GetMilitaryPersonnelInfo()")
            .ToListAsync();
        }
        public async Task AddPersonnel(string name, string lastName, string rank, string post, int idSpeciality, int idUnit)
        {
            var unit = await _context.Militaryunits.FindAsync(idUnit);
            var personnelItem = new Militarypersonnel
            {
                FirstName = name,
                LastName = lastName,
                Rank = rank,
                Unit = unit
            };
            await _personnelItem.AddAsync(personnelItem);

            if (GetIndexFromEnum(rank) > 8)
            {
                await AddOfficer(personnelItem.PersonnelId, post, unit);
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

        public async Task AddOfficer(int personnelId, string post, Militaryunit unit)
        {
            

            var officer = new Officer
            {
                PersonnelId = personnelId,
                Position = post

            };
            await _officer.AddAsync(officer);

            if (officer.Position.ToLower().Trim() == "командир части")
            {
                unit.CommanderId = officer.OfficerId;
                await _context.SaveChangesAsync();
            }


        }
        public async Task AddEnlistedPersonnel(int personnelId, string post)
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
            var existingConnections = _context.PersonnelSpecialties
                .Where(ps => ps.PersonnelId == personnelId)
                .ToList();

            if (existingConnections.Any())
            {
                _context.PersonnelSpecialties.RemoveRange(existingConnections);
                await _context.SaveChangesAsync();
            }

            var connectionPersSpecialty = new PersonnelSpecialties
            {
                PersonnelId = personnelId,
                SpecialtyId = specialtyId
            };

            await _personnelSpecialty.AddAsync(connectionPersSpecialty);
        }

        public async Task DeletePersonnel(int personnelId)
        {
            var personnel = await _personnelItem.GetByIdAsync(personnelId);
            await _personnelItem.DeleteAsync(personnel);
        }
        public async Task DeleteOfficer(int idOfficer)
        {
            var officer = await _context.Officers.FirstOrDefaultAsync(x => x.PersonnelId == idOfficer);
            await _officer.DeleteAsync(officer);
        }
        public async Task DeleteEnlistedPersonnel(int idEnlistedPersonnnel)
        {

            var enlistedPersonnel = await _context.Enlistedpersonnel.FirstOrDefaultAsync(x => x.PersonnelId == idEnlistedPersonnnel);
            await _enlistedPersonnel.DeleteAsync(enlistedPersonnel);
        }
        public async Task UpdateItem(int selectEntry, string newName, string newLastName, string newRank, string newPost, int newIdSpecialty, int newIdUnit)
        {
            var selectedPersonnel = await _personnelItem.GetByIdAsync(selectEntry);
            var unit = await _context.Militaryunits.FindAsync(newIdUnit);

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
                    //await AddOfficer(selectedPersonnel.PersonnelId, newPost, unit);
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
        public async Task UpdateOfficer(int selectedOfficer, string newPosition)
        {
            var officer = await _context.Officers.FirstOrDefaultAsync(x => x.PersonnelId == selectedOfficer);
            officer.Position = newPosition;
            await _officer.UpdateAsync(officer);
        }
        public async Task UpdateEnlistedPersonnel(int selectedEnlistedPersonnel, string newPosition)
        {
            var enlistedPersonnel = await _context.Enlistedpersonnel.FirstOrDefaultAsync(x => x.PersonnelId == selectedEnlistedPersonnel);
            enlistedPersonnel.Position = newPosition;
            await _enlistedPersonnel.UpdateAsync(enlistedPersonnel);
        }
    }
}
