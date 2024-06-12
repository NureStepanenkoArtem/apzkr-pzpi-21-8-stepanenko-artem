using SecureAndObserve.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureAndObserve.Core.DTO
{
    public class OrderGuardsAddRequest
    {
        public Guid OrderId { get; set; }
        public Guid GuardExstensionsId { get; set; }

        public OrderGuards ToOrderGuards()
        {
            return new OrderGuards()
            {
                OrderId = OrderId,
                GuardExstensionsId = GuardExstensionsId,
            };
        }
    }
}
