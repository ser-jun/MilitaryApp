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
    }
}
