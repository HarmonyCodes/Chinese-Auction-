
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.models;
using Project.DAL;
using System.Linq;
using Microsoft.Extensions.Configuration;
using BCrypt.Net;
using Project.BLL;
using Server.BLL;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Project.models.DTOs;

namespace Project.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly AppDbContext _context;

        public AuthController(IConfiguration config, AppDbContext context)
        {
            _config = config;
            _context = context;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            // וולידציה בסיסית
            if (string.IsNullOrWhiteSpace(registerDto.UserName) ||
                string.IsNullOrWhiteSpace(registerDto.FullName) ||
                string.IsNullOrWhiteSpace(registerDto.Phone) ||
                string.IsNullOrWhiteSpace(registerDto.Email) ||
                string.IsNullOrWhiteSpace(registerDto.Password))
            {
                return BadRequest("All fields are required");
            }
            if (!registerDto.Email.Contains("@"))
                return BadRequest("Invalid email");
            if (!registerDto.Phone.All(char.IsDigit) || registerDto.Phone.Length < 7)
                return BadRequest("Invalid phone");

            if (_context.Users.Any(u => u.UserName == registerDto.UserName))
                return BadRequest("Username already exists");
            if (_context.Users.Any(u => u.Email == registerDto.Email))
                return BadRequest("Email already exists");

            var user = new User
            {
                UserName = registerDto.UserName,
                FullName = registerDto.FullName,
                Phone = registerDto.Phone,
                Email = registerDto.Email,
                Role = Roles.User,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password)
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto loginDto)
        {
            var user = _context.Users.SingleOrDefault(u => u.UserName == loginDto.UserName);
            if (user == null || string.IsNullOrEmpty(user.PasswordHash) || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
                return Unauthorized("Invalid credentials");

            var token = new JWTService(_config).GenerateToken(user);
            return Ok(new { token });
        }
    }
}