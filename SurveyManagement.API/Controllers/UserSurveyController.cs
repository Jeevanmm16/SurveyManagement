using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SurveyManagement.Application.DTOS;

namespace SurveyManagement.API.Controllers
{
   
        [Route("api/[controller]")]
        [ApiController]
        public class UserSurveyController : ControllerBase
        {
            private readonly IUserSurveyService _service;

            public UserSurveyController(IUserSurveyService service)
            {
                _service = service;
            }
        [Authorize(Roles = "Admin")]
        // ✅ Create UserSurvey (checks User exists, Survey exists, and Role != Admin)
        [HttpPost]
            public async Task<ActionResult<UserSurveyDto>> Create([FromBody] UserSurveyCreateDto dto)
            {
                try
                {
                    var result = await _service.CreateAsync(dto);
                    return CreatedAtAction(nameof(GetById), new { id = result.UserSurveyId }, result);
                }
                catch (KeyNotFoundException ex)
                {
                    return NotFound(new { message = ex.Message });
                }
                catch (InvalidOperationException ex)
                {
                    return BadRequest(new { message = ex.Message });
                }
            }
        [Authorize]
        // ✅ Get UserSurvey by ID
        [HttpGet("{id}")]
            public async Task<ActionResult<UserSurveyDto>> GetById(Guid id)
            {
                try
                {
                    var result = await _service.GetByIdAsync(id);
                    return Ok(result);
                }
                catch (KeyNotFoundException ex)
                {
                    return NotFound(new { message = ex.Message });
                }
            }
        [Authorize]
        // ✅ Get all UserSurveys
        [HttpGet]
            public async Task<ActionResult<IEnumerable<UserSurveyDto>>> GetAll()
            {
                var result = await _service.GetAllAsync();
                return Ok(result);
            }
        [Authorize(Roles = "Admin")]
        // ✅ Delete UserSurvey (cascade delete Responses)
        [HttpDelete("{id}")]
            public async Task<IActionResult> Delete(Guid id)
            {
                try
                {
                    await _service.DeleteAsync(id);
                    return NoContent();
                }
                catch (KeyNotFoundException ex)
                {
                    return NotFound(new { message = ex.Message });
                }
            }
        }
    }


