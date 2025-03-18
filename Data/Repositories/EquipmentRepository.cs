using MilitaryApp.Data.Repositories.Interfaces;
using MilitaryApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MilitaryApp.DTO;
using Microsoft.EntityFrameworkCore;

namespace MilitaryApp.Data.Repositories
{
    public class EquipmentRepository
    {
        private readonly MilitaryDbContext _context;

        private readonly BaseCrudAbstract<Combatequipment> _combatEquipment;
        private readonly BaseCrudAbstract<Unitcombatequipment> _unitcombatEquipment;
        
        public EquipmentRepository(MilitaryDbContext context)
        {
            _context = context;
            _combatEquipment = new BaseCrudAbstract<Combatequipment>(context);
            _unitcombatEquipment = new BaseCrudAbstract<Unitcombatequipment>(context);
        }
        public async Task<List<EquipmentItem>> GetEquipmentInfo()
        {
            return await _context.Database.SqlQueryRaw<EquipmentItem>("CALL GetEquipmentInfo()").ToListAsync();
        }
    }
}
