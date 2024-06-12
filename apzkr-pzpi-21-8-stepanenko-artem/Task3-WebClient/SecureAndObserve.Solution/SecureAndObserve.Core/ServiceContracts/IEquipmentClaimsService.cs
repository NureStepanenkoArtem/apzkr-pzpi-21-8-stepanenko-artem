using SecureAndObserve.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureAndObserve.Core.ServiceContracts
{
    public interface IEquipmentClaimsService
    {
        Task<EquipmentClaimsResponse> AddEquipmentClaims(EquipmentClaimsAddRequest? equipmentClaimsAddRequest);
        Task<List<EquipmentClaimsResponse>> GetAllEquipmentClaims();
        Task<EquipmentClaimsResponse?> GetEquipmentClaimsByEquipmentClaimsId(Guid? equipmentClaimsId);
        Task<bool> DeleteEquipmentClaims(Guid? equipmentClaimsId);
    }
}
