﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MilitaryApp.Data;
using MilitaryApp.Data.Repositories;
using MilitaryApp.Models;
using MilitaryApp.DTO;

namespace MilitaryApp.Data.Repositories
{
    public class MilitaryStructureRepository<T> : BaseCrudAbstract<T> where T : class
    {
       
        public MilitaryStructureRepository(MilitaryDbContext context) : base(context)
        { 
        }
       
        public async Task<List<MilitaryStructureItem>> GetMilitaryStructure()
        {
            return await _context.Database
             .SqlQueryRaw<MilitaryStructureItem>("CALL GetMilitaryStructure()")
             .ToListAsync();
        }


    }
}
