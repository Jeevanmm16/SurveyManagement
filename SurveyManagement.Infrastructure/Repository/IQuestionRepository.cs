using SurveyManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyManagement.Infrastructure.Repository
{
    public interface IQuestionRepository
    {
        Task<IEnumerable<Question>> GetAllAsync();
        Task<Question?> GetByIdAsync(Guid id);
        Task<Question> AddAsync(Question question);
        Task<Question?> UpdateAsync(Question question);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> SurveyExistsAsync(Guid surveyId);
    }
}
