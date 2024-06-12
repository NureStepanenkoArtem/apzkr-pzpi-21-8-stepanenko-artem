using SecureAndObserve.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureAndObserve.Core.Domain.RepositoryContracts
{
    public interface IRanksRepository
    {
        Task<Rank> AddRank(Rank rank);
        Task<List<Rank>> GetAllRanks();
        Task<Rank?> GetRankByRankId(Guid rankId);
    }
}
