using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto.Generators;
using SurveyManagement.Application.DTOS;
using SurveyManagement.Application.Mappings;
using SurveyManagement.Domain.Entities;
using SurveyManagement.Infrastructure.Data;
using SurveyManagement.Infrastructure.Repository;
using System;

namespace TaskBoardManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly SurveyDbContext _context;
        private readonly ITokenRepository _tokenRepository;

        public AuthController(SurveyDbContext context, ITokenRepository tokenRepository)
        {
            _context = context;
            _tokenRepository = tokenRepository;
        }

        // ✅ Register
        // ✅ Register endpoint
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto dto)
        {
            // Force normal users to have RoleId = 2 (e.g., "User")
            int defaultRoleId = 2;

            var role = await _context.Roles.FindAsync(defaultRoleId);
            if (role == null)
            {
                return BadRequest("Default role not found. Please seed roles in database.");
            }

            var userExists = await _context.Users.AnyAsync(u => u.Email == dto.Email);
            if (userExists)
            {
                return BadRequest("User with this email already exists.");
            }

            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Email = dto.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                RoleId = defaultRoleId,
                Address = dto.Address
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok("User registered successfully with default role (User). Admin can upgrade role.");
        }


        // ✅ Login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
        {
            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (user == null) return Unauthorized("Invalid email or password.");

            var passwordValid = BCrypt.Net.BCrypt.Verify(dto.Password, user.Password);
            if (!passwordValid) return Unauthorized("Invalid email or password.");

            var token = _tokenRepository.CreateJwtToken(user, user.Role.Name);

            return Ok(new LoginResponseDto { JwtToken = token });
        }
    }
}
