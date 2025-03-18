using MilitaryApp.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilitaryApp.Data.Repositories.Interfaces
{
    public interface IEquipmentRepository
    {
        Task<List<EquipmentItem>> GetEquipmentInfoAsync();
        Task AddEquipment(string name, string type, int unitId, int quantity);
        Task DeleteEquipment(int equipmentId);
        Task UpdateEquipment(int equipmentId, string newName, string newType, int newUnitId, int newQuantity);
    }
}
