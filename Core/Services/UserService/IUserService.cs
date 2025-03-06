using System.Security.Claims;

namespace Core.Services.UserService
{
    public interface IUserService
    {
        public Guid GetUserId();
        public Guid? GetUserStoreId();
        public ClaimsPrincipal GetUserClaims();
    }
}
