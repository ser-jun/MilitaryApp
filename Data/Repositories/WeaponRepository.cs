using Microsoft.EntityFrameworkCore;
using MilitaryApp.Data.Repositories.Interfaces;
using MilitaryApp.DTO;
using MilitaryApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilitaryApp.Data.Repositories
{
    public class WeaponRepository : IWeaponRepository
    {
        private readonly MilitaryDbContext _context;
        private readonly BaseCrudAbstract<Weapon> _crudWeapon;
        private readonly BaseCrudAbstract<Unitweapon> _crudUnitWeapon;
        public WeaponRepository(MilitaryDbContext context) 
        { 
            _context = context;
            _crudWeapon = new BaseCrudAbstract<Weapon>(context);
            _crudUnitWeapon = new BaseCrudAbstract<Unitweapon>(context);
        }
        public async Task<List<WeaponItem>> GetWeaponInfoAsync()
        {
            return await _context.Database.SqlQueryRaw<WeaponItem>("CALL GetWeaponInfo()").ToListAsync();
        }
        public async Task AddWeapon(int unitId, string name, string type, int quantity)
        {
            var weapon = new Weapon
            {
                Name = name,
                Type = type
            };
            await _crudWeapon.AddAsync(weapon);
            await AddConnectionBetweenUnitWeapon(unitId, weapon.WeaponId, quantity);
        }
        private async Task AddConnectionBetweenUnitWeapon(int unitId, int weaponId, int quantity)
        {
            var unitWeapon = new Unitweapon
            {
                UnitId = unitId,
                WeaponId = weaponId,
                Quantity = quantity
            };
            await _crudUnitWeapon.AddAsync(unitWeapon);
        }
        public async Task DeleteWeapon(int weaponId)
        {
            var weapon = await _crudWeapon.GetByIdAsync(weaponId);
            await _crudWeapon.DeleteAsync(weapon);
        } 
    }
}
