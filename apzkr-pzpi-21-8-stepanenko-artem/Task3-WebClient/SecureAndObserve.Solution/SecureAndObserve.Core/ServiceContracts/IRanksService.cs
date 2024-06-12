using SecureAndObserve.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureAndObserve.Core.ServiceContracts
{
    public interface IRanksService
    {
        Task<RankResponse> AddRank(RankAddRequest? rankAddRequest);
        Task<List<RankResponse>> GetAllRanks();
        Task<RankResponse?> GetRankByRankId(Guid? rankId);
    }
}
