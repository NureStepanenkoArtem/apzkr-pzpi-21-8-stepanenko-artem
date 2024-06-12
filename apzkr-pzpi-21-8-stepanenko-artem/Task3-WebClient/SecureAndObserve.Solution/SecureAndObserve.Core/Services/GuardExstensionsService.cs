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
    public class GuardExstensionsService : IGuardExstensionsService
    {
        private readonly IGuardExstensionsRepository _guardExstensionsRepository;
        public GuardExstensionsService(IGuardExstensionsRepository guardExstensionsRepository) {
            _guardExstensionsRepository = guardExstensionsRepository;
        }
        public async Task<GuardExstensionsResponse> AddGuardExstensions(GuardExstensionsAddRequest? guardExstensionsAddRequest)
        {
            if (guardExstensionsAddRequest == null)
                throw new ArgumentNullException(nameof(guardExstensionsAddRequest));
            if (guardExstensionsAddRequest.UserId == null)
                throw new ArgumentException(nameof(guardExstensionsAddRequest.UserId));
            GuardExstensions guardExstensions = guardExstensionsAddRequest.ToGuardExstensions();
            guardExstensions.Id = Guid.NewGuid();
            await _guardExstensionsRepository.AddGuardExstensions(guardExstensions);
            return guardExstensions.ToGuardExstensionsResponse();
        }
        public async Task<List<GuardExstensionsResponse>> GetAllGuardExstensions()
        {
            List<GuardExstensions> guardExstensions = await _guardExstensionsRepository.GetAllGuardExstensions();
            return guardExstensions.Select(guardExstension => guardExstension.ToGuardExstensionsResponse()).ToList();
        }

        public async Task<GuardExstensionsResponse?> GetGuardsExstensionsByGuardExstensionsId(Guid? guardExstensionsId)
        {
            if (guardExstensionsId == null)
                return null;
            GuardExstensions? guardExstensions = await _guardExstensionsRepository.GetGuardExstensionyByGuardExstensionId(guardExstensionsId.Value);
            if (guardExstensions == null)
                return null;
            return guardExstensions.ToGuardExstensionsResponse();
        }
    }
}
