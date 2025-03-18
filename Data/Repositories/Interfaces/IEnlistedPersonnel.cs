using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilitaryApp.Data.Repositories.Interfaces
{
    public interface IEnlistedPersonnel
    {
        Task AddEnlistedPersonnel(int personnelId, string post);
        Task DeleteEnlistedPersonnel(int idEnlistedPersonnnel);
        Task UpdateEnlistedPersonnel(int selectedEnlistedPersonnel, string newPosition);

    }
}
