using MilitaryApp.Data.Repositories.Interfaces;
using MilitaryApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MilitaryApp.DTO;
using Microsoft.EntityFrameworkCore;
using MilitaryApp.Models;

namespace MilitaryApp.Data.Repositories
{
    public class EquipmentRepository : IEquipmentRepository
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
        public async Task<List<EquipmentItem>> GetEquipmentInfoAsync()
        {
            return await _context.Database.SqlQueryRaw<EquipmentItem>("CALL GetEquipmentInfo()").ToListAsync();
        }
      
        public async Task AddEquipment(string name, string type, int unitId, int quantity)
        {
            var equipment = new Combatequipment
            {
                Name = name,
                Type = type
            };
            await _combatEquipment.AddAsync(equipment);
            await ConnectionBetweenUnitEquipment(unitId, equipment.EquipmentId, quantity);
        }
        public async Task DeleteEquipment(int equipmentId)
        {
            var entity = await _combatEquipment.GetByIdAsync(equipmentId);
            await _combatEquipment.DeleteAsync(entity);
        }
        public async Task UpdateEquipment(int equipmentId, string newName, string newType, int newUnitId, int newQuantity)
        {
            var equipment = await _combatEquipment.GetByIdAsync(equipmentId);

            equipment.Name = newName;
            equipment.Type = newType;
            await _combatEquipment.UpdateAsync(equipment);
            await UpdateConnectionBetweenUnitEquipment(equipmentId, newUnitId, newQuantity);
        }
        private async Task UpdateConnectionBetweenUnitEquipment(int equipmentId, int newUnitId, int newQuantity)
        {
            var entity = await _context.Unitcombatequipments.FirstOrDefaultAsync(w => w.EquipmentId == equipmentId);
            await _unitcombatEquipment.DeleteAsync(entity);
            var newUnitCombatequipment = new Unitcombatequipment
            {
                EquipmentId = equipmentId,
                UnitId = newUnitId,
                Quantity = newQuantity
            };
            await _unitcombatEquipment.AddAsync(newUnitCombatequipment);
            //entity.UnitId = newUnitId;
            //entity.Quantity = newQuantity;
            //await _unitcombatEquipment.UpdateAsync(entity); 
        }
        private async Task ConnectionBetweenUnitEquipment(int unitId, int equipmentId, int quantity)
        {
            var unitCombatEquipment = new Unitcombatequipment
            {
                UnitId = unitId,
                EquipmentId = equipmentId,
                Quantity = quantity,
                
            };
            await _unitcombatEquipment.AddAsync(unitCombatEquipment);
        }
    }
}
