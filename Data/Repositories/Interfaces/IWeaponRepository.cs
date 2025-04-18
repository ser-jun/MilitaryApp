﻿using MilitaryApp.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilitaryApp.Data.Repositories.Interfaces
{
    public interface IWeaponRepository
    {
        Task<List<WeaponItem>> GetWeaponInfoAsync();
        Task<List<WeaponItem>> GetWeaponByTypeOrUnit(string? weaponType, int? unitId);
        Task<List<WeaponItem>> SearchWeapon(string nameWeapon);
        Task<List<WeaponItem>> FilterWeaponByQuatity(int quantity, int? unitId);
        Task AddWeapon(int unitId, string name, string type, int quantity);
        Task DeleteWeapon(int weaponId);
        Task UpdateWeapon(int weaponId, int newUnitId, string newName, string newType, int newQuantity);
    }
}
