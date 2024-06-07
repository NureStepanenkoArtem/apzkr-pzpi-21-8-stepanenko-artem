using SecureAndObserve.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SecureAndObserve.Core.Domain.RepositoryContracts
{
    public interface ITerritoriesRepository
    {
        Task<Territory> AddTerritory(Territory territory);
        Task<List<Territory>> GetAllTerritories();
        Task<Territory?> GetTerritoryByTerritoryId(Guid territoryId);
        Task<List<Territory>> GetFilteredTerritories(Expression<Func<Territory, bool>> predicate);
    }
}
