using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SurveyManagement.Application.DTOS;
using SurveyManagement.Application.Services;
using System;
using System.Threading.Tasks;

namespace SurveyManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SurveyController : ControllerBase
    {
        private readonly ISurveyService _surveyService;

        public SurveyController(ISurveyService surveyService)
        {
            _surveyService = surveyService;
        }

        private Guid GetLoggedInUserId()
        {
            var userIdClaim = User.FindFirst("userId")?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                throw new UnauthorizedAccessException("UserId claim missing in token");

            return Guid.Parse(userIdClaim);
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var surveys = await _surveyService.GetAllAsync();
            return Ok(surveys);
        }
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var survey = await _surveyService.GetByIdAsync(id);
            if (survey == null) return NotFound();
            return Ok(survey);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateSurveyDto dto)
        {
            var userId = GetLoggedInUserId();
            var survey = await _surveyService.CreateAsync(dto, userId);
            return CreatedAtAction(nameof(GetById), new { id = survey.SurveyId }, survey);
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateSurveyDto dto)
        {
            var updatedSurvey = await _surveyService.UpdateAsync(id, dto);
            if (updatedSurvey == null) return NotFound();
            return Ok(updatedSurvey);
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _surveyService.DeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
