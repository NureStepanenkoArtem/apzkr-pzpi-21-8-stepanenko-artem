using Microsoft.EntityFrameworkCore;
using SecureAndObserve.Core.Domain.Entities;
using SecureAndObserve.Core.Domain.RepositoryContracts;
using SecureAndObserve.Infrastructure.DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureAndObserve.Infrastructure.Repositories
{
    public class GuardExstensionsRepository : IGuardExstensionsRepository
    {
        private readonly ApplicationDbContext _db;

        public GuardExstensionsRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<GuardExstensions> AddGuardExstensions(GuardExstensions guardExstensions)
        {
            await _db.GuardExstensions.AddAsync(guardExstensions);
            await _db.SaveChangesAsync();
            return guardExstensions;
        }
        public async Task<List<GuardExstensions>> GetAllGuardExstensions()
        {
            return await _db.GuardExstensions.ToListAsync();
        }
        public async Task<GuardExstensions?> GetGuardExstensionyByGuardExstensionId(Guid guardExstensionId)
        {
            return await _db.GuardExstensions.FirstOrDefaultAsync(temp => temp.Id == guardExstensionId);
        }
    }
}
