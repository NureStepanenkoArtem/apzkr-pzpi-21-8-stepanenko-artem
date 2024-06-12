using SecureAndObserve.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureAndObserve.Core.DTO
{
    public class EquipmentClaimsResponse
    {
        public Guid Id { get; set; }
        public Guid GuardExstensionsId { get; set; }
        public Guid EquipmentId { get; set; }
    }

    public static class EquipmentClaimsExtentions
    {
        public static EquipmentClaimsResponse ToEquipmentClaimsResponse(this EquipmentClaims equipmentClaims)
        {
            return new EquipmentClaimsResponse()
            {
                Id = equipmentClaims.Id,
                GuardExstensionsId = equipmentClaims.GuardExstensionsId,
                EquipmentId = equipmentClaims.EquipmentId,
            };
        }
    }
}
