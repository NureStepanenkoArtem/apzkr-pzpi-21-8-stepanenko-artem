using SecureAndObserve.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureAndObserve.Core.DTO
{
    public class EquipmentClaimsAddRequest
    {
        [Required(ErrorMessage = "Guard can't be null")]
        public Guid GuardExstensionsId { get; set; }
        [Required(ErrorMessage = "Equipment can't be null")]
        public Guid EquipmentId { get; set; }
        public EquipmentClaims ToEquipmentClaims()
        {
            return new EquipmentClaims()
            {
                GuardExstensionsId = GuardExstensionsId,
                EquipmentId = EquipmentId
            };
        }
    }
}
