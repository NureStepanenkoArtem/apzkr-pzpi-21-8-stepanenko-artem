using Microsoft.EntityFrameworkCore;
using SecureAndObserve.Core.Domain.Entities;
using SecureAndObserve.Core.Domain.RepositoryContracts;
using SecureAndObserve.Infrastructure.DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureAndObserve.Infrastructure.Repositories
{
    public class GuardReportRepository : IGuardReportRepository
    {
        private readonly ApplicationDbContext _db;

        public GuardReportRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<GuardReport> AddGuardReport(GuardReport guardReport)
        {
            await _db.GuardReports.AddAsync(guardReport);
            await _db.SaveChangesAsync();
            return guardReport;
        }
        public async Task<List<GuardReport>> GetAllGuardReports()
        {
            return await _db.GuardReports.ToListAsync();
        }
        public async Task<GuardReport?> GetGuardReportByGuardReportId(Guid guardReportId)
        {
            return await _db.GuardReports.FirstOrDefaultAsync(temp => temp.Id == guardReportId);
        }
    }
}
