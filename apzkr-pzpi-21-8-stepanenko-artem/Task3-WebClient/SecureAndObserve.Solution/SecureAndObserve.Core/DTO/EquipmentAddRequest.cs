using SecureAndObserve.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureAndObserve.Core.DTO
{
    public class EquipmentAddRequest
    {
        [Required(ErrorMessage ="Name can't be blank")]
        public string? Name { get; set; }
        [Required(ErrorMessage = "Type can't be blank")]
        public string? Type { get; set; }
        [Required(ErrorMessage = "Amount can't be blank")]
        public int Amount { get; set; } = 0;

        public Equipment ToEquipment()
        {
            return new Equipment()
            {
                Name = Name,
                Type = Type,
                Amount = Amount
            };
        }
    }
}
