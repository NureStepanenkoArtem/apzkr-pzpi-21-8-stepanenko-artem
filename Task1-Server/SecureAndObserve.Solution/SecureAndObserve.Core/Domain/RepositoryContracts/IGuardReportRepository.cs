using SecureAndObserve.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureAndObserve.Core.Domain.RepositoryContracts
{
    public interface IGuardReportRepository
    {
        Task<GuardReport> AddGuardReport(GuardReport guardReport);
        Task<List<GuardReport>> GetAllGuardReports();
        Task<GuardReport?> GetGuardReportByGuardReportId(Guid guardReportId);
    }
}
