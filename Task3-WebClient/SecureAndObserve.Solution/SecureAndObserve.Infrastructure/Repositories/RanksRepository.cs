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
    public class RanksRepository : IRanksRepository
    {
        private readonly ApplicationDbContext _db;
        public RanksRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<Rank> AddRank(Rank rank)
        {
            await _db.Ranks.AddAsync(rank);
            await _db.SaveChangesAsync();
            return rank;
        }
        public async Task<List<Rank>> GetAllRanks()
        {
            return await _db.Ranks.ToListAsync();
        }
        public async Task<Rank?> GetRankByRankId(Guid rankId)
        {
            return await _db.Ranks.FirstOrDefaultAsync(temp => temp.Id == rankId);
        }
    }
}
