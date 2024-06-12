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
    public class TerritoryResponse
    {
        public Guid Id { get; set; }
        public Guid OwnerId { get; set; }
        public string? Name { get; set; }
        public string? Square { get; set; }
        public string? Description { get; set; }
        public string? Type { get; set; }
    }
    public static class TerritoryExtensions
    {
        public static TerritoryResponse ToTerritoryResponse(this Territory territory)
        {
            return new TerritoryResponse()
            {
                Id = territory.Id,
                OwnerId = territory.OwnerId,
                Name = territory.Name,
                Square = territory.Square,
                Description = territory.Description,
                Type = territory.Type,
            };
        }
    }
}
