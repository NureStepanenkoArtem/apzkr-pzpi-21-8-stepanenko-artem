using SecureAndObserve.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureAndObserve.Core.ServiceContracts
{
    public interface IGuardReportsService
    {
        Task<GuardReportResponse> AddGuardReport(GuardReportAddRequest? guardReportsAddRequest);

        Task<List<GuardReportResponse>> GetAllGuardReports();

        Task<GuardReportResponse?> GetGuardsReportByGuardReportId(Guid? guardReportId);
    }
}
