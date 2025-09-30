using SurveyManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyManagement.Infrastructure.Repository
{
    public interface ISurveyRepository
    {
        Task<Survey?> GetByIdAsync(Guid id);
        Task<IEnumerable<Survey>> GetAllAsync();
        Task AddAsync(Survey survey);
        Task UpdateAsync(Survey survey);
        Task DeleteAsync(Guid id);
    }
}
