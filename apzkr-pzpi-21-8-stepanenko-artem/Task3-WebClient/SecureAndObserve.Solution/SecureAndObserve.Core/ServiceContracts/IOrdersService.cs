using SecureAndObserve.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureAndObserve.Core.ServiceContracts
{
    public interface IOrdersService
    {
        Task<OrderResponse> AddOrder(OrderAddRequest? orderAddRequest);
        Task<List<OrderResponse>> GetAllOrders();
        Task<OrderResponse?> GetOrderByOrderId(Guid? orderId);
        Task<int> CostOfService(Guid orderId);
        Task<MemoryStream> GetOrdersExcel();
        Task<bool> DeleteOrder(Guid? orderID);
    }
}
