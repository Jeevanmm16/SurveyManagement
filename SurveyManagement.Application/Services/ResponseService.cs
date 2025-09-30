using SurveyManagement.Application.DTOS;
using SurveyManagement.Domain.Entities;
using SurveyManagement.Infrastructure.Repository;

namespace SurveyManagement.Application.Services
{
    public class ResponseService : IResponseService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ResponseService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<ResponseDto>> GetAllAsync()
        {
            var responses = await _unitOfWork.Responses.GetAllAsync();
            return responses.Select(MapToDto);
        }

        public async Task<ResponseDto> GetByIdAsync(Guid id)
        {
            var response = await _unitOfWork.Responses.GetByIdAsync(id);
            if (response == null) throw new KeyNotFoundException("Response not found");
            return MapToDto(response);
        }

        public async Task<ResponseDto> CreateAsync(CreateResponseDto dto, Question question)
        {
            var response = MapDtoToEntity(dto, question, Guid.NewGuid());
            await _unitOfWork.Responses.AddAsync(response);
            await _unitOfWork.CompleteAsync();
            return MapToDto(response);
        }

        public async Task<ResponseDto> UpdateAsync(Guid id, UpdateResponseDto dto, Question question)
        {
            var response = await _unitOfWork.Responses.GetByIdAsync(id);
            if (response == null) throw new KeyNotFoundException("Response not found");

            // Clear existing ResponseOptions if any
            response.ResponseOptions?.Clear();

            switch (question.QuestionType)
            {
                case QuestionType.Text:
                    if (string.IsNullOrWhiteSpace(dto.FeedbackText))
                        throw new ArgumentException("FeedbackText is required for Text question");
                    response.FeedbackText = dto.FeedbackText;
                    response.Rating = null;
                    break;

                case QuestionType.Rating:
                    if (!dto.Rating.HasValue || dto.Rating < 1 || dto.Rating > 5)
                        throw new ArgumentException("Rating must be between 1 and 5");
                    response.Rating = dto.Rating;
                    response.FeedbackText = null;
                    break;

                case QuestionType.Radio:
                    if (dto.OptionIds == null || dto.OptionIds.Count != 1)
                        throw new ArgumentException("Exactly one option required for MultipleChoice");
                    response.ResponseOptions = dto.OptionIds
                        .Select(idOpt => new ResponseOption { ResponseId = response.ResponseId, OptionId = idOpt })
                        .ToList();
                    response.FeedbackText = null;
                    response.Rating = null;
                    break;

                case QuestionType.Checkbox:
                    if (dto.OptionIds == null || !dto.OptionIds.Any())
                        throw new ArgumentException("At least one option required for Checkbox");
                    response.ResponseOptions = dto.OptionIds
                        .Select(idOpt => new ResponseOption { ResponseId = response.ResponseId, OptionId = idOpt })
                        .ToList();
                    response.FeedbackText = null;
                    response.Rating = null;
                    break;
            }

            await _unitOfWork.CompleteAsync();
            return MapToDto(response);
        }

        public async Task DeleteAsync(Guid id)
        {
            var response = await _unitOfWork.Responses.GetByIdAsync(id);
            if (response == null) throw new KeyNotFoundException("Response not found");
            _unitOfWork.Responses.Remove(response);
            await _unitOfWork.CompleteAsync();
        }

        // ---------------- Helper Mappers ----------------

        private static ResponseDto MapToDto(Response response) =>
            new()
            {
                ResponseId = response.ResponseId,
                UserSurveyId = response.UserSurveyId,
                QuestionId = response.QuestionId,
                FeedbackText = response.FeedbackText,
                Rating = response.Rating,
                OptionIds = response.ResponseOptions?.Select(ro => ro.OptionId).ToList()
            };

        private static Response MapDtoToEntity(CreateResponseDto dto, Question question, Guid responseId)
        {
            var response = new Response
            {
                ResponseId = responseId,
                UserSurveyId = dto.UserSurveyId,
                QuestionId = dto.QuestionId
            };

            switch (question.QuestionType)
            {
                case QuestionType.Text:
                    if (string.IsNullOrWhiteSpace(dto.FeedbackText))
                        throw new ArgumentException("FeedbackText is required for Text question");
                    response.FeedbackText = dto.FeedbackText;
                    break;

                case QuestionType.Rating:
                    if (!dto.Rating.HasValue || dto.Rating < 1 || dto.Rating > 5)
                        throw new ArgumentException("Rating must be between 1 and 5");
                    response.Rating = dto.Rating;
                    break;

                case QuestionType.Radio:
                    if (dto.OptionIds == null || dto.OptionIds.Count != 1)
                        throw new ArgumentException("Exactly one option required for MultipleChoice");
                    response.ResponseOptions = dto.OptionIds
                        .Select(idOpt => new ResponseOption { ResponseId = response.ResponseId, OptionId = idOpt })
                        .ToList();
                    break;

                case QuestionType.Checkbox:
                    if (dto.OptionIds == null || !dto.OptionIds.Any())
                        throw new ArgumentException("At least one option required for Checkbox");
                    response.ResponseOptions = dto.OptionIds
                        .Select(idOpt => new ResponseOption { ResponseId = response.ResponseId, OptionId = idOpt })
                        .ToList();
                    break;
            }

            return response;
        }
    }
}

