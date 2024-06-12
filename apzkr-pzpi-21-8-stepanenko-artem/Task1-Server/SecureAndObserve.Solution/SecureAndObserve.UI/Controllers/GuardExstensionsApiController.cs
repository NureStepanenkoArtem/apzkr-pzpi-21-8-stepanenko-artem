using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecureAndObserve.Core.Domain.Entities;
using SecureAndObserve.Infrastructure.DbContext;

namespace SecureAndObserve.UI.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    [ApiController]
    public class GuardExstensionsApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public GuardExstensionsApiController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GuardExstensions>>> GetGuardExstensions()
        {
            if (_context.GuardExstensions == null)
            {
                return NotFound();
            }
            return await _context.GuardExstensions.ToListAsync();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<GuardExstensions>> GetGuardExstensions(Guid id)
        {
            if (_context.GuardExstensions == null)
            {
                return NotFound();
            }
            var guard = await _context.GuardExstensions.FindAsync(id);

            if (guard == null)
            {
                return NotFound();
            }

            return guard;
        }

        [HttpPost]
        public async Task<ActionResult<GuardExstensions>> PostGuardExstentions(GuardExstensions guardExstensions)
        {
            if (_context.GuardExstensions == null)
            {
                return Problem("Entity set 'ApplicationDbContext.GuardExtensions'  is null.");
            }
            await _context.GuardExstensions.AddAsync(guardExstensions);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Get GuardExtensions", new { id = guardExstensions.Id }, guardExstensions);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGuardExstension(Guid id)
        {
            if (_context.GuardExstensions == null)
            {
                return NotFound();
            }
            var guardExstensions = await _context.GuardExstensions.FindAsync(id);
            if (guardExstensions == null)
            {
                return NotFound();
            }

            _context.GuardExstensions.Remove(guardExstensions);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(Guid id)
        {
            return (_context.GuardReports?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

