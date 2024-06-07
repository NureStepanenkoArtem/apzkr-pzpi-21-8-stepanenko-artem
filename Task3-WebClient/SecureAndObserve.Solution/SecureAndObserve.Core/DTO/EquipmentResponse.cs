using SecureAndObserve.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureAndObserve.Core.DTO
{
    public class EquipmentResponse
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Type { get; set; }
        public int Amount { get; set; } = 0;
    }
    public static class EquipmentExtentions
    {
        public static EquipmentResponse ToEquipmentResponse(this Equipment equipment)
        {
            return new EquipmentResponse()
            {
                Id = equipment.Id,
                Name = equipment.Name,
                Type = equipment.Type,
                Amount = equipment.Amount,
            };
        }
    }
}
