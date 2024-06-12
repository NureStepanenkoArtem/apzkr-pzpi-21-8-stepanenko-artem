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
    public class GuardReportsService : IGuardReportsService
    {
        private readonly IGuardReportRepository _guardReportsRepository;
        public GuardReportsService(IGuardReportRepository guardReportsRepository)
        {
            _guardReportsRepository = guardReportsRepository;
        }

        public async Task<GuardReportResponse> AddGuardReport(GuardReportAddRequest? guardReportsAddRequest)
        {
            if (guardReportsAddRequest == null)
                throw new ArgumentNullException(nameof(guardReportsAddRequest));
            if (guardReportsAddRequest.GuardExstensionsId == null)
                throw new ArgumentException(nameof(guardReportsAddRequest.GuardExstensionsId));
            GuardReport guardReport = guardReportsAddRequest.ToGuardReport();
            guardReport.Id = Guid.NewGuid();
            await _guardReportsRepository.AddGuardReport(guardReport);
            return guardReport.ToGuardReportResponse();
        }
        public async Task<List<GuardReportResponse>> GetAllGuardReports()
        {
            List<GuardReport> guardReports = await _guardReportsRepository.GetAllGuardReports();
            return guardReports.Select(guardReport => guardReport.ToGuardReportResponse()).ToList();
        }
        public async Task<GuardReportResponse?> GetGuardsReportByGuardReportId(Guid? guardReportId)
        {
            if (guardReportId == null)
                return null;
            GuardReport? guardReport = await _guardReportsRepository.GetGuardReportByGuardReportId(guardReportId.Value);
            if (guardReport == null)
                return null;
            return guardReport.ToGuardReportResponse();
        }
    }
}
