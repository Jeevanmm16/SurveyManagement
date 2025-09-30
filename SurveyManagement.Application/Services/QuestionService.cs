
using AutoMapper;
using SurveyManagement.Application.DTOS;
using SurveyManagement.Application.Services;
using SurveyManagement.Domain.Entities;
using SurveyManagement.Infrastructure.Repository;

namespace Application.Services
{
    public class QuestionService : IQuestionService
    {
        private readonly IQuestionRepository _repo;
        private readonly IMapper _mapper;

        public QuestionService(IQuestionRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<QuestionDto?> CreateAsync(CreateQuestionDto dto)
        {
            if (!await _repo.SurveyExistsAsync(dto.SurveyId))
                throw new Exception("Invalid survey id.");

            var question = new Question
            {
                QuestionText = dto.QuestionText,
                QuestionType = dto.QuestionType,
                IsMandatory = dto.IsMandatory,
                SurveyId = dto.SurveyId
            };

            var added = await _repo.AddAsync(question);
            return _mapper.Map<QuestionDto>(added);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {

            return await _repo.DeleteAsync(id);
        }

        public async Task<IEnumerable<QuestionDto>> GetAllAsync()
        {
            var questions = await _repo.GetAllAsync();
            return _mapper.Map<IEnumerable<QuestionDto>>(questions);
        }

        public async Task<QuestionDetailDto?> GetByIdAsync(Guid id)
        {
            var question = await _repo.GetByIdAsync(id);
            if(question == null)
            {
                throw new KeyNotFoundException($"Question {id} not exist");
            }
            return  _mapper.Map<QuestionDetailDto>(question);
        }

        public async Task<QuestionDto?> UpdateAsync(Guid id, UpdateQuestionDto dto)
        {
            var question = await _repo.GetByIdAsync(id);
            if (question == null)
            {
                throw new KeyNotFoundException($"Question {id} not exist");
            }

            question.QuestionText = dto.QuestionText;
            question.IsMandatory = dto.IsMandatory;

            var updated = await _repo.UpdateAsync(question);
            return _mapper.Map<QuestionDto>(updated);
        }


    }
}