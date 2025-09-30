using SurveyManagement.Application.DTOS;

public interface IUserSurveyService
{
    Task<UserSurveyDto> CreateAsync(UserSurveyCreateDto dto);
    Task<UserSurveyDto> GetByIdAsync(Guid id);
    Task<IEnumerable<UserSurveyDto>> GetAllAsync();
    Task DeleteAsync(Guid id);
}