using SecureAndObserve.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureAndObserve.Core.Domain.RepositoryContracts
{
    public interface IGuardExstensionsRepository
    {
        Task<GuardExstensions> AddGuardExstensions(GuardExstensions guardExstensions);
        Task<List<GuardExstensions>> GetAllGuardExstensions();
        Task<GuardExstensions?> GetGuardExstensionyByGuardExstensionId(Guid guardExstensionId);
    }
}
