using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecureAndObserve.Core.Domain.Entities;
using SecureAndObserve.Core.ServiceContracts;
using SecureAndObserve.Infrastructure.DbContext;

namespace SecureAndObserve.UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class OrdersApiController : ControllerBase
    {
        private ApplicationDbContext _context;

        public OrdersApiController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrder(Guid id)
        {
            if (_context.OrderGuards == null)
            {
                return NotFound();
            }
            return await _context.Orders.Where(x => x.Id == id).ToListAsync();
        }
    }
}
