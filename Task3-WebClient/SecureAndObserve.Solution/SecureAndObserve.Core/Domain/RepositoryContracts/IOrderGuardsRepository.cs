using SecureAndObserve.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureAndObserve.Core.Domain.RepositoryContracts
{
    public interface IOrderGuardsRepository
    {
        Task<OrderGuards> AddOrderGuards(OrderGuards orderGuards);
        Task<List<OrderGuards>> GetAllOrdersGuards();
        Task<OrderGuards?> GetOrderGuardsByOrderGuardsId(Guid orderGuardsId);
        Task<bool> DeleteOrderGuardsByOrderGuardsID(Guid orderGuardsID);
    }
}
