using SecureAndObserve.Core.Domain.Entities;
using SecureAndObserve.Core.Domain.RepositoryContracts;
using SecureAndObserve.Core.DTO;
using SecureAndObserve.Core.ServiceContracts;

namespace SecureAndObserve.Core.Services
{
    public class EquipmentClaimsService : IEquipmentClaimsService
    {
        private readonly IEquipmentClaimsRepository _equipmentClaimsRepository;

        public EquipmentClaimsService(IEquipmentClaimsRepository equipmentClaimsRepository)
        {
            _equipmentClaimsRepository = equipmentClaimsRepository;
        }
        public async Task<EquipmentClaimsResponse> AddEquipmentClaims(EquipmentClaimsAddRequest? equipmentClaimsAddRequest)
        {
            if (equipmentClaimsAddRequest == null)
                throw new ArgumentNullException(nameof(equipmentClaimsAddRequest));
            if (equipmentClaimsAddRequest.GuardExstensionsId == null)
                throw new ArgumentException(nameof(equipmentClaimsAddRequest.GuardExstensionsId));
            EquipmentClaims equipmentClaims = equipmentClaimsAddRequest.ToEquipmentClaims();
            equipmentClaims.Id = Guid.NewGuid();
            await _equipmentClaimsRepository.AddEquipmentClaims(equipmentClaims);
            return equipmentClaims.ToEquipmentClaimsResponse();
        }
        public async Task<List<EquipmentClaimsResponse>> GetAllEquipmentClaims()
        {
            List<EquipmentClaims> equipmentClaims = await _equipmentClaimsRepository.GetAllEquipmentClaims();
            return equipmentClaims.Select(equipmentClaims => equipmentClaims.ToEquipmentClaimsResponse()).ToList();
        }
        public async Task<EquipmentClaimsResponse?> GetEquipmentClaimsByEquipmentClaimsId(Guid? equipmentClaimsId)
        {
            if (equipmentClaimsId == null)
                return null;
            EquipmentClaims? equipmentClaims = await _equipmentClaimsRepository.GetEquipmentClaimsByEquipmentClaimsId(equipmentClaimsId.Value);
            if (equipmentClaims == null)
                return null;
            return equipmentClaims.ToEquipmentClaimsResponse();
        }
        public async Task<bool> DeleteEquipmentClaims(Guid? equipmentClaimsId)
        {
            if (equipmentClaimsId == null)
            {
                throw new ArgumentNullException(nameof(equipmentClaimsId));
            }
            EquipmentClaims? equipmentClaims = await _equipmentClaimsRepository.GetEquipmentClaimsByEquipmentClaimsId(equipmentClaimsId.Value);
            if (equipmentClaims == null)
            {
                return false;
            }
            await _equipmentClaimsRepository.DeleteEquipmentClaimsByEquipmentClaimsId(equipmentClaimsId.Value);
            return true;
        }
    }
}
