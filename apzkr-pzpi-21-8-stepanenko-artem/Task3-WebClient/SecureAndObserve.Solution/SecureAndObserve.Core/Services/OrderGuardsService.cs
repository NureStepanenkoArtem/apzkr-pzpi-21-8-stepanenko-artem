using OfficeOpenXml;
using SecureAndObserve.Core.Domain.Entities;
using SecureAndObserve.Core.Domain.RepositoryContracts;
using SecureAndObserve.Core.DTO;
using SecureAndObserve.Core.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureAndObserve.Core.Services
{
    public class OrderGuardsService : IOrderGuardsService
    {
        private readonly IOrderGuardsRepository _orderGuardsRepository;

        public OrderGuardsService(IOrderGuardsRepository orderGuardsRepository)
        {
            _orderGuardsRepository = orderGuardsRepository;
        }
        public async Task<OrderGuardsResponse> AddOrderGuards(OrderGuardsAddRequest? orderGuardsAddRequest)
        {
            if (orderGuardsAddRequest == null)
                throw new ArgumentNullException(nameof(orderGuardsAddRequest));
            if (orderGuardsAddRequest.GuardExstensionsId == null)
                throw new ArgumentException(nameof(orderGuardsAddRequest.GuardExstensionsId));
            OrderGuards orderGuards = orderGuardsAddRequest.ToOrderGuards();
            orderGuards.Id = Guid.NewGuid();
            await _orderGuardsRepository.AddOrderGuards(orderGuards);
            return orderGuards.ToOrderGuardsResponse();
        }
        public async Task<List<OrderGuardsResponse>> GetAllOrderGuards()
        {
            List<OrderGuards> orderGuards = await _orderGuardsRepository.GetAllOrdersGuards();
            return orderGuards.Select(order => order.ToOrderGuardsResponse()).ToList();
        }
        public async Task<OrderGuardsResponse?> GetOrderGuardsByOrderGuardsId(Guid? orderGuardsId)
        {
            if (orderGuardsId == null)
                return null;
            OrderGuards? orderGuards = await _orderGuardsRepository.GetOrderGuardsByOrderGuardsId(orderGuardsId.Value);
            if (orderGuards == null)
                return null;
            return orderGuards.ToOrderGuardsResponse();
        }

        public async Task<bool> DeleteOrderGuards(Guid? orderGuardsID)
        {
            if (orderGuardsID == null)
            {
                throw new ArgumentNullException(nameof(orderGuardsID));
            }
            OrderGuards? orderGuards = await _orderGuardsRepository.GetOrderGuardsByOrderGuardsId(orderGuardsID.Value);
            if (orderGuards == null)
            {
                return false;
            }
            await _orderGuardsRepository.DeleteOrderGuardsByOrderGuardsID(orderGuardsID.Value);
            return true;
        }
    }
}
