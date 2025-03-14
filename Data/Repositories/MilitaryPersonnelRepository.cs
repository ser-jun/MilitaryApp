using Microsoft.EntityFrameworkCore;
using MilitaryApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MilitaryApp.DTO;

namespace MilitaryApp.Data.Repositories
{
    public class MilitaryPersonnelRepository<T> : BaseCrudAbstract<T> where T : class
    {
        //private MilitaryDbContext _context;
        //private readonly DbSet<Militarypersonnel> militarypersonnelsDbSet;
        public MilitaryPersonnelRepository (MilitaryDbContext context) :base(context)
        {
            //_context = context;
            //militarypersonnelsDbSet = _context.Militarypersonnel;
        }
        public async Task<List<MilitaryPersonnelItem>> GetMilitaryPersonnel()
        {
            return await _context.Database
                .SqlQueryRaw<MilitaryPersonnelItem>("CALL GetMilitaryPersonnelInfo()").ToListAsync();
        }
      
    }
}
