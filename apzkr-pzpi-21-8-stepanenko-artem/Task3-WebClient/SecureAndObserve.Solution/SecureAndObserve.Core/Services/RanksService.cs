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
    public class RanksService : IRanksService
    {
        private readonly IRanksRepository _ranksRepository;
        public RanksService(IRanksRepository ranksRepository)
        {
            _ranksRepository = ranksRepository;
        }
        public async Task<RankResponse> AddRank(RankAddRequest? rankAddRequest)
        {
            if(rankAddRequest == null)
                throw new ArgumentNullException(nameof(rankAddRequest));
            if(rankAddRequest.Name == null)
                throw new ArgumentException(nameof(rankAddRequest.Name));
            Rank rank = rankAddRequest.ToRank();
            rank.Id = Guid.NewGuid();
            await _ranksRepository.AddRank(rank);
            return rank.ToRankResponse();
        }
        public async Task<List<RankResponse>> GetAllRanks()
        {
            List<Rank> ranks = await _ranksRepository.GetAllRanks();
            return ranks.Select(rank => rank.ToRankResponse()).ToList();
        }
        public async Task<RankResponse?> GetRankByRankId(Guid? rankId)
        {
            if(rankId == null)
                return null;
            Rank? rank = await _ranksRepository.GetRankByRankId(rankId.Value);
            if (rank == null)
                return null;
            return rank.ToRankResponse();
        }
    }
}
