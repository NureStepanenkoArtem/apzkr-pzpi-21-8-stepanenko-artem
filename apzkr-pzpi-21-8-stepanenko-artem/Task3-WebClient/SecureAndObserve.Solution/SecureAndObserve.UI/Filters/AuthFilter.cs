using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using SecureAndObserve.Core.Domain.IdentityEntities;
using SecureAndObserve.Infrastructure.DbContext;

namespace SecureAndObserve.UI.Filters
{
    public class AuthFilter : IResultFilter
    {
        private readonly ApplicationDbContext _context;
        public AuthFilter(ApplicationDbContext context)
        {
            _context = context;
        }

        public void OnResultExecuted(ResultExecutedContext context)
        {
        }

        public void OnResultExecuting(ResultExecutingContext context)
        {
            IDictionary<string, object?>? parameters = (IDictionary<string, object?>?)context.HttpContext.Items["arguments"];

            string? nickname = Convert.ToString(parameters?["loginDTO.Nickname"]);

            var userId = _context.Users.Where(x => x.UserNickname == nickname).Select(x => x.Id);

            if(userId != null)
                context.HttpContext.Response.Cookies.Append("Auth-Key", Convert.ToString(userId));
            else
                context.HttpContext.Response.Cookies.Append("Auth-Key", "null");
        }
    }
}
