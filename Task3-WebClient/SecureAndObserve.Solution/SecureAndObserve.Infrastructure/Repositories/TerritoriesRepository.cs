using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SecureAndObserve.Core.Domain.Entities;
using SecureAndObserve.Core.Domain.RepositoryContracts;
using SecureAndObserve.Infrastructure.DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SecureAndObserve.Infrastructure.Repositories
{
    public class TerritoriesRepository : ITerritoriesRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<TerritoriesRepository> _logger;

        public TerritoriesRepository(ApplicationDbContext db, ILogger<TerritoriesRepository> logger)
        {
            _db = db;
            _logger = logger;
        }
        public async Task<Territory> AddTerritory(Territory territory)
        {
            await _db.Territories.AddAsync(territory);
            await _db.SaveChangesAsync();
            return territory;
        }
        public async Task<List<Territory>> GetAllTerritories()
        {
            return await _db.Territories.ToListAsync();
        }
        public async Task<Territory?> GetTerritoryByTerritoryId(Guid territoryId)
        {
            return await _db.Territories.FirstOrDefaultAsync(temp => temp.Id == territoryId);
        }
        public async Task<List<Territory>> GetFilteredTerritories(Expression<Func<Territory, bool>> predicate)
        {
            _logger.LogInformation("GetFilteredTerritories of TerritoriesRepository");

            return await _db.Territories.Where(predicate).ToListAsync();
        }
    }
}
