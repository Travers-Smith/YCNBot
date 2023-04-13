using Microsoft.AspNetCore.Http;
using YCNBot.Core.Services;

namespace YCNBot.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public IdentityService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string? GetEmail()
        {
            return _httpContextAccessor
                .HttpContext
                .User
                .Claims
                .FirstOrDefault(x => x.Type == "preferred_username")
                ?.Value;
        }

        public string? GetName()
        {
            return _httpContextAccessor
                .HttpContext
                .User
                .Claims
                .FirstOrDefault(x => x.Type == "name")
                ?.Value;
        }

        public Guid? GetUserIdentifier()
        {
            string? identifier = _httpContextAccessor
                .HttpContext
                .User
                .Claims
                .FirstOrDefault(x => x.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier")
                ?.Value;

            if (identifier == null)
            {
                return null;
            }

            return Guid.Parse(identifier);
        }

        public bool IsAuthenticated() 
        { 
            return _httpContextAccessor
                ?.HttpContext
                ?.User
                ?.Identity
                ?.IsAuthenticated ?? false;
        }
    }
}
