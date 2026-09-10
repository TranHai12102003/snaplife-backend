using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace SL.Domain.Common.Constants
{
    public class Globals(IHttpContextAccessor contextAccessor)
    {
        private readonly IHttpContextAccessor _contextAccessor = contextAccessor;
        private ClaimsPrincipal? User => _contextAccessor?.HttpContext?.User;
        protected string GlobalUserId => User?.Identity != null && User.Identity.IsAuthenticated && User.Claims.Count() > 2 ? User.Claims.ToArray()[2].Value : string.Empty;
        protected string GlobalEmail => User?.Identity != null && User.Identity.IsAuthenticated && User.Claims.Count() > 3 ? User.Claims.ToArray()[3].Value : string.Empty;
        protected string GlobalUserName => User?.Identity != null && User.Identity.IsAuthenticated && User.Claims.Count() > 4 ? User.Claims.ToArray()[4].Value : string.Empty;
    }
}
