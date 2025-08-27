using BCrypt.Net;
using Microsoft.AspNetCore.Mvc;
using RideMobility.Api.Data;
using RideMobility.Api.Models;
using RideMobility.Api.Services;
using RideMobility.Api.Services.Helpers;

namespace RideMobility.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly TokenService _tokenService;

        public AuthController(ApplicationDbContext context, TokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest req)
        {
            if (string.IsNullOrWhiteSpace(req.Username) || string.IsNullOrWhiteSpace(req.Password))
                return BadRequest("Username and password are required");

            var user = _context.Users.FirstOrDefault(u => u.Username == req.Username);
            if (user == null) return Unauthorized("User not found");

            if (!BCrypt.Net.BCrypt.Verify(req.Password, user.PasswordHash))
                return Unauthorized("Invalid password");

            var token = _tokenService.GenerateToken(user);
            return Ok(new { token, role = user.Role });
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterRequest req)
        {
            if (string.IsNullOrWhiteSpace(req.Username) || string.IsNullOrWhiteSpace(req.Password))
                return BadRequest("Username and password are required");

            if (_context.Users.Any(u => u.Username == req.Username))
                return Conflict("Username already exists");

            if (!PasswordValidator.IsValid(req.Password))
                return BadRequest("Password must be at least 8 characters, include uppercase, lowercase, number, and symbol");

            var user = new User
            {
                Username = req.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(req.Password),
                Role = req.Role ?? "Rider"
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            return Ok(new { message = "User registered successfully" });
        }
    }

    public class LoginRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class RegisterRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string? Role { get; set; }
    }
}
