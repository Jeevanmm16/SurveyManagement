using global::SurveyManagement.Application.DTOS;

namespace SurveyManagement.Application.Services
{
    public interface ISurveyService
        {
            Task<IEnumerable<SurveyDto>> GetAllAsync();
            Task<SurveyDetailDto?> GetByIdAsync(Guid id);
            Task<SurveyDto> CreateAsync(CreateSurveyDto dto, Guid userId);
            Task<SurveyDto?> UpdateAsync(Guid id, UpdateSurveyDto dto);
            Task<bool> DeleteAsync(Guid id);
        }
    }

