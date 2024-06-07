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
    public class OrderGuardsResponse
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage ="Order can't be blank")]
        public Guid OrderId { get; set; }
        [Required(ErrorMessage = "Guard can't be blank")]
        public Guid GuardExstensionsId { get; set; }
    }
    public static class OrderGuardsExtensions
    {
        public static OrderGuardsResponse ToOrderGuardsResponse(this OrderGuards orderGuards)
        {
            return new OrderGuardsResponse
            {
                Id = orderGuards.Id,
                OrderId = orderGuards.OrderId,
                GuardExstensionsId = orderGuards.GuardExstensionsId
            };
        }
    }
}
