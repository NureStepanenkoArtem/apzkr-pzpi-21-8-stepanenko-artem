using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecureAndObserve.Core.Domain.Entities;
using SecureAndObserve.Core.Domain.IdentityEntities;
using SecureAndObserve.Infrastructure.DbContext;

namespace SecureAndObserve.UI.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    [ApiController]
    public class UsersApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public UsersApiController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ApplicationUser>>> GetUser()
        {
            if (_userManager.Users == null)
            {
                return NotFound();
            }
            return await _userManager.Users.ToListAsync();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ApplicationUser>> GetUser(Guid id)
        {
            if (_userManager.Users == null)
            {
                return NotFound();
            }
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }
    }
}
