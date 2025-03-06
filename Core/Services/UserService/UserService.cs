using Core.Extentions;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Core.Services.UserService
{
    public class UserService(IHttpContextAccessor httpContext) : IUserService
    {
        private readonly ClaimsPrincipal? _claimsPrincipal = httpContext.HttpContext?.User;

        public ClaimsPrincipal GetUserClaims()
        {
            return _claimsPrincipal ?? throw new ArgumentNullException("User Claims");
        }

        public Guid GetUserId()
        {
            var userId = _claimsPrincipal?.FindFirstValue(ClaimTypes.Sid) ?? throw new ArgumentNullException("UserId");
            return Guid.Parse(userId);
        }

        public Guid? GetUserStoreId()
        {
            var storeId = _claimsPrincipal?.FindFirstValue("StoreId");
            return storeId.IsNullOrEmpty() ? null : Guid.Parse(storeId);
        }
    }
}
