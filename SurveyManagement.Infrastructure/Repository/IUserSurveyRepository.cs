using SurveyManagement.Domain.Entities;

namespace SurveyManagement.Infrastructure.Repository
{
    public interface IUserSurveyRepository
    {
        Task<UserSurvey> AddAsync(UserSurvey userSurvey);
        Task<UserSurvey?> GetByIdAsync(Guid id);
        Task<IEnumerable<UserSurvey>> GetAllAsync();
        Task DeleteAsync(UserSurvey userSurvey);
    }
}
