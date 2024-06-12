using SecureAndObserve.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureAndObserve.Core.ServiceContracts
{
    public interface IEquipmentService
    {
        Task<EquipmentResponse> AddEquipment(EquipmentAddRequest? equipmentAddRequest);
        Task<List<EquipmentResponse>> GetAllEquipment();
        Task<EquipmentResponse?> GetEquipmentByEquipmentId(Guid? equipmentId);
        Task<bool> DeleteEquipment(Guid? equipmentId);
    }
}
