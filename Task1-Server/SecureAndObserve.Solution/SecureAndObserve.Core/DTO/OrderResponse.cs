using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SecureAndObserve.Core.Domain.Entities;

namespace SecureAndObserve.Core.DTO
{
    public class OrderResponse
    {
        public Guid Id { get; set; }
        public Guid OwnerId { get; set; }
        public string? TypeOfService { get; set; }
        public string? SecurityLevel { get; set; }
    }
    public static class OrderExtensions
    {
        public static OrderResponse ToOrderResponse(this Order order)
        {
            return new OrderResponse
            {
                Id = order.Id,
                OwnerId = order.OwnerId,
                TypeOfService = order.TypeOfService,
                SecurityLevel = order.SecurityLevel,
            };
        }
    }
}
