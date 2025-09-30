using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SurveyManagement.Application.DTOS;
using SurveyManagement.Application.Services;
using SurveyManagement.Infrastructure.Repository;

namespace SurveyManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResponsesController : ControllerBase
    {
        private readonly IResponseService _responseService;
        private readonly IQuestionRepository _questionRepository;

        public ResponsesController(IResponseService responseService, IQuestionRepository questionRepository)
        {
            _responseService = responseService;
            _questionRepository = questionRepository;
        }
        [Authorize(Roles = "Admin,User")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var responses = await _responseService.GetAllAsync();
            return Ok(responses);
        }
       [Authorize(Roles = "Admin,User")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var response = await _responseService.GetByIdAsync(id);
            return Ok(response);
        }
       [Authorize(Roles = "User")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateResponseDto dto)
        {
            var question = await _questionRepository.GetByIdAsync(dto.QuestionId);
            var response = await _responseService.CreateAsync(dto, question);
            return CreatedAtAction(nameof(GetById), new { id = response.ResponseId }, response);
        }
        [Authorize(Roles = "User")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateResponseDto dto)
        {
            var responseEntity = await _responseService.GetByIdAsync(id);
            var question = await _questionRepository.GetByIdAsync(responseEntity.QuestionId);
            var updatedResponse = await _responseService.UpdateAsync(id, dto, question);
            return Ok(updatedResponse);
        }
        [Authorize(Roles = "User")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _responseService.DeleteAsync(id);
            return NoContent();
        }
    }
}
