// Controllers/QuestionController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SurveyManagement.Application.DTOS;
using SurveyManagement.Application.Services;
using System;
using System.Threading.Tasks;

namespace SurveyManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionController : ControllerBase
    {
        private readonly IQuestionService _service;
        private readonly ILogger<QuestionController> _logger;
        public QuestionController(IQuestionService service, ILogger<QuestionController> logger)
        {
            _service = service;
            _logger = logger;
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogTrace("Trace: Fetching all questions");
            _logger.LogInformation("Fetching all questions");

            try
            {
                var questions = await _service.GetAllAsync();
                var count = questions?.Count() ?? 0;
                _logger.LogInformation($"Returned {count} questions");
                return Ok(questions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching all questions");
                return StatusCode(500, "Internal Server Error");
            }
        }
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            _logger.LogInformation($"Fetching question with ID: {id}");
            var question = await _service.GetByIdAsync(id);
            if (question == null)
            {
                _logger.LogWarning($"Question with ID {id} not found.");
                return NotFound();
            }

            _logger.LogInformation($"Question with ID {id} returned successfully.");
            return Ok(question);

          
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateQuestionDto dto)
        {
            _logger.LogInformation($"Creating new question: {dto.QuestionText}");

            var question = await _service.CreateAsync(dto);
            _logger.LogInformation($"Question created with ID: {question.QuestionId}");
            return CreatedAtAction(nameof(GetById), new { id = question!.QuestionId }, question);
            
           
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]

        public async Task<IActionResult> Update(Guid id, UpdateQuestionDto dto)
        {
            _logger.LogInformation($"Updating question ID: {id}");
            var updated = await _service.UpdateAsync(id, dto);
            _logger.LogInformation($"Question ID {id} updated successfully.");

            return Ok(updated);
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            _logger.LogInformation($"Deleting question ID: {id}");
            var deleted = await _service.DeleteAsync(id);
            if (!deleted)
            {
                _logger.LogWarning($"Question ID {id} not found to delete.");
                return NotFound();
            }
            _logger.LogInformation($"Question ID {id} deleted successfully.");
            return NoContent();
        }
    }
}
