using SurveyManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyManagement.Infrastructure.Repository
{
    public interface IOptionRepository
    {
        Task<Option?> GetByIdAsync(Guid id);
        Task<List<Option>> GetByQuestionIdAsync(Guid questionId);
        Task AddAsync(Option option);
        Task UpdateAsync(Option option);
        Task SaveChangesAsync();
    }
}
