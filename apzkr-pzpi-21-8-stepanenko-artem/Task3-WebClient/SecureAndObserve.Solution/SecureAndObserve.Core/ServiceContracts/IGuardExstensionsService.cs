using SecureAndObserve.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureAndObserve.Core.ServiceContracts
{
    public interface IGuardExstensionsService
    {
        Task<GuardExstensionsResponse> AddGuardExstensions(GuardExstensionsAddRequest? guardExstensionsAddRequest);
        Task<List<GuardExstensionsResponse>> GetAllGuardExstensions();
        Task<GuardExstensionsResponse?> GetGuardsExstensionsByGuardExstensionsId(Guid? guardExstensionsId);
    }
}
