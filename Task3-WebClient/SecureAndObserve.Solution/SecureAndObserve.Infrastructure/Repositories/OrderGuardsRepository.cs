using SecureAndObserve.Core.Domain.Entities;
using SecureAndObserve.Infrastructure.DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SecureAndObserve.Core.Domain.Entities;
using SecureAndObserve.Core.Domain.RepositoryContracts;
using SecureAndObserve.Infrastructure.DbContext;

namespace SecureAndObserve.Infrastructure.Repositories
{
    public class OrderGuardsRepository : IOrderGuardsRepository
    {
        private readonly ApplicationDbContext _db;
        public OrderGuardsRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<OrderGuards> AddOrderGuards(OrderGuards orderGuards)
        {
            await _db.OrderGuards.AddAsync(orderGuards);
            await _db.SaveChangesAsync();
            return orderGuards;
        }
        public async Task<List<OrderGuards>> GetAllOrdersGuards()
        {
            return await _db.OrderGuards.ToListAsync();
        }
        public async Task<OrderGuards?> GetOrderGuardsByOrderGuardsId(Guid orderGuardsId)
        {
            return await _db.OrderGuards.FirstOrDefaultAsync(temp => temp.Id == orderGuardsId);
        }

        public async Task<bool> DeleteOrderGuardsByOrderGuardsID(Guid orderGuardsID)
        {
            _db.OrderGuards.RemoveRange(_db.OrderGuards.Where(temp => temp.Id == orderGuardsID));
            int rowsDeleted = await _db.SaveChangesAsync();

            return rowsDeleted > 0;
        }
    }
}
