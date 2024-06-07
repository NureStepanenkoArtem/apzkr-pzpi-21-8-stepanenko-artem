using Microsoft.EntityFrameworkCore;
using SecureAndObserve.Core.Domain.Entities;
using SecureAndObserve.Core.Domain.RepositoryContracts;
using SecureAndObserve.Infrastructure.DbContext;

namespace SecureAndObserve.Infrastructure.Repositories
{
    public class OrdersRepository : IOrdersRepository
    {
        private readonly ApplicationDbContext _db;
        public OrdersRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<Order> AddOrder(Order order)
        {
            await _db.Orders.AddAsync(order);
            await _db.SaveChangesAsync();
            return order;
        }
        public async Task<List<Order>> GetAllOrders()
        {
            return await _db.Orders.ToListAsync();
        }
        public async Task<Order?> GetOrderByOrderId(Guid orderId)
        {
            return await _db.Orders.FirstOrDefaultAsync(temp => temp.Id == orderId);
        }
        public async Task<int> CostOfService(Guid orderId)
        {
            int result = 0;

            List<Guid> guardExstentionsGuids = _db.OrderGuards
                .Where(temp => temp.OrderId == orderId)
                .Select(temp => temp.GuardExstensionsId)
                .ToList();
            List<Guid> ranksGuids = new List<Guid>();
            foreach(Guid guardExstentionsGuid in guardExstentionsGuids)
            {
                ranksGuids.AddRange(_db.GuardExstensions
                .Where(temp => temp.Id == guardExstentionsGuid)
                .Select(temp => temp.RankId)
                .ToList());
            }
            List<int> payPerHourList = new List<int>();
            foreach(Guid ranksGuid in ranksGuids)
            {
                payPerHourList.AddRange(_db.Ranks
                .Where(temp => temp.Id == ranksGuid)
                .Select(temp => temp.PayPerHour)
                .ToList());
            }
            foreach(int payPerHour in payPerHourList)
            {
                result += payPerHour;
            }
            return result;
        }

        public async Task<bool> DeleteOrderByOrderID(Guid orderID)
        {
            _db.Orders.RemoveRange(_db.Orders.Where(temp => temp.Id == orderID));
            int rowsDeleted = await _db.SaveChangesAsync();

            return rowsDeleted > 0;
        }
    }
}
