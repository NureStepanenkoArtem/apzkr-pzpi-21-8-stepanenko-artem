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
    public class OrderGuardsApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public OrderGuardsApiController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<OrderGuards>>> GetOrderGuards(Guid id)
        {
            if (_context.OrderGuards == null)
            {
                return NotFound();
            }
            return await _context.OrderGuards.Where(x => x.GuardExstensionsId == id).ToListAsync();
        }
    }
}
