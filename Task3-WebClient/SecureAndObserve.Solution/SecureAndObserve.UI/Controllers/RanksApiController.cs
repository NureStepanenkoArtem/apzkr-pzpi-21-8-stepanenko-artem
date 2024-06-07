using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecureAndObserve.Core.Domain.Entities;
using SecureAndObserve.Infrastructure.DbContext;

namespace SecureAndObserve.UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class RanksApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public RanksApiController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Rank>>> GetRanks()
        {
            if (_context.Users == null)
            {
                return NotFound();
            }
            return await _context.Ranks.ToListAsync();
        }
        [HttpGet("{id}")]
        
        public async Task<ActionResult<Rank>> GetRank(Guid id)
        {
            if (_context.Ranks == null)
            {
                return NotFound();
            }
            var rank = await _context.Ranks.FindAsync(id);

            if (rank == null)
            {
                return NotFound();
            }

            return rank;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutRank(Guid id, Rank rank)
        {
            if (id != rank.Id)
            {
                return BadRequest();
            }

            _context.Entry(rank).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await _context.Ranks.FindAsync(id) == null)
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
        [HttpPost]
        public async Task<ActionResult<Territory>> PostRank(Rank rank)
        {
            if (_context.Ranks == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Ranks'  is null.");
            }
            await _context.Ranks.AddAsync(rank);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRanks", new { id = rank.Id }, rank);
        }
    }
}
