using SecureAndObserve.Core.DTO;
using SecureAndObserve.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureAndObserve.Core.ServiceContracts
{
    public interface ITerritoriesService
    {
        Task<TerritoryResponse> AddTerritory(TerritoryAddRequest? territoryAddRequest);
        Task<List<TerritoryResponse>> GetAllTerritories();
        Task<TerritoryResponse?> GetTerritoryByTerritoryId(Guid? territoryId);
        Task<List<TerritoryResponse>> GetSortedTerritories(List<TerritoryResponse> allTerritories, string sortBy, SortOrderOptions sortOrder);
        Task<List<TerritoryResponse>> GetFilteredTerritories(string searchBy, string? searchString);
    }
}
