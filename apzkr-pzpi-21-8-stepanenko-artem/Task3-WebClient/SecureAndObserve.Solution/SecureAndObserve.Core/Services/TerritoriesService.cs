using Microsoft.Extensions.Logging;
using SecureAndObserve.Core.Domain.Entities;
using SecureAndObserve.Core.Domain.RepositoryContracts;
using SecureAndObserve.Core.DTO;
using SecureAndObserve.Core.Enums;
using SecureAndObserve.Core.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureAndObserve.Core.Services
{
    public class TerritoriesService : ITerritoriesService
    {
        private readonly ITerritoriesRepository _territoriesRepository;
        private readonly ILogger<TerritoriesService> _logger;

        public TerritoriesService(ITerritoriesRepository territoriesRepository, ILogger<TerritoriesService> logger)
        {
            _territoriesRepository = territoriesRepository;
            _logger = logger;
        }
        public async Task<TerritoryResponse> AddTerritory(TerritoryAddRequest? territoryAddRequest)
        {
            if (territoryAddRequest == null)
                throw new ArgumentNullException(nameof(territoryAddRequest));
            if (territoryAddRequest.Name == null)
                throw new ArgumentException(nameof(territoryAddRequest.Name));
            Territory territory = territoryAddRequest.ToTerritory();
            territory.Id = Guid.NewGuid();
            await _territoriesRepository.AddTerritory(territory);
            return territory.ToTerritoryResponse();
        }

        public async Task<List<TerritoryResponse>> GetAllTerritories()
        {
            List<Territory> territories = await _territoriesRepository.GetAllTerritories();
            return territories.Select(territory => territory.ToTerritoryResponse()).ToList();
        }
        public async Task<TerritoryResponse?> GetTerritoryByTerritoryId(Guid? territoryId)
        {
            if (territoryId == null)
                return null;
            Territory? territory = await _territoriesRepository.GetTerritoryByTerritoryId(territoryId.Value);
            if (territory == null)
                return null;
            return territory.ToTerritoryResponse();
        }

        public async Task<List<TerritoryResponse>> GetFilteredTerritories(string searchBy, string? searchString)
        {
            _logger.LogInformation("GetFilteredTerritories of TerritoriesService");
            List<Territory> territories;

            territories = searchBy switch
            {
                nameof(TerritoryResponse.Name) =>
                   await _territoriesRepository.GetFilteredTerritories(temp => temp.Name.Contains(searchString)),

                nameof(TerritoryResponse.Square) =>
                    await _territoriesRepository.GetFilteredTerritories(temp => temp.Square.Contains(searchString)),

                nameof(TerritoryResponse.Description) =>
                    await _territoriesRepository.GetFilteredTerritories(temp => temp.Description.Contains(searchString)),

                nameof(TerritoryResponse.Type) =>
                    await _territoriesRepository.GetFilteredTerritories(temp => temp.Type.Contains(searchString)),

                _ => await _territoriesRepository.GetAllTerritories()
            };

            return territories.Select(temp => temp.ToTerritoryResponse()).ToList();
        }


        public async Task<List<TerritoryResponse>> GetSortedTerritories(List<TerritoryResponse> allTerritories, string sortBy, SortOrderOptions sortOrder)
        {
            _logger.LogInformation("GetSortedTerritories of TerritoriesService");

            if (string.IsNullOrEmpty(sortBy))
                return allTerritories;

            List<TerritoryResponse> sortedTerritories = (sortBy, sortOrder) switch
            {
                (nameof(TerritoryResponse.Name), SortOrderOptions.ASC) => allTerritories.OrderBy(temp => temp.Name, StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(TerritoryResponse.Name), SortOrderOptions.DESC) => allTerritories.OrderByDescending(temp => temp.Name, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(TerritoryResponse.Square), SortOrderOptions.ASC) => allTerritories.OrderBy(temp => temp.Square, StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(TerritoryResponse.Square), SortOrderOptions.DESC) => allTerritories.OrderByDescending(temp => temp.Square, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(TerritoryResponse.Description), SortOrderOptions.ASC) => allTerritories.OrderBy(temp => temp.Description).ToList(),
                (nameof(TerritoryResponse.Description), SortOrderOptions.DESC) => allTerritories.OrderByDescending(temp => temp.Description).ToList(),

                (nameof(TerritoryResponse.Type), SortOrderOptions.ASC) => allTerritories.OrderBy(temp => temp.Type, StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(TerritoryResponse.Type), SortOrderOptions.DESC) => allTerritories.OrderByDescending(temp => temp.Type, StringComparer.OrdinalIgnoreCase).ToList(),

                _ => allTerritories
            };

            return sortedTerritories;
        }
    }
}
