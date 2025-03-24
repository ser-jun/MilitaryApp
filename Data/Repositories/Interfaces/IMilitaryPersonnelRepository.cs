using MilitaryApp.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MilitaryApp.Models;

namespace MilitaryApp.Data.Repositories.Interfaces
{
    public interface IMilitaryPersonnelRepository
    {
        Task<List<MilitaryPersonnelItem>> GetMilitaryPersonnel();
        Task<List<MilitaryPersonnelItem>> SearchMilitaryPersonnel(string? rank, int? unitId);
        Task<List<MilitaryPersonnelItem>> SearchPersonnelBySpecialty(int speacialtyId, int? armyId, int? divisionId,
            int? corpsId, int? unitId);
        Task AddPersonnel(string name, string lastName, string rank, string post, int idSpeciality, int idUnit);
        Task DeletePersonnel(int personnelId);
        Task UpdateItem(int selectEntry, string newName, string newLastName, string newRank, string newPost, int newIdSpecialty, int newIdUnit);
    }
}
