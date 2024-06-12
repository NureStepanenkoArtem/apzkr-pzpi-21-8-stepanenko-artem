using SecureAndObserve.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureAndObserve.Core.ServiceContracts
{
    public interface IOrderGuardsService
    {
        Task<OrderGuardsResponse> AddOrderGuards(OrderGuardsAddRequest? orderGuardsAddRequest);
        Task<List<OrderGuardsResponse>> GetAllOrderGuards();
        Task<OrderGuardsResponse?> GetOrderGuardsByOrderGuardsId(Guid? orderGuardsId);
        Task<bool> DeleteOrderGuards(Guid? orderGuardsID);
    }
}
