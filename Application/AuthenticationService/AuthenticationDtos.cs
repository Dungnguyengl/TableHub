using Core.Enum;

namespace Application.AuthenticationService
{
    public class LoginCommand
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class LoginDto
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string Role { get; set; }
        public string FirstName { get; set; }
    }

    public class RegisterCommand
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Gender Gender { get; set; }
        public DateOnly Dob { get; set; }
        public bool IsOwner { get; set; }   
    }
}
