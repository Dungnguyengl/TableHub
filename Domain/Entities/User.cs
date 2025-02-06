using Microsoft.AspNetCore.Identity;

namespace Domain.Entities
{
    public class User : IdentityUser
    {
        public string? RefreshToken { get; set; }

        public DateTime? RefreshTokenExpire { get; set; }

        public Guid? StoreId { get; set; }

        public string? StoreSlug { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Role { get; set; }

        public string? Gender { get; set; }

        public DateOnly? Dob { get; set; }
    }
}
