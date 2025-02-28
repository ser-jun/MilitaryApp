using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MilitaryApp.Data.Repositories
{
    public class PersonnelRepository<T> : BaseCrudAbstract<T> where T : class
    {
        public PersonnelRepository(MilitaryDbContext context) : base(context)
        {
        }

    }
}
