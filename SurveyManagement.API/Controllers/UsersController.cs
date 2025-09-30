using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SurveyManagement.Application.DTOs;
using SurveyManagement.Application.Services;
using System;
using System.Threading.Tasks;

namespace SurveyManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUserService userService, ILogger<UsersController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserCreateDto dto)
        {
            _logger.LogInformation($"Creating new user: {dto.Email}");

            var createdUser = await _userService.AddUserAsync(dto);

            _logger.LogInformation($"User created successfully with ID: {createdUser.Id}");
            return Ok(createdUser);
        }

        [Authorize]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            _logger.LogInformation($"Fetching user with ID: {id}");

            var user = await _userService.GetUserByIdAsync(id);

            if (user == null)
            {
                _logger.LogWarning($"User with ID {id} not found.");
                return NotFound(new { message = $"User with Id '{id}' not found." });
            }

            _logger.LogInformation($"User with ID {id} returned successfully.");
            return Ok(user);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            _logger.LogTrace("Trace: Fetching all users");
            _logger.LogInformation("Fetching all users");

            var users = await _userService.GetAllUsersAsync();
            var count = users?.Count() ?? 0;

            _logger.LogInformation($"Returned {count} users");
            return Ok(users);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UserUpdateDto dto)
        {
            _logger.LogInformation($"Updating user with ID: {id}");

            var updatedUser = await _userService.UpdateUserAsync(id, dto);

            if (updatedUser == null)
            {
                _logger.LogWarning($"User with ID {id} not found for update.");
                return NotFound(new { message = $"User with Id '{id}' not found." });
            }

            _logger.LogInformation($"User with ID {id} updated successfully.");
            return Ok(updatedUser);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            _logger.LogInformation($"Deleting user with ID: {id}");

            var deletedUser = await _userService.DeleteUserAsync(id);

            if (deletedUser == null)
            {
                _logger.LogWarning($"User with ID {id} not found for deletion.");
                return NotFound(new { message = $"User with Id '{id}' not found." });
            }

            _logger.LogInformation($"User with ID {id} deleted successfully.");
            return Ok(deletedUser);
        }
    }
}
