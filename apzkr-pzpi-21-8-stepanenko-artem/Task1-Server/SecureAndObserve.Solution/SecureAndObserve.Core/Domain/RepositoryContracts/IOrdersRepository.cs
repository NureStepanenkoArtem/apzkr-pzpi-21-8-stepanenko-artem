using SecureAndObserve.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureAndObserve.Core.Domain.RepositoryContracts
{
    public interface IOrdersRepository
    {
        Task<Order> AddOrder(Order order);
        Task<List<Order>> GetAllOrders();
        Task<Order?> GetOrderByOrderId(Guid orderId);
        Task<int> CostOfService(Guid orderId);
        Task<bool> DeleteOrderByOrderID(Guid orderID);
    }
}
