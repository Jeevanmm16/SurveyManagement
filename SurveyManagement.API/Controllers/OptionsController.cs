using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SurveyManagement.Application.DTOS;
using SurveyManagement.Application.Services;

namespace SurveyManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OptionsController : ControllerBase
    {
        private readonly IOptionService _optionService;

        public OptionsController(IOptionService optionService)
        {
            _optionService = optionService;
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("bulk")]
        public async Task<IActionResult> CreateOption([FromBody] OptionCreateDto dto)
        {
            try
            {
                var option = await _optionService.CreateOptionAsync(dto);
                return Ok(option);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<IActionResult> UpdateOption([FromBody] OptionUpdateDto dto)
        {
            
                var updated = await _optionService.UpdateOptionAsync(dto);
                return Ok(updated);
           
        }
        [Authorize]
        [HttpGet("{questionId}")]
        public async Task<IActionResult> GetOptions(Guid questionId)
        {
            var options = await _optionService.GetOptionsByQuestionIdAsync(questionId);
            return Ok(options);
        }
    }
}
