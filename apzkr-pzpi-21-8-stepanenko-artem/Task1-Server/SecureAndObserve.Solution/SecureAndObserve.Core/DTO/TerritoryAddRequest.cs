using SecureAndObserve.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureAndObserve.Core.DTO
{
    public class TerritoryAddRequest
    {
        public Guid OwnerId { get; set; }
        [Required(ErrorMessage = "Name can't be blank")]
        public string? Name { get; set; }
        [Required(ErrorMessage = "Square of territory can't be blank")]
        public string? Square { get; set; }
        public string? Description { get; set; }
        [Required(ErrorMessage = "Type can't be blank")]
        public string? Type { get; set; }
        public Territory ToTerritory()
        {
            return new Territory()
            {
                OwnerId = OwnerId,
                Name = Name,
                Square = Square,
                Description = Description,
                Type = Type
            };
        }
    }
}
