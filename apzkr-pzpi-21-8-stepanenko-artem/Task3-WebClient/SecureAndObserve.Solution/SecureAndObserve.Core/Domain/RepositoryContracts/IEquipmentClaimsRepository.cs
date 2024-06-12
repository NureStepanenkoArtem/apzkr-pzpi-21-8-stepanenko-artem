using SecureAndObserve.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureAndObserve.Core.Domain.RepositoryContracts
{
    public interface IEquipmentClaimsRepository
    {
        Task<EquipmentClaims> AddEquipmentClaims(EquipmentClaims equipmentClaims);
        Task<List<EquipmentClaims>> GetAllEquipmentClaims();
        Task<EquipmentClaims?> GetEquipmentClaimsByEquipmentClaimsId(Guid equipmentClaimsId);
        Task<bool> DeleteEquipmentClaimsByEquipmentClaimsId(Guid equipmentClaimsId);
    }
}
