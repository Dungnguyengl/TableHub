using Application.AuthenticationService;
using Core.Enum;
using Domain.Constants;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController(UserManager<User> userManager, SignInManager<User> signInManager, IConfiguration configuration) : ControllerBase
    {
        private readonly UserManager<User> _userManager = userManager;
        private readonly SignInManager<User> _signInManager = signInManager;
        private readonly IConfiguration Configuration = configuration;

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginCommand command)
        {
            var user = await _userManager.FindByEmailAsync(command.Email);
            if (user == null)
            {
                return NotFound();
            }
            var result = await _signInManager.PasswordSignInAsync(user, command.Password, false, false);
            if (result.Succeeded)
            {
                var accessToken = GenerateJwtToken(user);
                return Ok(new LoginDto { AccessToken = accessToken });
            }
            return NotFound();
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterCommand command)
        {
            var user = new User
            {
                Email = command.Email,
                UserName = command.Email,
                FirstName = command.FirstName,
                LastName = command.LastName,
                Dob = command.Dob,
                Gender = command.Gender == Gender.Male ? Genders.MALE : Genders.FEMALE,
                Role = Roles.GUEST,
            };

            var registerUser = await _userManager.CreateAsync(user, command.Password);

            if (!registerUser.Succeeded)
            {
                return BadRequest(registerUser.Errors);
            }
            return Ok();
        }

        private string GenerateJwtToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration ["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new []
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var token = new JwtSecurityToken(
                issuer: Configuration ["Jwt:Issuer"],
                audience: Configuration ["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
