using lab4_KPZ.Data;
using lab4_KPZ.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace lab4_KPZ.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly CharityGameContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(CharityGameContext context, IConfiguration configuration)
        {
            _configuration = configuration;
            _context = context;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            var player = _context.Players.FirstOrDefault(p => p.Email.Equals(request.Email));

            if (player != null)
            {
                if (request.Password.Equals(player.Password))
                {
                    var token = GenerateJwtToken(request.Email);

                    return Ok(new { token });
                }
            }
            return Unauthorized(new { message = "Invalid credentials" });
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterRequest request)
        {
            var player = _context.Players.FirstOrDefault(p => p.Email.Equals(request.Email));

            if (player == null)
            {
                player = new Player();
                player.Email = request.Email;
                player.Password = request.Password;
                player.Nickname = request.Name;
                player.Sex = request.Sex;

                /*try
                {*/
                    _context.Players.Add(player);
                    await _context.SaveChangesAsync();

                    var token = GenerateJwtToken(request.Email);

                    return Ok(new { token });
/*                }
                catch (Exception ex)
                {
                    return Unauthorized(new { message = "Invalid credentials while registering" });
                }*/
            }
            return Unauthorized(new { message = "Invalid credentials for registration" });
        }

        private string GenerateJwtToken(string username)
        {
            // Ключ шифрування
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            // Об'єкт опису токена
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class RegisterRequest
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Sex { get; set; }
    }
}
