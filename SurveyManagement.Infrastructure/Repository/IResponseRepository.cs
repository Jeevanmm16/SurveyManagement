using SurveyManagement.Domain.Entities;

namespace SurveyManagement.Infrastructure.Repository
{
    public interface IResponseRepository
    {
        Task<IEnumerable<Response>> GetAllAsync();
        Task<Response> GetByIdAsync(Guid id);
        Task AddAsync(Response response);
        void Remove(Response response);
    }
}
