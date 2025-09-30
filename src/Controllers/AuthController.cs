using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaskManagementApi.Services;

namespace TaskManagementApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _service;
        private readonly IConfiguration _config;
        private readonly ILogger<AuthController> _logger;

        public record LoginModel(string Username, string Password);
        public AuthController(IUserService service, IConfiguration configuration, ILogger<AuthController> logger)
        {
            _service = service;
            _config = configuration;
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel login)
        {
            _logger.LogInformation("Attempting to log in user: {Username}", login.Username);

            var user = await _service.Login(login.Username,login.Password);

            if (user != null)
            {
                var token = GenerateJwtToken(user.Username,user.Role,user.UserId);
                _logger.LogInformation("User {Username} logged in successfully.", login.Username);
                return Ok(new { Token = token });
            }
            _logger.LogWarning("Login failed for user: {Username}", login.Username);
            return Unauthorized();
        }

        private string GenerateJwtToken(string username,string role,int id)
        {
            var jwtSetting = _config.GetSection("JWT");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSetting["Key"]));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);


            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = jwtSetting["Issuer"],
                Audience = jwtSetting["Audience"],
                Subject = new ClaimsIdentity(new[] { 
                    new Claim(ClaimTypes.Name, username),
                    new Claim(ClaimTypes.Role, role),
                    new Claim(ClaimTypes.NameIdentifier, id.ToString()),
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = cred
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

    }
}
