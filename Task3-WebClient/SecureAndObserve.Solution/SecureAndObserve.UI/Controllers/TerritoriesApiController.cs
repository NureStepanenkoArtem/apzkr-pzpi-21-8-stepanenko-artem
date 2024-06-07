using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecureAndObserve.Core.Domain.Entities;
using SecureAndObserve.Infrastructure.DbContext;

namespace SecureAndObserve.UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TerritoriesApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TerritoriesApiController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Territory>>> GetTerritories()
        {
            if (_context.Territories == null)
            {
                return NotFound();
            }
            return await _context.Territories.ToListAsync();
        }
        // GET: api/UsersApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Territory>> GetTerritory(Guid id)
        {
            if (_context.Territories == null)
            {
                return NotFound();
            }
            var territory = await _context.Territories.FindAsync(id);

            if (territory == null)
            {
                return NotFound();
            }

            return territory;
        }

        [HttpPost]
        public async Task<ActionResult<Territory>> PostTerritory(Territory territory)
        {
            if (_context.Territories == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Territories'  is null.");
            }
            await _context.Territories.AddAsync(territory);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTerritory", new { id = territory.Id }, territory);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTerritory(Guid id)
        {
            if (_context.Territories == null)
            {
                return NotFound();
            }
            var territory = await _context.Territories.FindAsync(id);
            if (territory == null)
            {
                return NotFound();
            }

            _context.Territories.Remove(territory);
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}
