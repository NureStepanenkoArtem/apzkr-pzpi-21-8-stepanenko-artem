using SecureAndObserve.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureAndObserve.Core.Domain.RepositoryContracts
{
    public interface IEquipmentRepository
    {
        Task<Equipment> AddEquipment(Equipment equipment);
        Task<List<Equipment>> GetAllEquipment();
        Task<Equipment?> GetEquipmentByEquipmentId(Guid equipmentId);
        Task<bool> DeleteEquipmentByEquipmentId(Guid equipmentId);
    }
}
