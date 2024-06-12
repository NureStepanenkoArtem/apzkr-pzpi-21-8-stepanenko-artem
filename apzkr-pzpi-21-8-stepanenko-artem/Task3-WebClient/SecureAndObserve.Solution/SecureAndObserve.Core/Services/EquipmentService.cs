using SecureAndObserve.Core.Domain.Entities;
using SecureAndObserve.Core.Domain.RepositoryContracts;
using SecureAndObserve.Core.DTO;
using SecureAndObserve.Core.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureAndObserve.Core.Services
{
    public class EquipmentService : IEquipmentService
    {
        private readonly IEquipmentRepository _equipmentRepository;

        public EquipmentService(IEquipmentRepository equipmentRepository)
        {
            _equipmentRepository = equipmentRepository;
        }
        public async Task<EquipmentResponse> AddEquipment(EquipmentAddRequest? equipmentAddRequest)
        {
            if (equipmentAddRequest == null)
                throw new ArgumentNullException(nameof(equipmentAddRequest));
            if (equipmentAddRequest.Name == null)
                throw new ArgumentException(nameof(equipmentAddRequest.Name));
            Equipment equipment = equipmentAddRequest.ToEquipment();
            equipment.Id = Guid.NewGuid();
            await _equipmentRepository.AddEquipment(equipment);
            return equipment.ToEquipmentResponse();
        }
        public async Task<List<EquipmentResponse>> GetAllEquipment()
        {
            List<Equipment> equipment = await _equipmentRepository.GetAllEquipment();
            return equipment.Select(equipment => equipment.ToEquipmentResponse()).ToList();
        }
        public async Task<EquipmentResponse?> GetEquipmentByEquipmentId(Guid? equipmentId)
        {
            if (equipmentId == null)
                return null;
            Equipment? equipment = await _equipmentRepository.GetEquipmentByEquipmentId(equipmentId.Value);
            if (equipment == null)
                return null;
            return equipment.ToEquipmentResponse();
        }
        public async Task<bool> DeleteEquipment(Guid? equipmentId)
        {
            if (equipmentId == null)
            {
                throw new ArgumentNullException(nameof(equipmentId));
            }
            Equipment? equipmentClaims = await _equipmentRepository.GetEquipmentByEquipmentId(equipmentId.Value);
            if (equipmentClaims == null)
            {
                return false;
            }
            await _equipmentRepository.DeleteEquipmentByEquipmentId(equipmentId.Value);
            return true;
        }
    }
}
