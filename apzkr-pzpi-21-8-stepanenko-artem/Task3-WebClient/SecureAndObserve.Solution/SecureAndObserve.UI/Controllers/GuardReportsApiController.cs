using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecureAndObserve.Core.Domain.Entities;
using SecureAndObserve.Infrastructure.DbContext;

namespace SecureAndObserve.UI.Controllers
{

    [Route("api/GuardReportsApi")]
    [ApiController]
    [AllowAnonymous]
    public class GuardReportsApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public GuardReportsApiController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GuardReport>>> GetGuardReports()
        {
            if (_context.GuardReports == null)
            {
                return NotFound();
            }
            return await _context.GuardReports.ToListAsync();
        }
        // GET: api/UsersApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<GuardReport>>> GetGuardReport(Guid id)
        {
            if (_context.GuardReports == null)
            {
                return NotFound();
            }

            return await _context.GuardReports.Where(x => x.OrderId == id).ToListAsync();
        }

        // POST: api/UsersApi
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<GuardReport>> PostGuardReport(GuardReport guardReport)
        {
            if (_context.GuardReports == null)
            {
                return Problem("Entity set 'ApplicationDbContext.GuardReports'  is null.");
            }
            await _context.GuardReports.AddAsync(guardReport);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGuardReport", new { id = guardReport.Id }, guardReport);
        }

        // DELETE: api/UsersApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGuardReport(Guid id)
        {
            if (_context.GuardReports == null)
            {
                return NotFound();
            }
            var guardReport = await _context.GuardReports.FindAsync(id);
            if (guardReport == null)
            {
                return NotFound();
            }

            _context.GuardReports.Remove(guardReport);
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}
